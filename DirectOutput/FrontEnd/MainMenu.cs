using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace DirectOutput.Frontend
{
    public partial class MainMenu : Form
    {
        private Pinball Pinball { get; set; }


        private MainMenu(Pinball Pinball)
        {
            InitializeComponent();

            this.Pinball = Pinball;
            
            Version.Text = "Version: {0}".Build(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

            TableName.Text = Pinball.Table.TableName;
            TableFilename.Text = Pinball.Table.TableFilename;
            TableRomname.Text = Pinball.Table.RomName;

            GlobalConfigFilename.Text = (GlobalConfig.Config.GetGlobalConfigFile().Exists ? GlobalConfig.Config.GlobalConfigFilename : "<no global config file found>");

            switch (Pinball.Table.ConfigurationSource)
            {
                case DirectOutput.Table.TableConfigSourceEnum.TableConfigurationFile:
                    TableConfigFilename.Text = Pinball.Table.TableConfigurationFilename;
                    break;
                case DirectOutput.Table.TableConfigSourceEnum.LedControlIni:
                    TableConfigFilename.Text = "Table config parsed from LedControl file.";
                    break;
                default:
                    TableConfigFilename.Text = "<no config file loaded>";
                    break;
            }

            if (Pinball.Cabinet.CabinetConfigurationFilename.IsNullOrWhiteSpace())
            {
                CabinetConfigFilename.Text = "<no config file loaded>";
            }
            else
            {
                CabinetConfigFilename.Text = Pinball.Cabinet.CabinetConfigurationFilename;
            }
             
        }


        public static void Open(Pinball Pinball) {
            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(MainMenu))
                {
                    F.Focus();
                    return;
                }
            }

            MainMenu M = new MainMenu(Pinball);
            M.Show();
        }

        private void ShowCabinetConfiguration_Click(object sender, EventArgs e)
        {
            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(CabinetInfo))
                {
                    F.Focus();
                    return;
                }
            }
            CabinetInfo CI = new CabinetInfo(Pinball.Cabinet);
            CI.Show();
            
        }

        private void ShowTableConfiguration_Click(object sender, EventArgs e)
        {
            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(TableInfo))
                {
                    F.Focus();
                    return;
                }
            }
            TableInfo CI = new TableInfo(Pinball) ;
            CI.Show();
        }

        private void EditGlobalConfiguration_Click(object sender, EventArgs e)
        {
            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(GlobalConfigEditor))
                {
                    F.Focus();
                    return;
                }
            }
            GlobalConfigEditor CI = new GlobalConfigEditor(GlobalConfig.Config.GlobalConfigFilename);
            CI.Show();
        }

        private void ShowLoadedScripts_Click(object sender, EventArgs e)
        {
            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(ScriptInfo))
                {
                    F.Focus();
                    return;
                }
            }
            ScriptInfo CI = new ScriptInfo(Pinball);
            CI.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(AvailableToysInfo))
                {
                    F.Focus();
                    return;
                }
            }
            AvailableToysInfo CI = new AvailableToysInfo();
            CI.Show();
        }

 

    }
}
