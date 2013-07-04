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

        public OpenConfigDialog(Settings Settings = null)
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
                Settings.RomNames.Sort();
                RomNameComboBox.Items.Clear();
                RomNameComboBox.Items.AddRange(Settings.RomNames.ToArray());
            }

            GlobalConfigFilenameComboBox.Items.Clear();
            GlobalConfigFilenameComboBox.Items.AddRange(Settings.GlobalConfigFilenames.ToArray());

            TableFilenameComboBox.Items.Clear();
            TableFilenameComboBox.Items.Add("");
            TableFilenameComboBox.Items.AddRange(Settings.TableFilenames.ToArray());
        }

        private void SaveData()
        {
            Settings.LastGlobalConfigFilename = GlobalConfigFilename;
            Settings.LastTableFilename = TableFilename;
            Settings.LastRomName = RomName;

            Settings.RomNames.Clear();
            foreach (string Item in RomNameComboBox.Items)
            {
                Settings.RomNames.Add(Item);
            }

            if (!Settings.RomNames.Contains(RomName))
            {
                Settings.RomNames.Add(RomName);
            }

            Settings.GlobalConfigFilenames.Remove(GlobalConfigFilename);
            Settings.GlobalConfigFilenames.Insert(0, GlobalConfigFilename);

            if (!TableFilename.IsNullOrWhiteSpace())
            {
                Settings.TableFilenames.Remove(TableFilename);
                Settings.TableFilenames.Insert(0, TableFilename);
            }
        }


        private void GlobalConfigFileSelectButton_Click(object sender, EventArgs e)
        {
            SelectGlobalConfigFile();
        }



        private void TableFileSelectButton_Click(object sender, EventArgs e)
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
            get { return GlobalConfigFilenameComboBox.Text; }
            set { GlobalConfigFilenameComboBox.Text = value; }
        }



        public string TableFilename
        {
            get { return TableFilenameComboBox.Text; }
            set { TableFilenameComboBox.Text = value; }
        }

        private void TableFileClearButton_Click(object sender, EventArgs e)
        {
            TableFilename = "";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            
            SaveData();
        }



    }
}
