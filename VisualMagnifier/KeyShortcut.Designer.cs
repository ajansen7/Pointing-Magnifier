using System.Windows.Forms;
namespace PointingMagnifier
{
    partial class KeyShortcut
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cb_Ctrl = new System.Windows.Forms.CheckBox();
            this.cb_Alt = new System.Windows.Forms.CheckBox();
            this.cb_Shift = new System.Windows.Forms.CheckBox();
            this.cmb_Keys = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cb_Ctrl
            // 
            this.cb_Ctrl.AutoSize = true;
            this.cb_Ctrl.Cursor = System.Windows.Forms.Cursors.Default;
            this.cb_Ctrl.Location = new System.Drawing.Point(4, 3);
            this.cb_Ctrl.Name = "cb_Ctrl";
            this.cb_Ctrl.Size = new System.Drawing.Size(41, 17);
            this.cb_Ctrl.TabIndex = 0;
            this.cb_Ctrl.Text = "Ctrl";
            this.cb_Ctrl.UseVisualStyleBackColor = true;
            this.cb_Ctrl.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // cb_Alt
            // 
            this.cb_Alt.AutoSize = true;
            this.cb_Alt.Cursor = System.Windows.Forms.Cursors.Default;
            this.cb_Alt.Location = new System.Drawing.Point(51, 3);
            this.cb_Alt.Name = "cb_Alt";
            this.cb_Alt.Size = new System.Drawing.Size(38, 17);
            this.cb_Alt.TabIndex = 1;
            this.cb_Alt.Text = "Alt";
            this.cb_Alt.UseVisualStyleBackColor = true;
            this.cb_Alt.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // cb_Shift
            // 
            this.cb_Shift.AutoSize = true;
            this.cb_Shift.Cursor = System.Windows.Forms.Cursors.Default;
            this.cb_Shift.Location = new System.Drawing.Point(95, 3);
            this.cb_Shift.Name = "cb_Shift";
            this.cb_Shift.Size = new System.Drawing.Size(47, 17);
            this.cb_Shift.TabIndex = 2;
            this.cb_Shift.Text = "Shift";
            this.cb_Shift.UseVisualStyleBackColor = true;
            this.cb_Shift.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // cmb_Keys
            // 
            this.cmb_Keys.FormattingEnabled = true;
            this.cmb_Keys.Items.AddRange(new object[] {
            System.Windows.Forms.Keys.A,
            System.Windows.Forms.Keys.B,
            System.Windows.Forms.Keys.C,
            System.Windows.Forms.Keys.D,
            System.Windows.Forms.Keys.E,
            System.Windows.Forms.Keys.F,
            System.Windows.Forms.Keys.G,
            System.Windows.Forms.Keys.H,
            System.Windows.Forms.Keys.I,
            System.Windows.Forms.Keys.J,
            System.Windows.Forms.Keys.K,
            System.Windows.Forms.Keys.L,
            System.Windows.Forms.Keys.M,
            System.Windows.Forms.Keys.N,
            System.Windows.Forms.Keys.O,
            System.Windows.Forms.Keys.P,
            System.Windows.Forms.Keys.Q,
            System.Windows.Forms.Keys.R,
            System.Windows.Forms.Keys.S,
            System.Windows.Forms.Keys.T,
            System.Windows.Forms.Keys.U,
            System.Windows.Forms.Keys.V,
            System.Windows.Forms.Keys.W,
            System.Windows.Forms.Keys.X,
            System.Windows.Forms.Keys.Y,
            System.Windows.Forms.Keys.Z,
            System.Windows.Forms.Keys.D0,
            System.Windows.Forms.Keys.D1,
            System.Windows.Forms.Keys.D2,
            System.Windows.Forms.Keys.D3,
            System.Windows.Forms.Keys.D4,
            System.Windows.Forms.Keys.D5,
            System.Windows.Forms.Keys.D6,
            System.Windows.Forms.Keys.D7,
            System.Windows.Forms.Keys.D8,
            System.Windows.Forms.Keys.D9,
            System.Windows.Forms.Keys.F1,
            System.Windows.Forms.Keys.F2,
            System.Windows.Forms.Keys.F3,
            System.Windows.Forms.Keys.F4,
            System.Windows.Forms.Keys.F5,
            System.Windows.Forms.Keys.F6,
            System.Windows.Forms.Keys.F7,
            System.Windows.Forms.Keys.F8,
            System.Windows.Forms.Keys.F9,
            System.Windows.Forms.Keys.F10,
            System.Windows.Forms.Keys.F11,
            System.Windows.Forms.Keys.F12,
            System.Windows.Forms.Keys.F13,
            System.Windows.Forms.Keys.F14,
            System.Windows.Forms.Keys.F15,
            System.Windows.Forms.Keys.F16,
            System.Windows.Forms.Keys.Escape});
            this.cmb_Keys.Location = new System.Drawing.Point(4, 27);
            this.cmb_Keys.Name = "cmb_Keys";
            this.cmb_Keys.Size = new System.Drawing.Size(138, 21);
            this.cmb_Keys.TabIndex = 3;
            this.cmb_Keys.SelectedIndexChanged += new System.EventHandler(this.cmb_Keys_SelectedIndexChanged);
            // 
            // KeyShortcut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmb_Keys);
            this.Controls.Add(this.cb_Shift);
            this.Controls.Add(this.cb_Alt);
            this.Controls.Add(this.cb_Ctrl);
            this.Name = "KeyShortcut";
            this.Size = new System.Drawing.Size(148, 49);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_Ctrl;
        private System.Windows.Forms.CheckBox cb_Alt;
        private System.Windows.Forms.CheckBox cb_Shift;
        private System.Windows.Forms.ComboBox cmb_Keys;
    }
}
