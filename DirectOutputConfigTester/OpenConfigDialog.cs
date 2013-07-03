using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectOutputConfigTester
{
    public partial class OpenConfigDialog : Form
    {
        private Settings Settings = new Settings();

        public OpenConfigDialog(Settings Settings=null)
        {
            if (Settings != null)
            {
                this.Settings = Settings;
            }
            InitializeComponent();

            LoadData();
            
        }

        private void LoadData()
        {
            GlobalConfigFilename = Settings.LastGlobalConfigFilename;
            TableFilename = Settings.LastTableFilename;
            RomName = Settings.LastRomName;
            if (Settings.RomNames.Count > 0)
            {
                RomNameComboBox.Items.Clear();
                RomNameComboBox.Items.AddRange(Settings.RomNames.ToArray());
               
            }
        }


        private void GlobalConfigFileSelectButton_Click(object sender, EventArgs e)
        {
            SelectGlobalConfigFile();
        }

        private void GlobalConfigFilename_Click(object sender, EventArgs e)
        {
            SelectGlobalConfigFile();
        }

        private void TableFileSelectButton_Click(object sender, EventArgs e)
        {
            SelectTableFile();
        }

        private void TableFilename_Click(object sender, EventArgs e)
        {
            SelectTableFile();
        }
        private void SelectTableFile()
        {
            if (OpenTableFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                TableFilename = OpenTableFileDialog.FileName;
            }
        }
        private void SelectGlobalConfigFile()
        {

            if (OpenGlobalConfigFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                GlobalConfigFilename = OpenGlobalConfigFileDialog.FileName;
            }
        }



        public string RomName
        {
            get { return RomNameComboBox.Text; }
            set { RomNameComboBox.Text = value; }
        }



        public string GlobalConfigFilename
        {
            get { return GlobalConfigFilenameLabel.Text; }
            set { GlobalConfigFilenameLabel.Text = value; }
        }



        public string TableFilename
        {
            get { return TableFilenameLabel.Text; }
            set { TableFilenameLabel.Text = value; }
        }



    }
}
