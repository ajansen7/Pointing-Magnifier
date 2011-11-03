using System;
using System.Collections.Generic;
using System.Text;

namespace ParseLog
{
    public abstract class PMEvent
    {
        protected DateTime Time { get; set; }
        protected DateTime ElapsedTime { get; set; }

        public abstract String WriteText();
    }
}
