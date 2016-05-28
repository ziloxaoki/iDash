using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iDash
{
    class IRacingConnector
    {
        private SerialManager sm;

        public IRacingConnector(SerialManager sm)
        {
            this.sm = sm;
            new Thread(new ThreadStart(sendToArduino)).Start();
        }

        private async void sendToArduino()
        {
            int x = 0;
            while (!MainForm.stopThreads)
            {                
                byte b = (byte)(57 + (x % 2));
                Command c = new Command((byte)'B', new byte[] { 91, 91, 91, b, 91, 91, 91, 91, 91, 91, 91, b, 91, 91, 91, 91, 91, 91, 91, b});
                sm.sendCommand(c);
                await Task.Delay(100);
                x++;
            }
        }
    }
}
