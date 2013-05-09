using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput;
using DirectOutput.GlobalConfiguration;
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





        

        private void Form1_Load(object sender, EventArgs e)
        {
            new DirectOutput.Frontend.CabinetEditor( DirectOutput.Cab.Cabinet.GetCabinetFromConfigXmlFile(@"C:\Users\Tom\Documents\GitHub\DirectOutput\DirectOutput\bin\Debug\Config\cabinet.xml")).Show();
        
        }
    }
}
