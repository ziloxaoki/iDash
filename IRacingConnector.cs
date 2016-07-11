using iRSDKSharp;
using iRacingSdkWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Management;
using System.Globalization;
using System.Text.RegularExpressions;

namespace iDash
{
    class IRacingConnector
    {
        private SerialManager sm;
        private string views;
        private byte[] colourPattern = { 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 1, 1, 255, 1, 1, 255, 1, 1, 255 };
        private float firstRpm = 0;
        private float lastRpm = 0;
        private float currentRpm = 0;
        private const int LED_NUM_TOTAL = 16;
        private const float FIRST_RPM = 0.7f;
        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;
        // Globally declared SdkWrapper object
        private readonly SdkWrapper wrapper;
        private SdkWrapper.TelemetryUpdatedEventArgs telemetryInfo;

        public IRacingConnector(SerialManager sm)
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

        private void sendRPMShiftMsg()
        {
            byte[] rpmLed = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }; // 1, 1, 1 = black
            float rpmPerLed = (lastRpm - firstRpm) / LED_NUM_TOTAL; //rpm range per led                
            int milSec = DateTime.Now.Millisecond;
            Command rgbShift = null;
            if (rpmPerLed > 0 && currentRpm > firstRpm - 2000)
            {
                if (currentRpm > firstRpm)
                {
                    int numActiveLeds = (int)(Math.Ceiling((currentRpm - firstRpm) / rpmPerLed));

                    if (numActiveLeds < LED_NUM_TOTAL)
                    {
                        Array.Copy(colourPattern, 0, rpmLed, 0, numActiveLeds * 3); //each led colour has 3 bytes

                        rgbShift = new Command(Command.CMD_RGB_SHIFT, rpmLed);
                    }
                    else //blink
                    {
                        /*if (milSec < 50 || (milSec > 100 && milSec < 150) ||
                        (milSec > 200 && milSec < 250) || (milSec > 300 && milSec < 350) ||
                        (milSec > 400 && milSec < 450) || (milSec > 500 && milSec < 550) ||
                        (milSec > 600 && milSec < 650) || (milSec > 700 && milSec < 750) ||
                        (milSec > 800 && milSec < 850) || (milSec > 900))*/
                        if (milSec < 100 || (milSec > 200 && milSec < 300) ||
                        (milSec > 400 && milSec < 500) || (milSec > 600 && milSec < 700) ||
                        (milSec > 800 && milSec < 900) || (milSec > 900))
                        {
                            rgbShift = new Command(Command.CMD_RGB_SHIFT, rpmLed);
                        }
                        else
                        {
                            rgbShift = new Command(Command.CMD_RGB_SHIFT, colourPattern);
                        }
                    }
                } else {
                    //clear shift lights
                    rgbShift = new Command(Command.CMD_RGB_SHIFT, rpmLed);
                }
          
                sm.sendCommand(rgbShift);
            }
        }


        private string formatString(string text, string pattern) 
        {
            string[] index = pattern.Split('=');

            if (index.Length > 1) {
                if (index[0].StartsWith("pl") || index[0].StartsWith("pr"))
                {
                    string[] par = index[1].Split('&');
                    if (par.Length != 2)
                        return text;
                    if(index[0].StartsWith("pl"))
                        return text.PadLeft(Int32.Parse(par[0]), par[1][0]);
                    return text.PadRight(Int32.Parse(par[0]), par[1][0]);
                }
                if (index[0].StartsWith("@"))
                {
                    return Regex.Replace(text, index[0], index[1]);
                }                              
            }

            return String.Format(pattern, text);
        }

        //needs to wait until MainForm 7Segment is loaded
        private void send7SegmentMsg()
        {
            StringBuilder msg = new StringBuilder();
            List<string> _7SegmentData = MainForm.get_7SegmentData();
            string[] strPatterns = MainForm.getStrFormat().Split(Utils.ITEM_SEPARATOR);

            if (_7SegmentData.Count > 0)
            {                
                for (int x = 0; x < _7SegmentData.Count; x++)
                {
                    string name = _7SegmentData.ElementAt(x);
                    string pattern = "{0}";                    

                    if(strPatterns.Length > 0) {
                        if(x < strPatterns.Length)
                            pattern = String.IsNullOrEmpty(strPatterns[x]) ? "{0}" : strPatterns[x];
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

            while (!MainForm.stopThreads)
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

                    sendRPMShiftMsg();
                    send7SegmentMsg();
                }
                else
                {
                    msg.Append("-OFF.");
                    msg.Append(DateTime.Now.ToString("dd.MM.yyyy"));
                    msg.Append(DateTime.Now.ToString("hh.mm.ss.ff"));
                    isConnected = false;

                    byte[] b = Utils.getBytes(msg.ToString());
                    Command c = new Command(Command.CMD_7_SEGS, Utils.convertByteTo7Segment(b, 0));
                    sm.sendCommand(c);
                }

                //c = new Command(Command.CMD_RGB_SHIFT, colourPattern);
                //sm.sendCommand(c);
                await Task.Delay(5);
            }

            wrapper.Stop();
        }


        private string getTelemetryData(string name, string strPattern)
        {
            string value = "";
            if (!String.IsNullOrEmpty(name))
            {
                string[] type = name.Split('.');
                
                if (type.Length == 2) {
                    switch(type[1])
                    {
                        case "int":
                            value = wrapper.GetTelemetryValue<int>(type[0]).Value.ToString();
                            break;
                        case "float":
                            value = ((int)Math.Floor(wrapper.GetTelemetryValue<float>(type[0]).Value)).ToString();
                            break;
                        case "bool":
                            value = wrapper.GetTelemetryValue<bool>(type[0]).Value.ToString();
                            break;
                        case "double":
                            value = wrapper.GetTelemetryValue<double>(type[0]).Value.ToString();
                            break;
                        case "bitfield":
                            value = wrapper.GetTelemetryValue<byte>(type[0]).Value.ToString();
                            break;
                        //todo: add handler for arrays
                        case "kmh":
                            value = ((int)Math.Floor(wrapper.GetTelemetryValue<float>(type[0]).Value * 3.6)).ToString();
                            break;
                        case "time":
                            float seconds = wrapper.GetTelemetryValue<float>(type[0]).Value;
                            TimeSpan interval = TimeSpan.FromSeconds(seconds);
                            value = interval.ToString(@"mm\.ss\.fff");
                            break;
                        case "dtime":
                            double dseconds = wrapper.GetTelemetryValue<double>(type[0]).Value;
                            TimeSpan dinterval = TimeSpan.FromSeconds(dseconds);
                            value = dinterval.ToString(@"mm\.ss\.fff");
                            break;
                    }

                    if (!String.IsNullOrEmpty(value))
                    {                        
                        value = formatString(value, strPattern);
                    }
                }
            }          

            return value;
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

        public void UpdateViewSelected(string s)
        {
            views = s;
        }
    }
}
