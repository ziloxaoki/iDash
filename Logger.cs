using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    public class Logger
    {
        public static string GetTempPath()
        {
            string path = System.Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }

        public static void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                GetTempPath() + "log.log");
            try
            {
                /*string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);*/
                sw.Write(msg);
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
