using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iDash
{    

    public partial class MainForm : Form
    {

        SerialManager sm;
        //ArduinoMonitor am;        

        public MainForm()
        {            
            sm = new SerialManager();
            //am = new ArduinoMonitor(sm);
            sm.Init();
            InitializeComponent();
        }

              
        
        private void buttonSend_Click(object sender, EventArgs e) // send button  event
        {
            byte[] aux = System.Text.Encoding.ASCII.GetBytes(richTextBoxSend.Text);
            sm.sendCommand(new Command(aux[0], Utils.getSubArray<byte>(aux, 1, aux.Length - 1)));     //transmit data
        }

        private void buttonReload_Click(object sender, EventArgs e)//reload button event ,most useful if you use virtual COM port e.g FTDI,Prolific 
        {

        }

        public void ProcessSerialData(String myString)
        {
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.serialDelegate = new AddDataDelegate(ProcessSerialData);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SerialManager.stopThreads = true;
        }

        private void richTextBoxReceive_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void clearDebug_Click(object sender, EventArgs e)
        {
           
        }
    }
}
