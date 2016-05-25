using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vJoyInterfaceWrap;

namespace iDash
{    

    public partial class MainForm : Form
    {
        private int WAIT_ARDUINO_SET_DEBUG_MODE = 100;

        private SerialManager sm;
        private ButtonHandler bh;
        private VJoyFeeder vf;

        public delegate void AppendToStatusBarDelegate(String s);
        public AppendToStatusBarDelegate appendToStatusBar;
        public delegate void AppendToDebugDialogDelegate(String s);
        public AppendToDebugDialogDelegate appendToDebugDialog;

        public MainForm()
        {
            this.appendToStatusBar = new AppendToStatusBarDelegate(UpdateStatusBar);
            this.appendToDebugDialog = new AppendToDebugDialogDelegate(AppendToDebugDialog);
            InitializeComponent();
            sm = new SerialManager();
            bh = new ButtonHandler(sm);
            vf = new VJoyFeeder(bh);
            sm.StatusMessageSubscribers += UpdateStatusBar;
            vf.StatusMessageSubscribers += UpdateStatusBar;
            sm.DebugMessageSubscribers += UpdateDebugData;
            sm.Init();
            vf.InitializeJoystick();
        }

              
        
        private void buttonSend_Click(object sender, EventArgs e) // send button  event
        {
            byte[] aux = System.Text.Encoding.ASCII.GetBytes(richTextBoxSend.Text);
            Command command = new Command(aux[0], Utils.getSubArray<byte>(aux, 1, aux.Length - 1));
            sm.sendCommand(command);     //transmit data
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SerialManager.stopThreads = true;
            sm.StatusMessageSubscribers -= UpdateStatusBar;
            vf.StatusMessageSubscribers -= UpdateStatusBar;
            sm.DebugMessageSubscribers -= UpdateDebugData;
        }

        public void AppendToStatusBar(String s)
        {
            statusBar.AppendText(s);
        }

        public void UpdateStatusBar(string s)
        {
            if (InvokeRequired)
                this.statusBar.BeginInvoke(this.appendToStatusBar, new Object[] { s });
            else this.AppendToStatusBar(s);

        }

        public void AppendToDebugDialog(String s)
        {
            debugData.AppendText(s);
        }

        public void UpdateDebugData(string s)
        {
            if (InvokeRequired)
                this.debugData.BeginInvoke(this.appendToDebugDialog, new Object[] { s });
            else this.AppendToDebugDialog(s);

        }

        private void statusBar_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            statusBar.SelectionStart = statusBar.Text.Length;
            // scroll it automatically
            statusBar.ScrollToCaret();
        }

        private void clearData_Click(object sender, EventArgs e)
        {
            debugData.Clear();
        }

        private async void debugModes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dbug mode off
            byte[] state = { 0x00 };
            if (debugModes.SelectedIndex != 0)
            {
                //debug mode on
                state[0] = 0x01;
            }
            Command command = new Command(Command.CMD_SET_DEBUG_MODE, state);
            SerialManager.debugMode = (DebugMode)debugModes.SelectedItem;
            //make sure Arduino has received/updated the debug mode state
            while (SerialManager.isArduinoInDebugMode != (state[0] == 1))
            {
                sm.sendCommand(command);     //transmit data
                await Task.Delay(WAIT_ARDUINO_SET_DEBUG_MODE);
            }            
        }

        private void debugData_TextChanged(object sender, EventArgs e)
        {            
            // set the current caret position to the end
            debugData.SelectionStart = statusBar.Text.Length;
            // scroll it automatically
            debugData.ScrollToCaret();

            if (debugData.Lines.Count() > 200)
            {
                debugData.Clear();
            }
        }
    }
}
