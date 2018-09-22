using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AssettoCorsaSharedMemory;

namespace iDash
{
    class AssettoCorsaConnector : ISimConnector
    {
        private bool disposed = false;
        private AssettoCorsa ac;

        private float firstRpm = 0;
        private float lastRpm = 0;
        private float currentRpm = 0;
        private int flag = 0;
        private StaticInfo si;
        private Graphics gr;
        private Physics ph;

        Logger logger = new Logger();

        protected override void start()
        {
            ac = new AssettoCorsa();

            ac.GraphicsInterval = 10;
            ac.PhysicsInterval = 10;
            ac.StaticInfoInterval = 5000;

            ac.StaticInfoUpdated += StaticInfoUpdated;
            ac.GraphicsUpdated += GraphicsUpdated;
            ac.PhysicsUpdated += PhysicsUpdated;
            ac.Start();

            NotifyStatusMessage("Waiting for Assetto Corsa...");

            bool isConnected = false;       

            while (!CancellationPending)
            {
                if (gr.Status != AC_STATUS.AC_OFF)
                {
                    if (!isConnected)
                    {
                        string s = "Connected to Assetto Corsa.";
                        logger.LogMessageToFile(s, true);
                        NotifyStatusMessage(s);
                    }

                    isConnected = true;

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
                    if (isConnected)
                    {
                        string s = "Assetto Corsa closed.";
                        logger.LogMessageToFile(s, true);
                        NotifyStatusMessage(s);
                    }

                    isConnected = false;

                    foreach (SerialManager serialManager in sm)
                    {
                        if (serialManager.deviceContains7Segments())
                        {
                            serialManager.enqueueCommand(Utils.getDisconnectedMsgCmd(), false);
                            serialManager.enqueueCommand(Utils.getBlackRPMCmd(), false);
                        }
                    }
                }
                
                Thread.Sleep(Constants.SharedMemoryReadRate);
            }

            NotifyStatusMessage("Assetto Corsa thread stopped.");
        }

        private string getValue(string name, string type, object clazz)
        {
            string result = "";

            Type pType = clazz.GetType();
            System.Reflection.FieldInfo field = pType.GetField(name);
            if (field != null)
            {
                try
                {
                    switch (type)
                    {                        
                        case "time":
                            if (field.FieldType.Name.Equals("String"))
                            {
                                result = (String)field.GetValue(clazz);
                                result = result.Replace(":",".");
                            }
                            break;
                        case "kmh":
                            if (field.FieldType.Name.Equals("Single"))
                            {
                                result = ((int)Math.Floor((Single)field.GetValue(clazz))).ToString();
                            }
                            break;
                        case "Single[]":
                            if (field.FieldType.Name.Equals("Single[]"))
                            {
                                Single[] values = (Single[])field.GetValue(clazz);
                                foreach (Single value in values)
                                {
                                    result += value.ToString() + ".";
                                }

                                result = result.Remove(result.Length - 1);
                            }
                            break;
                        default:
                            if (name.Equals("Gear"))
                            {
                                int gear = (int)field.GetValue(clazz) - 1;
                                if(gear < 0)
                                {
                                    return "R";
                                }

                                result = gear.ToString();
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
                    case "physics":
                        result = getValue(name, type, ph);
                        break;
                    case "graphics":
                        result = getValue(name, type, gr);
                        break;
                    case "static":
                        result = getValue(name, type, si);
                        break;                    
                }
            }

            return result;
        }

        protected void StaticInfoUpdated(object sender, StaticInfoEventArgs e)
        {            
            if (e.StaticInfo.MaxRpm == 0)
            {
                //auto calibrate the max rpm. Sometimes the car doesn't return this info.
                if (lastRpm < currentRpm && currentRpm > 5000)
                {
                    lastRpm = currentRpm;
                }
            }
            else
            {                
                lastRpm = e.StaticInfo.MaxRpm;
            }
            firstRpm = FIRST_RPM * lastRpm;
            //calibrate shift gear light rpm
            lastRpm *= MainForm.maxRPM;
            si = e.StaticInfo;

            flag = (int)gr.Flag;

            if (ph.PitLimiterOn > 0)
            {
                flag = (int)Constants.FLAG_TYPE.IN_PIT_FLAG;
            }                       
        }

        protected void GraphicsUpdated(object sender, GraphicsEventArgs e)
        {
            gr = e.Graphics;
        }

        protected void PhysicsUpdated(object sender, PhysicsEventArgs e)
        {
            currentRpm = e.Physics.Rpms;            
            ph = e.Physics;
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
                    if (ac != null)
                    {
                        ac.Stop();
                    }
                }
                // Release unmanaged resources.
                disposed = true;
            }
        }

        ~AssettoCorsaConnector() { Dispose(false); }
    }
}
