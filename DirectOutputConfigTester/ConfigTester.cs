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

namespace DirectOutputConfigTester
{
    public partial class ConfigTester : Form
    {
        private Pinball Pinball;
        private Settings Settings=new Settings();

        public ConfigTester()
        {
            InitializeComponent();

            Settings = Settings.LoadSettings();


            if (!LoadConfig())
            {
                this.Close();
            }
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

                Pinball = new Pinball(OCD.GlobalConfigFilename, OCD.TableFilename, OCD.RomName);

                DisplayTableElements();

                return true;
            }
            else
            {
                return false;
            }
        }

        public void DisplayTableElements() {

            Pinball.Table.TableElements.Sort((TE1,TE2)=>(TE1.TableElementType==TE2.TableElementType?TE1.Number.CompareTo(TE2.Number):TE1.TableElementType.CompareTo(TE2.TableElementType)));

            TableElements.Rows.Clear();

            foreach (TableElement TE in Pinball.Table.TableElements)
            {
                int RowIndex = TableElements.Rows.Add();

                TableElements[TEType.Name, RowIndex].Value = TE.TableElementType.ToString();
                TableElements[TEName.Name, RowIndex].Value = (TE.Name.IsNullOrWhiteSpace()?"":TE.Name);
                TableElements[TENumber.Name, RowIndex].Value = TE.Number;
                TableElements[TEValue.Name, RowIndex].Value = TE.Value;
                TableElements[TEActivate.Name, RowIndex].Value = (TE.Value!=0?"Deactivate":"Activate");
                TableElements[TEActivate.Name, RowIndex].Value = (TE.Value != 0 ? @"Pulse ¯\_/¯" : @"Pulse _/¯\_");

            }
            
           

        }

        private void ConfigTester_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.SaveSettings();
        }
        




    }
}
