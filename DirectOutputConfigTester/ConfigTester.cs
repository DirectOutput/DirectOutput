using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput;
using DirectOutput.Table;
using System.Threading;

namespace DirectOutputConfigTester
{
    public partial class ConfigTester : Form
    {
        private Pinball Pinball;
        private Settings Settings = new Settings();
        private bool OutputActive = false;
        public ConfigTester()
        {
            InitializeComponent();

            Settings = Settings.LoadSettings();

            PulseDurationInput.Value = Settings.PulseDurationMs;

        }

        public bool LoadConfig()
        {
            OpenConfigDialog OCD = new OpenConfigDialog(Settings);
            if (OCD.ShowDialog() == DialogResult.OK)
            {
                if (Pinball != null)
                {
                    Pinball.Finish();
                }


                Pinball = new Pinball();
                Pinball.Init(OCD.GlobalConfigFilename, OCD.TableFilename, OCD.RomName);

                DisplayTableElements();


                return true;
            }
            else
            {

                return false;
            }
        }

        public void DisplayTableElements()
        {
            OutputActive = false;
            Pinball.Table.TableElements.Sort((TE1, TE2) => (TE1.TableElementType == TE2.TableElementType ? TE1.Number.CompareTo(TE2.Number) : TE1.TableElementType.CompareTo(TE2.TableElementType)));

            TableElements.Rows.Clear();

            foreach (TableElement TE in Pinball.Table.TableElements)
            {
                int RowIndex = TableElements.Rows.Add();
                TableElements.Rows[RowIndex].Tag = TE;
                TableElements[TEType.Name, RowIndex].Value = TE.TableElementType.ToString();
                TableElements[TEName.Name, RowIndex].Value = (TE.Name.IsNullOrWhiteSpace() ? "" : TE.Name);
                TableElements[TENumber.Name, RowIndex].Value = TE.Number;
                TableElements[TEValue.Name, RowIndex].Value = TE.Value;
                TableElements[TEActivate.Name, RowIndex].Value = (TE.Value > 0 ? "Deactivate" : "Activate");
                TableElements[TEPulse.Name, RowIndex].Value = (TE.Value > 0 ? @"Pulse ¯\_/¯" : @"Pulse _/¯\_");

            }

            OutputActive = true;


        }

        private void ConfigTester_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void ConfigTester_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Pinball != null)
            {

                Pinball.Finish();
            }
            Settings.SaveSettings();
        }

        private void ConfigTester_Load(object sender, EventArgs e)
        {
            if (!LoadConfig())
            {
                this.Close();
            }
        }

        private void TableElements_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int OrgValue;
            if (e.ColumnIndex >= 0 && e.ColumnIndex < TableElements.ColumnCount)
            {
                if (e.RowIndex >= 0 && e.RowIndex < TableElements.RowCount)
                {
                    if (TableElements.Columns[e.ColumnIndex].Name == TEActivate.Name)
                    {
                        OrgValue = 0;
                        int.TryParse(TableElements[TEValue.Name, e.RowIndex].Value.ToString(), out OrgValue);
                        int NewValue = (OrgValue > 0 ? 0 : 1);
                        TableElements[TEValue.Name, e.RowIndex].Value = NewValue;
                    }
                    else if (TableElements.Columns[e.ColumnIndex].Name == TEPulse.Name)
                    {
                        OrgValue = 0;
                        int.TryParse(TableElements[TEValue.Name, e.RowIndex].Value.ToString(),out OrgValue);
                        int PulseValue = (OrgValue > 0 ? 0 : 1);

                        TableElements[TEValue.Name, e.RowIndex].Value = PulseValue;
                        Thread.Sleep(Settings.PulseDurationMs);
                        TableElements[TEValue.Name, e.RowIndex].Value = OrgValue;
                    }
                }
            }
        }

        private void TableElements_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < TableElements.ColumnCount)
            {
                if (e.RowIndex >= 0 && e.RowIndex < TableElements.RowCount)
                {
                    if (TableElements.Columns[e.ColumnIndex].Name == TEValue.Name)
                    {
                        object Value = TableElements[TEValue.Name, e.RowIndex].Value;
                        int NumericValue = 0;
                        if (!int.TryParse(Value.ToString(), out NumericValue))
                        {
                            MessageBox.Show("The value entered is not a valid number.\nWill set the value to 0.", "Invalid value entered", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            TableElements[TEValue.Name, e.RowIndex].Value = 0;
                            NumericValue = 0;
                        }
                        if (OutputActive)
                        {
                            TableElement TE = (TableElement)TableElements.Rows[e.RowIndex].Tag;

                            TableElementData D = TE.GetTableElementData();
                            D.Value = NumericValue;
                            Pinball.ReceiveData(D);
                        }

                        TableElements[TEActivate.Name, e.RowIndex].Value = (NumericValue > 0 ? "Deactivate" : "Activate");
                        TableElements[TEPulse.Name, e.RowIndex].Value = (NumericValue > 0 ? @"Pulse ¯\_/¯" : @"Pulse _/¯\_");

                    }
                }
            }
        }

        private void PulseDurationInput_ValueChanged(object sender, EventArgs e)
        {
            Settings.PulseDurationMs = (int)PulseDurationInput.Value;
        }

        private void ActivateAllButton_Click(object sender, EventArgs e)
        {
            for (int RowIndex = 0; RowIndex < TableElements.Rows.Count; RowIndex++)
            {
                TableElements[TEValue.Name, RowIndex].Value = 1;
            }
        }

        private void DeactivateAllButton_Click(object sender, EventArgs e)
        {
            for (int RowIndex = 0; RowIndex < TableElements.Rows.Count; RowIndex++)
            {
                TableElements[TEValue.Name, RowIndex].Value = 0;
            }
        }

        private void LoadConfigButton_Click(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void ShowFrontEndButton_Click(object sender, EventArgs e)
        {
            DirectOutput.Frontend.MainMenu.Open(Pinball);
        }







    }
}
