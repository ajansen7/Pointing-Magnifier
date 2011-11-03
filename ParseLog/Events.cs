using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ParseLog
{
    class Events : PMEvent
    {
        private Dictionary<string, string> _attributes;
        protected LinkedList<PMEvent> Evnts { get; set; }
        
        public Events(Dictionary<string,string> a) 
        {
            _attributes = a;
            Evnts = new LinkedList<PMEvent>();
        }

        public void ReadLog(XmlReader reader)
        {
            while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>();
                if (reader.Name == "ME")
                {
                    if (reader.HasAttributes)
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            reader.MoveToAttribute(i);
                            attributes.Add(reader.Name, reader.Value);
                        }
                    }
                    MouseEvent e = new MouseEvent(attributes);
                    Evnts.AddLast(e);
                    //do shit here
                }
                else if (reader.Name == "Clickthrough")
                {
                        if (reader.HasAttributes)
                        {
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                attributes.Add(reader.Name, reader.Value);
                            }
                        }
                    Clickthrough c = new Clickthrough(attributes);
                    c.ReadLog(reader);
                    Evnts.AddLast(c);
                }
            }
        }

        public void AddMouseEvent()
        {
            Evnts.AddLast(new MouseEvent(new Dictionary<string, string>()));
        }

        public void AddMagnifierEvent()
        {
            Evnts.AddLast(new MagnifierEvent());
        }

        public override string WriteText()
        {
            String str = "Event ";
            foreach (KeyValuePair<string, string> kvp in _attributes)
            {
                str += kvp.ToString() + " | ";
            }
            str += Environment.NewLine;
            str += "    ";
            foreach (PMEvent e in Evnts)
            {
                str += e.WriteText();
            }
            return str;
        }

        public int Count()
        {
            return Evnts.Count;
        }
        
    }
}
