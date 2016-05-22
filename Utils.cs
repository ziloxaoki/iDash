using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace iDash
{
    class Utils
    {

        public static string ByteArrayToString(byte[] ba)
        {
            if (ba != null && ba.Length > 0)
            {
                string hex = BitConverter.ToString(ba);
                return hex.Replace("-", "");
            }

            return "empty";
        }

        public static void resetArray(byte[] array)
        {
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
            }
        }

        public static T[] getSubArray<T>(T[] array, int from, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, from, result, 0, length);

            return result;
        }

        public static long getCurrentTimeMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static bool hasTimedOut(long startTime, long millisec)
        {
            return getCurrentTimeMillis() - startTime > millisec; 
        }

        public static string byteArrayToStr(byte[] array)
        {
            return System.Text.Encoding.Default.GetString(array);
        } 

    }

    public enum DebugMode
    {
        None,
        Default,
        Verbose
    }
}
