using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace PointingMagnifier
{
    public class Preferences
    {
        private Keys _sc1, _sc2,_sc3;
        public Preferences()
        {
            KeysConverter k = new KeysConverter();
            Properties.Settings s = Properties.Settings.Default;
            s.Reload();
            //if (s.UID == 0)
            //{
            //    //s.UID = new Random().Next(10000);
            //    //while (!uploader.InitializeUID(s.UID.ToString(), ""))
            //    //    s.UID = new Random().Next(10000);
            //    UID setID = new UID();
            //    if (setID.ShowDialog()==DialogResult.OK)
            //        s.UID = setID.ID;
            //}
            _sc1 = (Keys)k.ConvertFromString(s.Shortcut_1);
            _sc2 = (Keys)k.ConvertFromString(s.Shortcut_2);
            _sc3 = (Keys)k.ConvertFromString(s.Shortcut_3);
        }
        public void Reset()
        {
            int uid = Properties.Settings.Default.UID;
            System.TimeSpan elapsed = Properties.Settings.Default.ElapsedTime;
            bool logging = Properties.Settings.Default.Logging;
            bool baseline = Properties.Settings.Default.Baseline;
            bool study = Properties.Settings.Default.Study;
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.UID = uid;
            Properties.Settings.Default.ElapsedTime = elapsed;
            Properties.Settings.Default.Baseline = baseline;
            Properties.Settings.Default.Logging = logging;
            Properties.Settings.Default.Study = study;
            Properties.Settings.Default.Save();
            KeysConverter k = new KeysConverter();
            _sc1 = (Keys)k.ConvertFromString(Properties.Settings.Default.Shortcut_1);
            _sc2 = (Keys)k.ConvertFromString(Properties.Settings.Default.Shortcut_2);
            _sc3 = (Keys)k.ConvertFromString(Properties.Settings.Default.Shortcut_3);
        }
        public bool Study
        {
            get { return Properties.Settings.Default.Study; }
            set
            {
                Properties.Settings.Default.Study = value;
                Properties.Settings.Default.Save();
            }
        }
        public bool Logging
        {
            get { return Properties.Settings.Default.Logging; }
            set
            {
                Properties.Settings.Default.Logging = value;
                Properties.Settings.Default.Save();
            }
        }
        public bool Baseline
        {
            get { return Properties.Settings.Default.Baseline; }
            set
            {
                Properties.Settings.Default.Baseline = value;
                Properties.Settings.Default.Save();
            }
        }
        public System.TimeSpan ElapsedTime
        {
            get { return Properties.Settings.Default.ElapsedTime; }
            set
            {
                Properties.Settings.Default.ElapsedTime = value;
                Properties.Settings.Default.Save();
            }
        }
        public System.Drawing.Color Color
        {
            get { return Properties.Settings.Default.Color; }
            set {
            Properties.Settings.Default.Color = value;
            Properties.Settings.Default.Save();
            }
        }
        public int Size
        {
            get { return Properties.Settings.Default.Size; }
            set { 
            Properties.Settings.Default.Size = value;
            Properties.Settings.Default.Save();
            }
        }
        public int Magnification
        {
            get { return Properties.Settings.Default.Magnififaction; }
            set { 
            Properties.Settings.Default.Magnififaction = value;
            Properties.Settings.Default.Save();
            }
        }
        public double Transparency
        {
            get { return Properties.Settings.Default.Transparency; }
            set { 
            Properties.Settings.Default.Transparency = value;
            Properties.Settings.Default.Save();
            }
        }
        public bool Shortcuts
        {
            get { return Properties.Settings.Default.Shortcuts; }
            set { 
            Properties.Settings.Default.Shortcuts = value;
            Properties.Settings.Default.Save();
            }
        }
        public bool Gain
        {
            get { return Properties.Settings.Default.Gain; }
            set { 
            Properties.Settings.Default.Gain = value;
            Properties.Settings.Default.Save();
            }
        }
        public bool Crosshairs
        {
            get { return Properties.Settings.Default.Crosshairs; }
            set { 
            Properties.Settings.Default.Crosshairs = value;
            Properties.Settings.Default.Save();
            }
        }
        public Keys Shortcuts1
        {
            get { return _sc1; }
            set { 
                _sc1 = value;
                KeysConverter k = new KeysConverter();
                string s1 = k.ConvertToString(_sc1);
                Properties.Settings.Default.Shortcut_1 = s1;
                Properties.Settings.Default.Save();
            }
        }
        public Keys Shortcuts2
        {
            get { return _sc2; }
            set { 
                _sc2 = value;
                KeysConverter k = new KeysConverter();
                string s2 = k.ConvertToString(_sc2);
                Properties.Settings.Default.Shortcut_2 = s2;
                Properties.Settings.Default.Save();
            }
        }
        public Keys Shortcuts3
        {
            get { return _sc3; }
            set
            {
                _sc3 = value;
                KeysConverter k = new KeysConverter();
                string s3 = k.ConvertToString(_sc3);
                Properties.Settings.Default.Shortcut_3 = s3;
                Properties.Settings.Default.Save();
            }
        }
    }
}
