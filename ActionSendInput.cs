using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;

namespace iDash
{
    class ActionSendInput : ActionHandler
    {        
        const int INPUT_MOUSE = 0;
        const int INPUT_KEYBOARD = 1;
        const int INPUT_HARDWARE = 2;
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_KEYDOWN = 0x0000;
        const uint KEYEVENTF_UNICODE = 0x0004;
        const uint KEYEVENTF_SCANCODE = 0x0008;

        public ActionSendInput(MainForm mainForm) : base(mainForm)
        {
            this.mainForm = mainForm;
        }

        public override void process(string action, State state)
        {
            ushort keycode = (ushort)Utils.ConvertCharToVirtualKey(action);
            
            switch (state)
            {
                //case State.KeyDown | State.KeyHold:
                case State.KeyDown:
                    InputSimulator.SimulateKeyDown((VirtualKeyCode)keycode);
                    break;

                case State.KeyUp:
                    InputSimulator.SimulateKeyUp((VirtualKeyCode)keycode);
                    break;
            }        
        }
    }

}
