﻿namespace iDash
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.debugTab = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.debugModes = new System.Windows.Forms.ComboBox();
            this.clearData = new System.Windows.Forms.Button();
            this.debugData = new System.Windows.Forms.RichTextBox();
            this.statusBar = new System.Windows.Forms.RichTextBox();
            this.mainTab.SuspendLayout();
            this.debugTab.SuspendLayout();
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
            this.mainTab.Controls.Add(this.tabPage1);
            this.mainTab.Controls.Add(this.debugTab);
            this.mainTab.Location = new System.Drawing.Point(2, 12);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(800, 618);
            this.mainTab.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 592);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.debugTab.Size = new System.Drawing.Size(792, 592);
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
            this.debugData.Size = new System.Drawing.Size(777, 492);
            this.debugData.TabIndex = 10;
            this.debugData.Text = "";
            this.debugData.TextChanged += new System.EventHandler(this.debugData_TextChanged);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(3, 632);
            this.statusBar.Name = "statusBar";
            this.statusBar.ReadOnly = true;
            this.statusBar.Size = new System.Drawing.Size(795, 187);
            this.statusBar.TabIndex = 0;
            this.statusBar.Text = "";
            this.statusBar.TextChanged += new System.EventHandler(this.statusBar_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 831);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.mainTab);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "iDash";
            this.mainTab.ResumeLayout(false);
            this.debugTab.ResumeLayout(false);
            this.debugTab.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.RichTextBox richTextBoxSend;
        private System.Windows.Forms.Label label1;

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage debugTab;
        private System.Windows.Forms.RichTextBox statusBar;
        private System.Windows.Forms.Button clearData;
        private System.Windows.Forms.RichTextBox debugData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox debugModes;
    }
}

