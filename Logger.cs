using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    public class Logger
    {
        private static readonly object _sync = new object();
        private const int MAX_LINES = 5000;
        private const string FILE_NAME_PREFIX = "log";
        private static int CounterOfLines;

        private static DateTime date = new DateTime();

        public Logger()
        {
            CounterOfLines = 0;
        }

        public void LogDataToFile(object msg)
        {
            lock (_sync)
            {
                StreamWriter writer = new StreamWriter("log.log", true);
                if (CounterOfLines < MAX_LINES)
                {                    
                    writer.WriteLine(msg);
                    CounterOfLines++;
                    writer.Close();
                }
                else
                {
                    writer.Close();
                    CounterOfLines = 1;
                    date = date.AddMilliseconds(1);
                    string nameFile = FILE_NAME_PREFIX + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.Millisecond.ToString() + ".log";
                    File.Move(FILE_NAME_PREFIX + ".log", nameFile);
                    writer = new StreamWriter(GetAppPath() + FILE_NAME_PREFIX + ".log");
                    writer.WriteLine(msg);
                    writer.Close();
                }
            }
        }

        public string GetAppPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }

        private void LogMessageToFile(string msg)
        {
            LogMessageToFile(msg, false);
        }

        public void LogMessageToFile(string msg, bool isNewLine)
        {            
            string logLine = string.Format(
                "[{0:G}]: {1}", DateTime.Now, msg);

            LogDataToFile(logLine);

            if (isNewLine)
            {
                LogDataToFile("\n");
            }            
        }

        public void LogExceptionToFile(Exception e)
        {
            LogMessageToFile(e.ToString() + "\n", true);
        }

        public void LogExceptionToFile(Exception e, string msg)
        {
            LogMessageToFile(string.Format("{0}\n{1}\n",msg,e.ToString()), true);
        }
    }
}
