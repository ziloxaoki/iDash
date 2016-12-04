using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    public class Logger
    {
        public static string GetAppPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }

        private static void LogMessageToFile(string msg)
        {
            LogMessageToFile(msg, false);
        }

        public static void LogMessageToFile(string msg, bool isNewLine)
        {
            string logLine = System.String.Format(
                    "[{0:G}]: {1}", System.DateTime.Now, msg);

            LogDataToFile(logLine);

            if(isNewLine)
            {
                LogDataToFile("\n");
            }

        }

        public static void LogDataToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                GetAppPath() + "log.log");
            try
            {                
                sw.Write(msg);
            }
            finally
            {
                sw.Close();
            }
        }

        public static void LogExceptionToFile(Exception e)
        {
            LogMessageToFile(e.Source + ": " + e.Message + "\n", true);
        }
    }
}
