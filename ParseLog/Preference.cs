using System;
using System.Collections.Generic;
using System.Text;

namespace ParseLog
{
    class Preference : PMEvent
    {

        protected String Pref { get; set; }
        protected String Value { get; set; }

        public Preference()
        {

        }

        public override string WriteText()
        {
            throw new NotImplementedException();
        }
    }
}
