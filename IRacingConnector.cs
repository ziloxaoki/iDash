using iRSDKSharp;
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
        private iRacingSDK sdk;
        private string views;

        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;

        public IRacingConnector(SerialManager sm)
        {
            this.sm = sm;
            sdk = new iRacingSDK();
            new Thread(new ThreadStart(start)).Start();
        }

        private async void start()
        {
            StringBuilder msg = new StringBuilder();
            while (!MainForm.stopThreads)
            {
                msg.Clear();
                if (sdk.IsConnected())
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
                Command c = new Command((byte)'B', Utils.convertByteTo7Segment(b, 0));
                sm.sendCommand(c);
                await Task.Delay(10);
            }
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
