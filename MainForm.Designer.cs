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
            this.cmdData = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.settingsTab = new System.Windows.Forms.TabPage();
            this.leftTab = new System.Windows.Forms.TabControl();
            this.tm1637 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.addTemplate = new System.Windows.Forms.Button();
            this.deleteTemplate = new System.Windows.Forms.Button();
            this.buttonViewDown = new System.Windows.Forms.Button();
            this.buttonViewUp = new System.Windows.Forms.Button();
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
            this.isClockWise = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.keystrokeButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.deleteButtonBind = new System.Windows.Forms.Button();
            this.buttonView2Down = new System.Windows.Forms.Button();
            this.buttonView2Up = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.addButtonBind = new System.Windows.Forms.Button();
            this.views2 = new System.Windows.Forms.ListBox();
            this.buttonActions = new System.Windows.Forms.ListBox();
            this.buttonsActive = new System.Windows.Forms.ListBox();
            this.debugTab = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmdHeader = new System.Windows.Forms.RichTextBox();
            this.asHex = new System.Windows.Forms.CheckBox();
            this.isDisabledSerial = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.debugModes = new System.Windows.Forms.ComboBox();
            this.clearData = new System.Windows.Forms.Button();
            this.debugData = new System.Windows.Forms.RichTextBox();
            this.statusBar = new System.Windows.Forms.RichTextBox();
            this.mainmenu = new System.Windows.Forms.MenuStrip();
            this.simulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iRacingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.raceroomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assettoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.amsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rFactor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iRacingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.raceRoomToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.assettoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.amsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rFactor2ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.isTestMode = new System.Windows.Forms.CheckBox();
            this.mainTab.SuspendLayout();
            this.settingsTab.SuspendLayout();
            this.leftTab.SuspendLayout();
            this.tm1637.SuspendLayout();
            this.buttons.SuspendLayout();
            this.debugTab.SuspendLayout();
            this.mainmenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(945, 9);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(97, 34);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.Text = "Send Command";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // cmdData
            // 
            this.cmdData.Location = new System.Drawing.Point(246, 14);
            this.cmdData.Name = "cmdData";
            this.cmdData.Size = new System.Drawing.Size(693, 21);
            this.cmdData.TabIndex = 6;
            this.cmdData.Text = "1-255-1-1-255-1-1-255-1-1-255-1-1-255-1-1-255-1-255-1-1-255-1-1-255-1-1-255-1-1-2" +
    "55-1-1-255-1-1-255-1-1-1-1-255-1-1-255-1-1-255";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(171, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "CMD_DATA:";
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
            this.settingsTab.Controls.Add(this.leftTab);
            this.settingsTab.Location = new System.Drawing.Point(4, 22);
            this.settingsTab.Name = "settingsTab";
            this.settingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.settingsTab.Size = new System.Drawing.Size(1564, 734);
            this.settingsTab.TabIndex = 0;
            this.settingsTab.Text = "Dashboard";
            this.settingsTab.UseVisualStyleBackColor = true;
            // 
            // leftTab
            // 
            this.leftTab.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.leftTab.Controls.Add(this.tm1637);
            this.leftTab.Controls.Add(this.buttons);
            this.leftTab.Location = new System.Drawing.Point(-4, 0);
            this.leftTab.Multiline = true;
            this.leftTab.Name = "leftTab";
            this.leftTab.SelectedIndex = 0;
            this.leftTab.Size = new System.Drawing.Size(1568, 733);
            this.leftTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.leftTab.TabIndex = 0;
            this.leftTab.SelectedIndexChanged += new System.EventHandler(this.leftTab_SelectedIndexChanged);
            // 
            // tm1637
            // 
            this.tm1637.Controls.Add(this.label7);
            this.tm1637.Controls.Add(this.label6);
            this.tm1637.Controls.Add(this.button11);
            this.tm1637.Controls.Add(this.addTemplate);
            this.tm1637.Controls.Add(this.deleteTemplate);
            this.tm1637.Controls.Add(this.buttonViewDown);
            this.tm1637.Controls.Add(this.buttonViewUp);
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
            // addTemplate
            // 
            this.addTemplate.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.addTemplate.Location = new System.Drawing.Point(1328, 144);
            this.addTemplate.Name = "addTemplate";
            this.addTemplate.Size = new System.Drawing.Size(33, 48);
            this.addTemplate.TabIndex = 13;
            this.addTemplate.UseVisualStyleBackColor = true;
            this.addTemplate.Click += new System.EventHandler(this.addTemplate_Click);
            // 
            // deleteTemplate
            // 
            this.deleteTemplate.Image = global::iDash.Properties.Resources.bin;
            this.deleteTemplate.Location = new System.Drawing.Point(1489, 664);
            this.deleteTemplate.Name = "deleteTemplate";
            this.deleteTemplate.Size = new System.Drawing.Size(46, 48);
            this.deleteTemplate.TabIndex = 12;
            this.deleteTemplate.UseVisualStyleBackColor = true;
            this.deleteTemplate.Click += new System.EventHandler(this.deleteTemplate_Click);
            // 
            // buttonViewDown
            // 
            this.buttonViewDown.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.buttonViewDown.Location = new System.Drawing.Point(1489, 459);
            this.buttonViewDown.Name = "buttonViewDown";
            this.buttonViewDown.Size = new System.Drawing.Size(33, 48);
            this.buttonViewDown.TabIndex = 11;
            this.buttonViewDown.UseVisualStyleBackColor = true;
            this.buttonViewDown.Click += new System.EventHandler(this.viewDown_Click);
            // 
            // buttonViewUp
            // 
            this.buttonViewUp.Image = global::iDash.Properties.Resources._112_UpArrowShort_Grey_32x42_72;
            this.buttonViewUp.Location = new System.Drawing.Point(1489, 391);
            this.buttonViewUp.Name = "buttonViewUp";
            this.buttonViewUp.Size = new System.Drawing.Size(33, 48);
            this.buttonViewUp.TabIndex = 10;
            this.buttonViewUp.UseVisualStyleBackColor = true;
            this.buttonViewUp.Click += new System.EventHandler(this.viewUp_Click);
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
            this.buttons.Controls.Add(this.isClockWise);
            this.buttons.Controls.Add(this.label4);
            this.buttons.Controls.Add(this.keystrokeButton);
            this.buttons.Controls.Add(this.label5);
            this.buttons.Controls.Add(this.deleteButtonBind);
            this.buttons.Controls.Add(this.buttonView2Down);
            this.buttons.Controls.Add(this.buttonView2Up);
            this.buttons.Controls.Add(this.button15);
            this.buttons.Controls.Add(this.button16);
            this.buttons.Controls.Add(this.addButtonBind);
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
            // isClockWise
            // 
            this.isClockWise.AutoSize = true;
            this.isClockWise.Checked = true;
            this.isClockWise.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isClockWise.Location = new System.Drawing.Point(838, 661);
            this.isClockWise.Name = "isClockWise";
            this.isClockWise.Size = new System.Drawing.Size(74, 17);
            this.isClockWise.TabIndex = 34;
            this.isClockWise.Text = "Clockwise";
            this.isClockWise.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1172, 661);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 33);
            this.label4.TabIndex = 33;
            this.label4.Text = "Press a key.";
            this.label4.Visible = false;
            // 
            // keystrokeButton
            // 
            this.keystrokeButton.Location = new System.Drawing.Point(974, 652);
            this.keystrokeButton.Name = "keystrokeButton";
            this.keystrokeButton.Size = new System.Drawing.Size(170, 48);
            this.keystrokeButton.TabIndex = 32;
            this.keystrokeButton.Text = "Map to Keyboard";
            this.keystrokeButton.UseVisualStyleBackColor = true;
            this.keystrokeButton.Click += new System.EventHandler(this.keystroke_Click);
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
            // deleteButtonBind
            // 
            this.deleteButtonBind.Image = global::iDash.Properties.Resources.bin;
            this.deleteButtonBind.Location = new System.Drawing.Point(1487, 661);
            this.deleteButtonBind.Name = "deleteButtonBind";
            this.deleteButtonBind.Size = new System.Drawing.Size(48, 48);
            this.deleteButtonBind.TabIndex = 28;
            this.deleteButtonBind.UseVisualStyleBackColor = true;
            this.deleteButtonBind.Click += new System.EventHandler(this.deleteButtonBind_Click);
            // 
            // buttonView2Down
            // 
            this.buttonView2Down.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.buttonView2Down.Location = new System.Drawing.Point(1487, 424);
            this.buttonView2Down.Name = "buttonView2Down";
            this.buttonView2Down.Size = new System.Drawing.Size(33, 48);
            this.buttonView2Down.TabIndex = 27;
            this.buttonView2Down.UseVisualStyleBackColor = true;
            this.buttonView2Down.Click += new System.EventHandler(this.view2Down_Click);
            // 
            // buttonView2Up
            // 
            this.buttonView2Up.Image = global::iDash.Properties.Resources._112_UpArrowShort_Grey_32x42_72;
            this.buttonView2Up.Location = new System.Drawing.Point(1487, 356);
            this.buttonView2Up.Name = "buttonView2Up";
            this.buttonView2Up.Size = new System.Drawing.Size(33, 48);
            this.buttonView2Up.TabIndex = 26;
            this.buttonView2Up.UseVisualStyleBackColor = true;
            this.buttonView2Up.Click += new System.EventHandler(this.view2Up_Click);
            // 
            // button15
            // 
            this.button15.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.button15.Location = new System.Drawing.Point(1487, 95);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(33, 48);
            this.button15.TabIndex = 25;
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.Image = global::iDash.Properties.Resources._112_UpArrowShort_Grey_32x42_72;
            this.button16.Location = new System.Drawing.Point(1487, 27);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(33, 48);
            this.button16.TabIndex = 24;
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // addButtonBind
            // 
            this.addButtonBind.Image = global::iDash.Properties.Resources._112_RightArrowShort_Grey_32x32_72;
            this.addButtonBind.Location = new System.Drawing.Point(744, 388);
            this.addButtonBind.Name = "addButtonBind";
            this.addButtonBind.Size = new System.Drawing.Size(48, 34);
            this.addButtonBind.TabIndex = 22;
            this.addButtonBind.UseVisualStyleBackColor = true;
            this.addButtonBind.Click += new System.EventHandler(this.addButtonBind_Click);
            // 
            // views2
            // 
            this.views2.FormattingEnabled = true;
            this.views2.Location = new System.Drawing.Point(838, 196);
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
            this.debugTab.Controls.Add(this.isTestMode);
            this.debugTab.Controls.Add(this.label9);
            this.debugTab.Controls.Add(this.label8);
            this.debugTab.Controls.Add(this.cmdHeader);
            this.debugTab.Controls.Add(this.asHex);
            this.debugTab.Controls.Add(this.isDisabledSerial);
            this.debugTab.Controls.Add(this.label2);
            this.debugTab.Controls.Add(this.debugModes);
            this.debugTab.Controls.Add(this.clearData);
            this.debugTab.Controls.Add(this.debugData);
            this.debugTab.Controls.Add(this.label1);
            this.debugTab.Controls.Add(this.buttonSend);
            this.debugTab.Controls.Add(this.cmdData);
            this.debugTab.Location = new System.Drawing.Point(4, 22);
            this.debugTab.Name = "debugTab";
            this.debugTab.Padding = new System.Windows.Forms.Padding(3);
            this.debugTab.Size = new System.Drawing.Size(1564, 734);
            this.debugTab.TabIndex = 1;
            this.debugTab.Text = "Debug";
            this.debugTab.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(1090, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(373, 78);
            this.label9.TabIndex = 19;
            this.label9.Text = resources.GetString("label9.Text");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "CMD_HEADER:";
            // 
            // cmdHeader
            // 
            this.cmdHeader.Location = new System.Drawing.Point(100, 14);
            this.cmdHeader.Name = "cmdHeader";
            this.cmdHeader.Size = new System.Drawing.Size(42, 21);
            this.cmdHeader.TabIndex = 17;
            this.cmdHeader.Text = "67";
            // 
            // asHex
            // 
            this.asHex.AutoSize = true;
            this.asHex.Location = new System.Drawing.Point(945, 71);
            this.asHex.Name = "asHex";
            this.asHex.Size = new System.Drawing.Size(129, 17);
            this.asHex.TabIndex = 16;
            this.asHex.Text = "Show as hexadecimal";
            this.asHex.UseVisualStyleBackColor = true;
            this.asHex.CheckedChanged += new System.EventHandler(this.AsHex_CheckedChanged);
            // 
            // isDisabledSerial
            // 
            this.isDisabledSerial.AutoSize = true;
            this.isDisabledSerial.Location = new System.Drawing.Point(945, 48);
            this.isDisabledSerial.Name = "isDisabledSerial";
            this.isDisabledSerial.Size = new System.Drawing.Size(125, 17);
            this.isDisabledSerial.TabIndex = 15;
            this.isDisabledSerial.Text = "Ignore incoming data";
            this.isDisabledSerial.UseVisualStyleBackColor = true;
            this.isDisabledSerial.CheckedChanged += new System.EventHandler(this.IsDisabledSerial_CheckedChanged);
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
            this.assettoToolStripMenuItem,
            this.amsToolStripMenuItem,
            this.rFactor2ToolStripMenuItem,
            this.noneToolStripMenuItem});
            this.simulatorToolStripMenuItem.Name = "simulatorToolStripMenuItem";
            this.simulatorToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.simulatorToolStripMenuItem.Text = "Connect";
            // 
            // iRacingToolStripMenuItem
            // 
            this.iRacingToolStripMenuItem.Name = "iRacingToolStripMenuItem";
            this.iRacingToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.iRacingToolStripMenuItem.Text = "IRacing";
            this.iRacingToolStripMenuItem.Click += new System.EventHandler(this.iRacingToolStripMenuItem_Click);
            // 
            // raceroomToolStripMenuItem
            // 
            this.raceroomToolStripMenuItem.Name = "raceroomToolStripMenuItem";
            this.raceroomToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.raceroomToolStripMenuItem.Text = "Raceroom";
            this.raceroomToolStripMenuItem.Click += new System.EventHandler(this.raceroomToolStripMenuItem_Click);
            // 
            // assettoToolStripMenuItem
            // 
            this.assettoToolStripMenuItem.Name = "assettoToolStripMenuItem";
            this.assettoToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.assettoToolStripMenuItem.Text = "Assetto Corsa";
            this.assettoToolStripMenuItem.Click += new System.EventHandler(this.assettoToolStripMenuItem_Click);
            // 
            // amsToolStripMenuItem
            // 
            this.amsToolStripMenuItem.Name = "amsToolStripMenuItem";
            this.amsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.amsToolStripMenuItem.Text = "Automobilista";
            this.amsToolStripMenuItem.Click += new System.EventHandler(this.rFactorToolStripMenuItem_Click);
            // 
            // rFactor2ToolStripMenuItem
            // 
            this.rFactor2ToolStripMenuItem.Name = "rFactor2ToolStripMenuItem";
            this.rFactor2ToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.rFactor2ToolStripMenuItem.Text = "RFactor2";
            this.rFactor2ToolStripMenuItem.Click += new System.EventHandler(this.rFactor2ToolStripMenuItem_Click);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.noneToolStripMenuItem.Text = "None";
            this.noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iRacingToolStripMenuItem1,
            this.raceRoomToolStripMenuItem1,
            this.assettoToolStripMenuItem1,
            this.amsToolStripMenuItem1,
            this.rFactor2ToolStripMenuItem1});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // iRacingToolStripMenuItem1
            // 
            this.iRacingToolStripMenuItem1.Name = "iRacingToolStripMenuItem1";
            this.iRacingToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.iRacingToolStripMenuItem1.Text = "iRacing";
            this.iRacingToolStripMenuItem1.Click += new System.EventHandler(this.iRacingToolStripMenuItem1_Click);
            // 
            // raceRoomToolStripMenuItem1
            // 
            this.raceRoomToolStripMenuItem1.Name = "raceRoomToolStripMenuItem1";
            this.raceRoomToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.raceRoomToolStripMenuItem1.Text = "RaceRoom";
            this.raceRoomToolStripMenuItem1.Click += new System.EventHandler(this.raceRoomToolStripMenuItem1_Click);
            // 
            // assettoToolStripMenuItem1
            // 
            this.assettoToolStripMenuItem1.Name = "assettoToolStripMenuItem1";
            this.assettoToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.assettoToolStripMenuItem1.Text = "Assetto Corsa";
            this.assettoToolStripMenuItem1.Click += new System.EventHandler(this.assettoToolStripMenuItem1_Click);
            // 
            // amsToolStripMenuItem1
            // 
            this.amsToolStripMenuItem1.Name = "amsToolStripMenuItem1";
            this.amsToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.amsToolStripMenuItem1.Text = "Automobilista";
            this.amsToolStripMenuItem1.Click += new System.EventHandler(this.rFactorToolStripMenuItem1_Click);
            // 
            // rFactor2ToolStripMenuItem1
            // 
            this.rFactor2ToolStripMenuItem1.Name = "rFactor2ToolStripMenuItem1";
            this.rFactor2ToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.rFactor2ToolStripMenuItem1.Text = "RFactor2";
            this.rFactor2ToolStripMenuItem1.Click += new System.EventHandler(this.rFactor2ToolStripMenuItem1_Click);
            // 
            // isTestMode
            // 
            this.isTestMode.AutoSize = true;
            this.isTestMode.Location = new System.Drawing.Point(814, 47);
            this.isTestMode.Name = "isTestMode";
            this.isTestMode.Size = new System.Drawing.Size(108, 17);
            this.isTestMode.TabIndex = 20;
            this.isTestMode.Text = "Enable test mode";
            this.isTestMode.UseVisualStyleBackColor = true;
            this.isTestMode.CheckedChanged += new System.EventHandler(this.isTestMode_CheckedChanged);
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
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.mainTab.ResumeLayout(false);
            this.settingsTab.ResumeLayout(false);
            this.leftTab.ResumeLayout(false);
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
        private System.Windows.Forms.RichTextBox cmdData;
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
        private System.Windows.Forms.TabControl leftTab;
        private System.Windows.Forms.TabPage tm1637;
        private System.Windows.Forms.TabPage buttons;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox views;
        private System.Windows.Forms.ListBox selected;
        private System.Windows.Forms.ListBox props;
        private System.Windows.Forms.Button addTemplate;
        private System.Windows.Forms.Button deleteTemplate;
        private System.Windows.Forms.Button buttonViewDown;
        private System.Windows.Forms.Button buttonViewUp;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button buttonView2Down;
        private System.Windows.Forms.Button buttonView2Up;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.ListBox views2;
        private System.Windows.Forms.ListBox buttonActions;
        private System.Windows.Forms.ListBox buttonsActive;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox isSimConnected;
        private System.Windows.Forms.Button keystrokeButton;
        private System.Windows.Forms.Button deleteButtonBind;
        private System.Windows.Forms.Button addButtonBind;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip mainmenu;
        private System.Windows.Forms.ToolStripMenuItem simulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iRacingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem raceroomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iRacingToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem raceRoomToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assettoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assettoToolStripMenuItem1;
        private System.Windows.Forms.CheckBox isDisabledSerial;
        private System.Windows.Forms.CheckBox asHex;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox cmdHeader;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripMenuItem amsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem amsToolStripMenuItem1;
        private System.Windows.Forms.CheckBox isClockWise;
        private System.Windows.Forms.ToolStripMenuItem rFactor2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rFactor2ToolStripMenuItem1;
        private System.Windows.Forms.CheckBox isTestMode;
    }
}

