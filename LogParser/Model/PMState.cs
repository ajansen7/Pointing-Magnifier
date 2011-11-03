using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace LogParser.Model
{
    public class PMState
    {
        private List<PMEvent> _events;
        private List<Feedback> _feedback;
        private String _active;
        private bool _study;
        private bool _logging;
        private bool _baseline;
        public PMState()
        {
            _events = new List<PMEvent>();
            _feedback = new List<Feedback>();
        }

        public PMState(Dictionary<String,String> nvc)
        {
            foreach(KeyValuePair<String,String> k in nvc)
            {
                if(k.Key == "Active")
                    _active = k.Value;
                else if(k.Key == "Study")
                    _study = (k.Value =="true");
                else if(k.Key =="Logging")
                    _logging = (k.Value=="true");
                else if(k.Key =="Baseline")
                    _baseline = (k.Value == "true");
                        
            }
            _events = new List<PMEvent>();
            _feedback = new List<Feedback>();
        }

        public void AddEvent(Dictionary<string,string> a)
        {
            _events.Add(new PMEvent(a));
        }
        public void AddFeedback(Dictionary<string, string> a,string value)
        {
            _feedback.Add(new Feedback(a,value));
        }
        public List<Feedback> Feedback
        {
            get { return _feedback; }
            set { _feedback = value; }
        }
        public List<PMEvent> Events
        {
            get { return _events; }
            set { _events = value; }
        }
        public PMEvent CurrentEvent
        {
            get { return _events[_events.Count - 1]; }
        }

        public override string ToString()
        {
            return String.Format("State: Active={0} Study={1} Logging={2} Baseline={3}", _active, _study, _logging, _baseline);
        }
    }
}
