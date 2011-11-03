using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace LogParser.Model
{
    public class PMEvent
    {
        private List<Evnt> _events;
        public PMEvent(Dictionary<string,string> a)
        {
            _events = new List<Evnt>();
        }

        public List<Evnt> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        public void AddMouseEvent(Dictionary<string, string> a)
        {
            _events.Add(new MouseEvent(a));
        }

        public void AddCursorEvent(Dictionary<string, string> a)
        {
            _events.Add(new CursorEvent(a));
        }

        public override string ToString()
        {
            return "Main Event";
        }
    }
}
