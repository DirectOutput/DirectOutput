using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using DirectOutput;
using DirectOutput.Cab.Toys.Virtual;
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
                G.ToyNames.Add(R);
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
