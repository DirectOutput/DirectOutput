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

using DirectOutput.Cab.Toys;
using DirectOutput.Cab.Out.AdressableLedStrip;
using DirectOutput.FX.RGBAMatrixFX;
using System.Threading;
using DirectOutput.FX;
using DirectOutput.Cab.Out.LW;
using DirectOutput.General.Color;
using System.Xml;
using DirectOutput.Cab.Toys.Virtual;
using DirectOutput.Cab.Toys.Hardware;
using DirectOutput.General.BitmapHandling;


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

        Pinball P;

        private void button1_Click(object sender, EventArgs e)
        {


            FastImageList L = new FastImageList();

            FastImage I = L[@"C:\Users\Tom\Desktop\pleasewait.gif"];


            I = L[@"D:\Hochzeit Nani und Eflam\IMG_1708.JPG"];

        }




        private void button2_Click(object sender, EventArgs e)
        {
            RGBAToyGroup G = new RGBAToyGroup() { Name = "Test", LayerOffset = -10 };

           
            for (int y = 1 ; y < 4; y++)
            {
                List<string> R = new List<string>();

                for (int x = 1; x < 4; x++)
                {
                    if (x == 2)
                    {
                        R.Add("");
                    }
                    else
                    {
                        R.Add("Toy " + x + "." + y);
                    }
                }
                G.RGBAToyNames.Add(R);
            }



            XmlSerializer xsSubmit = new XmlSerializer(G.GetType());
           
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, G);
            Console.WriteLine(sww.ToString()); // Your xml


        }

        private void button3_Click(object sender, EventArgs e)
        {

           
        }
    }
}
