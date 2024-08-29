﻿using System;
using System.Windows.Forms;


namespace DirectOutput.Frontend
{
    public partial class MainMenu : Form
    {
        private Pinball Pinball { get; set; }


        private MainMenu(Pinball Pinball)
        {


            this.Pinball = Pinball;
            InitializeComponent();
            
            Version V = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            DateTime BuildDate = new DateTime(2000, 1, 1).AddDays(V.Build).AddSeconds(V.Revision * 2);

            Version.Text = "Version {0} as of {1}".Build(V.ToString(), BuildDate.ToString("yyyy.MM.dd HH:mm"));

            TableName.Text = (Pinball.Table.TableName.IsNullOrWhiteSpace() ? "<No table name set>" : Pinball.Table.TableName);
            TableFilename.Text = (Pinball.Table.TableFilename.IsNullOrWhiteSpace() ? "<No table file name set>" : Pinball.Table.TableFilename);
            TableRomname.Text = (Pinball.Table.RomName.IsNullOrWhiteSpace() ? "<No rom name set>" : Pinball.Table.RomName);
            DOFPath.Text = DirectOutputHandler.GetInstallFolder();

            GlobalConfigFilename.Text = (Pinball.GlobalConfig.GlobalConfigFilename.IsNullOrWhiteSpace() ? "<no global config file set>" : (Pinball.GlobalConfig.GetGlobalConfigFile().Exists ? Pinball.GlobalConfig.GlobalConfigFilename : "<no global config file found>"));


            switch (Pinball.Table.ConfigurationSource)
            {
                case DirectOutput.Table.TableConfigSourceEnum.TableConfigurationFile:
                    TableConfigFilename.Text = Pinball.Table.TableConfigurationFilename;
                    break;
                case DirectOutput.Table.TableConfigSourceEnum.IniFile:
                    TableConfigFilename.Text = "Table config parsed from LedControl file(s).";
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


        public static void Open(Pinball Pinball, Form Owner=null)
        {


            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(MainMenu))
                {
                    F.BringToFront();
                    F.Focus();
                    return;
                }
            }

            MainMenu M = new MainMenu(Pinball);

            if (Owner == null)
            {
                M.Show();
            }
            else
            {
                M.StartPosition = FormStartPosition.CenterParent;
                M.Show(Owner);
            }
        }

        private void ShowCabinetConfiguration_Click(object sender, EventArgs e)
        {
            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(CabinetInfo))
                {
                    F.BringToFront();
                    F.Focus();
                    return;
                }
            }
            CabinetInfo CI = new CabinetInfo(Pinball.Cabinet);
            CI.StartPosition = FormStartPosition.CenterParent;
            CI.Show(this);

        }

        private void ShowTableConfiguration_Click(object sender, EventArgs e)
        {
            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(TableInfo))
                {
                    F.BringToFront();
                    F.Focus();
                    return;
                }
            }
            TableInfo CI = new TableInfo(Pinball);
            CI.StartPosition = FormStartPosition.CenterParent;
            CI.Show(this);
        }




        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(AvailableToysInfo))
                {
                    F.BringToFront();
                    F.Focus();
                    return;
                }
            }
            AvailableToysInfo CI = new AvailableToysInfo();
            CI.StartPosition = FormStartPosition.CenterParent;
            CI.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Form F in Application.OpenForms)
            {
                if (F.GetType() == typeof(AvailableEffectsInfo))
                {
                    F.BringToFront();
                    F.Focus();
                    return;
                }
            }
            AvailableEffectsInfo CI = new AvailableEffectsInfo();
            CI.StartPosition = FormStartPosition.CenterParent;
            CI.Show(this);
        }

        //private void ShowSystemMonitor_Click(object sender, EventArgs e)
        //{
        //    foreach (Form F in Application.OpenForms)
        //    {
        //        if (F.GetType() == typeof(SystemMonitor))
        //        {
        //            F.BringToFront();
        //            F.Focus();
        //            return;
        //        }
        //    }
        //    SystemMonitor CI = new SystemMonitor(Pinball);
        //    CI.StartPosition = FormStartPosition.CenterParent;
        //    CI.Show(this);
        //}




    }
}
