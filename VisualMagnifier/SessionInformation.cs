using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PointingMagnifier
{
    public partial class SessionInformation : Form
    {
        public SessionInformation()
        {
            InitializeComponent();
            lbl_ID.Text = String.Format("User ID: {0}",Properties.Settings.Default.UID.ToString());
            lbl_Elapsed.Text = String.Format("Elapsed Time: {0}", Properties.Settings.Default.ElapsedTime.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
