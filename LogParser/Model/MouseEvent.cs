using System;
using System.Collections.Generic;
using System.Text;

namespace LogParser.Model
{
    public class MouseEvent : Evnt
    {
        private Dictionary<string, string> _attributes;
        //protected MouseButtons Btn { get; set; }
        //protected BtnState BtnState { get; set; }
        protected int BtnCount { get; set; }
        //protected Point Location { get; set; }
        protected bool Sythn { get; set; }
        protected bool Magnified { get; set; }

        public MouseEvent()
        {
           // _attributes = a;
        }

        public MouseEvent(Dictionary<string, string> a)
        {
            //<ME Btn="Left" Btn_St="D" Btn_cnt="0" X="750" Y="54" Synth="false" Mag="false" T="19:53:55:841" eT="00:17:46.4234416" />
            _attributes = a;
            //if (k.Key == "Type")
            //    _type = k.Value;
            //else if (k.Key == "Image")
            //    _image = (k.Value == "true");
            //else if (k.Key == "Filename")
            //    _filename = k.Value;
            //else if (k.Key == "T")
            //    _time = k.Value;
            //else if (k.Key == "eT")
            //    _elapsedTime = k.Value;
        }

        public override string ToString()
        {
            return "Mouse Event";
        }
    }
}
