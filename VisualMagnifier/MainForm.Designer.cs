namespace PointingMagnifier
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.niVMM = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.tsShow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLogs = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudSize = new System.Windows.Forms.NumericUpDown();
            this.btnDefaults = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cbGain = new System.Windows.Forms.CheckBox();
            this.cbCrosshairs = new System.Windows.Forms.CheckBox();
            this.nudMagnification = new System.Windows.Forms.NumericUpDown();
            this.Shortcuts = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.keyShortcut3 = new PointingMagnifier.KeyShortcut();
            this.keyShortcut1 = new PointingMagnifier.KeyShortcut();
            this.keyShortcut2 = new PointingMagnifier.KeyShortcut();
            this.label1 = new System.Windows.Forms.Label();
            this.cbKeyboard = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.cbEnable = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStart = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMinimize = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSurvey = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsIcon.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMagnification)).BeginInit();
            this.Shortcuts.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // niVMM
            // 
            this.niVMM.BalloonTipText = "The Pointing Magnifier is still running!\n Click on the icon to show the options.";
            this.niVMM.ContextMenuStrip = this.cmsIcon;
            this.niVMM.Icon = ((System.Drawing.Icon)(resources.GetObject("niVMM.Icon")));
            this.niVMM.Text = "Pointing Magnifier";
            this.niVMM.Visible = true;
            this.niVMM.BalloonTipClicked += new System.EventHandler(this.Feedback_event);
            this.niVMM.MouseDown += new System.Windows.Forms.MouseEventHandler(this.niVMM_MouseDown);
            // 
            // cmsIcon
            // 
            this.cmsIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsEnable,
            this.tsShow,
            this.toolStripSeparator5,
            this.tsFeedback,
            this.toolStripMenuItem2,
            this.toolStripSeparator1,
            this.tsExit});
            this.cmsIcon.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.cmsIcon.Name = "cmsIcon";
            this.cmsIcon.ShowImageMargin = false;
            this.cmsIcon.Size = new System.Drawing.Size(196, 126);
            this.cmsIcon.Click += new System.EventHandler(this.tsShow_Click);
            // 
            // tsEnable
            // 
            this.tsEnable.Name = "tsEnable";
            this.tsEnable.Size = new System.Drawing.Size(195, 22);
            this.tsEnable.Text = "Turn On Pointing Magnifier";
            this.tsEnable.Click += new System.EventHandler(this.ToggleMagnifier_event);
            // 
            // tsShow
            // 
            this.tsShow.Name = "tsShow";
            this.tsShow.Size = new System.Drawing.Size(195, 22);
            this.tsShow.Text = "Show Options";
            this.tsShow.Click += new System.EventHandler(this.tsShow_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(192, 6);
            // 
            // tsFeedback
            // 
            this.tsFeedback.Name = "tsFeedback";
            this.tsFeedback.Size = new System.Drawing.Size(195, 22);
            this.tsFeedback.Text = "Send Feedback";
            this.tsFeedback.Click += new System.EventHandler(this.Feedback_event);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(195, 22);
            this.toolStripMenuItem2.Text = "Send Daily Survey";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.DailySurvey_event);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // tsExit
            // 
            this.tsExit.Name = "tsExit";
            this.tsExit.Size = new System.Drawing.Size(195, 22);
            this.tsExit.Text = "Exit";
            this.tsExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // cmsMenu
            // 
            this.cmsMenu.Name = "cmsMenu";
            this.cmsMenu.Size = new System.Drawing.Size(155, 22);
            this.cmsMenu.Text = "toolStripMenuItem2";
            // 
            // btnLogs
            // 
            this.btnLogs.Enabled = false;
            this.btnLogs.Location = new System.Drawing.Point(251, 35);
            this.btnLogs.Name = "btnLogs";
            this.btnLogs.Size = new System.Drawing.Size(151, 23);
            this.btnLogs.TabIndex = 14;
            this.btnLogs.Text = "Send Feedback";
            this.btnLogs.UseVisualStyleBackColor = true;
            this.btnLogs.Click += new System.EventHandler(this.Feedback_event);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(251, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Send Daily Survey";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.DailySurvey_event);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudSize);
            this.groupBox1.Controls.Add(this.btnDefaults);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbGain);
            this.groupBox1.Controls.Add(this.cbCrosshairs);
            this.groupBox1.Controls.Add(this.nudMagnification);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 141);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Magnification Factor";
            // 
            // nudSize
            // 
            this.nudSize.AccessibleName = "Cursor Size";
            this.nudSize.Location = new System.Drawing.Point(123, 42);
            this.nudSize.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudSize.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudSize.Name = "nudSize";
            this.nudSize.Size = new System.Drawing.Size(60, 20);
            this.nudSize.TabIndex = 3;
            this.nudSize.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.nudSize.ValueChanged += new System.EventHandler(this.nudSize_ValueChanged);
            // 
            // btnDefaults
            // 
            this.btnDefaults.AccessibleName = "Defaults";
            this.btnDefaults.Location = new System.Drawing.Point(123, 112);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(104, 23);
            this.btnDefaults.TabIndex = 6;
            this.btnDefaults.Text = "Restore Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Cursor Size";
            // 
            // cbGain
            // 
            this.cbGain.AccessibleName = "Lower Mouse Gain";
            this.cbGain.AutoSize = true;
            this.cbGain.Location = new System.Drawing.Point(9, 68);
            this.cbGain.Name = "cbGain";
            this.cbGain.Size = new System.Drawing.Size(202, 17);
            this.cbGain.TabIndex = 4;
            this.cbGain.Text = "Lower Mouse Speed when Magnified";
            this.cbGain.UseVisualStyleBackColor = true;
            this.cbGain.CheckedChanged += new System.EventHandler(this.cbGain_CheckedChanged);
            // 
            // cbCrosshairs
            // 
            this.cbCrosshairs.AccessibleName = "Crosshairs";
            this.cbCrosshairs.AutoSize = true;
            this.cbCrosshairs.Checked = true;
            this.cbCrosshairs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCrosshairs.Location = new System.Drawing.Point(9, 94);
            this.cbCrosshairs.Name = "cbCrosshairs";
            this.cbCrosshairs.Size = new System.Drawing.Size(104, 17);
            this.cbCrosshairs.TabIndex = 5;
            this.cbCrosshairs.Text = "Show Crosshairs";
            this.cbCrosshairs.UseVisualStyleBackColor = true;
            this.cbCrosshairs.CheckedChanged += new System.EventHandler(this.cbCrosshairs_CheckedChanged);
            // 
            // nudMagnification
            // 
            this.nudMagnification.AccessibleName = "Magnification Factor";
            this.nudMagnification.Location = new System.Drawing.Point(123, 16);
            this.nudMagnification.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMagnification.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMagnification.Name = "nudMagnification";
            this.nudMagnification.Size = new System.Drawing.Size(60, 20);
            this.nudMagnification.TabIndex = 1;
            this.nudMagnification.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudMagnification.ValueChanged += new System.EventHandler(this.nudMagnification_ValueChanged);
            // 
            // Shortcuts
            // 
            this.Shortcuts.Controls.Add(this.label2);
            this.Shortcuts.Controls.Add(this.keyShortcut3);
            this.Shortcuts.Controls.Add(this.keyShortcut1);
            this.Shortcuts.Controls.Add(this.keyShortcut2);
            this.Shortcuts.Controls.Add(this.label1);
            this.Shortcuts.Controls.Add(this.cbKeyboard);
            this.Shortcuts.Controls.Add(this.label8);
            this.Shortcuts.Controls.Add(this.textBox3);
            this.Shortcuts.Location = new System.Drawing.Point(12, 168);
            this.Shortcuts.Name = "Shortcuts";
            this.Shortcuts.Size = new System.Drawing.Size(233, 264);
            this.Shortcuts.TabIndex = 1;
            this.Shortcuts.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Hide the Magnifier while pressed";
            // 
            // keyShortcut3
            // 
            this.keyShortcut3.Location = new System.Drawing.Point(9, 50);
            this.keyShortcut3.Name = "keyShortcut3";
            this.keyShortcut3.Size = new System.Drawing.Size(148, 49);
            this.keyShortcut3.TabIndex = 9;
            this.keyShortcut3.ShortcutChanged += new System.EventHandler(this.keyShortcut3_ShortcutChanged);
            // 
            // keyShortcut1
            // 
            this.keyShortcut1.Location = new System.Drawing.Point(6, 129);
            this.keyShortcut1.Name = "keyShortcut1";
            this.keyShortcut1.Size = new System.Drawing.Size(148, 49);
            this.keyShortcut1.TabIndex = 11;
            this.keyShortcut1.ShortcutChanged += new System.EventHandler(this.keyShortcut1_ShortcutChanged);
            // 
            // keyShortcut2
            // 
            this.keyShortcut2.Location = new System.Drawing.Point(6, 205);
            this.keyShortcut2.Name = "keyShortcut2";
            this.keyShortcut2.Size = new System.Drawing.Size(148, 49);
            this.keyShortcut2.TabIndex = 13;
            this.keyShortcut2.ShortcutChanged += new System.EventHandler(this.keyShortcut2_ShortcutChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Quit the Pointing Magnifier";
            // 
            // cbKeyboard
            // 
            this.cbKeyboard.AutoSize = true;
            this.cbKeyboard.Checked = true;
            this.cbKeyboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbKeyboard.Location = new System.Drawing.Point(6, 11);
            this.cbKeyboard.Name = "cbKeyboard";
            this.cbKeyboard.Size = new System.Drawing.Size(155, 17);
            this.cbKeyboard.TabIndex = 7;
            this.cbKeyboard.Text = "Enable Keyboard Shortcuts";
            this.cbKeyboard.UseVisualStyleBackColor = true;
            this.cbKeyboard.CheckedChanged += new System.EventHandler(this.cbKeyboard_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 113);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(181, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Toggle the Pointing Magnifier On/Off";
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.ForeColor = System.Drawing.SystemColors.Control;
            this.textBox3.Location = new System.Drawing.Point(8, 73);
            this.textBox3.Margin = new System.Windows.Forms.Padding(5);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(167, 19);
            this.textBox3.TabIndex = 10;
            // 
            // cbEnable
            // 
            this.cbEnable.Location = new System.Drawing.Point(251, 396);
            this.cbEnable.Name = "cbEnable";
            this.cbEnable.Size = new System.Drawing.Size(151, 36);
            this.cbEnable.TabIndex = 16;
            this.cbEnable.Text = "Turn On Pointing Magnifier";
            this.cbEnable.UseVisualStyleBackColor = true;
            this.cbEnable.Click += new System.EventHandler(this.ToggleMagnifier_event);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem3});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(411, 24);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStart,
            this.toolStripSeparator4,
            this.menuEnable,
            this.menuMinimize,
            this.toolStripSeparator3,
            this.menuFeedback,
            this.menuSurvey,
            this.toolStripSeparator2,
            this.menuExit});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // menuStart
            // 
            this.menuStart.Enabled = false;
            this.menuStart.Name = "menuStart";
            this.menuStart.Size = new System.Drawing.Size(220, 22);
            this.menuStart.Text = "Begin Study";
            this.menuStart.Click += new System.EventHandler(this.menuStart_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(217, 6);
            // 
            // menuEnable
            // 
            this.menuEnable.Name = "menuEnable";
            this.menuEnable.Size = new System.Drawing.Size(220, 22);
            this.menuEnable.Text = "Turn On Pointing Magnifier";
            this.menuEnable.Click += new System.EventHandler(this.ToggleMagnifier_event);
            // 
            // menuMinimize
            // 
            this.menuMinimize.Name = "menuMinimize";
            this.menuMinimize.Size = new System.Drawing.Size(220, 22);
            this.menuMinimize.Text = "Close Options";
            this.menuMinimize.Click += new System.EventHandler(this.minimizeToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(217, 6);
            // 
            // menuFeedback
            // 
            this.menuFeedback.Name = "menuFeedback";
            this.menuFeedback.Size = new System.Drawing.Size(220, 22);
            this.menuFeedback.Text = "Send Feedback";
            this.menuFeedback.Click += new System.EventHandler(this.Feedback_event);
            // 
            // menuSurvey
            // 
            this.menuSurvey.Name = "menuSurvey";
            this.menuSurvey.Size = new System.Drawing.Size(220, 22);
            this.menuSurvey.Text = "Send Daily Survey";
            this.menuSurvey.Click += new System.EventHandler(this.DailySurvey_event);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(217, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(220, 22);
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.menuAbout});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(44, 20);
            this.toolStripMenuItem3.Text = "Help";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(137, 22);
            this.toolStripMenuItem4.Text = "Session Info";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(137, 22);
            this.menuAbout.Text = "About";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 444);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLogs);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Shortcuts);
            this.Controls.Add(this.cbEnable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Pointing Magnifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.cmsIcon.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMagnification)).EndInit();
            this.Shortcuts.ResumeLayout(false);
            this.Shortcuts.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsIcon;
        private System.Windows.Forms.ToolStripMenuItem tsEnable;
        private System.Windows.Forms.ToolStripMenuItem tsShow;
        private System.Windows.Forms.ToolStripMenuItem tsExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.NotifyIcon niVMM;
        private System.Windows.Forms.ToolStripMenuItem cmsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsFeedback;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Button btnLogs;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudSize;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbGain;
        private System.Windows.Forms.CheckBox cbCrosshairs;
        private System.Windows.Forms.NumericUpDown nudMagnification;
        private System.Windows.Forms.GroupBox Shortcuts;
        private System.Windows.Forms.CheckBox cbKeyboard;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button cbEnable;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuEnable;
        private System.Windows.Forms.ToolStripMenuItem menuMinimize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuFeedback;
        private System.Windows.Forms.ToolStripMenuItem menuSurvey;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private KeyShortcut keyShortcut2;
        private KeyShortcut keyShortcut1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem menuStart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Label label2;
        private KeyShortcut keyShortcut3;
    }
}

