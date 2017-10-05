using iRacingSdkWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;

namespace iDash
{
    public class IRacingConnector : ISimConnector
    {             
        // Globally declared SdkWrapper object
        private readonly SdkWrapper wrapper = new SdkWrapper();
        private SdkWrapper.TelemetryUpdatedEventArgs telemetryInfo;

        private float firstRpm = 0;
        private float lastRpm = 0;
        private float currentRpm = 0;
        private int flag = 0;

        private bool disposed = false;

        private Logger logger = new Logger();     

        protected override void start()
        {
            //wrapper.TelemetryUpdateFrequency = 60;
            // Listen to events
            wrapper.TelemetryUpdated += OnTelemetryUpdated;
            wrapper.SessionInfoUpdated += OnSessionInfoUpdated;
            // Start it if Arduino is Connected
            wrapper.Start();

            StringBuilder msg = new StringBuilder();
            bool isConnected = false;

            NotifyStatusMessage("Waiting for IRacing...");

            while (!CancellationPending)
            {
                msg.Clear();
              
                if (wrapper.IsConnected)
                {
                    if (!isConnected)
                    {
                        string s = "Connected to iRacing.";
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
                        string s = "iRacing closed.";
                        logger.LogMessageToFile(s, true);
                        NotifyStatusMessage(s);
                    }

                    isConnected = false;                    
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

            wrapper.Stop();
            
            NotifyStatusMessage("iRacing thread stopped.");
        }

        protected override string getTelemetryValue(string name, string type, string clazz)
        {
            string result = "";

            try {
                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(type))
                {
                    switch (type)
                    {
                        case "int":
                            int value = wrapper.GetTelemetryValue<int>(name).Value;
                            if (name.Equals("Gear", StringComparison.InvariantCultureIgnoreCase))
                            {
                                //return reverse symbol
                                if (value < 0) return "R";
                            }
                            result = value.ToString();
                            break;
                        case "float":
                            result = wrapper.GetTelemetryValue<float>(name).Value.ToString();
                            break;
                        case "bool":
                            result = wrapper.GetTelemetryValue<bool>(name).Value.ToString();
                            break;
                        case "double":
                            result = wrapper.GetTelemetryValue<double>(name).Value.ToString();
                            break;
                        case "bitfield":
                            result = wrapper.GetTelemetryValue<byte>(name).Value.ToString();
                            break;
                        //todo: add handler for arrays
                        case "kmh":
                            result = ((int)Math.Floor(wrapper.GetTelemetryValue<float>(name).Value * 3.6)).ToString();
                            break;
                        case "time":
                            float seconds = wrapper.GetTelemetryValue<float>(name).Value;
                            TimeSpan interval = TimeSpan.FromSeconds(seconds);
                            result = interval.ToString(@"mm\.ss\.fff");
                            break;
                        case "dtime":
                            double dseconds = wrapper.GetTelemetryValue<double>(name).Value;
                            TimeSpan dinterval = TimeSpan.FromSeconds(dseconds);
                            result = dinterval.ToString(@"mm\.ss\.fff");
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                logger.LogExceptionToFile(e);
            }

            return result;
        }              

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            try
            {
                YamlQuery yLastRPM = e.SessionInfo["DriverInfo"]["DriverCarSLLastRPM"];
                YamlQuery yShiftRPM = e.SessionInfo["DriverInfo"]["DriverCarSLShiftRPM"];
                //YamlQuery yIsOnPit = e.SessionInfo["DriverInfo"]["OnPitRoad"];             

                if (yShiftRPM != null)
                {
                    lastRpm = float.Parse(yShiftRPM.Value, CultureInfo.InvariantCulture.NumberFormat);
                    //Logger.LogMessageToFile("Shift:" + maxRpm + "\n");
                }
                else
                {
                    if (yLastRPM != null)
                    {
                        //calibrate shift gear light rpm
                        lastRpm = float.Parse(yLastRPM.Value, CultureInfo.InvariantCulture.NumberFormat) * 0.97f;
                        //Logger.LogMessageToFile("Shift:" + maxRpm + "\n");
                    }
                }

                firstRpm = FIRST_RPM * lastRpm;
            }
            catch (Exception ex)
            {
                logger.LogExceptionToFile(ex);
            }
        }

        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            currentRpm = e.TelemetryInfo.RPM.Value;
            this.telemetryInfo = e;

            try
            {
                switch (wrapper.GetTelemetryValue<int>("SessionFlags").Value)
                {
                    case 32:
                        flag = (int)Constants.FLAG_TYPE.BLUE_FLAG;
                        break;
                    case 8:
                        flag = (int)Constants.FLAG_TYPE.YELLOW_FLAG;
                        break;
                    default:
                        flag = (int)Constants.FLAG_TYPE.NO_FLAG;
                        break;
                }

                flag = ((int)wrapper.GetTelemetryValue<int>("EngineWarnings").Value & 0x10) == 0x10 ? (int)Constants.FLAG_TYPE.SPEED_LIMITER : flag;

            }
            catch (Exception ex)
            {
                logger.LogExceptionToFile(ex);
            }
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
                    if (wrapper != null)
                    {
                        wrapper.Stop();
                    }
                }
                // Release unmanaged resources.
                disposed = true;
            }
        }

        ~IRacingConnector() { Dispose(false); }
    }
}
