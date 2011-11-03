using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections.Specialized;

namespace LogParser
{
    public class Controller
    {
        private Model.Root _root;
        private XmlReader _reader;

        public Controller(ref Model.Root r)
        {
            _root = r;
        }

        public bool ParseLogs()
        {
            try
            {
                foreach (string f in _root.Files)
                {
                    _reader = new XmlTextReader(f);
                    Console.WriteLine("Loading Log");
                    while (_reader.Read())
                    {
                        if (_reader.Name == "State")
                        {
                            Dictionary<String, String> attributes = GetAttributes();
                            _root.States.Add(new Model.PMState(attributes));  
                            ParseState(_reader.ReadSubtree());
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        //reads the attributes of the current node
        private Dictionary<string, string> GetAttributes()
        {
            Dictionary<String, String> attributes = new Dictionary<String, String>();
            if (_reader.HasAttributes)
            {
                for (int i = 0; i < _reader.AttributeCount; i++)
                {
                    _reader.MoveToAttribute(i);
                    attributes.Add(_reader.Name, _reader.Value);
                }
            }
            _reader.MoveToContent(); //move to the first child node
            return attributes;
        }
        private bool ParseState(XmlReader r)
        {
            try
            {
                //reader starts at "state" tag
                while (r.Read())
                {
                    //Console.WriteLine(r.Name);
                    if (r.Name == "Event")
                    {
                        Dictionary<String, String> attributes = GetAttributes();
                        _root.CurrentState.AddEvent(attributes);
                        ParseEvent(r.ReadSubtree());
                        //ParseState(_reader.ReadSubtree());
                    }
                    else if (r.Name == "Feedback")
                    {
                        Dictionary<String, String> attributes = GetAttributes();
                        _root.CurrentState.AddFeedback(attributes,_reader.Value);
                        //ParseState(_reader.ReadSubtree());
                    }
                    //if (r.HasAttributes)
                    //{
                    //    for (int i = 0; i < r.AttributeCount; i++)
                    //    {
                    //        r.MoveToAttribute(i);
                    //    }
                    //}
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool ParseEvent(XmlReader r)
        {
            try
            {
                //reader starts at "Event" tag
                while (r.Read())
                {
                    //Console.WriteLine(r.Name);
                    if (r.Name == "ME")
                    {
                        Dictionary<String, String> attributes = GetAttributes();
                        _root.CurrentState.CurrentEvent.AddMouseEvent(attributes);
                        //ParseEvent(r.ReadSubtree());
                        //ParseState(_reader.ReadSubtree());
                    }
                    else if (r.Name == "Magnify" || r.Name == "DeMagnify" || r.Name == "Clickthrough")
                    {
                        Dictionary<String, String> attributes = GetAttributes();
                        _root.CurrentState.CurrentEvent.AddCursorEvent(attributes);
                    }
                    //if (r.HasAttributes)
                    //{
                    //    for (int i = 0; i < r.AttributeCount; i++)
                    //    {
                    //        r.MoveToAttribute(i);
                    //    }
                    //}
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void PrintText()
        {
            foreach (Model.PMState s in _root.States)
            {
                Console.WriteLine(s.ToString());
                foreach (Model.PMEvent evt in s.Events)
                {
                    Console.WriteLine(evt.ToString());
                    foreach (Model.Evnt e in evt.Events)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                foreach (Model.Feedback f in s.Feedback)
                {
                    Console.WriteLine(f.ToString());
                }
            }
        }
    }
}
