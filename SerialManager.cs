using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace iDash
{
    class SerialManager
    {        
        private int commandLength;
        private const int BUFFER_SIZE = 40;
        private byte[] serialCommand = new byte[BUFFER_SIZE];
        private SerialPort serialPort = new SerialPort(); //create of serial port
        private static bool isArduinoConnected = false;        
        private object dataLock = new object();
        private object commandLock = new object();
        private double lastSynAck = 0;

        public static bool stopThreads = false;

        //CommandHandlers
        ButtonHandler bh;

        public SerialManager()
        {
            Debug.Print("Init serial...");
        }

        public void Init()
        {
            serialPort.Parity = Parity.None;     //selected parity 
            serialPort.StopBits = StopBits.One;  //selected stopbits
            serialPort.DataBits = 8;                           //selected data bits
            serialPort.BaudRate = 19200;                             //selected baudrate            
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);//received even handler  

            //new Thread(new ThreadStart(processData)).Start();
            new Thread(new ThreadStart(start)).Start();
            //new Thread(new ThreadStart(ConsumeCommand)).Start();

            bh = new ButtonHandler(this);

            foreach (string port in this.getportnames())
            {
                this.tryToConnect(port);

                this.waitForArduinoResponse();

                if (isArduinoConnected)
                {
                    //send ack to arduino
                    this.sendSynAck();
                    Debug.Print("Arduino found at port " + port + "...");
                    break;
                }

            }            

        }


        private void tryToConnect(string port)
        {
            if (serialPort.IsOpen)  //if port is  open 
            {
                serialPort.Close();  //close port
            }
            serialPort.PortName = port;    //selected name of port
            Debug.Print("Connecting to " + port + "...");
            try
            {
                serialPort.Open();        //open serial port                
            }
            catch
            {
                Thread.Sleep(300);        //port is probably closing, wait...
                serialPort.Open();        //try again
            }
        }


        private void waitForArduinoResponse()
        {
            long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            while (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - currentTime < 2000 && !isArduinoConnected)
            {
                Thread.Sleep(100);
            }
        }

        private void start()
        {
            long currentTime = 0;

            while (!stopThreads) {
                if (isArduinoConnected)
                {
                    isArduinoConnected = false;
                    currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    if (currentTime - lastSynAck > 2000)
                    {
                        this.sendSynAck();
                        lastSynAck = currentTime;
                    }
                }
            }
            if (serialPort.IsOpen)
            {
                serialPort.Dispose();
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
                Debug.Print("Command processed:" + Utils.ByteArrayToString(command.getRawData()));
                byte c = command.getData()[0];
                switch (c)
                {
                    case Command.CMD_SYN:
                        isArduinoConnected = true;
                        break;
                    case Command.CMD_BUTTON_STATUS:
                        bh.executeCommand(command);
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
                Debug.Print("com port is not available"); //if there are not is any COM port in PC show message
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
                Debug.Print("Raw Data Received: " + Utils.ByteArrayToString(data));     
                lock(dataLock)
                {
                    if(data.Length > 0)
                    {
                        processData(data);
                    }
                }
            }
        }

    }

}
