using iRacingSdkWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace iDash
{
    public class IRacingConnector : ISimConnector
    {             
        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;
        // Globally declared SdkWrapper object
        private readonly SdkWrapper wrapper;
        private SdkWrapper.TelemetryUpdatedEventArgs telemetryInfo;

        private float firstRpm = 0;
        private float lastRpm = 0;
        private float currentRpm = 0;

        private bool disposed = false;

        public IRacingConnector(SerialManager sm) : base(sm)
        {
            this.sm = sm;

            // Create instance
            wrapper = new SdkWrapper();
            //wrapper.TelemetryUpdateFrequency = 60;
            // Listen to events
            wrapper.TelemetryUpdated += OnTelemetryUpdated;
            wrapper.SessionInfoUpdated += OnSessionInfoUpdated;
            // Start it if Arduino is Connected
            wrapper.Start();            

            new Thread(new ThreadStart(start)).Start();
        }              


        private async void start()
        {
            StringBuilder msg = new StringBuilder();
            bool isConnected = false;

            NotifyStatusMessage("Waiting for IRacing...");

            while (!MainForm.stopThreads && !MainForm.stopIRacingThreads)
            {
                msg.Clear();
                if (wrapper.IsConnected)
                {
                    if (!isConnected)
                    {
                        string s = DateTime.Now.ToString("hh:mm:ss") + ": Connected to iRacing.";
                        NotifyStatusMessage(s);
                    }
                    isConnected = true;

                    sendRPMShiftMsg(currentRpm, firstRpm, lastRpm);
                    send7SegmentMsg();
                }
                else
                {
                    isConnected = false;
                    sm.sendCommand(Utils.getDisconnectedMsgCmd(), false);
                }

                //c = new Command(Command.CMD_RGB_SHIFT, colourPattern);
                //sm.sendCommand(c);
                await Task.Delay(5);
            }

            Dispose();
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
                Logger.LogExceptionToFile(e);
            }

            return result;
        }              

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            //gets max car rpm
            //YamlQuery yq = e.SessionInfo["DriverInfo"]["DriverCarSLBlinkRPM"];
            /*YamlQuery yq = e.SessionInfo["DriverInfo"]["DriverCarSLFirstRPM"];
            if (yq != null)
            {
                firstRpm = float.Parse(yq.Value, CultureInfo.InvariantCulture.NumberFormat);
                //Logger.LogMessageToFile("Shift:" + maxRpm + "\n");
            }*/
            YamlQuery yLastRPM = e.SessionInfo["DriverInfo"]["DriverCarSLLastRPM"];
            YamlQuery yShiftRPM = e.SessionInfo["DriverInfo"]["DriverCarSLShiftRPM"];
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
                    lastRpm = float.Parse(yLastRPM.Value, CultureInfo.InvariantCulture.NumberFormat) * 0.93f;
                    //Logger.LogMessageToFile("Shift:" + maxRpm + "\n");
                }
            }

            firstRpm = FIRST_RPM * lastRpm;
        }

        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            currentRpm = e.TelemetryInfo.RPM.Value;
            this.telemetryInfo = e;
            //Logger.LogMessageToFile("rpm:" + rpm + "\n");
        }

        //notify subscribers (statusbar) that a message has to be logged
        public void NotifyStatusMessage(string args)
        {
            StatusMessageHandler handler = StatusMessageSubscribers;

            if (handler != null)
            {
                handler(args + "\n");
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
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
