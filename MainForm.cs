using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
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
        private AssettoCorsaConnector acc;
        private RFactorConnector ams;
        private RFactor2Connector rf2;

        private List<ArrayList> TM1637ListBoxItems = new List<ArrayList>(Constants.None - 1);
        private List<ArrayList> ButtonsListBoxItems = new List<ArrayList>(Constants.None - 1);
        private Dictionary<String, int> buttonStateMap = new Dictionary<String, int>();
        private ArrayList bActions = new ArrayList();

        public static bool stopThreads = false;
        public static bool stopIRacingThreads = false;
        public static bool stopRaceRoomThreads = false;
        public static bool stopAssettoThreads = false;
        public static bool stopRFactorThreads = false;
        public static bool stopRFactor2Threads = false;
        private static List<string> _7Segment = new List<string>();
        private static string strFormat = "";
        private static readonly Object listLock = new Object();
        private int selectedSimulator = Constants.IRacing;

        public delegate void AppendToStatusBarDelegate(String s);
        public AppendToStatusBarDelegate appendToStatusBar;
        public delegate void AppendToDebugDialogDelegate(String s);
        public AppendToDebugDialogDelegate appendToDebugDialog;      
          
        private bool isSearchingButton = false;
        private const string BUTTON_PREFIX = "Button_";
        private bool isWaitingForKey = false;

        public delegate void HandleButtonActions(List<State> states);
        public HandleButtonActions handleButtonActions;
        public static bool formFinishedLoading;

        private bool isSimulatorDisconnected = true;


        public MainForm()
        {
            Logger.LogMessageToFile("Initializing IDash.", true);
            try {
                this.appendToStatusBar = new AppendToStatusBarDelegate(UpdateStatusBar);
                this.appendToDebugDialog = new AppendToDebugDialogDelegate(AppendToDebugDialog);
                //Action Handlers have a pointer to Form, so they can only be initialized after the form.
                InitializeComponent();

                this.Shown += new System.EventHandler(FormLoadComplete);

                handleButtonActions = new HandleButtonActions(handleButtons);

                Logger.LogMessageToFile("Initializing serial manager.", true);
                sm = new SerialManager();
                sm.StatusMessageSubscribers += UpdateStatusBar;
                sm.DebugMessageSubscribers += UpdateDebugData;

                Logger.LogMessageToFile("Initializing button handler.", true);
                bh = new ButtonHandler(sm);
                bh.buttonStateHandler += ButtonStateReceived;
                //wait 1 second until ButtonHandler initializes, otherwise VJoyFeeder may crash.
                Thread.Sleep(1000);

                Logger.LogMessageToFile("Initializing Vjoy.", true);
                vf = new VJoyFeeder(bh);
                vf.StatusMessageSubscribers += UpdateStatusBar;

                this.iRacingToolStripMenuItem1.PerformClick();

                sm.Init();
                vf.InitializeJoystick();

                new Thread(new ThreadStart(sendDatetimeToArduino)).Start();
                AppendToDebugDialog("Instalation dir: " + AppDomain.CurrentDomain.BaseDirectory + "\n");
            }
            catch (Exception e)
            {
                Logger.LogExceptionToFile(e);
            }
        }


        /*private int getSelectedSimulator()
        {
            for (int x = 0; x < simulatorToolStripMenuItem.DropDownItems.Count - 1; x++)
            {
                ToolStripMenuItem dropDownItem = (ToolStripMenuItem)simulatorToolStripMenuItem.DropDownItems[x];
                if (dropDownItem.Checked)
                    return x;
            }

            return -1;
        }

        private int getSelectedConfiguration()
        {
            for (int x = 0; x < settingsToolStripMenuItem.DropDownItems.Count; x++)
            {
                ToolStripMenuItem dropDownItem = (ToolStripMenuItem)settingsToolStripMenuItem.DropDownItems[x];
                if (dropDownItem.Checked)
                    return x;
            }

            return -1;
        }*/

        private async void sendDatetimeToArduino()
        {
            while (isSimulatorDisconnected)
            {
                sm.sendCommand(Utils.getDisconnectedMsgCmd(), false);
                await Task.Delay(10);
            }
        }

        //parse the view dialog so 7 segment display nows which properties to show
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

        //update the temporary array where the settings changes are stored. i.e when a new view is added
        private void syncViews()
        {
            //int selectedSimulator = getSelectedConfiguration();
            if (selectedSimulator < settingsToolStripMenuItem.DropDownItems.Count)
            {
                ArrayList objCollection = new ArrayList();
                objCollection.AddRange(Utils.convertObjectCollectionToStringArray(views.Items));
                TM1637ListBoxItems[selectedSimulator] = objCollection;

                ArrayList objCollection2 = new ArrayList();
                objCollection2.AddRange(Utils.convertObjectCollectionToStringArray(views2.Items));
                ButtonsListBoxItems[selectedSimulator] = objCollection2;
            }
        }

        private void loadViewProperties(int simulatorIndex)
        {
            this.views.Items.Clear();
            this.views2.Items.Clear();
            this.selected.Items.Clear();
            this.textFormat.Clear();

            //int selectedSimulator = getSelectedConfiguration();

            if (simulatorIndex < TM1637ListBoxItems.Count)
            {
                this.views.Items.AddRange(TM1637ListBoxItems[simulatorIndex].ToArray());
            }
            if (simulatorIndex < ButtonsListBoxItems.Count)
            {
                this.views2.Items.AddRange(ButtonsListBoxItems[simulatorIndex].ToArray());
            }

            if (views.Items.Count > 0)
                views.SelectedIndex = 0;

            if (views2.Items.Count > 0)
                views2.SelectedIndex = 0;
        }

        private void restoreAppConfiguration()
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

            //Default to first simulator
            loadViewProperties(0);
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
            byte[] data = null;
            byte header = 0;
            if (cmdData.Text.Length > 0)
                data = Utils.convertStringToInt(cmdData.Text);
            if (cmdHeader.Text.Length > 0)
                header = (byte)Convert.ToInt16(cmdHeader.Text);
            Command command = new Command(header, data, true);
            sm.sendCommand(command, true);     //transmit data
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
            if (statusBar.Lines.Count() > 1000)
                statusBar.Clear();
            statusBar.AppendText(s);
        }

        public void UpdateStatusBar(string s)
        {
            if (this.statusBar.InvokeRequired)
                this.statusBar.BeginInvoke(this.appendToStatusBar, new Object[] { s });
            else this.AppendToStatusBar(s);
        }

        public void AppendToDebugDialog(String s)
        {
            debugData.AppendText(s);
        }

        public void UpdateDebugData(string s)
        {
            if (this.debugData.InvokeRequired)
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
            AppendToDebugDialog("Verbose data saved to: " + AppDomain.CurrentDomain.BaseDirectory + "log.log\n");

            //0 = none, 1 = default, 2 = verbose
            byte[] state = { (byte)debugModes.SelectedIndex };
            //update formd debug mode reference
            sm.formDebugMode = (DebugMode)debugModes.SelectedIndex;

            Command command = new Command(Command.CMD_SET_DEBUG_MODE, state);

            //make sure Arduino and IDash debug state are in sync
            while (state[0] != (int)sm.arduinoDebugMode)
            {                
                sm.sendCommand(command, false);     //transmit data
                await Task.Delay(WAIT_ARDUINO_SET_DEBUG_MODE);
                //update state if value in combobox changed
                state[0] = (byte)debugModes.SelectedIndex;
                sm.formDebugMode = (DebugMode)debugModes.SelectedIndex;
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

        private void deleteTemplate_Click(object sender, EventArgs e)
        {
            if (views.SelectedIndex != -1)
            {
                for (int i = views.SelectedItems.Count - 1; i >= 0; i--)
                    views.Items.Remove(views.SelectedItems[i]);

                syncViews();
            }

            if (views.Items.Count > 0)
                views.SelectedIndex = 0;            
        }

        private void addTemplate_Click(object sender, EventArgs e)
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
                    syncViews();
                }
            }            
        }

        private void viewUp_Click(object sender, EventArgs e)
        {
            if (views.SelectedIndex > 0)
            {
                this.MoveItem(views, -1);
                syncViews();
            }            
        }

        private void viewDown_Click(object sender, EventArgs e)
        {
            if (views.SelectedIndex < views.Items.Count - 1)
            {
                this.MoveItem(views, 1);
                syncViews();
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

        private void view2Up_Click(object sender, EventArgs e)
        {
            if (views2.SelectedIndex > 0)
            {
                this.MoveItem(views2, -1);
                syncViews();
            }
        }


        private void view2Down_Click(object sender, EventArgs e)
        {
            if (views2.SelectedIndex < views2.Items.Count - 1)
            {
                this.MoveItem(views2, 1);
                syncViews();
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

        private void leftTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            isSearchingButton = leftTab.SelectedIndex == 1;
            //loadViewProperties();
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

        //send key action to game
        private void handleButtons(List<State> states)
        {
            if (states != null)
            {                                
                for (int x = 0; x < states.Count; x++)
                {                    
                    if (states[x] != State.None)
                    {
                        //it is configuring button
                        if (isSearchingButton)
                        {
                            this.addButtonToList(x);
                        }
                        else
                        {
                            //send the key action to the game
                            for (int y = 0; y < bActions.Count; y++)
                            {
                                string[] split = bActions[y].ToString().Split(Utils.SIGN_EQUALS);
                                bool isAntiClockwise = split[0].Contains("-");
                                string buttonId = split[0].Replace("+", "").Replace("-", "");
                                if (buttonId.Equals(BUTTON_PREFIX + x))
                                {                                    
                                    string[] commandSplit = split[1].Split(Utils.LIST_SEPARATOR);
                                    int nextAction = 0;

                                    if (!buttonStateMap.ContainsKey(split[1]))
                                    {
                                        buttonStateMap.Add(split[1], 0);
                                    }
                                    else
                                    {
                                        if (isAntiClockwise)
                                        {
                                            nextAction = buttonStateMap[split[1]] - 1;
                                        }
                                        else
                                        {
                                            nextAction = buttonStateMap[split[1]] + 1;
                                        }
                                        if (nextAction == commandSplit.Length)
                                        {
                                            nextAction = 0;
                                        }
                                        if (nextAction < 0)
                                        {
                                            nextAction = commandSplit.Length - 1;
                                        }
                                    }

                                    buttonStateMap[split[1]] = nextAction;

                                    this.processButton(commandSplit[nextAction], states[x]);

                                    /*MessageBox.Show(string.Format("{0}", commandSplit[nextAction]),
                                        "Important Note",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button1);*/

                                    break;
                                }
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

        private int isButtonBinded(string buttonId)
        {
            int offset = 0;

            foreach(string s in views2.Items) {
                string id = s.Split(Utils.SIGN_EQUALS)[0];
                if (buttonId.Equals(id.Replace("+", "").Replace("-", "")))
                {
                    /*MessageBox.Show(string.Format("Button {0} already binded.", buttonId),
                                "Important Note",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);*/
                    return offset;
                }

                offset++;
            }

            return -1;
        }

        private void addButtonBind_Click(object sender, EventArgs e)
        {
            if(buttonsActive.SelectedIndex > -1 && buttonActions.SelectedIndex > -1)
            {
                string buttonId = buttonsActive.SelectedItem.ToString();

                int buttonBinded = isButtonBinded(buttonId);
                //button not binded yet
                if (buttonBinded >= 0)
                {
                    return;
                }

                string value = buttonId + Utils.SIGN_EQUALS + buttonActions.SelectedItem.ToString();

                if (!views2.Items.Contains(value))
                {
                    views2.Items.Add(value);
                    syncViews();
                }
            }
            else
            {
                MessageBox.Show("Please select a button and one action.",
                                "Important Note",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
            }            
        }

        private void deleteButtonBind_Click(object sender, EventArgs e)
        {
            if (views2.SelectedIndex != -1)
            {
                for (int i = views2.SelectedItems.Count - 1; i >= 0; i--)
                    views2.Items.Remove(views2.SelectedItems[i]);

                syncViews();
            }

            if (views2.Items.Count > 0)
                views2.SelectedIndex = 0;            
        }        

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.iRacingToolStripMenuItem1.CheckState = CheckState.Checked;

            //restore TM1637 and Buttons settings
            restoreAppConfiguration();

            if (this.views.Items.Count > 0)
            {
                this.views.SelectedIndex = 0;
                this.parseViews();
            }

            foreach (string s in ActionHandler.ACTIONS) {
                buttonActions.Items.Add(s);
            }
        }

        private async void keystroke_Click(object sender, EventArgs e)
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

        private void resetConnectionUI()
        {
            irc = null;
            rrc = null;
            acc = null;
            ams = null;
            rf2 = null;
            ToolStripMenuItem menu = (ToolStripMenuItem)mainmenu.Items[0];
            foreach (ToolStripMenuItem mItem in menu.DropDownItems)
            {
                mItem.CheckState = CheckState.Unchecked;
            }

            this.settingsToolStripMenuItem.Enabled = true;
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

        private void setButtonHandler(int simulatorIndex)
        {
            //int selectedSimulator = getSelectedSimulator();

            if (simulatorIndex < ButtonsListBoxItems.Count)
            {
                bActions = ButtonsListBoxItems[simulatorIndex];
            }
              
        }

        private void stopAllSimThreads()
        {
            //update connection menu state
            resetConnectionUI();
            //keep iRacing threads alive
            stopIRacingThreads = true;
            //stop RaceRoom threads
            stopRaceRoomThreads = true;
            //stop Assetto threads
            stopAssettoThreads = true;
            //stop rFactor threads
            stopRFactorThreads = true;
            //stop rFactor2 threads
            stopRFactor2Threads = true;
        }

        private void iRacingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSimulatorDisconnected = false;

            stopAllSimThreads();
            //keep iRacing threads alive
            stopIRacingThreads = false;

            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked; 
                       
            if (irc == null)
            {
                irc = new IRacingConnector(sm);
                irc.StatusMessageSubscribers += UpdateStatusBar;
            }

            this.iRacingToolStripMenuItem1.PerformClick();

            this.settingsToolStripMenuItem.Enabled = false;
            selectedSimulator = Constants.IRacing;

            setButtonHandler(selectedSimulator);
        }

        private void raceroomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSimulatorDisconnected = false;

            stopAllSimThreads();
            //keep RaceRoom threads alive
            stopRaceRoomThreads = false;
          
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;

            if (rrc == null)
            {
                rrc = new RaceRoomConnector(sm);
                rrc.StatusMessageSubscribers += UpdateStatusBar;
            }

            this.raceRoomToolStripMenuItem1.PerformClick();

            this.settingsToolStripMenuItem.Enabled = false;
            selectedSimulator = Constants.Raceroom;

            setButtonHandler(selectedSimulator);
        }               

        private void assettoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSimulatorDisconnected = false;

            stopAllSimThreads();        
            //keep Assetto threads alive
            stopAssettoThreads = false;

            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;

            if (acc == null)
            {
                acc = new AssettoCorsaConnector(sm);
                acc.StatusMessageSubscribers += UpdateStatusBar;
            }

            this.assettoToolStripMenuItem1.PerformClick();

            this.settingsToolStripMenuItem.Enabled = false;
            selectedSimulator = Constants.Assetto;

            setButtonHandler(selectedSimulator);
        }

        private void rFactorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSimulatorDisconnected = false;

            stopAllSimThreads();            
            //keep rFactor threads alive
            stopRFactorThreads = false;
            
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;

            if (ams == null)
            {
                ams = new RFactorConnector(sm);
                ams.StatusMessageSubscribers += UpdateStatusBar;
            }

            this.amsToolStripMenuItem1.PerformClick();

            this.settingsToolStripMenuItem.Enabled = false;
            selectedSimulator = Constants.RFactor;

            setButtonHandler(selectedSimulator);
        }

        private void rFactor2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSimulatorDisconnected = false;

            stopAllSimThreads();
            //keep rFactor2 threads alive
            stopRFactor2Threads = false;

            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;

            if (rf2 == null)
            {
                rf2 = new RFactor2Connector(sm);
                rf2.StatusMessageSubscribers += UpdateStatusBar;
            }

            this.rFactor2ToolStripMenuItem1.PerformClick();

            this.settingsToolStripMenuItem.Enabled = false;
            selectedSimulator = Constants.RFactor2;

            setButtonHandler(selectedSimulator);
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopAllSimThreads();

            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
            statusBar.AppendText("Simulator disconnected.\n");
            selectedSimulator = Constants.None;
            if (!isSimulatorDisconnected)
            {
                isSimulatorDisconnected = true;
                new Thread(new ThreadStart(sendDatetimeToArduino)).Start();
            }
        }

        private void iRacingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            resetAllSettings();
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
            this.props.Items.AddRange(Constants.IRacingTelemetryData);
            this.selectedSimulator = Constants.IRacing;

            loadViewProperties(selectedSimulator);
        }

        private void raceRoomToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            resetAllSettings();
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
            this.props.Items.AddRange(Constants.RaceRoomTelemetryData);
            this.selectedSimulator = Constants.Raceroom;

            loadViewProperties(selectedSimulator);
        }

        private void assettoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            resetAllSettings();
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
            this.props.Items.AddRange(Constants.AssettoTelemetryData);
            this.selectedSimulator = Constants.Assetto;

            loadViewProperties(selectedSimulator);
        }

        private void rFactorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            resetAllSettings();
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
            this.props.Items.AddRange(Constants.RFactorTelemetryData);
            this.selectedSimulator = Constants.RFactor;

            loadViewProperties(selectedSimulator);
        }

        private void rFactor2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            resetAllSettings();
            ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
            this.props.Items.AddRange(Constants.RFactor2TelemetryData);
            this.selectedSimulator = Constants.RFactor2;

            loadViewProperties(selectedSimulator);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (isWaitingForKey)
            {
                isWaitingForKey = false;
                label4.Visible = false;
                string buttonId = buttonsActive.SelectedItem.ToString();
                string buttonAction = "";
                int buttonBinded = isButtonBinded(buttonId);
                //button not binded yet
                if (buttonBinded < 0)
                {
                    buttonAction = buttonId + (isClockWise.Checked ? "+" : "-") + Utils.SIGN_EQUALS + e.KeyCode;
                    views2.Items.Add(buttonAction);
                }
                else
                {
                    string actionsBinded = views2.Items[buttonBinded].ToString().Split('=')[1];
                    buttonAction = actionsBinded + "," + e.KeyCode;
                    views2.Items[buttonBinded] = buttonId + (isClockWise.Checked ? "+" : "-") + Utils.SIGN_EQUALS + buttonAction;
                }                    

                syncViews();
            }
        }

        private void IsDisabledSerial_CheckedChanged(object sender, EventArgs e)
        {
            sm.isDisabledSerial = isDisabledSerial.Checked;
        }

        private void AsHex_CheckedChanged(object sender, EventArgs e)
        {
            sm.asHex = asHex.Checked;
        }

        private void FormLoadComplete(object sender, EventArgs e)
        {
            formFinishedLoading = true;
        }        
    }    
}
