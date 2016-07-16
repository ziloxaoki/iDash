using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDash
{
    public abstract class ISimConnector : IDisposable
    {
        protected SerialManager sm;

        private const int LED_NUM_TOTAL = 16;
        public const float FIRST_RPM = 0.7f;

        public ISimConnector(SerialManager sm)
        {
            this.sm = sm;
        }

        protected void sendRPMShiftMsg(float currentRpm, float firstRpm, float lastRpm)
        {
            byte[] rpmLed = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }; // 1, 1, 1 = black
            float rpmPerLed = (lastRpm - firstRpm) / LED_NUM_TOTAL; //rpm range per led                
            int milSec = DateTime.Now.Millisecond;
            Command rgbShift = null;
            if (rpmPerLed > 0 && currentRpm > firstRpm - 2000)
            {
                if (currentRpm > firstRpm)
                {
                    int numActiveLeds = (int)(Math.Ceiling((currentRpm - firstRpm) / rpmPerLed));

                    if (numActiveLeds < LED_NUM_TOTAL)
                    {
                        Array.Copy(Utils.colourPattern, 0, rpmLed, 0, numActiveLeds * 3); //each led colour has 3 bytes

                        rgbShift = new Command(Command.CMD_RGB_SHIFT, rpmLed);
                    }
                    else //blink
                    {
                        /*if (milSec < 50 || (milSec > 100 && milSec < 150) ||
                        (milSec > 200 && milSec < 250) || (milSec > 300 && milSec < 350) ||
                        (milSec > 400 && milSec < 450) || (milSec > 500 && milSec < 550) ||
                        (milSec > 600 && milSec < 650) || (milSec > 700 && milSec < 750) ||
                        (milSec > 800 && milSec < 850) || (milSec > 900))*/
                        if (milSec < 100 || (milSec > 200 && milSec < 300) ||
                        (milSec > 400 && milSec < 500) || (milSec > 600 && milSec < 700) ||
                        (milSec > 800 && milSec < 900) || (milSec > 900))
                        {
                            rgbShift = new Command(Command.CMD_RGB_SHIFT, rpmLed);
                        }
                        else
                        {
                            rgbShift = new Command(Command.CMD_RGB_SHIFT, Utils.colourPattern);
                        }
                    }
                }
                else {
                    //clear shift lights
                    rgbShift = new Command(Command.CMD_RGB_SHIFT, rpmLed);
                }

                sm.sendCommand(rgbShift);
            }
        }

        public abstract void Dispose();        
    }
}
