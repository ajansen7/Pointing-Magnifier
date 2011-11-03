using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PointingMagnifier
{
    public partial class QuickFeedback : Form
    {
        public QuickFeedback()
        {
            InitializeComponent();
        }

        public void SetPicture(Bitmap b)
        {
            this.pictureBox1.Image = b;
            Invalidate();
        }
        public bool Checked
        {
            get
            {
                return cbImage.Checked;
            }
        }
        public String Feedback
        {
            get { return tbFeedback.Text; }
        }
    }
}
