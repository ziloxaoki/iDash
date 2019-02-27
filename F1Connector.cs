using F1Speed.Core;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace iDash
{
    public class F1Connector : ISimConnector
    {
        private bool disposed = false;

        private Logger logger = new Logger();

        private float firstRpm = 0;
        private float lastRpm = 0;
        private float currentRpm = 0;
        private int flag = 0;

        // Constants
        private const int PORTNUM = 20777;
        private const string IP = "127.0.0.1";
        private const int TIMERINTERVAL = 10;        // refresh data every 10th of a sec

        // This is the IP endpoint we are connecting to (i.e. the IP Address and Port F1 2012 is sending to)
        //private IPEndPoint remoteIP = new IPEndPoint(IPAddress.Parse(IP), PORTNUM);        
        private IPEndPoint remoteIP;
        // This is the IP endpoint for capturing who actually sent the data
        private IPEndPoint senderIP = new IPEndPoint(IPAddress.Any, 0);
        // UDP Socket for the connection
        private UdpClient udpSocket;

        private long lastTimeConnected = 0;

        // Holds the latest data captured from the game
        TelemetryPacket latestData;

        // Mutex used to protect latestData from simultaneous access by both threads
        static Mutex syncMutex = new Mutex();

        private TelemetryLapManager manager;

        private System.Windows.Forms.Timer timer1;

        protected override void start()
        {
            //if(manager == null)
            //    manager = new TelemetryLapManager();
            // Set up the socket for collecting game telemetry
            try
            {
                //remoteIP = F1SpeedSettings.AllowConnectionsFromOtherMachines ? new IPEndPoint(IPAddress.Any, PORTNUM) : new IPEndPoint(IPAddress.Parse(IP), PORTNUM);
                //remoteIP = new IPEndPoint(IPAddress.Parse(IP), PORTNUM);
                udpSocket = new UdpClient(PORTNUM);
                //udpSocket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                //udpSocket.ExclusiveAddressUse = false;
                //udpSocket.Client.Bind(remoteIP);
                NotifyStatusMessage("Connected to F1 Codemaster. " + IP + ":" + PORTNUM.ToString());
                while (!CancellationPending)
                {
                    if (udpSocket.Available > 0)
                    {
                        // Get the data (this will block until we get a packet)
                        Byte[] receiveBytes = udpSocket.Receive(ref senderIP);

                        // Lock access to the shared struct
                        //syncMutex.WaitOne();

                        // Convert the bytes received to the shared struct
                        latestData = PacketUtilities.ConvertToPacket(receiveBytes);
                        //manager.ProcessIncomingPacket(latestData);

                        // Release the lock again
                        //syncMutex.ReleaseMutex();

                        /*currentRpm = float.Parse(manager.GetCurrentData("EngineRevs"));
                        firstRpm = 1.10f * float.Parse(manager.GetCurrentData("IdleRpm"));
                        lastRpm = 0.90f * float.Parse(manager.GetCurrentData("MaxRpm"));*/

                        currentRpm = latestData.EngineRevs;
                        firstRpm = 1.10f * latestData.IdleRpm;
                        lastRpm = 0.90f * latestData.MaxRpm;

                        sendRPMShiftMsg(currentRpm, firstRpm, lastRpm, flag);

                        foreach (SerialManager serialManager in sm)
                        {
                            if (serialManager.deviceContains7Segments())
                            {
                                send7SegmentMsg();
                            }
                        }

                        lastTimeConnected = Utils.getCurrentTimeMillis();
                    }
                    else
                    {
                        if (Utils.getCurrentTimeMillis() - lastTimeConnected > 1000)
                        {
                            foreach (SerialManager serialManager in sm)
                            {
                                if (serialManager.deviceContains7Segments())
                                {
                                    serialManager.enqueueCommand(Utils.getDisconnectedMsgCmd(), false);
                                    serialManager.enqueueCommand(Utils.getBlackRPMCmd(), false);
                                }
                            }

                            //race session hasn't started or game paused
                            lastTimeConnected = 0;
                        }
                    }

                    Thread.Sleep(TIMERINTERVAL);
                }
            }
            catch (Exception e)
            {
                logger.LogMessageToFile(e.ToString(), true);
                // throw;
            }            
        }

        public string GetCurrentData(string fieldName)
        {
            if (!string.IsNullOrWhiteSpace(fieldName))
            {
                var type = typeof(TelemetryPacket);
                var value = type.GetFields().First(x => x.Name == fieldName).GetValue(latestData);

                return value.ToString();
            }
            return string.Empty;
        }

        // This method runs continously in the data collection thread.  It
        // waits to receive UDP packets from the game, converts them and writes
        // them to the shared struct variable.
        private void FetchData()
        {
            
        }

        protected override string getTelemetryValue(string name, string type, string clazz)
        {
            string result = "";
            try
            {
                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(type))
                {
                    switch (type)
                    {
                        case "float":
                            switch (name) {
                                //case "FastestLap":
                                //    result = manager.FastestLap.ToString();
                                //    break;                                    
                                default:
                                    result = GetCurrentData(name);
                                    break;
                            }             
                            break;
                        case "int":
                            //all value are returned as float, by manager, so it needs to be converted
                            int value = (int)(float.Parse(GetCurrentData(name), CultureInfo.InvariantCulture.NumberFormat));
                            switch (name)
                            {
                                case "Gear":
                                    value -= 1;
                                    break;
                            }
                            result = value.ToString();
                            break;
                        case "time":
                            float seconds = float.Parse(GetCurrentData(name), CultureInfo.InvariantCulture.NumberFormat);
                            TimeSpan interval = TimeSpan.FromSeconds(seconds);
                            result = interval.ToString(@"mm\.ss\.fff");
                            break;
                        case "kmh":
                            int speed = (int)(float.Parse(GetCurrentData(name), CultureInfo.InvariantCulture.NumberFormat) * (float)3.6);
                            result = speed.ToString();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogExceptionToFile(e);
            }
            return result;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if(udpSocket != null)
                    {
                        udpSocket.Close();
                    }
                }
                // Release unmanaged resources.
                disposed = true;
            }
        }

        ~F1Connector() { Dispose(false); }
    }
}
