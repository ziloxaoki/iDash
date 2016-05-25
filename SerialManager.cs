using System;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace iDash
{
    class SerialManager
    {
        private const int BUFFER_SIZE = 40;
        private const int WAIT_TO_RECONNECT = 300;
        private const int WAIT_SERIAL_CONNECT = 3000;
        private const int WAIT_TO_SEND_SYN_ACK = 2000;
        private const int ARDUINO_TIMED_OUT = 5000;

        private int commandLength;        
        private byte[] serialCommand = new byte[BUFFER_SIZE];
        private SerialPort serialPort = new SerialPort(); //create of serial port
        private static long lastArduinoResponse = 0;        
        private object dataLock = new object();
        private object commandLock = new object();

        public static bool stopThreads = false;
        public static DebugMode debugMode = DebugMode.None;
        public static bool isArduinoInDebugMode = false;

        //events
        public delegate void CommandReceivedHandler(Command command);
        public CommandReceivedHandler CommandReceivedSubscribers;
        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;
        public delegate void DebugMessageHandler(string m);
        public DebugMessageHandler DebugMessageSubscribers;

        public void Init()
        {
            serialPort.Parity = Parity.None;     //selected parity 
            serialPort.StopBits = StopBits.One;  //selected stopbits
            serialPort.DataBits = 8;                           //selected data bits
            serialPort.BaudRate = 19200;                             //selected baudrate            
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);//received even handler  

            new Thread(new ThreadStart(start)).Start();            
            
            this.tryToConnect();

        }

        private bool isArduinoAlive()
        {
            return !Utils.hasTimedOut(lastArduinoResponse, ARDUINO_TIMED_OUT);
        }

        private async void tryToConnect()
        {
            foreach (string port in this.getportnames())
            {                                

                if (serialPort.IsOpen)  //if port is  open 
                {
                    serialPort.Close();  //close port
                }

                serialPort.PortName = port;    //selected name of port
                NotifyStatusMessage("Searching for Arduino at " + port + "...");

                try
                {
                    serialPort.Open();        //open serial port                
                }
                catch
                {
                    Thread.Sleep(WAIT_TO_RECONNECT);        //port is probably closing, wait...
                    serialPort.Open();        //try again
                }

                await Task.Delay(WAIT_SERIAL_CONNECT);

                if (isArduinoAlive())
                {
                    //send ack to arduino
                    this.sendSynAck();
                    NotifyStatusMessage("Arduino found at port " + port + "...");
                    break;
                }

            }  
            if(!isArduinoAlive())
            {
                this.tryToConnect();
            }
        }


        private void start()
        {            
            while (!stopThreads) {
                this.sendArduinoSynAck();
            }
            if (serialPort.IsOpen)
            {
                serialPort.Dispose();
            }
        }

        //if arduino does not receive a SynAck in 5s it will disconnect and start sending ACK commands
        private async void sendArduinoSynAck()
        {            
            if (isArduinoAlive())
            {
                this.sendSynAck();
                await Task.Delay(WAIT_TO_SEND_SYN_ACK);                
            }
        }

        private void sendSynAck()
        {
            Command synack = new Command(Command.CMD_SYN_ACK, new byte[0]);
            sendCommand(synack);
        }        

        private void processCommand(Command command)
        {
            lock(commandLock)
            {
                String type = "undefined";   
                byte c = command.getData()[0];
                switch (c)
                {
                    case Command.CMD_SYN:
                        lastArduinoResponse = Utils.getCurrentTimeMillis();
                        type = "CMD_SYN";
                        break;
                    case Command.CMD_SET_DEBUG_MODE:                        
                        isArduinoInDebugMode = command.getData()[1] == 1;
                        type = "CMD_SET_DEBUG_MODE";
                        break;
                    case Command.CMD_BUTTON_STATUS:                        
                        NotifyCommandReceived(command);
                        type = "CMD_BUTTON_STATUS";
                        break;    
                }
                if (debugMode != DebugMode.None || isArduinoInDebugMode)
                {
                    NotifyDebugMessage(String.Format("Command processed:{0} - ({1})\n", Utils.ByteArrayToString(command.getRawData()), type));
                }
            }
        }      

        
        public string[] getportnames()
        {
            return SerialPort.GetPortNames(); //load all names of  com ports to string 
        }

        public void sendCommand(Command command)
        {
            try
            {
                serialPort.Write(command.getRawData(), 0, command.getLength());                            
            }
            catch
            {
                NotifyStatusMessage("com port is not available"); //if there are not is any COM port in PC show message
            }
        }        
        

        public void processData(byte[] serialData)
        {

            lock (dataLock)
            {

                foreach (byte b in serialData) { 
                    switch (b)
                    {
                        case Command.CMD_INIT:
                            serialCommand[0] = b;
                            commandLength = 1;
                            break;

                        case Command.CMD_END:
                            serialCommand[commandLength] = b;

                            Command command = new Command(serialCommand);

                            if (command != null && command.getLength() > 0)
                            {
                                lock (commandLock)
                                {
                                    processCommand(command);
                                }
                            }
                            commandLength = 0;
                            Utils.resetArray(serialCommand);
                            break;

                        default:
                            if (commandLength > 0)
                            {
                                serialCommand[commandLength++] = b;
                            }
                            if (commandLength == BUFFER_SIZE - 1)
                            {
                                commandLength = 0;
                                Utils.resetArray(serialCommand);
                            }
                            break;
                    }
                }
            }
        }
                

        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte[] data = new byte[serialPort.BytesToRead];
                serialPort.Read(data, 0, data.Length);

                if (debugMode == DebugMode.Verbose) {
                    NotifyDebugMessage(Utils.ByteArrayToString(data)+" ");
                }

                lock (dataLock)
                {
                    if(data.Length > 0)
                    {
                        processData(data);
                    }
                }
            }
        }

        protected virtual void NotifyCommandReceived(Command args)
        {
            CommandReceivedHandler handler = CommandReceivedSubscribers;

            if (handler != null)
            {
                handler(args);
            }
        }

        public void NotifyStatusMessage(string args)
        {
            StatusMessageHandler handler = StatusMessageSubscribers;

            if (handler != null)
            {
                handler(args + "\n");
            }
        }

        public void NotifyDebugMessage(string args)
        {
            DebugMessageHandler handler = DebugMessageSubscribers;

            if (handler != null)
            {
                handler(args);
            }
        }

    }
}
