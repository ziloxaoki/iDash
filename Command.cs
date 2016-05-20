using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    public class Command
    {
        private byte[] rawData;
        private byte[] data;
        private byte crc;

        public const byte COMMAND_INIT = (byte)'^'; //94
        public const byte COMMAND_END = (byte)'\n'; //10
        public const byte CMD_SYN = (byte)'A'; //65
        public const byte CMD_SYN_ACK = (byte)'a'; //97
        public const byte CMD_BUTTON_STATUS = (byte)'D'; //68

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

        public Command(byte command, byte[] cData)
        {
            int commandLength = getLength(cData) + 4;

            this.rawData = new byte[commandLength];
            this.rawData[0] = COMMAND_INIT;
            this.rawData[1] = command;
            Array.Copy(cData, 0, rawData, 2, cData.Length);
            this.data = new byte[commandLength - 2];
            this.data[0] = COMMAND_INIT;
            this.data[1] = command;
            Array.Copy(cData, 0, data, 2, cData.Length);
            this.crc = calculateCRC(this.data);
            this.rawData[commandLength - 2] = this.crc;
            this.rawData[commandLength - 1] = Command.COMMAND_END;
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
                    if (b == COMMAND_END)
                    {
                        return result;
                    }
                }
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
            if (command != null && commandLength > 3 && command[0] == COMMAND_INIT && command[commandLength - 1] == COMMAND_END)
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
