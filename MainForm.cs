using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using vJoyInterfaceWrap;

namespace iDash
{    

    public partial class MainForm : Form
    {

        private SerialManager sm;
        private ButtonHandler bh;
        private VJoyFeeder vf;        

        public MainForm()
        {
            InitializeComponent();
            sm = new SerialManager();
            bh = new ButtonHandler(sm);
            vf = new VJoyFeeder(bh);
            sm.StatusMessageSubscribers += UpdateStatusBar;
            vf.StatusMessageSubscribers += UpdateStatusBar;
            sm.Init();
            vf.InitializeJoystick();
        }

              
        
        private void buttonSend_Click(object sender, EventArgs e) // send button  event
        {
            byte[] aux = System.Text.Encoding.ASCII.GetBytes(richTextBoxSend.Text);
            Command command = new Command(0x01, Utils.getSubArray<byte>(aux, 0, aux.Length));
            sm.sendCommand(command);     //transmit data
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SerialManager.stopThreads = true;
        }

        public void UpdateStatusBar(string s)
        {
            this.statusBar.Text += s;
        }

        private void statusBar_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            statusBar.SelectionStart = statusBar.Text.Length;
            // scroll it automatically
            statusBar.ScrollToCaret();
        }
    }
}
