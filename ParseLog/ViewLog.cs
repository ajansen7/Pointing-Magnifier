using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ParseLog
{
    public partial class ViewLog : Form
    {
        public ViewLog()
        {
            InitializeComponent();
            
        }
        public void UpdateLog(PointingMagnifier pm)
        {
            rtbXML.Text = pm.WriteText();
        }

        public void ShowFeedback(PointingMagnifier pm)
        {
            rtbXML.Text = pm.GetFeedback();
        }

    }
}
