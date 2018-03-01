using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.GlobalConfiguration;
using System.IO;

namespace GlobalConfigEditor
{
    public partial class GlobalConfigEdit : Form
    {

        GlobalConfig Config = new GlobalConfig();

        string ConfigFilename = "";

        public GlobalConfigEdit()
        {
            InitializeComponent();
        }

        private void GlobalConfigEdit_Load(object sender, EventArgs e)
        {
            LoadConfig();
        }



        private void SaveConfig(string Filename)
        {
            Config.IniFilesPath = IniFilesPath.Text;
            Config.CabinetConfigFilePattern.Pattern = CabinetFilename.Text;
            Config.LogFilePattern.Pattern = LogFilename.Text;
            Config.EnableLogging = LoggingEnabled.Checked;
            Config.ClearLogOnSessionStart = ClearLogOnSessionStart.Checked;
            Config.LedWizDefaultMinCommandIntervalMs = (int)LedWizDefaultMinCommandIntervalMs.Value;
            Config.PacLedDefaultMinCommandIntervalMs = (int)PacLedDefaultMinCommandIntervalMs.Value;
            try
            {
                Config.SaveGlobalConfig(Filename);

                this.Text = "Global Configuration Editor - {0}".Build(Filename);
            }
            catch (Exception E)
            {
                MessageBox.Show("Could not save global configuration to file: {0}.\n{1}".Build(Filename, E.Message), "Save error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        private void LoadConfig(string Filename = "")
        {
            ConfigFilename = "";
            this.Text = "Global Configuration Editor - <New global configuration>".Build(Filename);
            if (!Filename.IsNullOrWhiteSpace())
            {
                try
                {
                    Config = GlobalConfig.GetGlobalConfigFromConfigXmlFile(Filename);
                    ConfigFilename = Filename;

                    this.Text = "Global Configuration Editor - {0}".Build(Filename);
                }
                catch (Exception)
                {
                    MessageBox.Show("A exception occured when loading the global config file {0}.\nWill use empty global config instead.".Build(Filename), "Global config loading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Config = new GlobalConfig();
                }
            }
            else
            {
                Config = new GlobalConfig();
            }



            IniFilesPath.Text = Config.IniFilesPath;

            LoggingEnabled.Checked = Config.EnableLogging;
            ClearLogOnSessionStart.Checked = Config.ClearLogOnSessionStart;
            LogFilename.Text = Config.LogFilePattern.Pattern;

            CabinetFilename.Text = Config.CabinetConfigFilePattern.Pattern;
            LedWizDefaultMinCommandIntervalMs.Value = Config.LedWizDefaultMinCommandIntervalMs;
            PacLedDefaultMinCommandIntervalMs.Value = Config.PacLedDefaultMinCommandIntervalMs;
        }

        private void SelectIniFilePathButton_Click(object sender, EventArgs e)
        {
            DirectoryInfo DI = null;
            if (!IniFilesPath.Text.IsNullOrWhiteSpace())
            {
                try
                {
                    DI = new DirectoryInfo(IniFilesPath.Text);
                    if (DI.Exists)
                    {
                        SelectIniFileDirectoryDialog.SelectedPath = DI.FullName;
                    }
                }
                catch { }
            }

            if (SelectIniFileDirectoryDialog.ShowDialog(Owner) == DialogResult.OK)
            {

                IniFilesPath.Text = SelectIniFileDirectoryDialog.SelectedPath;


            }

        }



        private void SelectCabinetConfigFileButton_Click(object sender, EventArgs e)
        {
            FileInfo FI = null;
            if (!CabinetFilename.Text.IsNullOrWhiteSpace())
            {
                try
                {
                    FI = new FileInfo(CabinetFilename.Text);
                    if (FI.Exists)
                    {
                        SelectCabinetConfigFileDialog.FileName = FI.FullName;
                    }
                }
                catch { }
            }

            if (SelectCabinetConfigFileDialog.ShowDialog(Owner) == DialogResult.OK)
            {

                CabinetFilename.Text = SelectCabinetConfigFileDialog.FileName;



            }
        }





        private void SelectLogFileButton_Click(object sender, EventArgs e)
        {
            FileInfo FI = null;
            if (!LogFilename.Text.IsNullOrWhiteSpace())
            {
                try
                {
                    FI = new FileInfo(LogFilename.Text);
                    if (FI.Exists)
                    {
                        SelectLogFileDialog.FileName = FI.FullName;
                    }
                }
                catch { }
            }

            if (SelectLogFileDialog.ShowDialog(Owner) == DialogResult.OK)
            {

                LogFilename.Text = SelectLogFileDialog.FileName;



            }
        }



        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfigFilename.IsNullOrWhiteSpace())
            {
                SaveConfig(ConfigFilename);
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveGlobalConfigDialog.FileName = ConfigFilename;

            if (SaveGlobalConfigDialog.ShowDialog(Owner) == DialogResult.OK)
            {
                SaveConfig(SaveGlobalConfigDialog.FileName);

            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ContinueDialog())
            {
                LoadConfig("");
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ContinueDialog())
            {
                OpenGlobalConfigDialog.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                if (OpenGlobalConfigDialog.ShowDialog(Owner) == DialogResult.OK)
                {
                    LoadConfig(OpenGlobalConfigDialog.FileName);
                }

            }
        }

        private bool ContinueDialog()
        {
            if (ConfigHasChanged)
            {
                switch (MessageBox.Show(Owner, "The current configuration has changed. Do you want to save the changes?\nPress Yes to save your changes.\nPress No to discard your changes.\nPress Cancel to abort this operation.", "Save changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        SaveGlobalConfigDialog.FileName = ConfigFilename;

                        if (SaveGlobalConfigDialog.ShowDialog(Owner) == DialogResult.OK)
                        {
                            SaveConfig(SaveGlobalConfigDialog.FileName);

                        }
                        else
                        {
                            if (MessageBox.Show(Owner, "You config has not been saved.\nDo you want to continue and discard your changes?", "Discard changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                            {
                                return false;
                            }
                        }
                        return true;

                    case System.Windows.Forms.DialogResult.No:
                        return true;

                    default:
                        return false;

                }

            }
            else
            {
                return true;

            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.Close();

        }

        private void GlobalConfigEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !ContinueDialog();
        }


        private bool ConfigHasChanged
        {
            get
            {
                return (Config.IniFilesPath != IniFilesPath.Text || Config.CabinetConfigFilePattern.Pattern != CabinetFilename.Text || Config.EnableLogging != LoggingEnabled.Checked || Config.ClearLogOnSessionStart != ClearLogOnSessionStart.Checked || Config.LogFilePattern.Pattern != LogFilename.Text);





            }
        }
    }
}
