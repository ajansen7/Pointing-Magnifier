using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PointingMagnifier
{
    public partial class UID : Form
    {
        public UID()
        {
            InitializeComponent();
        }

        public int ID
        {
            get { return (int)numericUpDown1.Value; }
        }
    }
}
