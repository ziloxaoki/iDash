using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        struct INPUT
        {
            public int type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            /*Virtual Key code.  Must be from 1-254.  If the dwFlags member specifies KEYEVENTF_UNICODE, wVk must be 0.*/
            public ushort wVk;
            /*A hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, wScan specifies a Unicode character which is to be sent to the foreground application.*/
            public ushort wScan;
            /*Specifies various aspects of a keystroke.  See the KEYEVENTF_ constants for more information.*/
            public uint dwFlags;
            /*The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.*/
            public uint time;
            /*An additional value associated with the keystroke. Use the GetMessageExtraInfo function to obtain this information.*/
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern int MapVirtualKey(int uCode, uint uMapType);

        public override void process(string action, State state)
        {
            INPUT[] inputs = null;
            ushort keycode = (ushort)Utils.ConvertCharToVirtualKey(action);
            ushort wscan = (ushort)MapVirtualKey(keycode, 0);
            switch (state)
            {
                //case State.KeyDown | State.KeyHold:
                case State.KeyDown:
                    inputs = getKeyAction(wscan, false);
                    break;

                case State.KeyUp:
                    //inputs = getKeyAction(wscan, false);
                    //SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
                    inputs = getKeyAction(wscan, true);
                    //SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
                    break;
            } 
            
            if(inputs != null)
            {
                SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
            }           
        }

        private INPUT[] getKeyAction(ushort key, bool isKeyUp)
        {
            INPUT[] inputs = new INPUT[]
                {
                    new INPUT
                    {
                        type = INPUT_KEYBOARD,
                        u = new InputUnion
                        {
                            ki = new KEYBDINPUT
                            {
                                wVk = 0, 
                                wScan = key,
                                dwFlags = (isKeyUp ? KEYEVENTF_KEYUP : KEYEVENTF_KEYDOWN) | KEYEVENTF_SCANCODE,
                                //dwFlags = KEYEVENTF_KEYDOWN | KEYEVENTF_SCANCODE,
                                dwExtraInfo = GetMessageExtraInfo(),
                            }
                        }
                    }
                };

            return inputs;
        }
    }

}
