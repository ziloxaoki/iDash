using System;

namespace iDash
{
    public class Command
    {
        private byte[] rawData;
        private byte[] data;
        private byte crc;
        //BC e BF
        public const byte CMD_INIT = 200; //94d 5Eh
        public const byte CMD_INIT_DEBUG = 201; //95d 5Fh
        public const byte CMD_END = (byte)'~'; //126d 7Eh
        public const byte CMD_SET_DEBUG_MODE = 11;
        public const byte CMD_RESPONSE_SET_DEBUG_MODE = 12;
        public const byte CMD_SYN = (byte)'A'; //65d 41h
        public const byte CMD_7_SEGS = (byte)'B'; //66d 42h
        public const byte CMD_SYN_ACK = (byte)'a'; //97d 61h
        public const byte CMD_RGB_SHIFT = (byte)'C'; //67d 43h
        public const byte CMD_BUTTON_STATUS = (byte)'D'; //68d 44h
        public const byte CMD_INVALID = (byte)0xef; //239d EFh

        public Command(byte[] buffer)
        {
            int commandLength = getLength(buffer);

            if (isValidCommand(buffer))
            {
                this.rawData = new byte[commandLength];
                Array.Copy(buffer, 0, rawData, 0, commandLength);
                this.data = new byte[commandLength - 2];
                Array.Copy(buffer, 1, data, 0, commandLength - 3);
                this.crc = buffer[commandLength - 2];
            }
        }

        public Command(byte command, byte[] cData) : this(command, cData, false) { }

        public Command(byte command, byte[] cData, bool isDebug)
        {
            //header, code, crc, end
            int commandLength = 4;
            if (cData != null)
            {
                commandLength += cData.Length;
            } 
            //int commandLength = getLength(cData) + 4;
            //rawdata
            this.rawData = new byte[commandLength];
            this.rawData[0] = CMD_INIT;
            this.rawData[1] = command;

            if(commandLength > 4)
                Array.Copy(cData, 0, rawData, 2, cData.Length);

            //data - exclude crc and end
            this.data = new byte[commandLength - 2];
            this.data[0] = CMD_INIT;
            this.data[1] = command;

            if(isDebug)
            {
                this.rawData[0] = CMD_INIT_DEBUG;
                this.data[0] = CMD_INIT_DEBUG;
            }

            if (commandLength > 4)
                Array.Copy(cData, 0, data, 2, cData.Length);

            //set crc            
            this.crc = calculateCRC(this.data);
            this.rawData[commandLength - 2] = this.crc;
            this.rawData[commandLength - 1] = Command.CMD_END;                
        }

        public string getCommandType()
        {
            string result = "invalid";
            switch (rawData[1])
            {
                case CMD_SET_DEBUG_MODE:
                    result = "CMD_SET_DEBUG_MODE";
                    break;

                case CMD_RESPONSE_SET_DEBUG_MODE:
                    result = "CMD_RESPONSE_SET_DEBUG_MODE";
                    break;

                case CMD_SYN:
                    result = "CMD_SYN";
                    break; //65d 41h

                case CMD_7_SEGS:
                    result = "CMD_7_SEGS";
                    break; //66d 42h

                case CMD_SYN_ACK:
                    result = "CMD_SYN_ACK";
                    break; //97d 61h

                case CMD_RGB_SHIFT:
                    result = "CMD_RGB_SHIFT";
                    break; //67d 43h

                case CMD_BUTTON_STATUS:
                    result = "CMD_BUTTON_STATUS";
                    break; //68d 44h

                case CMD_INVALID:
                    result = "CMD_INVALID";
                    break; //239d EFh 
            }
            return result;
        }

        public static byte calculateCRC(byte[] data)
        {
            int crc = 0;
            foreach (byte b in data)
            {
                crc += b;
            }

            crc %= 256;

            return (byte)crc;
        }

        private int getLength(byte[] data)
        {
            int result = 0;
            if (data != null)
            {
                foreach (byte b in data)
                {
                    result++;
                    if (b == CMD_END)
                    {
                        return result;
                    }
                }
                return result;
            }
            return 0;
        }

        public int getLength()
        {
            return getLength(rawData);
        }

        public byte[] getData()
        {
            return data;
        }

        public byte[] getRawData()
        {
            return rawData;
        }

        public bool isValidCommand(byte[] command)
        {
            int commandLength = getLength(command);
            if (command != null && commandLength > 3 && (command[0] == CMD_INIT || command[0] == CMD_INIT_DEBUG) && command[commandLength - 1] == CMD_END)
            {
                byte[] temp = new byte[commandLength - 2];
                Array.Copy(command, 0, temp, 0, temp.Length);
                byte tCrc = calculateCRC(temp);
                return command[commandLength - 2] == tCrc;
            }

            return false;
        }

        public bool isCommandX(byte c, byte[] command)
        {
            return isValidCommand(command) && command[1] == c;
        }
    }
}
