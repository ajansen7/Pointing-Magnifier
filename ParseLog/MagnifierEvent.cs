using System;
using System.Collections.Generic;
using System.Text;

namespace ParseLog
{
    public enum MEvnt{Magnify, Demagnify, Clickthrough, NonClickthrough}

    class MagnifierEvent : PMEvent
    {
        protected MEvnt MEvnt { get; set; }

        public MagnifierEvent() { }

        public override string WriteText()
        {
            throw new NotImplementedException();
        }
    }
}
