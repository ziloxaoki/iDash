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
            this.debugTab = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.debugModes = new System.Windows.Forms.ComboBox();
            this.clearData = new System.Windows.Forms.Button();
            this.debugData = new System.Windows.Forms.RichTextBox();
            this.statusBar = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tm1637 = new System.Windows.Forms.TabPage();
            this.max7221 = new System.Windows.Forms.TabPage();
            this.props = new System.Windows.Forms.ListBox();
            this.selected = new System.Windows.Forms.ListBox();
            this.views = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.isSimConnected = new System.Windows.Forms.CheckBox();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.mainTab.SuspendLayout();
            this.settingsTab.SuspendLayout();
            this.debugTab.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tm1637.SuspendLayout();
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
            this.mainTab.Location = new System.Drawing.Point(2, 12);
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
            this.settingsTab.Text = "Settings";
            this.settingsTab.UseVisualStyleBackColor = true;
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
            this.statusBar.Location = new System.Drawing.Point(2, 771);
            this.statusBar.Name = "statusBar";
            this.statusBar.ReadOnly = true;
            this.statusBar.Size = new System.Drawing.Size(1568, 187);
            this.statusBar.TabIndex = 0;
            this.statusBar.Text = "";
            this.statusBar.TextChanged += new System.EventHandler(this.statusBar_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Controls.Add(this.tm1637);
            this.tabControl1.Controls.Add(this.max7221);
            this.tabControl1.Location = new System.Drawing.Point(-4, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1568, 733);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tm1637
            // 
            this.tm1637.Controls.Add(this.button11);
            this.tm1637.Controls.Add(this.button10);
            this.tm1637.Controls.Add(this.button9);
            this.tm1637.Controls.Add(this.button7);
            this.tm1637.Controls.Add(this.button8);
            this.tm1637.Controls.Add(this.button5);
            this.tm1637.Controls.Add(this.button6);
            this.tm1637.Controls.Add(this.button4);
            this.tm1637.Controls.Add(this.button3);
            this.tm1637.Controls.Add(this.button2);
            this.tm1637.Controls.Add(this.button1);
            this.tm1637.Controls.Add(this.isSimConnected);
            this.tm1637.Controls.Add(this.textBox1);
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
            // max7221
            // 
            this.max7221.Location = new System.Drawing.Point(23, 4);
            this.max7221.Name = "max7221";
            this.max7221.Padding = new System.Windows.Forms.Padding(3);
            this.max7221.Size = new System.Drawing.Size(1541, 725);
            this.max7221.TabIndex = 1;
            this.max7221.Text = "MAX7221";
            this.max7221.UseVisualStyleBackColor = true;
            // 
            // props
            // 
            this.props.FormattingEnabled = true;
            this.props.Items.AddRange(new object[] {
            "AirDensity ",
            "AirPressure ",
            "AirTemp",
            "Alt ",
            "Brake ",
            "BrakeRaw",
            "CamCameraNumber ",
            "CamCameraState ",
            "CamCarIdx ",
            "CamGroupNumber ",
            "Clutch",
            "CpuUsageBG ",
            "DCDriversSoFar ",
            "DCLapStatus ",
            "DisplayUnits ",
            "DriverMarker ",
            "EngineWarnings ",
            "EnterExitReset ",
            "FogLevel ",
            "FrameRate ",
            "FuelLevel ",
            "FuelLevelPct ",
            "FuelPress ",
            "FuelUsePerHour",
            "Gear",
            "IsDiskLoggingActive",
            "IsDiskLoggingEnabled",
            "IsInGarage",
            "IsOnTrack",
            "IsOnTrackCar",
            "IsReplayPlaying",
            "Lap",
            "LapBestLap",
            "LapBestLapTime",
            "LapBestNLapLap",
            "LapBestNLapTime",
            "LapCurrentLapTime",
            "LapDeltaToBestLap ",
            "LapDeltaToBestLap_DD",
            "LapDeltaToBestLap_OK",
            "LapDeltaToOptimalLap",
            "LapDeltaToOptimalLap_DD ",
            "LapDeltaToOptimalLap_OK",
            "LapDeltaToSessionBestLap",
            "LapDeltaToSessionBestLap_DD ",
            "LapDeltaToSessionBestLap_OK",
            "LapDeltaToSessionLastlLap",
            "LapDeltaToSessionLastlLap_DD ",
            "LapDeltaToSessionLastlLap_OK",
            "LapDeltaToSessionOptimalLap",
            "LapDeltaToSessionOptimalLap_DD",
            "LapDeltaToSessionOptimalLap_OK",
            "LapDist",
            "LapDistPct",
            "LapLasNLapSeq",
            "LapLastLapTime",
            "LapLastNLapTime",
            "Lat",
            "LatAccel",
            "Lon",
            "LongAccel",
            "ManifoldPress",
            "OilLevel",
            "OilPress",
            "OilTemp",
            "OnPitRoad",
            "Pitch",
            "PitchRate",
            "PitOptRepairLeft",
            "PitRepairLeft",
            "PitSvFlags",
            "PitSvFuel",
            "PitSvLFP",
            "PitSvLRP",
            "PitSvRFP",
            "PitSvRRP",
            "PlayerCarClassPosition",
            "PlayerCarPosition",
            "RaceLaps",
            "RadioTransmitCarIdx",
            "RadioTransmitFrequencyIdx",
            "RadioTransmitRadioIdx",
            "RelativeHumidity",
            "ReplayFrameNum",
            "ReplayFrameNumEnd",
            "ReplayPlaySlowMotion",
            "ReplayPlaySpeed",
            "ReplaySessionNum",
            "ReplaySessionTime",
            "Roll",
            "RollRate",
            "RPM",
            "SessionFlags",
            "SessionLapsRemain",
            "SessionNum",
            "SessionState",
            "SessionTime",
            "SessionTimeRemain",
            "SessionUniqueID",
            "ShiftGrindRPM",
            "ShiftIndicatorPct",
            "ShiftPowerPct",
            "Skies",
            "Speed",
            "SteeringWheelAngle",
            "SteeringWheelAngleMax",
            "SteeringWheelPctDamper",
            "SteeringWheelPctTorque",
            "SteeringWheelPctTorqueSign",
            "SteeringWheelPctTorqueSignStops",
            "SteeringWheelPeakForceNm",
            "SteeringWheelTorque",
            "Throttle",
            "ThrottleRaw",
            "TrackTemp",
            "TrackTempCrew",
            "VelocityX",
            "VelocityY",
            "VelocityZ",
            "VertAccel",
            "Voltage",
            "WaterLevel",
            "WaterTemp",
            "WeatherType",
            "WindDir",
            "WindVel",
            "Yaw",
            "YawNorth",
            "YawRate",
            "CarIdxClassPosition",
            "CarIdxEstTime",
            "CarIdxF2Time",
            "CarIdxGear",
            "CarIdxLap",
            "CarIdxLapDistPct",
            "CarIdxOnPitRoad",
            "CarIdxPosition",
            "CarIdxRPM",
            "CarIdxSteer",
            "CarIdxTrackSurface"});
            this.props.Location = new System.Drawing.Point(6, 6);
            this.props.Name = "props";
            this.props.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.props.Size = new System.Drawing.Size(695, 706);
            this.props.TabIndex = 0;
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
            // views
            // 
            this.views.FormattingEnabled = true;
            this.views.Location = new System.Drawing.Point(840, 215);
            this.views.Name = "views";
            this.views.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.views.Size = new System.Drawing.Size(643, 498);
            this.views.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(837, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Text Format:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(909, 142);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(162, 20);
            this.textBox1.TabIndex = 4;
            // 
            // isSimConnected
            // 
            this.isSimConnected.AutoSize = true;
            this.isSimConnected.Location = new System.Drawing.Point(1088, 144);
            this.isSimConnected.Name = "isSimConnected";
            this.isSimConnected.Size = new System.Drawing.Size(109, 17);
            this.isSimConnected.TabIndex = 5;
            this.isSimConnected.Text = "When connected";
            this.isSimConnected.UseVisualStyleBackColor = true;
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
            // button10
            // 
            this.button10.Image = global::iDash.Properties.Resources.Delete_black_32;
            this.button10.Location = new System.Drawing.Point(1489, 688);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(33, 31);
            this.button10.TabIndex = 15;
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Image = global::iDash.Properties.Resources.Delete_black_32;
            this.button9.Location = new System.Drawing.Point(1489, 133);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(33, 31);
            this.button9.TabIndex = 14;
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.button7.Location = new System.Drawing.Point(1312, 144);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(33, 48);
            this.button7.TabIndex = 13;
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Image = global::iDash.Properties.Resources._112_UpArrowShort_Grey_32x42_72;
            this.button8.Location = new System.Drawing.Point(1254, 144);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(33, 48);
            this.button8.TabIndex = 12;
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Image = global::iDash.Properties.Resources._112_DownArrowShort_Grey_32x32_72;
            this.button5.Location = new System.Drawing.Point(1489, 459);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(33, 48);
            this.button5.TabIndex = 11;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Image = global::iDash.Properties.Resources._112_UpArrowShort_Grey_32x42_72;
            this.button6.Location = new System.Drawing.Point(1489, 391);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(33, 48);
            this.button6.TabIndex = 10;
            this.button6.UseVisualStyleBackColor = true;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1575, 961);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.mainTab);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "iDash";
            this.mainTab.ResumeLayout(false);
            this.settingsTab.ResumeLayout(false);
            this.debugTab.ResumeLayout(false);
            this.debugTab.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tm1637.ResumeLayout(false);
            this.tm1637.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TabPage max7221;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox isSimConnected;
        private System.Windows.Forms.TextBox textBox1;
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
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button11;
    }
}

