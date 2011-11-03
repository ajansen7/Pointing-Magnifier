using System;
using System.Collections.Generic;
using System.Text;

namespace LogParser.Model
{
    public class Root
    {
        private List<string> _files { get; set; }
        private List<PMState> _states { get; set; }

        public Root()
        {
            _files = new List<string>();
            _states = new List<PMState>();
        }

        public List<string> Files {
            get { return _files; }
            set { _files = value; }
        }

        public void AddFiles(string[] fs)
        {
              _files.AddRange(fs);
        }

        public List<PMState> States
        {
            get { return _states; }
            set { _states = value; }
        }

        public PMState CurrentState
        {
            get { return _states[_states.Count - 1]; }
        }
    }
}
