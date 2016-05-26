using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        private void button11_Click(object sender, EventArgs e)
        {
            byte[] PDF = Properties.Resources.telemetry_11_23_15;



            MemoryStream ms = new MemoryStream(PDF);



            //Create PDF File From Binary of resources folders help.pdf

            FileStream f = new FileStream("telemetry_11_23_15.pdf", FileMode.OpenOrCreate);



            //Write Bytes into Our Created help.pdf

            ms.WriteTo(f);

            f.Close();

            ms.Close();


            // Finally Show the Created PDF from resources

            Process.Start("telemetry_11_23_15.pdf");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string item in props.SelectedItems)
            {
                if(!selected.Items.Contains(item))
                {
                    selected.Items.Add(item);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selected.SelectedIndex != -1)
            {
                for (int i = selected.SelectedItems.Count - 1; i >= 0; i--)
                    selected.Items.Remove(selected.SelectedItems[i]);
            }
        }

        public void MoveItem(ListBox lb, int direction)
        {
            // Checking selected item
            if (lb.SelectedItem == null || lb.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = lb.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= lb.Items.Count)
                return; // Index out of range - nothing to do

            object selected = lb.SelectedItem;

            // Removing removable element
            lb.Items.Remove(selected);
            // Insert it in new position
            lb.Items.Insert(newIndex, selected);
            // Restore selection
            lb.SetSelected(newIndex, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selected.SelectedIndex > 0)
            {
                this.MoveItem(selected, -1);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selected.SelectedIndex < selected.Items.Count - 1)
            {
                this.MoveItem(selected, 1);
            }
        }
    }
}
