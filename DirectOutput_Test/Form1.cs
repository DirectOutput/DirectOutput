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
          //  DirectOutput.Frontend.MainMenu.Open(new Pinball(new FileInfo(@"X:\Visual Pinball\Tables\plugins\DirectOutput\Config\GlobalConfig_B2SServer.xml"),new FileInfo(@"Y:\Media\Visual Pinball\Tables\Big Brave\Big_Brave_VP915_1.1.2FS_dB2S_NoLw.vpt"),""));


            DirectOutput.Table.Table T = new DirectOutput.Table.Table();
            T.Effects.Add(new DirectOutput.FX.BasicFX.BasicDigitalEffect() { Name = "BumRight", DigitalToyName = "Contactor xxxx" });

            DirectOutput.Table.TableElement E = new DirectOutput.Table.TableElement(DirectOutput.TableElementTypeEnum.EMTable, 15, 0);
            E.AssignedEffects.Add(new DirectOutput.FX.AssignedEffectOrder("BumRight", 1));

            T.TableElements.Add(E);

            T.SaveConfigXmlFile(@"Y:\Media\Visual Pinball\Tables\Big Brave\test.xml");



   

        }
    }
}
