using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.FX;
using DirectOutput.Table;
using System.Reflection;
using System.IO;

namespace DirectOutput.FrontEnd
{
    public partial class TableInfo : Form
    {
        private Pinball _Pinball;

        public Pinball Pinball
        {
            get { return _Pinball; }
            set
            {
                _Pinball = value;
                UpdateAll();
            }
        }

        private void UpdateAll()
        {
            UpdateWindowTitle();
            UpdateTableGeneral();
            UpdateTableEffects();
            UpdateTableElements();
            UpdateTableAssignedStaticEffects();
        }

        private void UpdateTableGeneral()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Property", typeof(string));
            DT.Columns.Add("Value", typeof(string));

            DT.Rows.Add("Table name", Pinball.Table.TableName);
            DT.Rows.Add("Rom name", Pinball.Table.RomName);
            DT.Rows.Add("Table filename", Pinball.Table.TableFilename);
            DT.Rows.Add("Table configuration source", ((TableConfigSourceEnum)Pinball.Table.ConfigurationSource).ToString());
            if (Pinball.Table.ConfigurationSource == TableConfigSourceEnum.TableConfigurationFile)
            {
                DT.Rows.Add("Table configuration filename", Pinball.Table.TableConfigurationFilename);
            }
            foreach (TableElementTypeEnum TET in Enum.GetValues(typeof(TableElementTypeEnum)))
            {
                DT.Rows.Add("{0} count".Build(((TableElementTypeEnum)TET).ToString()), Pinball.Table.TableElements.GetTableElementDictonaryForType(TET).Count);
            }

            DT.Rows.Add("Configured effects", Pinball.Table.Effects.Count);
            DT.Rows.Add("Assigned startup effects", Pinball.Table.AssignedStaticEffects.Count);

            TableGeneral.ClearSelection();
            TableGeneral.Columns.Clear();
            TableGeneral.AutoGenerateColumns = true;
            TableGeneral.DataSource = DT;
            TableGeneral.Refresh();
            TableGeneral.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }


        private void UpdateTableAssignedStaticEffects()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Name", typeof(string));
            DT.Columns.Add("Type", typeof(string));
            DT.Columns.Add("Configured in", typeof(string));

