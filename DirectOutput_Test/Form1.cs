using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput;
using DirectOutput.GlobalConfig;
using DirectOutput_Test.Properties;
using System.Configuration;


namespace DirectOutput_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DirectOutput.Cab.Cabinet C = new DirectOutput.Cab.Cabinet();
            C.AutoConfig();
            DirectOutput.Frontend.CabinetInfo CI = new DirectOutput.Frontend.CabinetInfo(C);
            CI.Show();


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {



        }
    }
}
