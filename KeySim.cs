using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iDash
{
    class KeySim : ActionHandler
    {
        #region P/Invoke
        private const uint WM_KEYDOWN = 0x100,
            WM_KEYUP = 0x101;

        public KeySim(MainForm mainForm) : base(mainForm)
        {
            this.mainForm = mainForm;
        }

        [DllImport("user32", EntryPoint = "PostMessage", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool InternalPostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        #endregion


        public override void process(string action, State state)
        {
            int Keycode = (ushort)Utils.ConvertCharToVirtualKey(action[0]);

            if (state == State.KeyDown)
            {
                if (Process.GetProcessesByName("RRRE").Length > 0) {
                    if (!InternalPostMessage(Process.GetProcessesByName("RRRE")[0].Handle,  // Insert your WowProcessHandle!
                    WM_KEYDOWN,
                    new IntPtr(IntPtr.Size == 4 ? (int)Keycode : (long)Keycode), // WPARAM is x64/x86 dependant, therefore cast from long/int to IntPtr
                    IntPtr.Zero)) throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            if (state == State.KeyUp)
            {
                if (!InternalPostMessage(Process.GetProcessesByName("RRRE")[0].Handle,
                WM_KEYUP,
                new IntPtr(IntPtr.Size == 4 ? (int)Keycode : (long)Keycode),
                IntPtr.Zero)) throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
