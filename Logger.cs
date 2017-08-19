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
                    writer.Write(msg);
                    CounterOfLines++;
                    writer.Close();
                }
                else
                {                    
                    writer.Close();
                    string name = GetAppPath() + "log.log";
                    System.IO.File.WriteAllText(@name, string.Empty);
                    CounterOfLines = 1;
                    writer = new StreamWriter(name);
                    writer.Write(msg);
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

            if (isNewLine && !msg.EndsWith("\n"))
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
