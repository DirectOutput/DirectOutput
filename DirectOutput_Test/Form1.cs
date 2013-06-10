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



            Pinball P = new Pinball();
            //P.InitStatistics();
            //P.UpdateTableElementStatistics(new TableElementData('S',1,1), TimeSpan.FromTicks(279)) ;
            //P.UpdateTableElementStatistics(new TableElementData('S', 1, 1), TimeSpan.FromTicks(157)); ;
            //P.WriteStatisticsToLog();

   

        }
    }
}
