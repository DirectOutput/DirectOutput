using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LedControlConverter.Settings;
using System.IO;
using DirectOutput.LedControl.Loader;
using DirectOutput.GlobalConfiguration;
using DirectOutput.Cab;
using DirectOutput.Cab.Out;

namespace LedControlConverter
{
    public partial class ConverterInput : Form
    {

        public ConverterInput()
        {
            InitializeComponent();

            PageNr = 0;

            UpdateButtons();

            BeforePageChange += new EventHandler<BeforePageChangeEventArgs>(InputFiles_BeforePageChange);

            LoadSettings();

        }



        ConverterSettings Settings = new ConverterSettings();

        public void LoadSettings()
        {
            CabinetConfigMode.SelectedIndex = (int)Settings.CabinetConfigMode;

            foreach (LedControlIniFile IF in Settings.InputFiles)
            {
                ShowInputFile(IF);
            }

            UpdateButtons();
        }






        #region Form close
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConverterImput_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to close the LedControl Converter?", "Close LedControl Converter", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        #endregion







        #region Page handling

        public int PageNr
        {
            get { return Wizard.SelectedIndex; }
            set
            {
                if (Wizard.SelectedIndex != value)
                {
                    int LastPage = Wizard.SelectedIndex;
                    if (!OnBeforePageChange(LastPage, value))
                    {
                        Wizard.SelectTab(value);
                        OnAfterPageChange(LastPage, value);
                    }
                }
            }
        }



        private void BackButton_Click(object sender, EventArgs e)
        {
            if (PageNr > 0)
            {
                PageNr--;
                UpdateButtons();
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (PageNr < Wizard.TabCount - 1) { PageNr++; }
            UpdateButtons();
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {

        }


        public class PageChangeEventArgs : EventArgs
        {
            public int LastPageNr;
            public int NextPageNr;
        }

        public class BeforePageChangeEventArgs : PageChangeEventArgs
        {
            public bool Cancel;
        }


        public event EventHandler<BeforePageChangeEventArgs> BeforePageChange;
        /// <summary>
        /// Occurs before the wizard changes its page
        /// </summary>
        /// <param name="LastPageNr">The last page nr.</param>
        /// <param name="NextPageNr">The next page nr.</param>
        /// <returns>true of the page change should be canceled, otherwise false.</returns>
        protected bool OnBeforePageChange(int LastPageNr, int NextPageNr)
        {
            if (BeforePageChange != null)
            {
                BeforePageChangeEventArgs EA = new BeforePageChangeEventArgs() { LastPageNr = LastPageNr, NextPageNr = NextPageNr };
                BeforePageChange(this, EA);
                return EA.Cancel;
            }
            return false;
        }

        public event EventHandler<PageChangeEventArgs> AfterPageChange;
        /// <summary>
        /// Occurs After the wizard changes its page
        /// </summary>
        protected void OnAfterPageChange(int LastPageNr, int NextPageNr)
        {
            if (AfterPageChange != null)
            {
                AfterPageChange(this, new PageChangeEventArgs() { LastPageNr = LastPageNr, NextPageNr = NextPageNr });
            }
        }



        public void UpdateButtons()
        {

            BackButton.Enabled = (PageNr > 0);

            int MaxPageNr = 0;
            if (Settings.InputFiles.Count > 0 && Settings.InputFiles.All(IF => IF.LedWizNumber > 0))
            {
                MaxPageNr = 1;

                if (Settings.ConfigsToConvert.Count > 0)
                {
                    MaxPageNr = 2;
                }
            }

            NextButton.Enabled = (PageNr < MaxPageNr);
            FinishButton.Enabled = (PageNr == Wizard.TabCount - 1);

        }


        #endregion


        #region InputFiles handling

        bool RomNamesUpdateRequired = false;

        private void InputFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.All(File => File.EndsWith(".ini")))
                {
                    AddInputFiles(files);
                    UpdateButtons();
                }
                else
                {
                    MessageBox.Show("One or several files have no .ini extension.\nFiles have not been added.", "Illegal extension", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

        private void InputFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (((string[])e.Data.GetData(DataFormats.FileDrop)).All(File => File.EndsWith(".ini")))
                {


                    e.Effect = DragDropEffects.Link;
                }
            }
        }

        private void InputFiles_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            string Filename = (string)e.Row.Cells[InputFiles.Columns[InputFilesFilename.Name].Index].Value;
            Settings.InputFiles.RemoveAll(F => F.Filename == Filename);
            RomNamesUpdateRequired = true;
            UpdateButtons();
        }


