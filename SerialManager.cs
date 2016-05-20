using System;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace iDash
{
    class SerialManager
    {

        private const int BUFFER_SIZE = 40;
        private const int WAIT_TO_RECONNECT = 300;
        private const int WAIT_SERIAL_CONNECT = 1000;
        private const int WAIT_TO_SEND_SYN_ACK = 2000;
        private const int ARDUINO_TIMED_OUT = 5000;

        private int commandLength;        
        private byte[] serialCommand = new byte[BUFFER_SIZE];
        private SerialPort serialPort = new SerialPort(); //create of serial port
        private static long lastArduinoResponse = 0;        
        private object dataLock = new object();
        private object commandLock = new object();

        public static bool stopThreads = false;

        //events
        public delegate void CommandReceivedHandler(Command command);
        public CommandReceivedHandler CommandReceivedSubscribers;
        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;        


        public SerialManager()
        {
            NotifyStatusMessage("Starting...");            
        }

        public async void Init()
        {
            serialPort.Parity = Parity.None;     //selected parity 
            serialPort.StopBits = StopBits.One;  //selected stopbits
            serialPort.DataBits = 8;                           //selected data bits
            serialPort.BaudRate = 19200;                             //selected baudrate            
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);//received even handler  

            new Thread(new ThreadStart(start)).Start();

            //bh = new ButtonHandler(this);

            foreach (string port in this.getportnames())
            {
                this.tryToConnect(port);

                await Utils.WaitWithoutBlocking(WAIT_SERIAL_CONNECT);

                if (isArduinoAlive())
                {
                    //send ack to arduino
                    this.sendSynAck();
                    NotifyStatusMessage("Arduino found at port " + port + "...");
                    break;
                }

            }            

        }

        private bool isArduinoAlive()
        {
            return !Utils.hasTimedOut(lastArduinoResponse, ARDUINO_TIMED_OUT);
        }

        private void tryToConnect(string port)
        {
            if (serialPort.IsOpen)  //if port is  open 
            {
                serialPort.Close();  //close port
            }
            serialPort.PortName = port;    //selected name of port
            NotifyStatusMessage("Connecting to " + port + "...");
            try
            {
                serialPort.Open();        //open serial port                
            }
            catch
            {
                Thread.Sleep(WAIT_TO_RECONNECT);        //port is probably closing, wait...
                serialPort.Open();        //try again
            }
        }


        private void start()
        {            
            while (!stopThreads) {
                this.tellArduinoWeAreAlive();
            }
            if (serialPort.IsOpen)
            {
                serialPort.Dispose();
            }
        }


        private async void tellArduinoWeAreAlive()
        {
            if (isArduinoAlive())
            {
                this.sendSynAck();
                await Utils.WaitWithoutBlocking(WAIT_TO_SEND_SYN_ACK);                
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
                //Debug.Print("Command processed:" + Utils.ByteArrayToString(command.getRawData()));
                byte c = command.getData()[0];
                switch (c)
                {
                    case Command.CMD_SYN:
                        lastArduinoResponse = Utils.getCurrentTimeMillis();
                        break;
                    case Command.COMMAND_DEBUG:
                        NotifyStatusMessage(Utils.byteArrayToStr(command.getRawData()));
                        break;
                    case Command.CMD_BUTTON_STATUS:                        
                        NotifyCommandReceived(command);
                        break;    
                }
            }
        }      

        
        public string[] getportnames()
        {
            return SerialPort.GetPortNames(); //load all names of  com ports to string 
        }

        public void sendCommand(Command command)
        {
            //Debug.Print("Sending:" + Utils.ByteArrayToString(command.getRawData()));
            try
            {
                serialPort.Write(System.Text.Encoding.Default.GetString(command.getRawData())); //write string  to serial port from rich text box                               
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
                        case Command.COMMAND_INIT:
                            serialCommand[0] = b;
                            commandLength = 1;
                            break;

                        case Command.COMMAND_END:
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
                //Debug.Print("Raw Data Received: " + Utils.ByteArrayToString(data));     
                lock(dataLock)
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

    }
}
