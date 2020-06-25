using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using vJoyInterfaceWrap;

namespace iDash
{
    static class Program
    {        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Process[] proc = Process.GetProcessesByName(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            proc[0].PriorityClass = ProcessPriorityClass.High;
            Application.Run(new MainForm());
        }
    }
}
