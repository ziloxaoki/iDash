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
        private Shared data;

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

                            //new Thread(new ThreadStart(Map)).Start();
                            Map();
                        }

                        sm.sendCommand(Utils.getDisconnectedMsgCmd());
                    }
                    else
                    {                        
                        _view.Read(0, out data);

                        int sessionType = data.SessionType;

                        if (sessionType >= 0)
                        {
                            lastRpm = RpsToRpm(data.MaxEngineRps);
                            firstRpm = FIRST_RPM * lastRpm;
                            currentRpm = RpsToRpm(data.EngineRps);

                            sendRPMShiftMsg(currentRpm, firstRpm, lastRpm);
                            send7SegmentMsg();
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

        private bool Map()
        {
            try
            {
                _file = MemoryMappedFile.OpenExisting(Constant.SharedMemoryName);
                _view = _file.CreateViewAccessor(0, Marshal.SizeOf(typeof(Shared)));
                NotifyStatusMessage("Memory mapped successfully");
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }            
        }

        /*private async void Map()
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
        }*/

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
                    Logger.LogMessageToFile(e.Source + ": " + e.Message);
                }
            }

            return result;
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
