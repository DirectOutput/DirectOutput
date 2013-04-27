using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.GlobalConfig;

namespace DirectOutput.Frontend
{
    public partial class GlobalConfigEdit : Form
    {


        #region Config data loading
        private void LoadConfigData(Config Config)
        {
            LoadLedcontrolFiles(Config);

            LoadFilePatterns(Config.CabinetConfigFilePatterns, CabinetConfigFilePatterns);
            LoadFilePatterns(Config.CabinetScriptFilePatterns, CabinetScriptFilePatterns);

            LoadFilePatterns(Config.TableConfigFilePatterns, TableConfigFilePatterns);
            LoadFilePatterns(Config.TableScriptFilePatterns, TableScriptFilePatterns);

        }

        private void LoadFilePatterns(FilePatternList Patterns, DataGridView Destination)
        {
            DataTable DataTable = new DataTable();
            DataTable.Columns.Add("File pattern", typeof(string));
            DataTable.Columns.Add("Status", typeof(string));
            DataTable.Columns[1].ReadOnly = true;
            
            DataTable.Rows.Clear();
            foreach (FilePattern FP in Patterns)
            {
                DataTable.Rows.Add(FP.Pattern, (string)(FP.IsValid ? "OK" : "Invalid file pattern"));
            }
            Destination.ClearSelection();
            Destination.AutoGenerateColumns = true;
            Destination.DataSource = DataTable;
            
            Destination.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            
            Destination.Refresh();
        }


        private void LoadLedcontrolFiles(Config Config)
        {
            DataTable LedcontrolFilesDataTable = new DataTable();
            LedcontrolFilesDataTable.Columns.Add("Ledwiz Number", typeof(int));
            LedcontrolFilesDataTable.Columns.Add("Filename", typeof(string));
            LedcontrolFilesDataTable.Columns.Add("File Status", typeof(string));
            LedcontrolFilesDataTable.Columns[2].ReadOnly = true;
            LedcontrolFilesDataTable.Rows.Clear();
            foreach (LedControlIniFile LCF in Config.LedControlIniFiles)
            {
                LedcontrolFilesDataTable.Rows.Add(LCF.LedWizNumber, LCF.Filename, LCF.Status);

            }
            LedcontrolFiles.ClearSelection();
            LedcontrolFiles.AutoGenerateColumns = true;
            LedcontrolFiles.DataSource = LedcontrolFilesDataTable;
            LedcontrolFiles.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            LedcontrolFiles.Refresh();
        }

        
        #endregion

        #region Constructor
        public GlobalConfigEdit()
            : this(new GlobalConfig.Config())
        {

        }


        public GlobalConfigEdit(Config Config)
        {
            InitializeComponent();

            LoadConfigData(Config);
        }
        #endregion


    }
}
