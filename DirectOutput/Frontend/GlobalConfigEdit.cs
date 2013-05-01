using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DirectOutput.GlobalConfiguration;

namespace DirectOutput.Frontend
{
    public partial class GlobalConfigEdit : Form
    {
        protected bool Modified = false;


        #region Validate config data
        public bool ValidateConfigData()
        {
            return ValidateLedcontrolFiles()
                && ValidateFilePatterns(CabinetConfigFilePatterns)
                && ValidateFilePatterns(CabinetScriptFilePatterns)
                && ValidateFilePatterns(TableScriptFilePatterns)
                && ValidateFilePatterns(TableConfigFilePatterns)
                ;
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


        #region Save config data
        public void SaveConfigData(GlobalConfig Config)
        {
            if (ValidateConfigData())
            {
                SaveLedcontrolFiles(Config);

                SaveFilePatterns(Config.CabinetConfigFilePatterns, CabinetConfigFilePatterns);
                SaveFilePatterns(Config.CabinetScriptFilePatterns, CabinetScriptFilePatterns);

                SaveFilePatterns(Config.TableConfigFilePatterns, TableConfigFilePatterns);
                SaveFilePatterns(Config.TableScriptFilePatterns, TableScriptFilePatterns);

                Config.UpdateTimerIntervall = (int)UpdateTimerIntervalMs.Value;
            } 
        }


        private void SaveLedcontrolFiles(GlobalConfig Config)
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
            LoadLedcontrolFiles(Config);

            LoadFilePatterns(Config.CabinetConfigFilePatterns, CabinetConfigFilePatterns);
            LoadFilePatterns(Config.CabinetScriptFilePatterns, CabinetScriptFilePatterns);

            LoadFilePatterns(Config.TableConfigFilePatterns, TableConfigFilePatterns);
            LoadFilePatterns(Config.TableScriptFilePatterns, TableScriptFilePatterns);

            UpdateTimerIntervalMs.Value = Config.UpdateTimerIntervall;
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







    }
}
