using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iDash
{
    public class SerialManager : IDisposable
    {

        int bs = 0;

        private const int BUFFER_SIZE = 40;
        private const int WAIT_TO_RECONNECT = 1000;        
        //arduino will wait 5 secs for a SYN ACK        
        private const int ARDUINO_TIMED_OUT = 1000;
        private const int WAIT_SERIAL_CONNECT = 1000;
        private const int WAIT_TO_SEND_SYN_ACK = ARDUINO_TIMED_OUT / 5;

        private int commandLength;        
        private byte[] serialCommand = new byte[BUFFER_SIZE];
        private SerialPort serialPort = new SerialPort(); //create of serial port
        private static long lastArduinoResponse = 0;        
        private object dataLock = new object();
        private object sendLock = new object();

        public static DebugMode debugMode = DebugMode.None;
        public static int arduinoInDebugMode = 0;        
        public bool isDisabledSerial = false;
        private long lastMessageLogged = 0;

        //events
        public delegate void CommandReceivedHandler(Command command);
        public CommandReceivedHandler CommandReceivedSubscribers;
        public delegate void StatusMessageHandler(string m);
        public StatusMessageHandler StatusMessageSubscribers;
        public delegate void DebugMessageHandler(string m);
        public DebugMessageHandler DebugMessageSubscribers;        

        private bool disposed = false;

        public void Init()
        {
            serialPort.Parity = Parity.None;     //selected parity 
            serialPort.StopBits = StopBits.One;  //selected stopbits
            serialPort.DataBits = 8;             //selected data bits
            serialPort.BaudRate = 38400;         //selected baudrate            
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);//received even handler                         
            
            this.tryToConnect();

            //timer1 = new System.Timers.Timer(1000);
            //timer1.Elapsed += timer1_Tick;
            // Set it to go off every five seconds
            // And start it        
            //timer1.Enabled = true;

        }

        private bool isArduinoAlive()
        {
            return !Utils.hasTimedOut(lastArduinoResponse, ARDUINO_TIMED_OUT);
        }

        private async void tryToConnect()
        {            

            while (!MainForm.stopThreads)
            {
                if (isArduinoAlive())
                {
                    //if arduino does not receive a SynAck in 5s it will disconnect and start sending ACK commands
                    if (!Utils.hasTimedOut(lastArduinoResponse, WAIT_TO_SEND_SYN_ACK))
                    {
                        this.sendSynAck();
                    }
                    await Task.Delay(WAIT_TO_SEND_SYN_ACK);
                }
                else
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

                        //check if connected
                        sendSynAck();

                        //wait for arduino ACK message
                        await Task.Delay(WAIT_SERIAL_CONNECT);

                        if (isArduinoAlive())
                        {
                            NotifyStatusMessage("Arduino found at port " + port + "...");
                            break;
                        }
                    }
                }
            }

        }


        /*private async void start()
        {            
            while (!MainForm.stopThreads) {
                if (isArduinoAlive())
                {
                    //if arduino does not receive a SynAck in 5s it will disconnect and start sending ACK commands
                    this.sendSynAck();
                    await Task.Delay(WAIT_TO_SEND_SYN_ACK, false);
                }
                else
                {
                    tryToConnect();
                    //wait for arduino ACK message
                    await Task.Delay(WAIT_SERIAL_CONNECT);
                }                     
            }

            Dispose();
        }*/

        private void sendSynAck()
        {
            Command synack = new Command(Command.CMD_SYN_ACK, new byte[0]);
            sendCommand(synack, true);
        }        

        private string processCommand(Command command)
        {
            //called by processData that is already sync
            String type = "undefined";
            try {                
                byte c = command.getData()[0];
                switch (c)
                {
                    //ACK message sent by Arduino
                    case Command.CMD_SYN:
                        type = "CMD_SYN";
                        break;
                    //Arduino response to a set debug mode message
                    case Command.CMD_RESPONSE_SET_DEBUG_MODE:
                        arduinoInDebugMode = command.getData()[1];
                        type = "CMD_RESPONSE_SET_DEBUG_MODE";                        
                        break;
                    //Arduino buttons state message
                    case Command.CMD_BUTTON_STATUS:                            
                        NotifyCommandReceived(command);
                        type = "CMD_BUTTON_STATUS";
                        break;
                    //Arduino response when crc command failed
                    case Command.CMD_INVALID:
                        type = "CMD_INVALID";
                        break;
                }
                if (debugMode == DebugMode.Default) {
                    if (isDisabledSerial) {
                        if (command.getRawData()[0] == Command.CMD_INIT_DEBUG) {
                            NotifyDebugMessage(String.Format("Command processed:{0} - ({1})\n", Utils.byteArrayToString(command.getRawData()), type));
                            lastMessageLogged = Utils.getCurrentTimeMillis();
                        }
                    }
                    else
                    {
                        if ((c != Command.CMD_BUTTON_STATUS && c != Command.CMD_SYN) || Utils.hasTimedOut(lastMessageLogged, 1000))
                        {
                            NotifyDebugMessage(String.Format("Command processed:{0} - ({1})\n", Utils.byteArrayToString(command.getRawData()), type));
                            lastMessageLogged = Utils.getCurrentTimeMillis();
                        }
                    }
                }
            } 
            catch(Exception e)
            {
                Logger.LogExceptionToFile(e);
            }

            return type;                        
        }      

        
        public string[] getportnames()
        {
            return SerialPort.GetPortNames(); //load all names of  com ports to string 
        }

        public void sendCommand(Command command, bool forcePost)
        {
            lock(sendLock)
            {
                try
                {
                    if (serialPort.IsOpen && (!isDisabledSerial || forcePost))
                    {

                        bs += command.getLength();
                        serialPort.Write(command.getRawData(), 0, command.getLength());

                    }
                }
                catch (Exception e)
                {
                    Logger.LogExceptionToFile(e);
                    NotifyStatusMessage("Error sending command to Arduino."); //if there are not is any COM port in PC show message
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("{0}/s", bs);
            bs = 0;
        }

        public void processData(byte[] serialData)
        {
            StringBuilder logData = new StringBuilder("");

            lock (dataLock)
            {
                if (serialData != null) { 
                    foreach (byte b in serialData)
                    {
                        switch (b)
                        {
                            //command init char found
                            case Command.CMD_INIT_DEBUG:
                            case Command.CMD_INIT:
                                serialCommand[0] = b;
                                commandLength = 1;

                                logData.Append("\n");
                                logData.Append(b + "-");
                                break;

                            //command end char found, send it to be processed
                            case Command.CMD_END:
                                serialCommand[commandLength] = b;

                                Command command = new Command(serialCommand);

                                if (command != null && command.getLength() > 0)
                                {
                                    string type = processCommand(command);
                                    logData.Append(b);
                                    logData.Append(" - " + type + "\n");
                                }
                                commandLength = 0;
                                Utils.resetArray(serialCommand);
                                break;

                            //command init char already found, start adding the command data to buffer
                            default:
                                logData.Append(b + "-");
                                if (commandLength > 0)
                                {
                                    serialCommand[commandLength++] = b;
                                }
                                if (commandLength == BUFFER_SIZE - 1)
                                {
                                    commandLength = 0;
                                    Utils.resetArray(serialCommand);

                                    Logger.LogDataToFile(" - CMD_INVALID \n");
                                }
                                break;
                        }                        
                    }
                }
            }
            if (debugMode == DebugMode.Verbose)
            {
                Logger.LogDataToFile(logData.ToString());
            }
        }
                
        //event handler triggered by serial port
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte[] data = new byte[serialPort.BytesToRead];
                serialPort.Read(data, 0, data.Length);                

                if(data.Length > 0)
                {
                    lastArduinoResponse = Utils.getCurrentTimeMillis();
                    processData(data);
                }
            }
        }

        //notify subscribers that a command was received
        protected virtual void NotifyCommandReceived(Command args)
        {
            CommandReceivedHandler handler = CommandReceivedSubscribers;

            if (handler != null)
            {
                handler(args);
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

        //notify subscribers (debug field) that a message has to be logged
        public void NotifyDebugMessage(string args)
        {
            DebugMessageHandler handler = DebugMessageSubscribers;

            if (handler != null)
            {
                handler(args);
            }
        }

        public void Dispose()
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
                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                }
                // Release unmanaged resources.
                disposed = true;
            }
        }

        ~SerialManager() { Dispose(false); }

    }
}
