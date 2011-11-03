using System;
using System.Collections.Generic;
using System.Text;

namespace LogParser.Model
{
    public abstract class Evnt
    {
        protected DateTime Time { get; set; }
        protected DateTime ElapsedTime { get; set; }
    }
}
