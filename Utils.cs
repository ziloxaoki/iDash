using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
