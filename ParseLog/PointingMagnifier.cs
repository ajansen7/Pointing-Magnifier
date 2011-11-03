using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections.Specialized;
namespace ParseLog
{
    public class PointingMagnifier : PMEvent
    {
        protected LinkedList<State> States { get; set; }

        public PointingMagnifier()
        {
            this.States = new LinkedList<State>();
        }

        public void ReadLog(XmlTextReader reader)
        {
            while (reader.Read())
            {
                if (reader.Name == "State")
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
                    State s = new State(attributes);
                    s.ReadLog(reader);
                    States.AddLast(s);
                }
            }
        }
        public int NumStates()
        {
            int i = 0;
            i = States.Count;
            return i;
        }
        public int NumActiveEvents()
        {
            int i = 0;
            foreach (State s in States)
            {
                if (s.Active == Active.True || s.Active == Active.Initialized)
                {
                    i += s.Events.Count;
                }
            }
            return i;
        }
        public int NumNonActiveEvents()
        {
            int i = 0;
            foreach (State s in States)
            {
                if (s.Active == Active.False)
                {
                    i += s.Events.Count;
                }
            }
            return i;
        }
        public override string WriteText()
        {
            String str = "";
            foreach (State s in States)
            {
                str += s.WriteText();
            }
            return str;
        }

        public String GetFeedback()
        {
            String str = "";
            foreach (State s in States)
            {
                str += s.GetFeedback();
            }
            return str;
        }
    }
}
