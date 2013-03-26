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
            Table T = new Table();
            T.AssignedStaticEffects.Add(new DirectOutput.FX.AssignedEffect("Test", 5));
            T.TableElements.Add(new TableElement(TableElementTypeEnum.Solenoid, 1, 1));
            T.TableElements[0].AssignedEffects.Add(new DirectOutput.FX.AssignedEffect("TestEffect", 2));
            T.Effects.Add(new DirectOutput.FX.BasicFX.BasicDigitalEffect() { Name = "TestEffect", DigitalToyName = "Blabla" });
            string S = T.GetConfigXml();
            Console.WriteLine(S);
        }
    }
}
