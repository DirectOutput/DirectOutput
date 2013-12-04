using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DirectOutput.GlobalConfiguration;
using DirectOutput.General;

namespace DirectOutput.Frontend
{
    public partial class GlobalConfigEdit : Form
    {
        private bool _Modified = false;

        protected bool Modified
        {
            get { return _Modified; }
            set { _Modified = value;
                this.Text="Global configuration editor {0}".Build(_Modified?" <contains unsaved changes>":"");
            }
        }


        private GlobalConfig GlobalConfig;


        public bool SaveConfigData()
        {
            try
            {

                if (!ValidateConfigData())
                {
                    MessageBox.Show(this, "The data specified for the Global Configuration contains some invalid data.\nPlease correct the data before saving.", "Invalid global configuration data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (GlobalConfig.GlobalConfigFilename.IsNullOrWhiteSpace())
                {
                    SaveGlobalConfigDialog.InitialDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
                    if (SaveGlobalConfigDialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return false;
                    }
                    GlobalConfig.GlobalConfigFilename = SaveGlobalConfigDialog.FileName;
                }


                UpdateGlobalConfigData(GlobalConfig);

                GlobalConfig.SaveGlobalConfig();

                MessageBox.Show(this, "Global config saved to\n{0}.\n\nYou must restart the framework to activate the new global config settings.".Build(GlobalConfig.GlobalConfigFilename), "Global configuration saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Modified = false;
                return true;
            }
            catch (Exception E)
            {
                MessageBox.Show(this, "Could not save global configuration.\nA exception occured:\n{0}".Build(E.Message), "Global config save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Exception("Could not save global configuration.\nA exception occured", E);
                return false;
            }
        }


        #region Validate config data
        public bool ValidateConfigData()
        {
            return ValidateLedcontrolFiles()
                && ValidateFilePatterns(GlobalScriptFilePatterns)
                && ValidateFilePatterns(CabinetConfigFilePatterns)
 
                && ValidateFilePatterns(TableScriptFilePatterns)
                && ValidateFilePatterns(TableConfigFilePatterns)
                && ValidateLogFilePattern();
        }

        private bool ValidateLogFilePattern()
        {
            if (!LogFilePattern.Text.IsNullOrWhiteSpace())
            {
                if (!new FilePattern(LogFilePattern.Text).IsValid)
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidateLedcontrolFiles()
        {
            return UpdateLedcontrolFileStatus();
        }

        private bool ValidateFilePatterns(DataGridView Source)
        {
            for (int r = 0; r < Source.Rows.Count; r++)
            {
                if (!new FilePattern((string)Source[0, r].Value).IsValid)
                {
                    return false;
                };
            }
            return true;
        }

        #endregion


        #region Update global config data


        public void UpdateGlobalConfigData(GlobalConfig Config)
        {
            if (ValidateConfigData())
            {
                UpdateLedcontrolFiles(Config);

                SaveFilePatterns(Config.GlobalScriptFilePatterns, GlobalScriptFilePatterns);

                SaveFilePatterns(Config.CabinetConfigFilePatterns, CabinetConfigFilePatterns);
                

                SaveFilePatterns(Config.TableConfigFilePatterns, TableConfigFilePatterns);
                SaveFilePatterns(Config.TableScriptFilePatterns, TableScriptFilePatterns);


                Config.LedControlMinimumEffectDurationMs = (int)MinLedControlEffectDuration.Value;
                Config.LedControlMinimumRGBEffectDurationMs = (int)MinLedControlRGBLedEffectDuration.Value;

                Config.LogFilePattern.Pattern = LogFilePattern.Text;
                Config.EnableLogging = EnableLogging.Checked;
            }
        }


        private void UpdateLedcontrolFiles(GlobalConfig Config)
        {
            Config.LedControlIniFiles.Clear();

            for (int r = 0; r < LedcontrolFiles.Rows.Count; r++)
            {
                Config.LedControlIniFiles.Add(new LedControlIniFile((string)LedcontrolFiles[1, r].Value, (int)LedcontrolFiles[0, r].Value));
            }
        }


        private void SaveFilePatterns(FilePatternList Patterns, DataGridView Source)
        {
            Patterns.Clear();

            for (int r = 0; r < Source.Rows.Count; r++)
            {
                Patterns.Add(new FilePattern((string)Source[0, r].Value));
            }
        }

        #endregion





        #region Config data loading
        private void LoadConfigData(GlobalConfig Config)
        {
            GlobalConfig = Config;

            LoadLedcontrolFiles(Config);

            LoadFilePatterns(Config.GlobalScriptFilePatterns, GlobalScriptFilePatterns);

            LoadFilePatterns(Config.CabinetConfigFilePatterns, CabinetConfigFilePatterns);


            LoadFilePatterns(Config.TableConfigFilePatterns, TableConfigFilePatterns);
            LoadFilePatterns(Config.TableScriptFilePatterns, TableScriptFilePatterns);

            MinLedControlEffectDuration.Value = Config.LedControlMinimumEffectDurationMs;
            MinLedControlRGBLedEffectDuration.Value = Config.LedControlMinimumRGBEffectDurationMs;
    
            EnableLogging.Checked = Config.EnableLogging;
            LogFilePattern.Text = Config.LogFilePattern.Pattern;
            LogFilePatternStatus.Text = (Config.LogFilePattern.IsValid ? "OK" : "Invalid file pattern");
        }

        private void LoadFilePatterns(FilePatternList Patterns, DataGridView Destination)
        {
            foreach (FilePattern FP in Patterns)
            {
                int RowIndex = Destination.Rows.Add();
                Destination[0, RowIndex].Value = FP.Pattern;
                Destination[1, RowIndex].Value = (FP.IsValid ? "OK" : "Invalid file pattern");
            }


            Destination.ClearSelection();
            Destination.Refresh();
        }


        private void LoadLedcontrolFiles(GlobalConfig Config)
        {
            foreach (LedControlIniFile LCF in Config.LedControlIniFiles)
            {
                int RowIndex = LedcontrolFiles.Rows.Add();

                LedcontrolFiles[0, RowIndex].Value = LCF.LedWizNumber.ToString();
                LedcontrolFiles[1, RowIndex].Value = LCF.Filename;
                LedcontrolFiles[2, RowIndex].Value = LCF.Status;

            }
            LedcontrolFiles.ClearSelection();
            LedcontrolFiles.Refresh();
        }


        #endregion

        #region Constructor
        public GlobalConfigEdit()
            : this(new GlobalConfiguration.GlobalConfig())
        {

        }


        public GlobalConfigEdit(GlobalConfig Config)
        {
            InitializeComponent();

            LoadConfigData(Config);
        }
        #endregion



        private string LastDirectory = "";
        private void LedcontrolFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex.IsBetween(0, LedcontrolFiles.Rows.Count - 1))
            {
                if (!LastDirectory.IsNullOrWhiteSpace())
                {
                    SelectLedcontrolFile.InitialDirectory = LastDirectory;
                }
                else
                {
                    SelectLedcontrolFile.InitialDirectory = Assembly.GetExecutingAssembly().Location;
                };
                if (SelectLedcontrolFile.ShowDialog(this) == DialogResult.OK)
                {
                    FileInfo F = new FileInfo(SelectLedcontrolFile.FileName);
                    LastDirectory = F.Directory.FullName;
                    if (!LedcontrolFiles.IsCurrentCellInEditMode)
                    {
                        LedcontrolFiles.BeginEdit(true);
                    }
                    ;
                    LedcontrolFiles.EditingControl.Text = F.FullName;
                    LedcontrolFiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    LedcontrolFiles.Refresh();
                }


            }
        }

        private void LedcontrolFiles_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {


            int Nr = 0;
            List<int> NrList = new List<int>();

            for (int i = 0; i < LedcontrolFiles.Rows.Count; i++)
            {
                if (Nr < Convert.ToInt32((string)LedcontrolFiles.Rows[i].Cells[0].Value))
                {
                    Nr = Convert.ToInt32((string)LedcontrolFiles.Rows[i].Cells[0].Value);
                }
                NrList.Add(Convert.ToInt32((string)LedcontrolFiles.Rows[i].Cells[0].Value));
            }
            if (Nr > 0) Nr++;
            if (!Nr.IsBetween(1, 16))
            {
                Nr = 1;
                for (int i = 1; i < 17; i++)
                {
                    if (!NrList.Contains(i))
                    {
                        Nr = i;
                        break;
                    }
                }
            }

            e.Row.Cells[0].Value = Nr.ToString();
            e.Row.Cells[1].Value = "";
        }


        private void LedcontrolFiles_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (LedcontrolFiles.Rows.Count > 16)
            {
                LedcontrolFiles.AllowUserToAddRows = false;

                while (LedcontrolFiles.Rows.Count > 17)
                {
                    LedcontrolFiles.Rows.RemoveAt(LedcontrolFiles.Rows.Count - 1);
                    LedcontrolFiles.Refresh();
                }

            }
            else
            {
                LedcontrolFiles.AllowUserToAddRows = true;
            }
        }

        private void LedcontrolFiles_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            LedcontrolFiles.AllowUserToAddRows = !(LedcontrolFiles.Rows.Count > 16);
        }

        private void LedcontrolFiles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateLedcontrolFileStatus();
            Modified = true;

        }

        private void LedcontrolFiles_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete this row?", "Delete row", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }


        private void UpdateFilePatternStatus(DataGridView DataGridView, int RowIndex)
        {
            if (RowIndex.IsBetween(0, DataGridView.Rows.Count - 1))
            {
                FilePattern F = new FilePattern((string)DataGridView[0, RowIndex].Value);
                DataGridView[1, RowIndex].Value = (F.IsValid ? "OK" : "Invalid file pattern");
            }
        }

        private void TableConfigFilePatterns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateFilePatternStatus((DataGridView)sender, e.RowIndex);
            Modified = true;
        }

