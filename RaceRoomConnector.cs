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
                    if(_view != null)
                        _view.Dispose();
                    if (_file != null)
                        _file.Dispose();
                }
                // Release unmanaged resources.
                disposed = true;
            }
        }

        ~RaceRoomConnector() { Dispose(false); }

        public void start()
        {
            StringBuilder msg = new StringBuilder();

            NotifyStatusMessage("Looking for RRRE.exe...");

            while (!MainForm.stopThreads && !MainForm.stopRaceRoomThreads)
            {
                msg.Clear();
                if (Utils.isRrreRunning())
                {
                    if (!Mapped)
                    {
                        string s = DateTime.Now.ToString("hh:mm:ss") + ": Connected to RaceRoom.";
                        NotifyStatusMessage(s);
                    }
                    else
                    {

                        Shared data;
                        _view.Read(0, out data);

                        lastRpm = RpsToRpm(data.MaxEngineRps);
                        firstRpm = FIRST_RPM * lastRpm;
                        currentRpm = RpsToRpm(data.EngineRps);

                        sendRPMShiftMsg(currentRpm, firstRpm, lastRpm);

                        /*if (data.Gear >= -1)
                        {
                            Console.WriteLine("Gear: {0}", data.Gear);
                        }

                        if (data.EngineRps > -1.0f)
                        {
                            //Console.WriteLine("RPM: {0}", Utilities.RpsToRpm(data.EngineRps));
                            //Console.WriteLine("Speed: {0}", Utilities.MpsToKph(data.CarSpeed));
                        }*/
                    }
                }
                else
                {
                    msg.Append("-OFF.");
                    msg.Append(DateTime.Now.ToString("dd.MM.yyyy"));
                    msg.Append(DateTime.Now.ToString("hh.mm.ss.ff"));

                    byte[] b = Utils.getBytes(msg.ToString());
                    Command c = new Command(Command.CMD_7_SEGS, Utils.convertByteTo7Segment(b, 0));
                    sm.sendCommand(c);
                }
            }

            Dispose();
        }

        private bool Map()
        {
            try
            {
                _file = MemoryMappedFile.OpenExisting(Constant.SharedMemoryName);
                _view = _file.CreateViewAccessor(0, Marshal.SizeOf(typeof(Shared)));
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
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
    }
}
