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
            string[] actions = action.Split('+');
            List<VirtualKeyCode> keycodes = new List<VirtualKeyCode>();
            List<VirtualKeyCode> modifierKeyCodes = new List<VirtualKeyCode>();


            if (actions != null)
            {
                foreach (string a in actions)
                {
                    VirtualKeyCode keycode = (VirtualKeyCode)Utils.ConvertCharToVirtualKey(a);
                    switch (keycode)
                    {
                        case VirtualKeyCode.CONTROL:
                        case VirtualKeyCode.RCONTROL:
                        case VirtualKeyCode.LCONTROL:
                        case VirtualKeyCode.SHIFT:
                        case VirtualKeyCode.RSHIFT:
                        case VirtualKeyCode.LSHIFT:
                        case VirtualKeyCode.MENU:
                        case VirtualKeyCode.LWIN:
                        case VirtualKeyCode.RWIN:
                            modifierKeyCodes.Add(keycode);
                            break;

                        default:
                            keycodes.Add(keycode);
                            break;
                    }
                }
            }            
            
            switch (state)
            {                
                case State.KeyDown:
                    if (modifierKeyCodes.Count == 0)
                    {
                        foreach (VirtualKeyCode keycode in keycodes) {
                            InputSimulator.SimulateKeyDown(keycode);
                        }
                    }
                    break;

                case State.KeyUp:
                    if (modifierKeyCodes.Count > 0)
                    {
                        InputSimulator.SimulateModifiedKeyStroke(modifierKeyCodes, keycodes);
                    }
                    else
                    {
                        foreach (VirtualKeyCode keycode in keycodes)
                        {
                            InputSimulator.SimulateKeyDown(keycode);
                        }
                    }
                    break;
            }        
        }
    }

}
