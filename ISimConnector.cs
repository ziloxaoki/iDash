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

                    if (numActiveLeds <= LED_NUM_TOTAL + 1)
                    {
                        if (numActiveLeds > LED_NUM_TOTAL)
                            numActiveLeds = LED_NUM_TOTAL;
                        Array.Copy(Utils.colourPattern, 0, rpmLed, 0, numActiveLeds * 3); //each led colour has 3 bytes

                        rgbShift = new Command(Command.CMD_RGB_SHIFT, rpmLed);
                    }
                    else //blink
                    {
                        if (milSec < 50 || (milSec > 100 && milSec < 150) ||
                        (milSec > 200 && milSec < 250) || (milSec > 300 && milSec < 350) ||
                        (milSec > 400 && milSec < 450) || (milSec > 500 && milSec < 550) ||
                        (milSec > 600 && milSec < 650) || (milSec > 700 && milSec < 750) ||
                        (milSec > 800 && milSec < 850) || (milSec > 900 && milSec > 950))
                        /*if (milSec < 100 || (milSec > 200 && milSec < 300) ||
                        (milSec > 400 && milSec < 500) || (milSec > 600 && milSec < 700) ||
                        (milSec > 800 && milSec < 900) || (milSec > 900))*/
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

                sm.sendCommand(rgbShift, false);
            }
        }
        
        //implemented by child classes
        protected abstract string getTelemetryValue(string name, string type, string clazz);

        protected string getTelemetryData(string name, string strPattern)
        {
            if (name.Equals("hour.time"))
                return DateTime.Now.ToString("hh.mm.ss.ff");

            string result = "";

            if (!String.IsNullOrEmpty(name))
            {
                string[] type = name.Split('.');

                if (type.Length > 1)
                {
                    string clazz = null;
                    if(type.Length > 2)
                    {
                        clazz = type[2];
                    }

                    //type[0] = property name, type[1] = property type
                    result = this.getTelemetryValue(type[0], type[1], clazz);
                }

                if (!String.IsNullOrEmpty(result))
                {
                    result = Utils.formatString(result, strPattern);
                }
            }

            return result;
        }

        //needs to wait until MainForm 7Segment is loaded
        protected void send7SegmentMsg()
        {
            StringBuilder msg = new StringBuilder();
            List<string> _7SegmentData = MainForm.get7SegmentData();
            string[] strPatterns = MainForm.getStrFormat().Split(Utils.ITEM_SEPARATOR);

            if (_7SegmentData.Count > 0)
            {
                for (int x = 0; x < _7SegmentData.Count; x++)
                {
                    string name = _7SegmentData.ElementAt(x);
                    string pattern = "{0}";

                    if (strPatterns.Length > 0)
                    {
                        if (x < strPatterns.Length)
                            pattern = string.IsNullOrEmpty(strPatterns[x]) ? "{0}" : strPatterns[x];
                    }

                    msg.Append(this.getTelemetryData(name, pattern));
                }

                if (msg.Length > 0)
                {
                    byte[] b = Utils.getBytes(msg.ToString());
                    Command c = new Command(Command.CMD_7_SEGS, Utils.convertByteTo7Segment(b, 0));
                    sm.sendCommand(c, false);
                }
            }
        }

        public abstract void Dispose();        
    }
}
