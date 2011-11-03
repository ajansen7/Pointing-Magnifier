using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ParseLog
{
    public partial class LoadLog : Form
    {

        private XmlTextReader XML;
        private bool loaded;
        private PointingMagnifier p;

        public LoadLog()
        {
            InitializeComponent();
            loaded = false;
            p = new PointingMagnifier();
        }

        private void btnLoadLog_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                LoadLogs(openFileDialog1.FileNames);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ViewLog vl = new ViewLog();
            vl.UpdateLog(p);
            vl.Show();
        }

        public bool LoadLogs(String[] uris)
        {
            try
            {
                foreach (String uri in uris)
                {
                    XML = new XmlTextReader(uri);

                    while (XML.Read())
                    {
                        Console.Write("loading log");
                        if (XML.Name == "PointingMagnifier")
                        {
                            p.ReadLog(XML);
                        }

                        if (XML.HasAttributes)
                        {
                            for (int i = 0; i < XML.AttributeCount; i++)
                            {
                                XML.MoveToAttribute(i);
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            lblNumStates.Text = String.Format("States: {0}", p.NumStates());
            lblNumEvents.Text = String.Format("Events: Active - {0}, Inactive - {1}", p.NumActiveEvents(), p.NumNonActiveEvents());
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ViewLog vl = new ViewLog();
            vl.ShowFeedback(p);
            vl.Show();
        }
    }
}
