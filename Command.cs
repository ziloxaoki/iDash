using System;

namespace iDash
{
    public class Command
    {
        //Never use bit 0 in the command as it will indicate an end of string and command will be broken
        private byte[] rawData;
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
        public const byte CMD_DATA_INIT_OFFSET = (byte)3;

        public Command(byte[] buffer)
        {
            int commandLength = getLength(buffer);

            if (isValidCommand(buffer))
            {
                //buffer[0] is the command header
                //buffer[1] is the arduino id
                //buffer[2] is the command id
                //Utils.printByteArray(Utils.getSubArray(buffer, 0, commandLength));
                this.rawData = new byte[commandLength];
                Array.Copy(buffer, 0, rawData, 0, commandLength);
                this.crc = buffer[commandLength - 2];
            }
        }

        public Command(byte commandId, byte[] cData) : this(commandId, cData, false) { }

        public Command(byte commandId, byte[] cData, bool isDebug)
        {
            //header, device, id, crc, end
            int commandLength = 5;
            if (cData != null)
            {
                commandLength += cData.Length;
            } 
            //int commandLength = getLength(cData) + 4;
            //rawdata
            this.rawData = new byte[commandLength];
            Utils.resetArray(rawData);
            this.rawData[0] = CMD_INIT;            
            this.rawData[1] = 99;
            this.rawData[2] = commandId;

            if(commandLength > 5)
                Array.Copy(cData, 0, rawData, 3, cData.Length);

            if(isDebug)
            {
                this.rawData[0] = CMD_INIT_DEBUG;
            }

            //crc = rawdata - 2 last bytes (crc and cmd_end), but the last 2 bytes are still 0 so they don't affect crc value           
            this.crc = calculateCRC(rawData);
            this.rawData[commandLength - 2] = this.crc;
            this.rawData[commandLength - 1] = Command.CMD_END;                
        }

        public string getByteCodeName()
        {
            //0 - command header
            //1 - arduino id
            //2 - command id
            if (this.rawData.Length > 2)
            {
                switch (rawData[2])
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

        //data excludes cmd_init, cmd_device, cmd_id, crc and cmd_end
        public byte[] getData()
        {
            return Utils.getSubArray(rawData, 3, rawData.Length - 5);
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
                byte tCrc = calculateCRC(Utils.getSubArray(command, 0, command.Length - 2));
                return command[commandLength - 2] == tCrc;
            }

            return false;
        }

        public bool isValid()
        {            
            int crc = calculateCRC(Utils.getSubArray(rawData, 0, rawData.Length - 2));
            int dataCrc = this.rawData[rawData.Length - 2];
            return crc == dataCrc;
        }

        public bool isCommandX(byte c, byte[] command)
        {
            return isValidCommand(command) && command[1] == c;
        }

        public byte getCommandId()
        {
            if (rawData != null && rawData.Length > 4)
            {
                return rawData[2];
            }

            return 0;
        }

        public byte getDeviceId()
        {
            if (rawData != null && rawData.Length > 4)
            {
                return rawData[1];
            }

            return 0;
        }
    }
}
