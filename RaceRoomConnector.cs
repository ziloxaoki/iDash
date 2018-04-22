using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using R3E.Data;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;

namespace iDash
{
    public class RaceRoomConnector : ISimConnector
    {
        private bool Mapped
        {
            get { return (_file != null); }
        }

        private MemoryMappedFile _file;

        private Shared data;
        private byte[] buffer;

        private bool disposed = false;
        private Logger logger = new Logger();

        private bool isRrreRunning()
        {
            return Process.GetProcessesByName("RRRE").Length > 0;
        }

        protected override void start()
        {
            StringBuilder msg = new StringBuilder();
            bool isConnected = false;            

            NotifyStatusMessage("Waiting for RRRE.exe...");

            while (!CancellationPending)
            {
                msg.Clear();

                if (isRrreRunning())
                {
                    if (!Mapped)
                    {
                        if (!isConnected)
                        {
                            string s = "Connected to RaceRoom.";
                            logger.LogMessageToFile(s, true);
                            NotifyStatusMessage(s);
                            isConnected = true;

                            if (Map())
                                buffer = new Byte[Marshal.SizeOf(typeof(Shared))];
                        }

                        foreach (SerialManager serialManager in sm)
                        {
                            if (serialManager.deviceContains7Segments())
                            {
                                serialManager.enqueueCommand(Utils.getDisconnectedMsgCmd(), false);
                                serialManager.enqueueCommand(Utils.getBlackRPMCmd(), false);
                            }
                        }
                    }
                    else
                    {
                        if (Read())
                        {
                            if (data.SessionType >= 0)
                            {
                                float lastRpm = RpsToRpm(data.MaxEngineRps);
                                float firstRpm = FIRST_RPM * lastRpm;
                                //calibrate shift gear light rpm
                                lastRpm *= 0.95f;
                                float currentRpm = RpsToRpm(data.EngineRps);

                                int flag = 0;

                                try
                                {
                                    if (data.Flags.Blue > 0)
                                    {
                                        flag = (int)Constants.FLAG_TYPE.BLUE_FLAG;
                                    }
                                    if (data.Flags.Yellow > 0)
                                    {
                                        flag = (int)Constants.FLAG_TYPE.YELLOW_FLAG;
                                    }

                                    flag = data.InPitlane > 0 ? (int)Constants.FLAG_TYPE.IN_PIT_FLAG : flag;
                                }
                                catch (Exception)
                                {
                                    
                                }

                                sendRPMShiftMsg(currentRpm, firstRpm, lastRpm, flag);

                                foreach (SerialManager serialManager in sm)
                                {
                                    if (serialManager.deviceContains7Segments())
                                    {
                                        send7SegmentMsg();
                                    }
                                }
                            }
                            else
                            {
                                foreach (SerialManager serialManager in sm)
                                {
                                    if (serialManager.deviceContains7Segments())
                                    {
                                        serialManager.enqueueCommand(Utils.getDisconnectedMsgCmd(), false);
                                        serialManager.enqueueCommand(Utils.getBlackRPMCmd(), false);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (SerialManager serialManager in sm)
                    {
                        serialManager.enqueueCommand(Utils.getDisconnectedMsgCmd(), false);
                        serialManager.enqueueCommand(Utils.getBlackRPMCmd(), false);
                    }

                    if(isConnected)
                    {
                        string s = "RaceRoom closed.";
                        logger.LogMessageToFile(s, true);
                        NotifyStatusMessage(s);
                        isConnected = false;
                    }
                }

                Thread.Sleep(Constants.SharedMemoryReadRate);
            }

            NotifyStatusMessage("RaceRoom thread stopped.");
        }


        private bool Read()
        {
            try
            {
                var _view = _file.CreateViewStream();
                BinaryReader _stream = new BinaryReader(_view);
                buffer = _stream.ReadBytes(Marshal.SizeOf(typeof(Shared)));
                GCHandle _handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                data = (Shared)Marshal.PtrToStructure(_handle.AddrOfPinnedObject(), typeof(Shared));
                _handle.Free();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool Map()
        {
            try
            {
                _file = MemoryMappedFile.OpenExisting(R3E.Constant.SharedMemoryName);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        protected override string getTelemetryValue(string name, string type, string clazz)
        {
            String result = "";

            //retrieve field by name
            FieldInfo prop = data.GetType().GetField(name);

            if (prop != null && !String.IsNullOrEmpty(type))
            {
                //pType = real field type
                string pType = prop.FieldType.Name;
                //type = expected field type, has to match to pType otherwise abort
                try
                {
                    switch (type)
                    {
                        case "Int32":
                            if (type.Equals(pType)) {
                                int val = ((int)prop.GetValue(data));
                                if (name.Equals("Gear", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    //return reverse symbol
                                    if (val < 0) return "R";
                                }                                
                                result = val.ToString();
                            }
                            break;
                        case "Single":
                            if (type.Equals(pType))
                            {
                                result = ((Single)prop.GetValue(data)).ToString();
                            }
                            break;
                        case "kmh":
                            if (pType.Equals("Single"))
                            {
                                result = ((int)Math.Floor((Single)prop.GetValue(data) * 3.6)).ToString();
                            }
                            break;
                        case "time":
                            if (pType.Equals("Single"))
                            {
                                float seconds = (Single)prop.GetValue(data);
                                TimeSpan interval = TimeSpan.FromSeconds(seconds);
                                result = interval.ToString(@"mm\.ss\.fff");
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

        private DriverData getDriverData(int slot_id)
        {
            foreach (DriverData driverData in data.DriverData)
            {
                if (driverData.DriverInfo.SlotId == slot_id)
                {
                    return driverData;
                }
            }
            throw new Exception("no driver data for slotID " + slot_id);
        }

        private Single RpsToRpm(Single rps)
        {
            return rps * (60 / (2 * (Single)Math.PI));
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_file != null)
                        _file.Dispose();
                }
                // Release unmanaged resources.
                disposed = true;
            }
        }

        ~RaceRoomConnector() { Dispose(false); }
    }
}
