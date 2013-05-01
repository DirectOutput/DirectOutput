using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.Cab;
using DirectOutput.Cab.Out;
using System.Reflection;
using DirectOutput.Cab.Toys;
using System.IO;

namespace DirectOutput.Frontend
{
    public partial class CabinetInfo : Form
    {
        #region Constructor
        public CabinetInfo() : this(new Cabinet()) { }

        public CabinetInfo(Cabinet Cabinet)
        {

            InitializeComponent();


            this.Cabinet = Cabinet;

        }
        #endregion



        private void UpdateToys()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Name", typeof(string));
            DT.Columns.Add("Type", typeof(string));

            foreach (IToy T in Cabinet.Toys)
            {
                DT.Rows.Add(T.Name, T.GetType().Name);
            }
            CabinetToys.ClearSelection();
            CabinetToys.Columns.Clear();
            CabinetToys.AutoGenerateColumns = true;
            CabinetToys.DataSource = DT;
            CabinetToys.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            CabinetToys.Refresh();

            UpdateToyProperties(null);

        }

        private void UpdateToyProperties(IToy Toy)
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Property", typeof(string));
            DT.Columns.Add("Value", typeof(string));
            if (Toy != null)
            {
                DT.Rows.Add("Name", Toy.Name);
                DT.Rows.Add("Type", Toy.GetType().Name);


                Type T = Toy.GetType();

                foreach (PropertyInfo PI in T.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (PI.Name != "Name")
                    {
                        DT.Rows.Add(PI.Name, PI.GetValue(Toy, new object[] { }).ToString());
                    }
                }
            }
            CabinetToyProperties.ClearSelection();
            CabinetToyProperties.Columns.Clear();
            CabinetToyProperties.AutoGenerateColumns = true;
            CabinetToyProperties.DataSource = DT;
            CabinetToyProperties.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            CabinetToyProperties.Refresh();

        }



        private void UpdateOutputControllerProperties(IOutputController OutputController)
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Property", typeof(string));
            DT.Columns.Add("Value", typeof(string));
            if (OutputController != null)
            {
                DT.Rows.Add("Name", OutputController.Name);
                DT.Rows.Add("Output Count", OutputController.Outputs.Count);
                
                Type T = OutputController.GetType();

                foreach (PropertyInfo PI in T.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (PI.Name != "Name" && PI.Name != "Outputs")
                    {
                        DT.Rows.Add(PI.Name, PI.GetValue(OutputController, new object[] { }).ToString());
                    }
                }
            }
            CabinetOutputControllerProperties.ClearSelection();
            CabinetOutputControllerProperties.Columns.Clear();
            CabinetOutputControllerProperties.AutoGenerateColumns = true;

            CabinetOutputControllerProperties.DataSource = DT;
            CabinetOutputControllerProperties.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            CabinetOutputControllerProperties.Refresh();

        }

        private void UpdateOutputs(IOutputController OutputController)
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Name", typeof(string));
            if(OutputController!=null) {
                if (OutputController.Outputs.Count > 0)
                {
                    Type T = OutputController.Outputs[0].GetType();

                    foreach (PropertyInfo PI in T.GetProperties(BindingFlags.Instance|BindingFlags.Public))
                    {
                        if (PI.Name != "Name")
                        {
                            DT.Columns.Add(PI.Name, PI.PropertyType);
                        }
                    }

                    foreach (IOutput O in OutputController.Outputs)
                    {
                        object[] values=new object[DT.Columns.Count];
                        values[0] = O.Name;
                        int Index = 1;
                        foreach (PropertyInfo PI in T.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            if (PI.Name != "Name")
                            {
                                values[Index] = PI.GetValue(O, new object[] { });
                                Index++;
                            }
                        }
                        DT.Rows.Add(values);
                    }
                }
                CabinetOutputControllerOutputs.ClearSelection();
                CabinetOutputControllerOutputs.Columns.Clear();
                CabinetOutputControllerOutputs.AutoGenerateColumns = true;
                CabinetOutputControllerOutputs.DataSource = DT;
                CabinetOutputControllerOutputs.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                CabinetOutputControllerOutputs.Refresh();
            }

        }




        private void UpdateOutputControllers()
        {
            DataTable OutputControllersDataTable = new DataTable();
            OutputControllersDataTable.Columns.Add("Name", typeof(string));
            OutputControllersDataTable.Columns.Add("Type", typeof(string));
            OutputControllersDataTable.Columns.Add("Output Count", typeof(int));
            OutputControllersDataTable.Rows.Clear();
            foreach (IOutputController OC in Cabinet.OutputControllers)
            {
                OutputControllersDataTable.Rows.Add(OC.Name, OC.GetType().Name, OC.Outputs.Count);
            }
            CabinetOutputControllers.ClearSelection();
            CabinetOutputControllers.AutoGenerateColumns = true;
            CabinetOutputControllers.DataSource = OutputControllersDataTable;
            CabinetOutputControllers.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            CabinetOutputControllers.Refresh();

            UpdateOutputs(null);
            UpdateOutputControllerProperties(null);

        }

        private void UpdateWindowTitle()
        {
            string S = "Cabinet Configuration";
            if (!Cabinet.Name.IsNullOrWhiteSpace())
            {
                S += " - {0}".Build(Cabinet.Name);
            }
            if (!Cabinet.CabinetConfigurationFilename.IsNullOrWhiteSpace())
            {
                S += " - {0}".Build(Cabinet.CabinetConfigurationFilename);
            }
            else
            {
                S += " - <unsaved cabinet configuration>";
            }
            this.Text = S;
        }


        private Cabinet _Cabinet = new Cabinet();

        public Cabinet Cabinet
        {
            get { return _Cabinet; }
            set
            {
                _Cabinet = value;
                UpdateOutputControllers();
                UpdateToys();
                UpdateWindowTitle();


            }
        }

        private void CabinetOutputControllers_SelectionChanged(object sender, EventArgs e)
        {
            if (CabinetOutputControllers.SelectedRows.Count > 0)
            {
              string N=(string)CabinetOutputControllers.SelectedRows[0].Cells[0].Value;
              if (Cabinet.OutputControllers.Contains(N))
              {
                  UpdateOutputs(Cabinet.OutputControllers[N]);
                  UpdateOutputControllerProperties(Cabinet.OutputControllers[N]);
              }
              else
              {
                  UpdateOutputs(null);
                  UpdateOutputControllerProperties(null);
              }
            }
        }

        private void CabinetToys_SelectionChanged(object sender, EventArgs e)
        {
            if (CabinetToys.SelectedRows.Count > 0)
            {

                string N = (string)CabinetToys.SelectedRows[0].Cells[0].Value;
                if (Cabinet.Toys.Contains(N))
                {
                    UpdateToyProperties(Cabinet.Toys[N]);
                }
                else
                {
                    UpdateToyProperties(null);
                }
            }
        }

        private void ExportCabinetConfiguration_Click(object sender, EventArgs e)
        {
            if (!Cabinet.CabinetConfigurationFilename.IsNullOrWhiteSpace())
            {
                FileInfo FI = new FileInfo(Cabinet.CabinetConfigurationFilename);
                SaveCabinetConfiguration.InitialDirectory = FI.Directory.FullName;
                SaveCabinetConfiguration.FileName = FI.FullName;
            }
            else
            {
                SaveCabinetConfiguration.InitialDirectory = GlobalConfiguration.GlobalConfig.GlobalConfigDirectoryName;
            }
            if (SaveCabinetConfiguration.ShowDialog() == DialogResult.OK)
            {
                Cabinet.CabinetConfigurationFilename = SaveCabinetConfiguration.FileName;
                try
                {
                    Cabinet.SaveConfigXmlFile(SaveCabinetConfiguration.FileName);
                    UpdateWindowTitle();
                    MessageBox.Show("Cabinet configuration saved to\n{0}".Build(SaveCabinetConfiguration.FileName), "Cabinet configuration saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception E)
                {
                    MessageBox.Show("Could not save cabinet config to\n{0}\n\nThe following error occured:\n{1}".Build(SaveCabinetConfiguration.FileName, E.Message), "File save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }











    }
}
