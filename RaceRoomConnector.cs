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
    public class RaceRoomConnector : IDisposable
    {        
        private bool Mapped
        {
            get { return (_file != null && _view != null); }
        }

        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;

        private SerialManager sm;
        private MemoryMappedFile _file;
        private MemoryMappedViewAccessor _view;

        public RaceRoomConnector(SerialManager sm)
        {
            this.sm = sm;

            new Thread(new ThreadStart(start)).Start();
        }

        public void Dispose()
        {
            _view.Dispose();
            _file.Dispose();
        }        

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

                        if (Map())
                        {
                            Console.WriteLine("Memory mapped successfully");
                        }
                    }
                    else
                    {

                        Shared data;
                        _view.Read(0, out data);

                        if (data.Gear >= -1)
                        {
                            Console.WriteLine("Gear: {0}", data.Gear);
                        }

                        if (data.EngineRps > -1.0f)
                        {
                            //Console.WriteLine("RPM: {0}", Utilities.RpsToRpm(data.EngineRps));
                            //Console.WriteLine("Speed: {0}", Utilities.MpsToKph(data.CarSpeed));
                        }
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
    }
}
