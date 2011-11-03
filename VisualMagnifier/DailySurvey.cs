using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PointingMagnifier
{
    public partial class DailySurvey : Form
    {
        public DailySurvey()
        {
            InitializeComponent();
        }
        public String Values
        {
            get { return String.Format("{0},{1},{2},{3},{4},{5}", mental.Value, physical.Value, temporal.Value, performance.Value, effort.Value, frustration.Value); }
        }
    }
}