        private void AddInputFiles(IEnumerable<string> Filenames, bool HideWarningMessage = false)
        {
            List<string> IgnoredFilenames = new List<string>();
            List<string> IgnoredFilenumbers = new List<string>();


            foreach (string Filename in Filenames)
            {
                if (!Settings.InputFiles.Any(F => F.Filename == Filename))
                {
                    FileInfo FI = new FileInfo(Filename);
                    if (FI.Extension.ToLower() == ".ini")
                    {
                        FI.GetNameWithoutExtension();
                        string Name = FI.GetNameWithoutExtension();
                        if (Name.Length > 0)
                        {
                            int Number = 1;
                            for (int i = Name.Length - 1; i >= 0; i--)
                            {
                                if (Name.Substring(i).IsInteger())
                                {
                                    Number = Name.Substring(i).ToInteger();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (Settings.InputFiles.Any(F => F.LedWizNumber == Number))
                            {
                                IgnoredFilenumbers.Add(Filename);

                                Number = -1;
                                while (Settings.InputFiles.Any(F => F.LedWizNumber == Number))
                                {
                                    Number--;
                                }
                            }

                            LedControlIniFile IF = new LedControlIniFile() { Filename = Filename, LedWizNumber = Number };
                            Settings.InputFiles.Add(IF);
                            ShowInputFile(IF);
                            RomNamesUpdateRequired = true;
                        }
                    }
                }
                else
                {
                    IgnoredFilenames.Add(Filename);
                }
            }

            if (!HideWarningMessage)
            {
                StringBuilder S = new StringBuilder();
                if (IgnoredFilenames.Count > 0)
                {
                    if (IgnoredFilenames.Count > 10)
                    {
                        S.AppendLine("{0} files have not been added to the list because they already exist.".Build(IgnoredFilenames.Count));
                    }
                    {
                        S.AppendLine("The following files have not been added because they already exist in the list:");
                        foreach (string F in IgnoredFilenames)
                        {
                            S.AppendLine("-  {0}".Build(F));
                        }
                    }
                    S.AppendLine();
                }

                if (IgnoredFilenumbers.Count > 0)
                {
                    if (IgnoredFilenumbers.Count > 10)
                    {
                        S.AppendLine("The numbers in the filenames of {0} files has been replaced with another number since the number did already exist.".Build(IgnoredFilenumbers.Count));
                    }
                    {
                        S.AppendLine("The numbers of the following files has been replaced with another number since the number did already exist:");
                        foreach (string F in IgnoredFilenumbers)
                        {
                            S.AppendLine("-  {0}".Build(F));
                        }

                    }
                    S.AppendLine("Please change the number for the files to a unique positive number.");
                }

                if (S.Length > 0)
                {
                    MessageBox.Show(S.ToString(), "Add files warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            UpdateButtons();
        }

        private int ShowInputFile(LedControlIniFile InputFile)
        {
            int RowIndex = InputFiles.Rows.Add();
            InputFiles[InputFilesFilename.Name, RowIndex].Value = InputFile.Filename;
            InputFiles[InputFilesLedwizNr.Name, RowIndex].Value = InputFile.LedWizNumber;
            return RowIndex;
        }
        private void AddLedControlFilesButton_Click(object sender, EventArgs e)
        {
            if (SelectInputFiles.ShowDialog(this) == DialogResult.OK)
            {
                AddInputFiles(SelectInputFiles.FileNames);
            }
        }

        void InputFiles_BeforePageChange(object sender, ConverterInput.BeforePageChangeEventArgs e)
        {
            if (e.LastPageNr == 0 && e.NextPageNr == 1)
            {
                if (RomNamesUpdateRequired)
                {
                    LedControlConfigList L = new LedControlConfigList();
                    L.LoadLedControlFiles(Settings.InputFiles, false);

                    List<string> RomNames = new List<string>();

                    foreach (LedControlConfig LC in L)
                    {
                        foreach (TableConfig TC in LC.TableConfigurations)
                        {
                            if (!RomNames.Any(R => R.ToUpper() == TC.ShortRomName.ToUpper()))
                            {
                                RomNames.Add(TC.ShortRomName);
                            }
                        }
                    }

                    if (RomNames.Count > 0)
                    {

                        UpdateAvailableRomNames(RomNames);

                        RomNamesUpdateRequired = false;
                    }
                    else
                    {
                        e.Cancel = true;
                        MessageBox.Show("The specified files do not contain any table configurations.\nPlease select other files.", "No configs found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }


        #endregion


        #region Config selection handling
        private void UpdateAvailableRomNames(List<string> RomNames)
        {
            ConfigsToConvert.Items.Clear();

            Settings.ConfigsToConvert.RemoveAll(CC => !RomNames.Any(RN => RN.ToUpper() == CC.ToUpper()));

            foreach (string NewItem in RomNames)
            {
                ConfigsToConvert.Items.Add(NewItem, Settings.ConfigsToConvert.Any(C => C.ToUpper() == NewItem.ToUpper()));
            }
            UpdateButtons();
        }

        private void SelectAllConfigsButton_Click(object sender, EventArgs e)
        {
            Settings.ConfigsToConvert.Clear();
            for (int x = 0; x < ConfigsToConvert.Items.Count; x++)
            {
                ConfigsToConvert.SetItemChecked(x, true);
                Settings.ConfigsToConvert.Add((string)ConfigsToConvert.Items[x]);
            }


            UpdateButtons();
        }

        private void UnselectAllConfigsButton_Click(object sender, EventArgs e)
        {
            Settings.ConfigsToConvert.Clear();
            for (int x = 0; x < ConfigsToConvert.Items.Count; x++)
            {
                ConfigsToConvert.SetItemChecked(x, false);
            }

            UpdateButtons();

        }


        private void ConfigsToConvert_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (!Settings.ConfigsToConvert.Any(C => C.ToUpper() == ((string)ConfigsToConvert.Items[e.Index]).ToUpper()))
                {
                    Settings.ConfigsToConvert.Add((string)ConfigsToConvert.Items[e.Index]);
                }
            }
            else
            {
                if (Settings.ConfigsToConvert.Any(C => C.ToUpper() == ((string)ConfigsToConvert.Items[e.Index]).ToUpper()))
                {
                    Settings.ConfigsToConvert.RemoveAll(C => C.ToUpper() == ((string)ConfigsToConvert.Items[e.Index]).ToUpper());
                }
            }
            UpdateButtons();
        }

        #endregion

        



        private void UpdateCabinetData()
        {
            
            switch (Settings.CabinetConfigMode)
            {
                case CabinetConfigModeEnum.CreateNewCabinetConfig:
                    CabinetConfigFilename.Enabled = false;
                    CabinetConfigFilenameLabel.Text = "New cabinet config file:";
                    CabinetConfigFilename.Text = Settings.NewCabinetConfigFilename;
                    break;
                case CabinetConfigModeEnum.UpdateExistingConfig:
                    CabinetConfigFilename.Enabled = true;
                    CabinetConfigFilenameLabel.Text = "Cabinet config file to update:";
                    CabinetConfigFilename.Text = Settings.ExistingCabinetConfigFilename;


                    break;
                case CabinetConfigModeEnum.MatchExistingConfig:
                    CabinetConfigFilename.Enabled = true;
                    CabinetConfigFilenameLabel.Text = "Cabinet config file to match:";
                    CabinetConfigFilename.Text = Settings.ExistingCabinetConfigFilename;

                    break;
            }

        }




        private void CabinetConfigMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            Settings.CabinetConfigMode = (CabinetConfigModeEnum)((ComboBox)sender).SelectedIndex;

            UpdateCabinetData();
        }

        private void CabinetConfigFilename_Click(object sender, EventArgs e)
        {
            if (Settings.CabinetConfigMode == CabinetConfigModeEnum.CreateNewCabinetConfig)
            {
                if (SaveCabinetConfigFile.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.NewCabinetConfigFilename = SaveCabinetConfigFile.FileName;
                    UpdateCabinetData();
                }
            } else {
                if (SelectCabinetConfigFile.ShowDialog(this) == DialogResult.OK)
                {
                    if (Cabinet.TestCabinetConfigXmlFile(SelectCabinetConfigFile.FileName))
                    {
                        Settings.ExistingCabinetConfigFilename = SelectCabinetConfigFile.FileName;
                        UpdateCabinetData();
                    }
                    else
                    {
                        MessageBox.Show("The file {0} does not contain a valid cabinet configuration.".Build(SelectCabinetConfigFile.FileName), "Invalid file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void CabinetConfigFilename_DragDrop(object sender, DragEventArgs e)
        {
            //TODO: Capture directory drop
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1)
                {
                    if (Cabinet.TestCabinetConfigXmlFile(files[0]))
                    {
                        Settings.ExistingCabinetConfigFilename = files[0];
                        UpdateCabinetData();
                    }
                    else
                    {
                        MessageBox.Show("The file {0} does not contain a valid cabinet configuration.".Build(files[0]),"Invalid file",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void CabinetConfigFilename_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (((string[])e.Data.GetData(DataFormats.FileDrop)).Length == 1)
                {
                    e.Effect = DragDropEffects.Link;
                }
            }
        }




    }
}
