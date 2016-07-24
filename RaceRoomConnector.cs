using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using iDash.Data;
using System.IO;
using System.Threading;
using System.Reflection;

namespace iDash
{
    public class RaceRoomConnector : ISimConnector
    {        
        private bool Mapped
        {
            get { return (_file != null && _view != null); }
        }

        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;

        private MemoryMappedFile _file;
        private MemoryMappedViewAccessor _view;

        private float firstRpm = 0;
        private float lastRpm = 0;
        private float currentRpm = 0;

        private bool disposed = false;

        public RaceRoomConnector(SerialManager sm) : base(sm)
        {
            this.sm = sm;

            new Thread(new ThreadStart(start)).Start();
        }        

        public async void start()
        {
            StringBuilder msg = new StringBuilder();
            bool isConnected = false;

            NotifyStatusMessage("Looking for RRRE.exe...");

            while (!MainForm.stopThreads && !MainForm.stopRaceRoomThreads)
            {
                msg.Clear();

                if (Utils.isRrreRunning())
                {
                    if (!Mapped)
                    {
                        if (!isConnected)
                        {
                            string s = DateTime.Now.ToString("hh:mm:ss") + ": Connected to RaceRoom.";
                            NotifyStatusMessage(s);
                            isConnected = true;

                            new Thread(new ThreadStart(Map)).Start();
                        }

                        sm.sendCommand(Utils.getDisconnectedMsgCmd());
                    }
                    else
                    {

                        Shared data;
                        _view.Read(0, out data);

                        int sessionType = data.SessionType;

                        if (sessionType >= 0)
                        {
                            lastRpm = RpsToRpm(data.MaxEngineRps);
                            firstRpm = FIRST_RPM * lastRpm;
                            currentRpm = RpsToRpm(data.EngineRps);

                            sendRPMShiftMsg(currentRpm, firstRpm, lastRpm);
                            send7SegmentMsg(data);
                        }
                        else
                        {
                            sm.sendCommand(Utils.getDisconnectedMsgCmd());
                        }
                    }
                }
                else
                {
                    sm.sendCommand(Utils.getDisconnectedMsgCmd());
                }

                await Task.Delay(5);
            }

            Dispose();
        }

        

        private async void Map()
        {
            while (!Mapped)
            {
                try
                {
                    _file = MemoryMappedFile.OpenExisting(Constant.SharedMemoryName);
                    _view = _file.CreateViewAccessor(0, Marshal.SizeOf(typeof(Shared)));
                    NotifyStatusMessage("Memory mapped successfully");
                }
                catch (FileNotFoundException)
                {
                }
                await Task.Delay(1);
            }
        }        

        private string getTelemetryValue(string name, string type, Shared data)
        {
            String result = "";

            //retrieve field by name
            FieldInfo prop = data.GetType().GetField(name);

            if (prop != null)
            {
                //pType = real field type
                string pType = prop.FieldType.Name;
                //type = expected field type, has to match to pType otherwise abort
                try
                {
                    switch (type)
                    {
                        case "Int32":
                            if (type.Equals(pType))
                                result = ((int)prop.GetValue(data)).ToString();
                            break;
                        case "Single":
                            if (type.Equals(pType))
                                result = ((Single)prop.GetValue(data)).ToString();
                            break;
                        case "kmh":
                            if (pType.Equals("Single"))
                                result = ((int)Math.Floor((Single)prop.GetValue(data) * 3.6)).ToString();
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
                catch { }              
            }

            return result;
        }

        private string getTelemetryData(string name, string strPattern, Shared data)
        {

            string result = "";

            if (!String.IsNullOrEmpty(name))
            {
                string[] type = name.Split('.');

                if (type.Length > 1)
                {
                    //type[0] = property name, type[1] = property type
                    result = this.getTelemetryValue(type[0], type[1], data);    
                }

                if (!String.IsNullOrEmpty(result))
                {
                    result = Utils.formatString(result, strPattern);
                }
            }

            return result;
        }

        //needs to wait until MainForm 7Segment is loaded
        private void send7SegmentMsg(Shared data)
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

                    if (strPatterns.Length > 0)
                    {
                        if (x < strPatterns.Length)
                            pattern = string.IsNullOrEmpty(strPatterns[x]) ? "{0}" : strPatterns[x];
                    }

                    msg.Append(this.getTelemetryData(name, pattern, data));
                }

                if (msg.Length > 0)
                {
                    byte[] b = Utils.getBytes(msg.ToString());
                    Command c = new Command(Command.CMD_7_SEGS, Utils.convertByteTo7Segment(b, 0));
                    sm.sendCommand(c);
                }
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

        private Single RpsToRpm(Single rps)
        {
            return rps * (60 / (2 * (Single)Math.PI));
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
                    if (_view != null)
                        _view.Dispose();
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
