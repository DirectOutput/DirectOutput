using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.LedControl;

namespace LedControlFileTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SelectLedControlFile_Click(object sender, EventArgs e)
        {
            if (OpenLedControlFile.ShowDialog() == DialogResult.OK)
            {
                string Filename = OpenLedControlFile.FileName;
                LedControlFileName.Text = Filename;

                LedControlConfig L = new LedControlConfig(Filename, 1);

                
            }
        }
    }
}
