using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ParseLog
{
    public enum BtnState { Down, Up }

    public class MouseEvent : PMEvent
    {
        private Dictionary<string, string> _attributes;
        protected MouseButtons Btn { get; set; }
        protected BtnState BtnState { get; set; }
        protected int BtnCount { get; set; }
        protected Point Location { get; set; }
        protected bool Sythn { get; set; }
        protected bool Magnified { get; set; }

        public MouseEvent(Dictionary<string,string> a) 
        {
            _attributes = a;
        }

        public override string WriteText()
        {
            String str = "ME: ";
            foreach (KeyValuePair<string, string> kvp in _attributes)
            {
                str += kvp.ToString() + " | ";
            }
            str += Environment.NewLine;
            str += "    ";
            return str;
        }
    }
}