        private void TableScriptFilePatterns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateFilePatternStatus((DataGridView)sender, e.RowIndex);
            Modified = true;
        }

        private void CabinetConfigFilePatterns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateFilePatternStatus((DataGridView)sender, e.RowIndex);
            Modified = true;
        }

        private void CabinetScriptFilePatterns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateFilePatternStatus((DataGridView)sender, e.RowIndex);
            Modified = true;
        }


        private bool UpdateLedcontrolFileStatus()
        {

            bool AllEntriesValid = true;
            Dictionary<int, int> N = new Dictionary<int, int>();
            foreach (DataGridViewRow R in LedcontrolFiles.Rows)
            {
                int Nr = Convert.ToInt32(R.Cells[0].Value);
                if (N.ContainsKey(Nr))
                {
                    N[Nr]++;
                }
                else
                {
                    N.Add(Nr, 1);
                }
            }

            foreach (DataGridViewRow R in LedcontrolFiles.Rows)
            {
                string S = "";
                string F = (string)R.Cells[1].Value;
                if (F.IsNullOrWhiteSpace())
                {
                    S = "No filename set";
                    AllEntriesValid = false;
                }
                else
                {
                    try
                    {
                        if (F.IndexOfAny(Path.GetInvalidPathChars()) == -1)
                        {

                            FileInfo FI = new FileInfo(F);
                            if (FI.Exists)
                            {
                                if (N[Convert.ToInt32(R.Cells[0].Value)] > 1)
                                {
                                    S = "Duplicate Ledwiz number";
                                    AllEntriesValid = false;
                                }
                                else
                                {
                                    S = "OK";
                                }
                            }
                            else
                            {
                                S = "File does not exist";
                            }
                        }
                        else
                        {
                            S = "Invalid filename";
                            AllEntriesValid = false;
                        }
                    }
                    catch
                    {
                        S = "Invalid filename";
                    }
                    AllEntriesValid = false;
                }
                R.Cells[2].Value = S;
            }
            return AllEntriesValid;
        }

        private void LogFilePattern_TextChanged(object sender, EventArgs e)
        {
            LogFilePatternStatus.Text = (new FilePattern(LogFilePattern.Text).IsValid ? "OK" : "Invalid file pattern");
            Modified = true;
        }

        string LastLogDirectory = "";
        private void ShowLogFileDialog_Click(object sender, EventArgs e)
        {
            if (LastLogDirectory.IsNullOrWhiteSpace())
            {
                SelectLogFile.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            }
            else
            {
                SelectLogFile.InitialDirectory = LastLogDirectory;
            }
            if (SelectLogFile.ShowDialog(this) == DialogResult.OK)
            {
                LogFilePattern.Text = SelectLogFile.FileName;
                LastLogDirectory = new FileInfo(SelectLogFile.FileName).Directory.FullName;
                LogFilePatternStatus.Text = (new FilePattern(LogFilePattern.Text).IsValid ? "OK" : "Invalid file pattern");
                Modified = true;
            }
        }

        private void UpdateTimerIntervalMs_ValueChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void EnableLogging_CheckedChanged(object sender, EventArgs e)
        {
            Modified = true;
        }

        private void GlobalScriptFilePatterns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateFilePatternStatus((DataGridView)sender, e.RowIndex);
            Modified = true;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveConfigData();
        }

        private void SaveExitButton_Click(object sender, EventArgs e)
        {
            if (SaveConfigData())
            {
                this.Close();
            };
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();




        }

        private void GlobalConfigEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                switch (MessageBox.Show(this, "The global config editor contains unsaved changes.\nDo you want to save before closing the window?", "Close global config editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3))
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                    case DialogResult.No:
                        break;
                    case DialogResult.Yes:
                        if (!SaveConfigData())
                        {
                            e.Cancel = true;
                            return;
                        }
                        break;
                    default:
                        return;
                }
            }

        }















    }
}
