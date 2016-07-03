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

namespace iDash
{
    class IRacingConnector
    {
        private SerialManager sm;
        private string views;
        private byte[] colourPattern = { 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 1, 1, 255, 1, 1, 255, 1, 1, 255 };
        private float maxRpm = 0;
        private float rpm = 0;
        private const int BLINK = 18; 
        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;
        // Globally declared SdkWrapper object
        private readonly SdkWrapper wrapper;

        public IRacingConnector(SerialManager sm)
        {
            this.sm = sm;

            // Create instance
            wrapper = new SdkWrapper();
            wrapper.TelemetryUpdateFrequency = 60;
            // Listen to events
            wrapper.TelemetryUpdated += OnTelemetryUpdated;
            wrapper.SessionInfoUpdated += OnSessionInfoUpdated;
            // Start it if Arduino is Connected
            wrapper.Start();

            new Thread(new ThreadStart(start)).Start();
        }

        private void sendRPMShiftMsg()
        {
            float minRpm = 0.1f * maxRpm;
            float rpmPerLed = (maxRpm - minRpm) / (BLINK); //16 is the number of rgb leds
            byte[] rpmLed = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int milSec = DateTime.Now.Millisecond;
            Command rgbShift = null;
            if (rpmPerLed > 0 && rpm > minRpm)
            {
                int numLeds = (int)((rpm - minRpm) / rpmPerLed);

                if (numLeds < BLINK - 1)
                {
                    Array.Copy(colourPattern, 0, rpmLed, 0, numLeds * 3); //each led colour has 3 bytes

                    rgbShift = new Command(Command.CMD_RGB_SHIFT, rpmLed);                    
                }
                else //blink
                {
                    if (milSec < 50 || (milSec > 100 && milSec < 150) || 
                        (milSec > 200 && milSec < 250) || (milSec > 300 && milSec < 350) ||
                        (milSec > 400 && milSec < 450) || (milSec > 500 && milSec < 550) ||
                        (milSec > 600 && milSec < 650) || (milSec > 700 && milSec < 750) ||
                        (milSec > 800 && milSec < 850) || (milSec > 900))
                    {
                        rgbShift = new Command(Command.CMD_RGB_SHIFT, rpmLed);
                    }
                    else
                    {
                        rgbShift = new Command(Command.CMD_RGB_SHIFT, colourPattern);
                    }
                }

                sm.sendCommand(rgbShift);
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

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            YamlQuery yq = e.SessionInfo["DriverInfo"]["DriverCarSLBlinkRPM"];
            if (yq != null)
            {
                maxRpm = float.Parse(yq.Value, CultureInfo.InvariantCulture.NumberFormat);
                //Logger.LogMessageToFile("Shift:" + maxRpm + "\n");
            }
        }

        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            rpm = e.TelemetryInfo.RPM.Value;
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
