using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iDash
{
    class RFactorConnector : ISimConnector
    {
        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;

        public Boolean running = false;
        private Object memoryMappedFileStruct;
        private float firstRpm = 0;
        private float lastRpm = 0;
        private float currentRpm = 0;
        public static GameDefinition gameDefinition;
        private Boolean mapped = false;
        private RFactorDataReader gameDataReader;
        private RF1SharedMemoryReader.RF1StructWrapper wrapper;

        private bool disposed = false;

        public RFactorConnector(SerialManager sm) : base(sm)
        {
            this.sm = sm;

            new Thread(new ThreadStart(start)).Start();
        }

        public async void start()
        {
            gameDataReader = new RF1SharedMemoryReader();

            Object rawGameData;
            NotifyStatusMessage("Waiting for RFactor...");

            while (!MainForm.stopThreads && !MainForm.stopRFactorThreads)
            {
                if (Utils.IsGameRunning(GameDefinition.automobilista.processName))
                {
                    if (!mapped)
                    {
                        mapped = gameDataReader.Initialise();
                    }
                    else
                    {
                        rawGameData = gameDataReader.ReadGameData();
                        wrapper = (RF1SharedMemoryReader.RF1StructWrapper)rawGameData;
                        if (wrapper.data.numVehicles > 0)
                        {
                            lastRpm = wrapper.data.engineMaxRPM;
                            firstRpm = FIRST_RPM * lastRpm;
                            currentRpm = wrapper.data.engineRPM;

                            sendRPMShiftMsg(currentRpm, firstRpm, lastRpm);
                            //send7SegmentMsg();
                            
                        }
                        else
                        {
                            sm.sendCommand(Utils.getDisconnectedMsgCmd(), false);
                        }
                    }

                    await Task.Delay(5);
                }
                else
                {
                    await Task.Delay(2000);
                }
            }            
            
            Dispose();
        }

        protected override string getTelemetryValue(string name, string type, string clazz)
        {
            String result = "";

            if (wrapper != null)
            {
                //retrieve field by name
                FieldInfo prop = wrapper.GetType().GetField(name);

                if (prop != null && !String.IsNullOrEmpty(type))
                {
                    //pType = real field type
                    string pType = prop.FieldType.Name;
                    //type = expected field type, has to match to pType otherwise abort
                    try
                    {
                        switch (type)
                        {
                            case "int":
                                if (type.Equals(pType))
                                {
                                    int val = ((int)prop.GetValue(wrapper));
                                    if (name.Equals("gear", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        //return reverse symbol
                                        if (val < 0) return "R";
                                    }
                                    result = val.ToString();
                                }
                                break;
                            case "float":
                                if (type.Equals(pType))
                                {
                                    result = ((Single)prop.GetValue(wrapper)).ToString();
                                }
                                break;
                            case "kmh":
                                if (pType.Equals("Single"))
                                {
                                    result = ((int)Math.Floor((Single)prop.GetValue(wrapper) * 3.6)).ToString();
                                }
                                break;
                            case "time":
                                if (pType.Equals("Single"))
                                {
                                    float seconds = (Single)prop.GetValue(wrapper);
                                    TimeSpan interval = TimeSpan.FromSeconds(seconds);
                                    result = interval.ToString(@"mm\.ss\.fff");
                                }
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogExceptionToFile(e);
                    }
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
                    if (gameDataReader != null)
                    {
                        gameDataReader.Dispose();
                    }
                }
                // Release unmanaged resources.
                disposed = true;
            }
        }

        ~RFactorConnector() { Dispose(false); }
    }
}
