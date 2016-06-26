using iRSDKSharp;
using iRacingSdkWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Management;

namespace iDash
{
    class IRacingConnector
    {
        private SerialManager sm;
        private string views;

        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;
        // Globally declared SdkWrapper object
        private readonly SdkWrapper wrapper;

        public IRacingConnector(SerialManager sm)
        {
            this.sm = sm;

            // Create instance
            wrapper = new SdkWrapper();
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
            byte[] rpm = { 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 1, 1, 255, 1, 1, 255, 1, 1, 255 };
            while (!MainForm.stopThreads)
            {
                msg.Clear();
                if (wrapper.IsConnected)
                {
                    NotifyStatusMessage("Connected to iRacing.");
                }
                else
                {
                    msg.Append("-OFF.");
                    msg.Append(DateTime.Now.ToString("dd.MM.yyyy"));
                    msg.Append(DateTime.Now.ToString("hh.mm.ss.ff"));                                        
                }

                byte[] b = Utils.getBytes(msg.ToString());
                Command c = new Command(Command.CMD_7_SEGS, Utils.convertByteTo7Segment(b, 0));
                sm.sendCommand(c);
                //c = new Command(Command.CMD_RGB_SHIFT, rpm);
                //sm.sendCommand(c);
                await Task.Delay(10);
            }

            wrapper.Stop();
        }

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            YamlQuery yq = e.SessionInfo["DriverInfo"]["DriverCarSLBlinkRPM"];
            if (yq != null)
            {
                string shift = yq.Value;
            }
        }

        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            string rpm = e.TelemetryInfo.RPM.Value.ToString();
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
