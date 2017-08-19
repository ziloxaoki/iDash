using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using iDash.rFactor2Data;
using System.IO.MemoryMappedFiles;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;

namespace iDash
{
    class RFactor2Connector : ISimConnector
    {
        // Connection fields
        private const int CONNECTION_RETRY_INTERVAL_MS = 1000;
        private const int DISCONNECTED_CHECK_INTERVAL_MS = 15000;
        private const float DEGREES_IN_RADIAN = 57.2957795f;
        private const int LIGHT_MODE_REFRESH_MS = 500;

        private bool disposed = false;
        private bool isConnected = false;
        private bool disableNotification = false;
        private bool isGameRunning = false;

        // Plugin access fields
        Mutex fileAccessMutex = null;
        MemoryMappedFile memoryMappedFile1 = null;
        MemoryMappedFile memoryMappedFile2 = null;

        // Shared memory buffer fields
        readonly int SHARED_MEMORY_SIZE_BYTES = Marshal.SizeOf(typeof(rF2State));
        readonly int SHARED_MEMORY_HEADER_SIZE_BYTES = Marshal.SizeOf(typeof(rF2StateHeader));
        byte[] sharedMemoryReadBuffer = null;

        // Marshalled view
        protected rF2State currrF2State;
        protected rF2VehScoringInfo carData;
        protected rF2Wheel wheel;

        private Logger logger = new Logger();

