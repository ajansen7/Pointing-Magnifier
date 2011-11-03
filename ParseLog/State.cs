using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ParseLog
{
    public enum Active { Initialized, True, False }

    public class State
    {
        Dictionary<string, string> _attributes;

        public Active Active { get; set; }
        public bool Study { get; set; }
        public bool Logging { get; set; }
        public bool Baseline { get; set; }
        public LinkedList<PMEvent> Events { get; set; }

        public State(Dictionary<string,string> a)
        {
            _attributes = a;
            string value;
            if (_attributes.TryGetValue("Active", out value))
                Active = (Active)Enum.Parse(typeof(Active),value,true);
            this.Events = new LinkedList<PMEvent>();
        }

        public void ReadLog(XmlTextReader reader)
        {
            while (reader.Read()) //need end element otherwise will step through entire log
            {
                if (reader.Name == "Event")
                {
                    Dictionary<string, string> attributes = new Dictionary<string, string>();
                    if (reader.HasAttributes)
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            reader.MoveToAttribute(i);
                            attributes.Add(reader.Name, reader.Value);
                        }
                    }
                    Events e = new Events(attributes);
                    e.ReadLog(reader.ReadSubtree());
                    Events.AddLast(e);
                }
                else if (reader.Name == "Feedback")
                {
                    Dictionary<string, string> attributes = new Dictionary<string, string>();
                    if (reader.HasAttributes)
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            reader.MoveToAttribute(i);
                            attributes.Add(reader.Name, reader.Value);
                        }
                    }
                    Feedback e = new Feedback(attributes, reader.ReadElementString());
                    Events.AddLast(e);
                }
                else
                {
                    return;
                }
            }
        }

        public String WriteText()
        {
            String str = "State: ";
            foreach (KeyValuePair<string,string> kvp in _attributes)
            {
                str += kvp.ToString() + " | ";
            }
            str += Environment.NewLine;
            str += "    ";
            foreach (PMEvent e in Events)
            {
                str += e.WriteText();
            }
            return str;
            
        }
        public int CountEventType(PMEvent type)
        {
            int count = 0;
            foreach (PMEvent e in Events)
            {
                if (e.GetType() == type.GetType())
                    count++;
            }
            return count;

        }

        public string GetFeedback()
        {
            string f = "";
            foreach (PMEvent e in Events)
            {
                if (e.GetType() == new Feedback(null,null).GetType())
                {
                    f += e.WriteText();
                    f += Environment.NewLine;
                }
            }
            return f;
        }
        //public void AddPreference()
        //{
        //    Events.AddLast(new Preference());
        //}
        //public void AddEvent()
        //{
        //    Events.AddLast(new Events());
        //}
    }
}
