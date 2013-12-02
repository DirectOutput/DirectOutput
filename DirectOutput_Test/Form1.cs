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
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.FX.RGBAFX;
using System.Xml.Serialization;
using DirectOutput.FX.TimmedFX;
using System.Reflection;
using DirectOutput.FX;
using DirectOutput.Cab.Toys;


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

            Curve F = new Curve();
            F.Data[1] = 20;
            //F.Name = "Test";

            string Xml = "";
            using (MemoryStream ms = new MemoryStream())
            {
                new XmlSerializer(F.GetType()).Serialize(ms, F);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms, Encoding.Default))
                {
                    Xml = sr.ReadToEnd();
                    sr.Dispose();
                }
            }
            Console.WriteLine(Xml);


            byte[] xmlBytes = Encoding.Default.GetBytes(Xml);
            using (MemoryStream ms = new MemoryStream(xmlBytes))
            {
                try
                {
                   Curve F2=(Curve)new XmlSerializer(typeof(Curve)).Deserialize(ms);
                }
                catch (Exception E)
                {

                    Exception Ex = new Exception("Could not deserialize the cabinet config from XML data.", E);
                    Ex.Data.Add("XML Data", Xml);
                    Log.Exception("Could not load cabinet config from XML data.", Ex);
                    throw Ex;
                }
            }

        }




        private void button2_Click(object sender, EventArgs e)
        {

  


        }
    }
}
