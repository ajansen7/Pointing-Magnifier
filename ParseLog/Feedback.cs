using System;
using System.Collections.Generic;
using System.Text;

namespace ParseLog
{
    public class Feedback : PMEvent
    {
        private Dictionary<string, string> _attributes;
        private String _content;
        public Feedback(Dictionary<string,string> a,string content) 
        {
            _attributes = a;
            _content = content;
        }

        public override string WriteText()
        {
            String str = "Feedback: ";
            foreach (KeyValuePair<string, string> kvp in _attributes)
            {
                str += kvp.ToString() + " | ";
            }
            str += _content;
            str += Environment.NewLine;
            str += "    ";
            return str;
        }

    }
}
