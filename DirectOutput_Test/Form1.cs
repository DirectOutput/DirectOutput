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
using System.IO;
using DirectOutput.LedControl;
using DirectOutput.Table;
using DirectOutput.Cab;
using DirectOutput.Cab.Out.DMX;


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

    

   

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Pinball P = new Pinball();

            Cabinet C = new Cabinet();

            ArtNet N = new ArtNet();
            N.Name = "Artnet Node 1";
            N.Universe = 0;
            N.BroadcastAddress = "255.255.255.255";

            N.Init(P);

            N.Finish();

            C.OutputControllers.Add(N);

           Console.WriteLine(   C.GetConfigXml());


        }
    }
}
