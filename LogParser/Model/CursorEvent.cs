using System;
using System.Collections.Generic;
using System.Text;

namespace LogParser.Model
{
    public class CursorEvent : Evnt
    {
        private Dictionary<string, string> _attributes;
        //protected MouseButtons Btn { get; set; }
        //protected BtnState BtnState { get; set; }
        protected int BtnCount { get; set; }
        //protected Point Location { get; set; }
        protected bool Sythn { get; set; }
        protected bool Magnified { get; set; }

        public CursorEvent()
        {
            //_attributes = a;
        }
        public CursorEvent(Dictionary<string, string> a)
        {
            _attributes = a;
        }

        public override string ToString()
        {
            return "Cursor Event";
        }
    }
}
