using System;

namespace iDash
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonSend = new System.Windows.Forms.Button();
            this.richTextBoxSend = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.settingsTab = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tm1637 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.isSimConnected = new System.Windows.Forms.CheckBox();
            this.textFormat = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.views = new System.Windows.Forms.ListBox();
            this.selected = new System.Windows.Forms.ListBox();
            this.props = new System.Windows.Forms.ListBox();
            this.buttons = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.views2 = new System.Windows.Forms.ListBox();
            this.buttonActions = new System.Windows.Forms.ListBox();
            this.buttonsActive = new System.Windows.Forms.ListBox();
            this.debugTab = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.debugModes = new System.Windows.Forms.ComboBox();
            this.clearData = new System.Windows.Forms.Button();
            this.debugData = new System.Windows.Forms.RichTextBox();
            this.statusBar = new System.Windows.Forms.RichTextBox();
            this.mainmenu = new System.Windows.Forms.MenuStrip();
            this.simulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iRacingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.raceroomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iRacingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.raceRoomToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTab.SuspendLayout();
            this.settingsTab.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tm1637.SuspendLayout();
            this.buttons.SuspendLayout();
            this.debugTab.SuspendLayout();
            this.mainmenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(689, 11);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(97, 34);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.Text = "Send Command";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // richTextBoxSend
            // 
            this.richTextBoxSend.Location = new System.Drawing.Point(68, 19);
            this.richTextBoxSend.Name = "richTextBoxSend";
            this.richTextBoxSend.Size = new System.Drawing.Size(615, 21);
            this.richTextBoxSend.TabIndex = 6;
            this.richTextBoxSend.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Send data";
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.settingsTab);
            this.mainTab.Controls.Add(this.debugTab);
            this.mainTab.Location = new System.Drawing.Point(3, 27);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(1572, 760);
            this.mainTab.TabIndex = 10;
            // 
            // settingsTab
            // 
            this.settingsTab.Controls.Add(this.tabControl1);
            this.settingsTab.Location = new System.Drawing.Point(4, 22);
            this.settingsTab.Name = "settingsTab";
            this.settingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.settingsTab.Size = new System.Drawing.Size(1564, 734);
            this.settingsTab.TabIndex = 0;
            this.settingsTab.Text = "Dashboard";
            this.settingsTab.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Controls.Add(this.tm1637);
            this.tabControl1.Controls.Add(this.buttons);
            this.tabControl1.Location = new System.Drawing.Point(-4, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1568, 733);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tm1637
            // 
            this.tm1637.Controls.Add(this.label7);
            this.tm1637.Controls.Add(this.label6);
            this.tm1637.Controls.Add(this.button11);
            this.tm1637.Controls.Add(this.button7);
            this.tm1637.Controls.Add(this.button8);
            this.tm1637.Controls.Add(this.button5);
            this.tm1637.Controls.Add(this.button6);
            this.tm1637.Controls.Add(this.button4);
            this.tm1637.Controls.Add(this.button3);
            this.tm1637.Controls.Add(this.button2);
            this.tm1637.Controls.Add(this.button1);
            this.tm1637.Controls.Add(this.isSimConnected);
            this.tm1637.Controls.Add(this.textFormat);
            this.tm1637.Controls.Add(this.label3);
            this.tm1637.Controls.Add(this.views);
            this.tm1637.Controls.Add(this.selected);
            this.tm1637.Controls.Add(this.props);
            this.tm1637.Location = new System.Drawing.Point(23, 4);
            this.tm1637.Name = "tm1637";
            this.tm1637.Padding = new System.Windows.Forms.Padding(3);
            this.tm1637.Size = new System.Drawing.Size(1541, 725);
            this.tm1637.TabIndex = 0;
            this.tm1637.Text = "TM1637";
            this.tm1637.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(840, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(329, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "i.e. pl=3&&0 (pad left with 0s). Regex can be used. use \';\' as separator";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(707, 619);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 31);
            this.label6.TabIndex = 32;
            this.label6.Text = "TM1637";
            // 
            // button11
            // 
            this.button11.Image = global::iDash.Properties.Resources.PrintEntireDocument;
            this.button11.Location = new System.Drawing.Point(752, 169);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(48, 41);
            this.button11.TabIndex = 16;
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button7
            // 
            this.button7.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.button7.Location = new System.Drawing.Point(1328, 144);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(33, 48);
            this.button7.TabIndex = 13;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Image = global::iDash.Properties.Resources.bin;
            this.button8.Location = new System.Drawing.Point(1489, 664);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(46, 48);
            this.button8.TabIndex = 12;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button5
            // 
            this.button5.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.button5.Location = new System.Drawing.Point(1489, 459);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(33, 48);
            this.button5.TabIndex = 11;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Image = global::iDash.Properties.Resources._112_UpArrowShort_Grey_32x42_72;
            this.button6.Location = new System.Drawing.Point(1489, 391);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(33, 48);
            this.button6.TabIndex = 10;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.button4.Location = new System.Drawing.Point(1489, 74);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(33, 48);
            this.button4.TabIndex = 9;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Image = global::iDash.Properties.Resources._112_UpArrowShort_Grey_32x42_72;
            this.button3.Location = new System.Drawing.Point(1489, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(33, 48);
            this.button3.TabIndex = 8;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Image = global::iDash.Properties.Resources._112_LeftArrowShort_Grey_32x32_72;
            this.button2.Location = new System.Drawing.Point(752, 74);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 34);
            this.button2.TabIndex = 7;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = global::iDash.Properties.Resources._112_RightArrowShort_Grey_32x32_72;
            this.button1.Location = new System.Drawing.Point(752, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 34);
            this.button1.TabIndex = 6;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // isSimConnected
            // 
            this.isSimConnected.AutoSize = true;
            this.isSimConnected.Location = new System.Drawing.Point(1389, 162);
            this.isSimConnected.Name = "isSimConnected";
            this.isSimConnected.Size = new System.Drawing.Size(109, 17);
            this.isSimConnected.TabIndex = 5;
            this.isSimConnected.Text = "When connected";
            this.isSimConnected.UseVisualStyleBackColor = true;
            // 
            // textFormat
            // 
            this.textFormat.Location = new System.Drawing.Point(906, 159);
            this.textFormat.Name = "textFormat";
            this.textFormat.Size = new System.Drawing.Size(400, 20);
            this.textFormat.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(834, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Text Format:";
            // 
            // views
            // 
            this.views.FormattingEnabled = true;
            this.views.Location = new System.Drawing.Point(840, 215);
            this.views.Name = "views";
            this.views.Size = new System.Drawing.Size(643, 498);
            this.views.TabIndex = 2;
            this.views.SelectedIndexChanged += new System.EventHandler(this.views_SelectedIndexChanged);
            // 
            // selected
            // 
            this.selected.FormattingEnabled = true;
            this.selected.Location = new System.Drawing.Point(840, 5);
            this.selected.Name = "selected";
            this.selected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.selected.Size = new System.Drawing.Size(643, 121);
            this.selected.TabIndex = 1;
            // 
            // props
            // 
            this.props.FormattingEnabled = true;
            this.props.Location = new System.Drawing.Point(6, 6);
            this.props.Name = "props";
            this.props.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.props.Size = new System.Drawing.Size(695, 706);
            this.props.TabIndex = 0;
            // 
            // buttons
            // 
            this.buttons.Controls.Add(this.label4);
            this.buttons.Controls.Add(this.button9);
            this.buttons.Controls.Add(this.label5);
            this.buttons.Controls.Add(this.button12);
            this.buttons.Controls.Add(this.button13);
            this.buttons.Controls.Add(this.button14);
            this.buttons.Controls.Add(this.button15);
            this.buttons.Controls.Add(this.button16);
            this.buttons.Controls.Add(this.button18);
            this.buttons.Controls.Add(this.views2);
            this.buttons.Controls.Add(this.buttonActions);
            this.buttons.Controls.Add(this.buttonsActive);
            this.buttons.Location = new System.Drawing.Point(23, 4);
            this.buttons.Name = "buttons";
            this.buttons.Padding = new System.Windows.Forms.Padding(3);
            this.buttons.Size = new System.Drawing.Size(1541, 725);
            this.buttons.TabIndex = 1;
            this.buttons.Text = "Buttons";
            this.buttons.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1111, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 33);
            this.label4.TabIndex = 33;
            this.label4.Text = "Press a key.";
            this.label4.Visible = false;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(913, 204);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(170, 48);
            this.button9.TabIndex = 32;
            this.button9.Text = "Map to Key";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(705, 617);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 31);
            this.label5.TabIndex = 31;
            this.label5.Text = "Buttons";
            // 
            // button12
            // 
            this.button12.Image = global::iDash.Properties.Resources.bin;
            this.button12.Location = new System.Drawing.Point(1487, 661);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(48, 48);
            this.button12.TabIndex = 28;
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.button13.Location = new System.Drawing.Point(1487, 456);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(33, 48);
            this.button13.TabIndex = 27;
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Image = global::iDash.Properties.Resources._112_UpArrowShort_Grey_32x42_72;
            this.button14.Location = new System.Drawing.Point(1487, 388);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(33, 48);
            this.button14.TabIndex = 26;
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.button15.Location = new System.Drawing.Point(1487, 71);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(33, 48);
            this.button15.TabIndex = 25;
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.Image = global::iDash.Properties.Resources._112_UpArrowShort_Grey_32x42_72;
            this.button16.Location = new System.Drawing.Point(1487, 3);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(33, 48);
            this.button16.TabIndex = 24;
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button18
            // 
            this.button18.Image = global::iDash.Properties.Resources._112_RightArrowShort_Grey_32x32_72;
            this.button18.Location = new System.Drawing.Point(746, 417);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(48, 34);
            this.button18.TabIndex = 22;
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // views2
            // 
            this.views2.FormattingEnabled = true;
            this.views2.Location = new System.Drawing.Point(838, 277);
            this.views2.Name = "views2";
            this.views2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.views2.Size = new System.Drawing.Size(643, 433);
            this.views2.TabIndex = 19;
            // 
            // buttonActions
            // 
            this.buttonActions.FormattingEnabled = true;
            this.buttonActions.Location = new System.Drawing.Point(838, 2);
            this.buttonActions.Name = "buttonActions";
            this.buttonActions.Size = new System.Drawing.Size(643, 173);
            this.buttonActions.TabIndex = 18;
            // 
            // buttonsActive
            // 
            this.buttonsActive.FormattingEnabled = true;
            this.buttonsActive.Location = new System.Drawing.Point(4, 3);
            this.buttonsActive.Name = "buttonsActive";
            this.buttonsActive.Size = new System.Drawing.Size(695, 706);
            this.buttonsActive.TabIndex = 17;
            // 
            // debugTab
            // 
            this.debugTab.Controls.Add(this.label2);
            this.debugTab.Controls.Add(this.debugModes);
            this.debugTab.Controls.Add(this.clearData);
            this.debugTab.Controls.Add(this.debugData);
            this.debugTab.Controls.Add(this.label1);
            this.debugTab.Controls.Add(this.buttonSend);
            this.debugTab.Controls.Add(this.richTextBoxSend);
            this.debugTab.Location = new System.Drawing.Point(4, 22);
            this.debugTab.Name = "debugTab";
            this.debugTab.Padding = new System.Windows.Forms.Padding(3);
            this.debugTab.Size = new System.Drawing.Size(1564, 734);
            this.debugTab.TabIndex = 1;
            this.debugTab.Text = "Debug";
            this.debugTab.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Mode";
            // 
            // debugModes
            // 
            this.debugModes.FormattingEnabled = true;
            this.debugModes.Items.AddRange(new object[] {
            iDash.DebugMode.None,
            iDash.DebugMode.Default,
            iDash.DebugMode.Verbose});
            this.debugModes.Location = new System.Drawing.Point(9, 67);
            this.debugModes.Name = "debugModes";
            this.debugModes.Size = new System.Drawing.Size(165, 21);
            this.debugModes.TabIndex = 13;
            this.debugModes.SelectedIndexChanged += new System.EventHandler(this.debugModes_SelectedIndexChanged);
            // 
            // clearData
            // 
            this.clearData.Location = new System.Drawing.Point(689, 56);
            this.clearData.Name = "clearData";
            this.clearData.Size = new System.Drawing.Size(97, 34);
            this.clearData.TabIndex = 12;
            this.clearData.Text = "Clear All";
            this.clearData.UseVisualStyleBackColor = true;
            this.clearData.Click += new System.EventHandler(this.clearData_Click);
            // 
            // debugData
            // 
            this.debugData.Location = new System.Drawing.Point(9, 94);
            this.debugData.Name = "debugData";
            this.debugData.Size = new System.Drawing.Size(1555, 637);
            this.debugData.TabIndex = 10;
            this.debugData.Text = "";
            this.debugData.TextChanged += new System.EventHandler(this.debugData_TextChanged);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(3, 793);
            this.statusBar.Name = "statusBar";
            this.statusBar.ReadOnly = true;
            this.statusBar.Size = new System.Drawing.Size(1568, 187);
            this.statusBar.TabIndex = 0;
            this.statusBar.Text = "";
            this.statusBar.TextChanged += new System.EventHandler(this.statusBar_TextChanged);
            // 
            // mainmenu
            // 
            this.mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.simulatorToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.mainmenu.Location = new System.Drawing.Point(0, 0);
            this.mainmenu.Name = "mainmenu";
            this.mainmenu.Size = new System.Drawing.Size(1575, 24);
            this.mainmenu.TabIndex = 11;
            this.mainmenu.Text = "menuStrip1";
            // 
            // simulatorToolStripMenuItem
            // 
            this.simulatorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iRacingToolStripMenuItem,
            this.raceroomToolStripMenuItem,
            this.noneToolStripMenuItem});
            this.simulatorToolStripMenuItem.Name = "simulatorToolStripMenuItem";
            this.simulatorToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.simulatorToolStripMenuItem.Text = "Connect";
            // 
            // iRacingToolStripMenuItem
            // 
            this.iRacingToolStripMenuItem.Name = "iRacingToolStripMenuItem";
            this.iRacingToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.iRacingToolStripMenuItem.Text = "IRacing";
            this.iRacingToolStripMenuItem.Click += new System.EventHandler(this.iRacingToolStripMenuItem_Click);
            // 
            // raceroomToolStripMenuItem
            // 
            this.raceroomToolStripMenuItem.Name = "raceroomToolStripMenuItem";
            this.raceroomToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.raceroomToolStripMenuItem.Text = "Raceroom";
            this.raceroomToolStripMenuItem.Click += new System.EventHandler(this.raceroomToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iRacingToolStripMenuItem1,
            this.raceRoomToolStripMenuItem1});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // iRacingToolStripMenuItem1
            // 
            this.iRacingToolStripMenuItem1.Name = "iRacingToolStripMenuItem1";
            this.iRacingToolStripMenuItem1.Size = new System.Drawing.Size(131, 22);
            this.iRacingToolStripMenuItem1.Text = "iRacing";
            this.iRacingToolStripMenuItem1.Click += new System.EventHandler(this.iRacingToolStripMenuItem1_Click);
            // 
            // raceRoomToolStripMenuItem1
            // 
            this.raceRoomToolStripMenuItem1.Name = "raceRoomToolStripMenuItem1";
            this.raceRoomToolStripMenuItem1.Size = new System.Drawing.Size(131, 22);
            this.raceRoomToolStripMenuItem1.Text = "RaceRoom";
            this.raceRoomToolStripMenuItem1.Click += new System.EventHandler(this.raceRoomToolStripMenuItem1_Click);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.noneToolStripMenuItem.Text = "None";
            this.noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1575, 1017);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.mainTab);
            this.Controls.Add(this.mainmenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainmenu;
            this.Name = "MainForm";
            this.Text = "iDash";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.mainTab.ResumeLayout(false);
            this.settingsTab.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tm1637.ResumeLayout(false);
            this.tm1637.PerformLayout();
            this.buttons.ResumeLayout(false);
            this.buttons.PerformLayout();
            this.debugTab.ResumeLayout(false);
            this.debugTab.PerformLayout();
            this.mainmenu.ResumeLayout(false);
            this.mainmenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.RichTextBox richTextBoxSend;
        private System.Windows.Forms.Label label1;

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage settingsTab;
        private System.Windows.Forms.TabPage debugTab;
        private System.Windows.Forms.RichTextBox statusBar;
        private System.Windows.Forms.Button clearData;
        private System.Windows.Forms.RichTextBox debugData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox debugModes;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tm1637;
        private System.Windows.Forms.TabPage buttons;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox views;
        private System.Windows.Forms.ListBox selected;
        private System.Windows.Forms.ListBox props;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.ListBox views2;
        private System.Windows.Forms.ListBox buttonActions;
        private System.Windows.Forms.ListBox buttonsActive;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox isSimConnected;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip mainmenu;
        private System.Windows.Forms.ToolStripMenuItem simulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iRacingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem raceroomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iRacingToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem raceRoomToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
    }
}

