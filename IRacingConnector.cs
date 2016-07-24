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
               

        //needs to wait until MainForm 7Segment is loaded
        private void send7SegmentMsg()
        {
            StringBuilder msg = new StringBuilder();
            List<string> _7SegmentData = MainForm.get7SegmentData();
            string[] strPatterns = MainForm.getStrFormat().Split(Utils.ITEM_SEPARATOR);

            if (_7SegmentData.Count > 0)
            {                
                for (int x = 0; x < _7SegmentData.Count; x++)
                {
                    string name = _7SegmentData.ElementAt(x);
                    string pattern = "{0}";                    

                    if(strPatterns.Length > 0) {
                        if(x < strPatterns.Length)
                            pattern = string.IsNullOrEmpty(strPatterns[x]) ? "{0}" : strPatterns[x];
                    }

                    msg.Append(this.getTelemetryData(name, pattern));
                }

                if (msg.Length > 0)
                {
                    byte[] b = Utils.getBytes(msg.ToString());
                    Command c = new Command(Command.CMD_7_SEGS, Utils.convertByteTo7Segment(b, 0));
                    sm.sendCommand(c);
                }
            }
        }


        private async void start()
        {
            StringBuilder msg = new StringBuilder();
            bool isConnected = false;

            NotifyStatusMessage("Looking for IRacing...");

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
                    sm.sendCommand(Utils.getDisconnectedMsgCmd());
                }

                //c = new Command(Command.CMD_RGB_SHIFT, colourPattern);
                //sm.sendCommand(c);
                await Task.Delay(5);
            }

            Dispose();
        }


        private string getTelemetryData(string name, string strPattern)
        {
            string result = "";
            if (!String.IsNullOrEmpty(name))
            {
                string[] type = name.Split('.');
                
                if (type.Length > 1) {
                    switch(type[1])
                    {
                        case "int":
                            result = wrapper.GetTelemetryValue<int>(type[0]).Value.ToString();
                            break;
                        case "float":
                            result = (wrapper.GetTelemetryValue<float>(type[0]).Value).ToString();
                            break;
                        case "bool":
                            result = wrapper.GetTelemetryValue<bool>(type[0]).Value.ToString();
                            break;
                        case "double":
                            result = wrapper.GetTelemetryValue<double>(type[0]).Value.ToString();
                            break;
                        case "bitfield":
                            result = wrapper.GetTelemetryValue<byte>(type[0]).Value.ToString();
                            break;
                        //todo: add handler for arrays
                        case "kmh":
                            result = ((int)Math.Floor(wrapper.GetTelemetryValue<float>(type[0]).Value * 3.6)).ToString();
                            break;
                        case "time":
                            float seconds = wrapper.GetTelemetryValue<float>(type[0]).Value;
                            TimeSpan interval = TimeSpan.FromSeconds(seconds);
                            result = interval.ToString(@"mm\.ss\.fff");
                            break;
                        case "dtime":
                            double dseconds = wrapper.GetTelemetryValue<double>(type[0]).Value;
                            TimeSpan dinterval = TimeSpan.FromSeconds(dseconds);
                            result = dinterval.ToString(@"mm\.ss\.fff");
                            break;
                    }

                    if (!String.IsNullOrEmpty(result))
                    {
                        result = Utils.formatString(result, strPattern);
                    }
                }
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
            YamlQuery yq = e.SessionInfo["DriverInfo"]["DriverCarSLLastRPM"];
            if (yq != null)
            {
                lastRpm = float.Parse(yq.Value, CultureInfo.InvariantCulture.NumberFormat);
                firstRpm = FIRST_RPM * lastRpm;
                //Logger.LogMessageToFile("Shift:" + maxRpm + "\n");
            }
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
