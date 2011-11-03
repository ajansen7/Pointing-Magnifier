using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PointingMagnifier
{
    public partial class KeyShortcut : UserControl
    {
        private Keys shortcut;

        public event EventHandler ShortcutChanged;

        public KeyShortcut()
        {
            shortcut = Keys.None;
            InitializeComponent();
            
        }
        private void CheckedChanged(object sender, EventArgs e)
        {
            SetShortcut();
        }
        private void cmb_Keys_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetShortcut();
        }
        private void SetShortcut()
        {
            shortcut = Keys.None;
            if (cb_Alt.Checked)
                shortcut = shortcut | Keys.Alt;
            if (cb_Ctrl.Checked)
                shortcut = shortcut | Keys.Control;
            if (cb_Shift.Checked)
                shortcut = shortcut | Keys.Shift;
            if (cmb_Keys.SelectedItem != null)
            {
                shortcut = shortcut | (Keys)cmb_Keys.SelectedItem;
                if (ShortcutChanged != null)
                    ShortcutChanged(this, null);
            }
        }

        public void LoadShortcut(Keys k)
        {
            cb_Alt.Checked = false;
            cb_Ctrl.Checked = false;
            cb_Shift.Checked = false;
            if ((k & Keys.Alt) == Keys.Alt)
            {
                cb_Alt.Checked = true;
                k = k &~Keys.Alt;
            }
            if ((k & Keys.Control) == Keys.Control)
            {
                cb_Ctrl.Checked = true;
                k = k & ~Keys.Control;
            }
            if ((k & Keys.Shift) == Keys.Shift)
            {
                cb_Shift.Checked = true;
                k = k & ~Keys.Shift;
            }
            cmb_Keys.SelectedItem = k;
        }

        public Keys Shortcut
        {
            get { return shortcut; }
        }
    }
}
