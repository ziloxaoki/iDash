using F1Speed.Core;
using F1UdpNet;
using System.Reactive;
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

        private int firstRpm = 0;
        private int lastRpm = 0;
        private int currentRpm = 0;
        private int flag = 0;
        private const int TIMERINTERVAL = 10;        // refresh data every 10th of a sec
        private long lastTimeConnected = 0;
        private PacketCarTelemetryData carTelemetry;
        private PacketCarStatusData carStatus;
        private PacketLapData lapData;


        protected override void start()
        {
            //if(manager == null)
            //    manager = new TelemetryLapManager();
            // Set up the socket for collecting game telemetry
            try
            {
                //remoteIP = F1SpeedSettings.AllowConnectionsFromOtherMachines ? new IPEndPoint(IPAddress.Any, PORTNUM) : new IPEndPoint(IPAddress.Parse(IP), PORTNUM);
                //remoteIP = new IPEndPoint(IPAddress.Parse(IP), PORTNUM);
                //udpSocket = new UdpClient(PORTNUM);
                //udpSocket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                //udpSocket.ExclusiveAddressUse = false;
                //udpSocket.Client.Bind(remoteIP);                
                IObserver<IF1Packet> observer = Observer
                    .Create<IF1Packet>(output =>
                    {
                        if (output != null)
                        {
                            if ((e_PacketId)output.Header.m_packetId == e_PacketId.CarStatus)
                            {
                                carStatus = (PacketCarStatusData)output;
                                lastRpm = carStatus.m_carStatusData[carStatus.Header.m_playerCarIndex].m_maxRPM;
                                firstRpm = carStatus.m_carStatusData[carStatus.Header.m_playerCarIndex].m_idleRPM;
                                flag = (int)carStatus.m_carStatusData[carStatus.Header.m_playerCarIndex].m_vehicleFiaFlags;
                            }

                            if ((e_PacketId)output.Header.m_packetId == e_PacketId.LapData)
                            {
                                lapData = (PacketLapData)output;
                            }

                            if ((e_PacketId)output.Header.m_packetId == e_PacketId.CarTelemetry)
                            {
                                carTelemetry = (PacketCarTelemetryData)output;

                                currentRpm = carTelemetry.m_carTelemetryData[carTelemetry.Header.m_playerCarIndex]
                                    .m_engineRPM;

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
                        }


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
                    });
                F1UdpClient.StartRead(new UdpClient(20777)).Subscribe(observer);
                NotifyStatusMessage("Connected to F1 Codemaster. Port 20777");
                while (!CancellationPending)
                {
                    /*if (udpSocket.Available > 0)
                    {
                        // Get the data (this will block until we get a packet)
                        Byte[] receiveBytes = udpSocket.Receive(ref senderIP);

                        // Lock access to the shared struct
                        //syncMutex.WaitOne();

                        // Convert the bytes received to the shared struct
                        latestData = PacketUtilities.ConvertToPacket(receiveBytes);
                        //manager.ProcessIncomingPacket(latestData);

                        // Release the lock again
                        //syncMutex.ReleaseMutex();*/

                    /*currentRpm = float.Parse(manager.GetCurrentData("EngineRevs"));
                    firstRpm = 1.10f * float.Parse(manager.GetCurrentData("IdleRpm"));
                    lastRpm = 0.90f * float.Parse(manager.GetCurrentData("MaxRpm"));*/

                    /*     currentRpm = latestData.EngineRevs;
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

                     Thread.Sleep(TIMERINTERVAL);*/
                }
            }
            catch (Exception e)
            {
                logger.LogMessageToFile(e.ToString(), true);
                // throw;
            }            
        }

        private string getValue(string name, string type, object clazz)
        {
            string result = "";

            Type pType = clazz.GetType();

            System.Reflection.FieldInfo field = pType.GetField(name);

            if (field != null)
            {
                try
                {
                    switch (type)
                    {
                        case "time":
                            float seconds = 0;

                            if (field.FieldType.Name.Equals("Double"))
                            {
                                seconds = Convert.ToSingle(field.GetValue(clazz));
                            }
                            TimeSpan interval = TimeSpan.FromSeconds(seconds);
                            result = interval.ToString(@"mm\.ss\.fff");
                            break;
                        case "kmh":
                            result = ((ushort)field.GetValue(clazz)).ToString();
                            break;
                        default:
                            if (name.Equals("m_gear"))
                            {
                                int gear = (sbyte)field.GetValue(clazz) & 0xff;
                                if (gear == 255)
                                {
                                    return "R";
                                }

                                result = (gear).ToString();
                            }
                            else
                            {
                                result = field.GetValue(clazz).ToString();
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    logger.LogExceptionToFile(e);
                }
            }

            return result;
        }

        protected override string getTelemetryValue(string name, string type, string clazz)
        {
            string result = "";

            if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(clazz))
            {
                switch (clazz)
                {
                    case "CarTelemetryData":
                        result = getValue(name, type, carTelemetry.m_carTelemetryData[carTelemetry.Header.m_playerCarIndex]);
                        break;
                    case "CarStatusData":
                        result = getValue(name, type, carStatus.m_carStatusData[carTelemetry.Header.m_playerCarIndex]);
                        break;
                    case "LapData":
                        result = getValue(name, type, lapData.m_lapData[carTelemetry.Header.m_playerCarIndex]);
                        break;
                }
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
                }
                // Release unmanaged resources.
                disposed = true;
            }
        }

        ~F1Connector() { Dispose(false); }
    }
}
