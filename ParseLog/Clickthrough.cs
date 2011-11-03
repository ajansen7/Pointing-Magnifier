using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ParseLog
{
    public class Clickthrough:PMEvent
    {
        private Dictionary<string, string> _attributes;
        protected LinkedList<PMEvent> Ct { get; set; }

        public Clickthrough(Dictionary<string, string> a)
        {
            _attributes = a;
            this.Ct = new LinkedList<PMEvent>();
        }
        public override string WriteText()
        {
            String str = "Clickthrough: ";
            foreach (KeyValuePair<string, string> kvp in _attributes)
            {
                str += kvp.ToString() + " | ";
            }
            str += Environment.NewLine;
            str += "    ";
            foreach (PMEvent e in Ct)
            {
                str += e.WriteText();
            }
            str += "End CT";
            str += Environment.NewLine;
            str += "    ";
            return str;
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
                    Ct.AddLast(e);
                    //do shit here
                }
            }
        }
    }
}
