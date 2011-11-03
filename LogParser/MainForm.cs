using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LogParser
{
    public partial class MainForm : Form
    {
        private Model.Root _model;
        private Controller _controller;

        public MainForm()
        {
            _model = new Model.Root();
            _controller = new Controller(ref _model);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //open file dialog, load filenames
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _model.AddFiles(openFileDialog1.FileNames);
            }

            _controller.ParseLogs();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _controller.PrintText();
        }
        
    }
}