        protected override void start()
        {
            StringBuilder msg = new StringBuilder();

            NotifyStatusMessage("Waiting for RFactor2...");

            while (!CancellationPending)
            {
                msg.Clear();
                if (isGameRunning)
                {
                    if (!isConnected)
                    {
                        if (!disableNotification)
                        {
                            string s = "Connected to RFactor2.";
                            logger.LogMessageToFile(s, true);
                            NotifyStatusMessage(s);
                            disableNotification = true;
                        }
                        connect();                        
                    }
                    else
                    { 
                        readGameData();
                        if (currrF2State.mNumVehicles > 0)
                        {
                            float lastRpm = (float)currrF2State.mEngineMaxRPM;
                            float firstRpm = FIRST_RPM * lastRpm;
                            //calibrate shift gear light rpm
                            lastRpm *= 0.97f;
                            float currentRpm = (float)currrF2State.mEngineRPM;

                            int flag = 0;

                            if (carData.mFlag == 6)
                            {
                                flag = (int)Constants.FLAG_TYPE.BLUE_FLAG; 
                            }

                            if (currrF2State.mYellowFlagState > 0)
                            {
                                flag = (int)Constants.FLAG_TYPE.YELLOW_FLAG;
                            }

                            flag = currrF2State.mSpeedLimiter > 0 ? (int)Constants.FLAG_TYPE.SPEED_LIMITER : flag;

                            sendRPMShiftMsg(currentRpm, firstRpm, lastRpm, flag);

                            foreach (SerialManager serialManager in sm)
                            {
                                if (serialManager.deviceContains7Segments())
                                {
                                    send7SegmentMsg();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if(isConnected)
                    {
                        string s = "RFactor2 closed.";
                        logger.LogMessageToFile(s, true);
                        NotifyStatusMessage(s);
                        disableNotification = true;
                    }

                    checkRFactor2Running();
                    isConnected = false;
                    disableNotification = false;

                    foreach (SerialManager serialManager in sm)
                    {
                        if (serialManager.deviceContains7Segments())
                        {
                            serialManager.enqueueCommand(Utils.getDisconnectedMsgCmd(), false);
                        }
                    }
                }
                
                Thread.Sleep(Constants.SharedMemoryReadRate);
            }

            NotifyStatusMessage("RFactor2 thread stopped.");
            Dispose();
        }

        private void readGameData()
        {
            try
            {
                // Note: if it is critical for client minimize wait time, same strategy as plugin uses can be employed.
                // Pass 0 timeout and skip update if someone holds the lock.
                if (this.fileAccessMutex.WaitOne(5000))
                {
                    try
                    {
                        bool buf1Current = false;
                        // Try buffer 1:
                        using (var sharedMemoryStreamView = this.memoryMappedFile1.CreateViewStream())
                        {
                            var sharedMemoryStream = new BinaryReader(sharedMemoryStreamView);
                            this.sharedMemoryReadBuffer = sharedMemoryStream.ReadBytes(this.SHARED_MEMORY_HEADER_SIZE_BYTES);

                            // Marhsal header
                            var headerHandle = GCHandle.Alloc(this.sharedMemoryReadBuffer, GCHandleType.Pinned);
                            var header = (rF2StateHeader)Marshal.PtrToStructure(headerHandle.AddrOfPinnedObject(), typeof(rF2StateHeader));
                            headerHandle.Free();

                            if (header.mCurrentRead == 1)
                            {
                                sharedMemoryStream.BaseStream.Position = 0;
                                this.sharedMemoryReadBuffer = sharedMemoryStream.ReadBytes(this.SHARED_MEMORY_SIZE_BYTES);
                                buf1Current = true;
                            }
                        }

                        // Read buffer 2
                        if (!buf1Current)
                        {
                            using (var sharedMemoryStreamView = this.memoryMappedFile2.CreateViewStream())
                            {
                                var sharedMemoryStream = new BinaryReader(sharedMemoryStreamView);
                                this.sharedMemoryReadBuffer = sharedMemoryStream.ReadBytes(this.SHARED_MEMORY_SIZE_BYTES);
                            }
                        }
                    }
                    finally
                    {
                        this.fileAccessMutex.ReleaseMutex();
                    }

                    // Marshal rF2State
                    var handle = GCHandle.Alloc(this.sharedMemoryReadBuffer, GCHandleType.Pinned);
                    this.currrF2State = (rF2State)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(rF2State));
                    this.carData = currrF2State.mVehicles[0];
                    this.wheel = currrF2State.mWheels[0];
                    handle.Free();
                }
            }
            catch (Exception)
            {
                disconnect();
            }
        }

        private void connect()
        {
            if (!isConnected)
            {
                try
                {
                    this.fileAccessMutex = Mutex.OpenExisting(rFactor2Constants.MM_FILE_ACCESS_MUTEX);
                    this.memoryMappedFile1 = MemoryMappedFile.OpenExisting(rFactor2Constants.MM_FILE_NAME1);
                    this.memoryMappedFile2 = MemoryMappedFile.OpenExisting(rFactor2Constants.MM_FILE_NAME2);
                    // NOTE: Make sure that SHARED_MEMORY_SIZE_BYTES matches the structure size in the plugin (debug mode prints).
                    this.sharedMemoryReadBuffer = new byte[this.SHARED_MEMORY_SIZE_BYTES];
                    isConnected = true;
                }
                catch (Exception)
                {
                    disconnect();
                }
            }
        }

        protected string getValue(string name, string type, object clazz)
        {
            string result = "";

            Type pType = clazz.GetType();

            FieldInfo field = pType.GetField(name);

            if (field != null)
            {
                try
                {
                    switch (type)
                    {
                        case "time":
                            float seconds = 0;

                            if (field.FieldType.Name.Equals("Double"))
                            {
                                seconds = Convert.ToSingle(field.GetValue(clazz));
                            }
                            TimeSpan interval = TimeSpan.FromSeconds(seconds);
                            result = interval.ToString(@"mm\.ss\.fff");
                            break;
                        case "kmh":
                            if (field.FieldType.Name.Equals("Double"))
                            {
                                result = ((int)Math.Floor((double)field.GetValue(clazz) * 3.6)).ToString();
                            }
                            break;
                        default:
                            if (name.Equals("gear"))
                            {
                                int gear = (int)field.GetValue(clazz) - 1;
                                if (gear < 0)
                                {
                                    return "R";
                                }

                                result = (gear + 1).ToString();
                            }
                            else
                            {
                                result = field.GetValue(clazz).ToString();
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    logger.LogExceptionToFile(e);
                }
            }

            return result;
        }

        protected override string getTelemetryValue(string name, string type, string clazz)
        {
            string result = "";

            if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(clazz))
            {
                switch (clazz)
                {
                    case "rF2State":
                        result = getValue(name, type, currrF2State);
                        break;
                    case "rF2VehScoringInfo":
                        result = getValue(name, type, carData);
                        break;
                    case "rF2Wheel":
                        result = getValue(name, type, wheel);
                        break;
                }
            }

            return result;
        }

        private void checkRFactor2Running()
        {
            isGameRunning = Utils.IsGameRunning(GameDefinition.rfactor2.processName);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void disconnect()
        {
            if (this.memoryMappedFile1 != null)
                this.memoryMappedFile1.Dispose();

            if (this.memoryMappedFile2 != null)
                this.memoryMappedFile2.Dispose();

            if (this.fileAccessMutex != null)
                this.fileAccessMutex.Dispose();

            this.memoryMappedFile1 = null;
            this.memoryMappedFile2 = null;
            this.sharedMemoryReadBuffer = null;
            isConnected = false;
            this.fileAccessMutex = null;
        }        

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    disconnect();
                }
                // Release unmanaged resources.
                disposed = true;
            }
        }

        ~RFactor2Connector() { Dispose(false); }
    }
}
