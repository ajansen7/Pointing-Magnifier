using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace LogParser.Model
{
    public class Feedback
    {
        private string _type;
        private bool _image;
        private string _filename;
        private string _time;
        private string _elapsedTime;
        private string _feedback;
        public Feedback(Dictionary<string,string> a, string f)
        {
            foreach (KeyValuePair<String, String> k in a)
            {
                if (k.Key == "Type")
                    _type = k.Value;
                else if (k.Key == "Image")
                    _image = (k.Value == "true");
                else if (k.Key == "Filename")
                    _filename = k.Value;
                else if (k.Key == "T")
                    _time = k.Value;
                else if (k.Key == "eT")
                    _elapsedTime = k.Value;
            }
            _feedback = f;
            //<Feedback Type="QuickFeedback" Image="true" Filename="103_211_8818971.jpg" T="19:54:02:591" eT="00:17:52.2017504" />
        }

        public override string ToString()
        {
            return String.Format("{0} - Image={1} Filename={2} T={3} eT={4} :: {5}", _type, _image, _filename, _time, _elapsedTime, _feedback);
        }
    }
}
