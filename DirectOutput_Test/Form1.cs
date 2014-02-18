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

            Cabinet C = new Cabinet();
            C.OutputControllers.Add(new WS2811StripController() { Name = "StripController 1", ControllerNumber = 1, NumberOfLeds = 96+65 });
            C.Toys.Add(new LedStrip() {Name="Strip 1",OutputControllerName="StripController 1",Width=32, Height=3,ColorOrder=RGBOrderEnum.WS2812});
            C.Toys.Add(new LedStrip() { Name = "Strip 2", OutputControllerName = "StripController 1", FadingCurveName = "SwissLizardsLedCurve", Width = 65, Height = 1, ColorOrder = RGBOrderEnum.WS2812, FirstLedNumber = 97 });

            Table T = new Table();
            T.Effects.Add(new RGBAMatrixColorEffect() { Name = "SetColor", ToyName = "Strip 1", FadeMode = FadeModeEnum.Fade, ActiveColor = new DirectOutput.General.Color.RGBAColor("#0000ffff"), Top = 15, Left = 20, Width = 60, Height = 70, LayerNr = 1 });

            T.Effects.Add(new RGBAMatrixColorFlickerEffect() {  Name = "Flicker", ToyName = "Strip 1", FadeMode = FadeModeEnum.Fade, ActiveColor = new DirectOutput.General.Color.RGBAColor("#ffffff80"), Top = 15, Left = 20, Width = 60, Height = 70, LayerNr = 3,Density=50,MinFlickerDurationMs=30,MaxFlickerDurationMs=50 });
            T.Effects.Add(new RGBAMatrixColorShiftEffect() { Name = "ShiftColor2", ToyName = "Strip 2", FadeMode = FadeModeEnum.Fade, ActiveColor = new DirectOutput.General.Color.RGBAColor("#00ff00ff"), ShiftSpeed = 4, Top = 0, Left = 20, Width = 60, Height = 100, LayerNr = 2 });


            T.TableElements.Add(TableElementTypeEnum.Switch, 48, 0);
            T.TableElements[TableElementTypeEnum.Switch, 48].AssignedEffects.Add(new AssignedEffect("SetColor"));
            T.TableElements[TableElementTypeEnum.Switch, 48].AssignedEffects.Add(new AssignedEffect("Flicker"));
            T.TableElements[TableElementTypeEnum.Switch, 48].AssignedEffects.Add(new AssignedEffect("ShiftColor2"));
            P = new Pinball();
            P.Table = T;
            P.Cabinet = C;
            P.Init();




           // P.Finish();

        }




        private void button2_Click(object sender, EventArgs e)
        {
            P.ReceiveData('W', 48, 255);


        }

        private void button3_Click(object sender, EventArgs e)
        {

            P.ReceiveData('W', 48, 0);
        }
    }
}
