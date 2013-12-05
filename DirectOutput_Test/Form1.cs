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
using DirectOutput.Cab.Out.AdressableLedStrip;
using DirectOutput.FX.RGBAMatrixFX;
using System.Threading;


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
            T.Effects.Add(new LedStripColorEffect() {Name="SetColor", ToyName="Strip 1",FadeMode=FadeModeEnum.Fade,  ActiveColor=new DirectOutput.Cab.Color.RGBAColor("#ff0000ff"),Top=15, Left=20, Width=60,Height=70,LayerNr=1 });

            T.Effects.Add(new LedStripShiftColorEffect() { ShiftDirection=ShiftDirectionEnum.Down, Name = "ShiftColor", ToyName = "Strip 1", FadeMode = FadeModeEnum.Fade, ActiveColor = new DirectOutput.Cab.Color.RGBAColor("#00ff00ff"), ShiftSpeed = 20, Top = 15, Left = 20, Width = 60, Height = 70, LayerNr = 2 });
            T.Effects.Add(new LedStripShiftColorEffect() { Name = "ShiftColor2", ToyName = "Strip 2", FadeMode = FadeModeEnum.Fade, ActiveColor = new DirectOutput.Cab.Color.RGBAColor("#00ff00ff"), ShiftSpeed =4, Top = 0, Left = 20, Width = 60, Height = 100, LayerNr = 2 });


            T.TableElements.Add(TableElementTypeEnum.Switch, 48, 0);
            T.TableElements[TableElementTypeEnum.Switch, 48].AssignedEffects.Add(new AssignedEffect("ShiftColor"));
            T.TableElements[TableElementTypeEnum.Switch, 48].AssignedEffects.Add(new AssignedEffect("ShiftColor2"));
            P = new Pinball();
            P.Table = T;
            P.Cabinet = C;
            P.Init();




           // P.Finish();

        }




        private void button2_Click(object sender, EventArgs e)
        {

            TableElementData D;
            D.TableElementType = TableElementTypeEnum.Switch;
            D.Number = 48;
            D.Value = 100;
           P.Table.Effects["SetColor"].Trigger(D);
           P.Cabinet.Update();
           P.MainThreadSignal();
            Thread.Sleep(2000);

            D.Value = 0;

            P.Table.Effects["SetColor"].Trigger(D);
            P.Cabinet.Update();
            P.MainThreadSignal();


        }

        private void button3_Click(object sender, EventArgs e)
        {

           // P.ReceiveData('W', 48, 255);


            //Thread.Sleep(3000);


           // P.ReceiveData('W', 48, 0);

            //Thread.Sleep(2000);

            P.ReceiveData('W', 48, 255);
            Thread.Sleep(400);


            P.ReceiveData('W', 48, 0);
        
        }
    }
}
