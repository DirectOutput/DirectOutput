using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.Cab;

namespace DirectoutputCabinetConfigEditor
{
    public partial class Main : Form
    {

        public Cabinet Cabinet { get; set; }

        public Main()
        {
            InitializeComponent();
        }

        private void LoadCabinetConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadCabinetConfigDialog.ShowDialog(this) == DialogResult.OK)
            {
                Cabinet C;

                try
                {
                    C = Cabinet.GetCabinetFromConfigXmlFile(LoadCabinetConfigDialog.FileName);

                }
                catch (Exception E)
                {
                    MessageBox.Show("Cant load the cabinet configuration from file\n{0}.\nException Message:\n{0}".Build(LoadCabinetConfigDialog.FileName,string.Join(", ",E.GetNestedMessages())));
                }

            }
        }
    }
}
