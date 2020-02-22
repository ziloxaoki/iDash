using System;

namespace iDash
{
    public class Command
    {
        private byte[] rawData;
        private byte[] data;
        private byte crc;
        //BC e BF
        public const byte CMD_INIT = 200; //C8h
        public const byte CMD_INIT_DEBUG = 201; //C9h
        public const byte CMD_DEBUG_BUTTON = 202; //CAh
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
                Array.Copy(buffer, 0, data, 0, commandLength - 2);
                this.crc = buffer[commandLength - 1];
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

        public string getByteCodeName()
        {
            switch (rawData[1])
            {
                case CMD_SET_DEBUG_MODE:
                    return "CMD_SET_DEBUG_MODE";

                case CMD_RESPONSE_SET_DEBUG_MODE:
                    return "CMD_RESPONSE_SET_DEBUG_MODE";

                case CMD_SYN:
                    return "CMD_SYN";

                case CMD_7_SEGS:
                    return "CMD_7_SEGS";

                case CMD_SYN_ACK:
                    return "CMD_SYN_ACK";

                case CMD_RGB_SHIFT:
                    return "CMD_RGB_SHIFT";

                case CMD_BUTTON_STATUS:
                    return "CMD_BUTTON_STATUS";

                case CMD_DEBUG_BUTTON:
                    return "CMD_DEBUG_BUTTON";

                case CMD_INVALID:
                    return "CMD_INVALID";

                case CMD_INIT:
                    return "CMD_INIT";

                case CMD_INIT_DEBUG:
                    return "CMD_INIT_DEBUG";

                case CMD_END:
                    return "CMD_END";
            }

            return "invalid";
        }

        public static byte calculateCRC(byte[] data)
        {
            int sum = 0;
            foreach (byte b in data)
            {
                sum += b;
            }

            int crc = sum % 256;

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

        public bool isValid()
        {
            int crc = calculateCRC(this.data);
            int dataCrc = this.rawData[rawData.Length - 2];
            return crc == dataCrc;
        }

        public bool isCommandX(byte c, byte[] command)
        {
            return isValidCommand(command) && command[1] == c;
        }
    }
}
