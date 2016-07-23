using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iDash
{

    public partial class MainForm : Form
    {
        private const int WAIT_ARDUINO_SET_DEBUG_MODE = 100;
        private const int WAIT_UI_FREQUENCY = 1000;
        private const int WAIT_THREADS_TO_CLOSE = 3500;

        private SerialManager sm;
        private ButtonHandler bh;
        private VJoyFeeder vf;
        private IRacingConnector irc;
        private RaceRoomConnector rrc;

        private List<ArrayList> TM1637ListBoxItems = new List<ArrayList>();
        private List<ArrayList> ButtonsListBoxItems = new List<ArrayList>();
        private ArrayList bActions = new ArrayList();

        public static bool stopThreads = false;
        public static bool stopIRacingThreads = false;
        public static bool stopRaceRoomThreads = false;
        private static List<string> _7Segment;
        private static string strFormat = "";
        private static readonly Object listLock = new Object();

        public delegate void AppendToStatusBarDelegate(String s);
        public AppendToStatusBarDelegate appendToStatusBar;
        public delegate void AppendToDebugDialogDelegate(String s);
        public AppendToDebugDialogDelegate appendToDebugDialog;      
          
        private bool isSearchingButton = false;
        private const string BUTTON_PREFIX = "Button_";
        private bool isWaitingForKey = false;

        public delegate void HandleButtonActions(List<State> states);
        public HandleButtonActions handleButtonActions;                


        public MainForm()
        {
            this.appendToStatusBar = new AppendToStatusBarDelegate(UpdateStatusBar);
            this.appendToDebugDialog = new AppendToDebugDialogDelegate(AppendToDebugDialog);
            InitializeComponent();

            handleButtonActions = new HandleButtonActions(handleButtons);            

            sm = new SerialManager();
            bh = new ButtonHandler(sm);
            vf = new VJoyFeeder(bh);
            sm.StatusMessageSubscribers += UpdateStatusBar;
            vf.StatusMessageSubscribers += UpdateStatusBar;
            sm.DebugMessageSubscribers += UpdateDebugData;
            bh.buttonStateHandler += ButtonStateReceived;

            this.iRacingToolStripMenuItem1.PerformClick();

            sm.Init();
            vf.InitializeJoystick();                                    
        }

        private void parseViews()
        {
            if (this.views.Items.Count > 0)
            {
                string[] items = this.views.SelectedItem.ToString().Split(Utils.LIST_SEPARATOR);
                lock (listLock)
                {
                    _7Segment = new List<string>();

                    //ignoring "when connected" flag
                    for (int x = 0; x < items.Length - 1; x++)
                    {
                        if (x == items.Length - 2)
                        {
                            strFormat = items[x];
                        }
                        else
                        {
                            _7Segment.Add(items[x]);
                        }
                    }
                }
            }
        }

        private void syncViews()
        {
            for (int x = 0; x < settingsToolStripMenuItem.DropDownItems.Count; x++)
            {
                ToolStripMenuItem dropDownItem = (ToolStripMenuItem)settingsToolStripMenuItem.DropDownItems[x];
                //search for selected simulator settings
                if (dropDownItem.Checked)
                {
                    ArrayList objCollection = new ArrayList();
                    objCollection.AddRange(Utils.convertObjectCollectionToStringArray(views.Items));
                    TM1637ListBoxItems.Insert(x, objCollection);

                    ArrayList objCollection2 = new ArrayList();
                    objCollection2.AddRange(Utils.convertObjectCollectionToStringArray(views2.Items));
                    ButtonsListBoxItems.Insert(x, objCollection2);

                    break;
                }
            }
        }

        private void loadViewProperties()
        {
            this.views.Items.Clear();
            this.views2.Items.Clear();
            this.selected.Items.Clear();
            this.textFormat.Clear();

            for (int x = 0; x < settingsToolStripMenuItem.DropDownItems.Count; x++)
            {
                ToolStripMenuItem dropDownItem = (ToolStripMenuItem)settingsToolStripMenuItem.DropDownItems[x];
                if (dropDownItem.Checked)
                {
                    if (x < TM1637ListBoxItems.Count)
                    {
                        this.views.Items.AddRange(TM1637ListBoxItems[x].ToArray());
                    }
                    if (x < ButtonsListBoxItems.Count)
                    {
                        this.views2.Items.AddRange(ButtonsListBoxItems[x].ToArray());
                    }
                }
            }

            if (views.Items.Count > 0)
                views.SelectedIndex = 0;

            if (views2.Items.Count > 0)
                views2.SelectedIndex = 0;
        }

        private void restoreViewProperties()
        {           
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.TM1637)))
            {
                if (ms.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    TM1637ListBoxItems = (List<ArrayList>)bf.Deserialize(ms);
                }
            }

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.BUTTONS)))
            {
                if (ms.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    ButtonsListBoxItems = (List<ArrayList>)bf.Deserialize(ms);
                }
            }

            loadViewProperties();
        }


        private void saveAppSettings()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                byte[] buffer = null;

                //serialize TM1637 to Base64 to be saved
                if (TM1637ListBoxItems.Count > 0)
                {
                    bf.Serialize(ms, TM1637ListBoxItems);
                    ms.Position = 0;
                    buffer = new byte[(int)ms.Length];
                    ms.Read(buffer, 0, buffer.Length);
                    Properties.Settings.Default.TM1637 = Convert.ToBase64String(buffer);
                    ms.Close();
                }                             
            }
                        
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                byte[] buffer = null;

                //serialize Buttons to Base64 to be saved
                if (ButtonsListBoxItems.Count > 0)
                {
                    bf = new BinaryFormatter();
                    bf.Serialize(ms, ButtonsListBoxItems);
                    ms.Position = 0;
                    buffer = new byte[(int)ms.Length];
                    ms.Read(buffer, 0, buffer.Length);
                    Properties.Settings.Default.BUTTONS = Convert.ToBase64String(buffer);
                    ms.Close();
                }
            }

            Properties.Settings.Default.Save();
        }

        private void buttonSend_Click(object sender, EventArgs e) // send button  event
        {
            byte[] aux = System.Text.Encoding.ASCII.GetBytes(richTextBoxSend.Text);
            Command command = new Command(aux[0], Utils.getSubArray<byte>(aux, 1, aux.Length - 1));
            sm.sendCommand(command);     //transmit data
        }

        protected async override void OnFormClosing(FormClosingEventArgs e)
        {
            stopThreads = true;
            sm.StatusMessageSubscribers -= UpdateStatusBar;
            vf.StatusMessageSubscribers -= UpdateStatusBar;
            sm.DebugMessageSubscribers -= UpdateDebugData;
            if (irc != null)
                irc.StatusMessageSubscribers -= UpdateStatusBar;
            if (rrc != null)
                rrc.StatusMessageSubscribers -= UpdateStatusBar;

            saveAppSettings();

            //wait all threads to close
            await Task.Delay(WAIT_THREADS_TO_CLOSE);
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
                if (!selected.Items.Contains(item))
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

        private void button8_Click(object sender, EventArgs e)
        {
            if (views.SelectedIndex != -1)
            {
                for (int i = views.SelectedItems.Count - 1; i >= 0; i--)
                    views.Items.Remove(views.SelectedItems[i]);
            }

            if (views.Items.Count > 0)
                views.SelectedIndex = 0;

            syncViews();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string viewValue = null;

            for (int i = 0; i < selected.SelectedItems.Count; i++) {             
                viewValue += (string)selected.SelectedItems[i] + Utils.LIST_SEPARATOR;
            }
            if (viewValue != null && viewValue.Length > 0)
            {
                viewValue += textFormat.Text + Utils.LIST_SEPARATOR + isSimConnected.Checked;
                if (!views.Items.Contains(viewValue))
                {
                    views.Items.Add(viewValue);
                }
            }

            syncViews();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (views.SelectedIndex > 0)
            {
                this.MoveItem(views, -1);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (views.SelectedIndex < views.Items.Count - 1)
            {
                this.MoveItem(views, 1);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (buttonActions.SelectedIndex > 0)
            {
                this.MoveItem(buttonActions, -1);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (buttonActions.SelectedIndex < buttonActions.Items.Count - 1)
            {
                this.MoveItem(buttonActions, 1);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (views2.SelectedIndex > 0)
            {
                this.MoveItem(views2, -1);
            }
        }


        private void button13_Click(object sender, EventArgs e)
        {
            if (views2.SelectedIndex < views2.Items.Count - 1)
            {
                this.MoveItem(views2, 1);
            }
        }

        private void views_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (views.SelectedIndex >= 0)
            {
                string temp = views.SelectedItem.ToString();
                string[] selectedValue = views.SelectedItem.ToString().Split(Utils.LIST_SEPARATOR);
                isSimConnected.Checked = Convert.ToBoolean(selectedValue[selectedValue.Length - 1]);
                textFormat.Text = selectedValue[selectedValue.Length - 2];
                selected.Items.Clear();
                for (int i = 0; i < selectedValue.Length - 2; i++)
                {
                    selected.Items.Add(selectedValue[i]);
                }

                //irc.UpdateViewSelected(views.GetItemText(views.SelectedIndex));
                this.parseViews();
            }
        }

        public void setNextView()
        {
            if (views.SelectedIndex < views.Items.Count - 1)
            {
                views.SelectedIndex++;
            }
            else
            {
                views.SelectedIndex = 0;
            }

            this.parseViews();
        }

        public void setPreviousView()
        {
            if (views.SelectedIndex > 0)
            {
                views.SelectedIndex--;
            }
            else
            {
                views.SelectedIndex = views.Items.Count - 1;
            }

            this.parseViews();
        }

        public static List<string> get7SegmentData()
        {
            lock (listLock)
            {
                return _7Segment;
            }
        }

        public static string getStrFormat()
        {
            return strFormat;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isSearchingButton = tabControl1.SelectedIndex == 1;
            loadViewProperties();
        }

        private void addButtonToList(int index)
        {
            if (buttonsActive != null)
            {
                if (!buttonsActive.Items.Contains(BUTTON_PREFIX + index))
                {
                    buttonsActive.Items.Add(BUTTON_PREFIX + index);
                    buttonsActive.SelectedIndex = buttonsActive.Items.Count - 1;
                }
                else
                {
                    for (int x = 0; x < buttonsActive.Items.Count; x++)
                    {
                        if (buttonsActive.Items[x].ToString() == BUTTON_PREFIX + index)
                        {
                            buttonsActive.SelectedIndex = x;
                        }
                    }
                }
            }
        }

        private void processButton(string actionId, State state)
        {
            ActionHandler actionHandler = (new ActionHandlerFactory(this)).getInstance(actionId);
            
            if (actionHandler != null)
                actionHandler.process(actionId, state);
        }

        private void handleButtons(List<State> states)
        {
            if (states != null)
            {                                
                for (int x = 0; x < states.Count; x++)
                {
                    if (states[x] != State.None)
                    {
                        if (isSearchingButton)
                        {
                            this.addButtonToList(x);
                        }

                        for (int y = 0; y < bActions.Count; y++)
                        {
                            string[] split = bActions[y].ToString().Split(Utils.SIGN_EQUALS);
                            if (split[0] == BUTTON_PREFIX + x)
                            {
                                this.processButton(split[1], states[x]);
                                break;
                            }
                        }
                    }
                }
            }            
        }

        public void ButtonStateReceived(List<State> states)
        {
            if (this.views2.InvokeRequired)
            {
                this.BeginInvoke
                    (handleButtonActions, states);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(views2.SelectedIndex > -1)
            {
                views.Items.Remove(views2.SelectedIndex);
            }
        }

        private bool isButtonBinded(string buttonId)
        {
            foreach(string s in views2.Items) {
                string id = s.Split(Utils.SIGN_EQUALS)[0];
                if (buttonId.Equals(id))
                    return true;
            }

            return false;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if(buttonsActive.SelectedIndex > -1 && buttonActions.SelectedIndex > -1)
            {
                string buttonId = buttonsActive.SelectedItem.ToString();
                
                if(isButtonBinded(buttonId))
                {
                    MessageBox.Show(string.Format("Button {0} already binded.", buttonId),
                                "Important Note",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                    return;
                }

                string value = buttonId + Utils.SIGN_EQUALS + buttonActions.SelectedItem.ToString();
                if (!views2.Items.Contains(value))
                    views2.Items.Add(value);
            }
            else
            {
                MessageBox.Show("Please select a buttons and one action.",
                                "Important Note",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
            }

            syncViews();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (views2.SelectedIndex != -1)
            {
                for (int i = views2.SelectedItems.Count - 1; i >= 0; i--)
                    views2.Items.Remove(views2.SelectedItems[i]);
            }

            if (views2.Items.Count > 0)
                views2.SelectedIndex = 0;

            syncViews();
        }        

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.iRacingToolStripMenuItem1.CheckState = CheckState.Checked;

            //restore TM1637 and Buttons settings
            restoreViewProperties();

            if (this.views.Items.Count > 0)
            {
                this.views.SelectedIndex = 0;
                this.parseViews();
            }

            foreach (string s in ActionHandler.ACTIONS) {
                buttonActions.Items.Add(s);
            }
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            if (buttonsActive.SelectedIndex > -1)
            {                
                isWaitingForKey = true;
                label4.Visible = true;
                while(isWaitingForKey)
                {
                    if (label4.ForeColor == Color.Black)
                        label4.ForeColor = Color.Red;
                    else
                        label4.ForeColor = Color.Black;
                    await Task.Delay(250);
                }
            }
            else
            {
                MessageBox.Show("Please select a button.",
                                "Important Note",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
            }            
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isWaitingForKey)
            {
                isWaitingForKey = false;
                label4.Visible = false;
                string buttonId = buttonsActive.SelectedItem.ToString();

                if (isButtonBinded(buttonId))
                {
                    MessageBox.Show(string.Format("Button {0} already binded.", buttonId),
                                "Important Note",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                    return;
                }

                string value = buttonId + Utils.SIGN_EQUALS + e.KeyChar;
                if (!views2.Items.Contains(value))
                    views2.Items.Add(value);

                syncViews();
            }
        }

        private void resetConnectionUI()
        {
            irc = null;
            rrc = null;
            ToolStripMenuItem menu = (ToolStripMenuItem)mainmenu.Items[0];
            foreach (ToolStripMenuItem mItem in menu.DropDownItems)
            {
                mItem.CheckState = CheckState.Unchecked;
            }
        }

        private void resetAllSettings()
        {
            props.Items.Clear();
            selected.Items.Clear();

            ToolStripMenuItem menu = (ToolStripMenuItem)mainmenu.Items[1];
            foreach (ToolStripMenuItem mItem in menu.DropDownItems)
            {
                mItem.CheckState = CheckState.Unchecked;
            }
        }

        private void setButtonHandler()
        {
            for (int x = 0; x < settingsToolStripMenuItem.DropDownItems.Count; x++)
            {
                ToolStripMenuItem dropDownItem = (ToolStripMenuItem)simulatorToolStripMenuItem.DropDownItems[x];
                if (dropDownItem.Checked)
                {
                    if (x < ButtonsListBoxItems.Count)
                    {
                        bActions = ButtonsListBoxItems[x];
                    }

                    break;
                }
            }
        }

        private void iRacingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //update connection menu state
            resetConnectionUI();
            //stop iRacing threads
            stopIRacingThreads = false;
            //keep RaceRoom threads alive
            stopRaceRoomThreads = true;
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked; 
                       
            if (irc == null)
            {
                irc = new IRacingConnector(sm);
                irc.StatusMessageSubscribers += UpdateStatusBar;
            }

            this.iRacingToolStripMenuItem1.PerformClick();

            setButtonHandler();
        }

        private void raceroomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //update connection menu state
            resetConnectionUI();
            //keep iRacing threads alive
            stopIRacingThreads = true;
            //stop RaceRoom threads
            stopRaceRoomThreads = false;
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;

            if (rrc == null)
            {
                rrc = new RaceRoomConnector(sm);
                rrc.StatusMessageSubscribers += UpdateStatusBar;
            }

            this.raceRoomToolStripMenuItem1.PerformClick();

            setButtonHandler();
        }

        private void iRacingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            resetAllSettings();
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
            this.props.Items.AddRange(Constants.IRacingTelemetryData);

            loadViewProperties();
        }

        private void raceRoomToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            resetAllSettings();
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
            this.props.Items.AddRange(Constants.RaceRoomTelemetryData);

            loadViewProperties();
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //update connection menu state
            resetConnectionUI();
            //stop iRacing threads
            stopIRacingThreads = true;
            //keep RaceRoom threads alive
            stopRaceRoomThreads = true;
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
        }
    }
}
