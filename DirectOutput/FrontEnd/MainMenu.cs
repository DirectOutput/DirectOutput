using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectOutput.FrontEnd
{
    public partial class MainMenu : Form
    {
        private Pinball Pinball { get; set; }


        private MainMenu()
        {
            InitializeComponent();
            Version.Text = "Version: {0}".Build(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
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

            MainMenu M = new MainMenu() {Pinball=Pinball};
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

    }
}