            if (Pinball != null)
            {
                foreach (AssignedEffect AE in Pinball.Table.AssignedStaticEffects)
                {
                    Type T = null;
                    string Configured = "<not configured>";
                    if (Pinball.Table.Effects.Contains(AE.EffectName))
                    {
                        T = Pinball.Table.Effects[AE.EffectName].GetType();
                        Configured = "Table";
                    }
                    else if (Pinball.Cabinet.Effects.Contains(AE.EffectName))
                    {
                        T = Pinball.Cabinet.Effects[AE.EffectName].GetType();
                        Configured = "Cabinet";
                    }
                    DT.Rows.Add(AE.EffectName, T.Name, Configured);
                }
            }
            TableAssignedStaticEffects.ClearSelection();
            TableAssignedStaticEffects.Columns.Clear();
            TableAssignedStaticEffects.AutoGenerateColumns = true;
            TableAssignedStaticEffects.DataSource = DT;
            TableAssignedStaticEffects.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            TableAssignedStaticEffects.Refresh();
        }


        private void UpdateTableEffects()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Name", typeof(string));
            DT.Columns.Add("Type", typeof(string));

            if (Pinball != null)
            {
                foreach (IEffect E in Pinball.Table.Effects)
                {
                    DT.Rows.Add(E.Name, E.GetType().Name);
                }
            }
            TableEffects.ClearSelection();
            TableEffects.Columns.Clear();
            TableEffects.AutoGenerateColumns = true;
            TableEffects.DataSource = DT;
            TableEffects.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            TableEffects.Refresh();

            UpdateEffect();
        }

        private void UpdateTableElements()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Type", typeof(string));
            DT.Columns.Add("Number", typeof(int));
            DT.Columns.Add("Name", typeof(string));
            DT.Columns.Add("Value", typeof(int));
            DT.Columns.Add("Assigned FX", typeof(int));

            if (Pinball != null)
            {
                foreach (TableElement E in Pinball.Table.TableElements)
                {
                    DT.Rows.Add(((TableElementTypeEnum)E.TableElementType).ToString(), E.Number, E.Name, E.Value, E.AssignedEffects.Count);
                }
            };

            DT.DefaultView.Sort = "Type ASC, Number ASC";
            TableElements.ClearSelection();
            TableElements.Columns.Clear();
            TableElements.AutoGenerateColumns = true;
            TableElements.DataSource = DT;
            TableElements.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            //TableElements.Refresh();
        }

        private void UpdateTableElement()
        {
            if (TableElements.SelectedRows.Count > 0)
            {
                DataRow DR = ((DataRowView)TableElements.SelectedRows[0].DataBoundItem).Row;
                
                if(Pinball.Table.TableElements.Contains((TableElementTypeEnum)Enum.Parse(typeof(TableElementTypeEnum), (string)DR.ItemArray[0]), (int)DR.ItemArray[1])) {
                    UpdateTableElementAssignedEffects(Pinball.Table.TableElements[(TableElementTypeEnum)Enum.Parse(typeof(TableElementTypeEnum), (string)DR.ItemArray[0]), (int)DR.ItemArray[1]]);
                    return;
                }
            }
            UpdateTableElementAssignedEffects(null);
        }

        private void UpdateTableElementAssignedEffects(TableElement TableElement)
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Name", typeof(string));
            DT.Columns.Add("Type", typeof(string));
            DT.Columns.Add("Configured in", typeof(string));

            if (TableElement != null)
            {
                foreach (AssignedEffect AE in TableElement.AssignedEffects)
                {
                    Type T = null;
                    string Configured="<not configured>";
                    if(Pinball.Table.Effects.Contains(AE.EffectName)) {
                     T= Pinball.Table.Effects[AE.EffectName].GetType();
                     Configured = "Table";
                    } else if (Pinball.Cabinet.Effects.Contains(AE.EffectName)) {
                        T = Pinball.Cabinet.Effects[AE.EffectName].GetType();
                        Configured = "Cabinet";
                    }
                    DT.Rows.Add(AE.EffectName,T.Name,Configured);
                }
            }
            TableElementAssignedEffects.ClearSelection();
            TableElementAssignedEffects.Columns.Clear();
            TableElementAssignedEffects.AutoGenerateColumns = true;
            TableElementAssignedEffects.DataSource = DT;
            TableElementAssignedEffects.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            TableElementAssignedEffects.Refresh();
        }


        private void UpdateEffect()
        {
            if (TableEffects.SelectedRows.Count > 0)
            {
                string N = (string)TableEffects.SelectedRows[0].Cells[0].Value;
                if (Pinball.Table.Effects.Contains(N))
                {
                    UpdateEffectProperties(Pinball.Table.Effects[N]);
                    return;
                }
            }
            UpdateEffectProperties(null);
        }

        private void UpdateEffectProperties(IEffect Effect)
        {


            DataTable DT = new DataTable();
            DT.Columns.Add("Property", typeof(string));
            DT.Columns.Add("Value", typeof(string));
            if (Effect != null)
            {
                DT.Rows.Add("Name", Effect.Name);

                Type T = Effect.GetType();
                DT.Rows.Add("Type", T.Name);

                foreach (PropertyInfo PI in T.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (PI.Name != "Name" )
                    {
                        DT.Rows.Add(PI.Name, PI.GetValue(Effect, new object[] { }).ToString());
                    }
                }
            }
            TableEffectProperties.ClearSelection();
            TableEffectProperties.Columns.Clear();
            TableEffectProperties.AutoGenerateColumns = true;

            TableEffectProperties.DataSource = DT;
            TableEffectProperties.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            TableEffectProperties.Refresh();

            

        }


        private void UpdateWindowTitle()
        {
            string S = "Table Configuration";
            if (!Pinball.Table.TableName.IsNullOrWhiteSpace())
            {
                S += " - {0}".Build(Pinball.Table.TableName);
            }

            if (!Pinball.Table.RomName.IsNullOrWhiteSpace())
            {
                S += " - {0}".Build(Pinball.Table.RomName);
            }
            this.Text = S;
        }



        public TableInfo(Pinball Pinball)
        {
            InitializeComponent();
            this.Pinball = Pinball;
        }

        private void TableEffects_SelectionChanged(object sender, EventArgs e)
        {
            UpdateEffect();
        }

        private void TableElements_SelectionChanged(object sender, EventArgs e)
        {
            UpdateTableElement();
        }

        private void RefreshWindow_Click(object sender, EventArgs e)
        {
            UpdateAll();
        }

        private void ExportTableConfiguration_Click(object sender, EventArgs e)
        {
            if (!Pinball.Table.TableConfigurationFilename.IsNullOrWhiteSpace())
            {
                FileInfo FI = new FileInfo(Pinball.Table.TableConfigurationFilename);
                SaveTableConfigDialog.InitialDirectory = FI.Directory.FullName;
                SaveTableConfigDialog.FileName = FI.FullName;
            }
            else if (!Pinball.Table.TableFilename.IsNullOrWhiteSpace())
            {
                FileInfo FI = new FileInfo(Pinball.Table.TableFilename);
                SaveTableConfigDialog.InitialDirectory = FI.Directory.FullName;
                SaveTableConfigDialog.FileName = Path.Combine(FI.Directory.FullName,"{0}.xml".Build(FI.GetNameWithoutExtension()));
            }
            else
            {
                SaveTableConfigDialog.InitialDirectory = GlobalConfig.Config.GlobalConfigDirectoryName;
            }
            if (SaveTableConfigDialog.ShowDialog() == DialogResult.OK)
            {
                Pinball.Table.TableConfigurationFilename = SaveTableConfigDialog.FileName;
                try
                {
                    Pinball.Table.SaveConfigXmlFile(SaveTableConfigDialog.FileName);
                    UpdateWindowTitle();
                    MessageBox.Show("Table configuration saved to\n{0}".Build(SaveTableConfigDialog.FileName), "Table configuration saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception E)
                {
                    MessageBox.Show("Could not save table config to\n{0}\n\nThe following error occured:\n{1}".Build(SaveTableConfigDialog.FileName, E.Message), "File save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




    
  

    
    }
}
