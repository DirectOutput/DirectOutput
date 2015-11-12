using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.FX.MatrixFX.BitmapShapes;
using DirectOutput.General.BitmapHandling;
using DirectOutput.FX.MatrixFX;
using DirectOutput.Cab;
using DirectOutput.General;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Cabinet C = new Cabinet();
            Curve CU = new Curve()
            {
                Name="Binary0Curve"
            };


            CU.Data[0] = 0;
            for (int i = 1; i < 256; i++)
            {
                CU.Data[i] = 255;
            }

            C.Curves.Add(CU);


            CU = new Curve()
            {
                Name = "SinusCurve"
            };


            CU.Data[0] = 255;
            for (int i = 0; i < 256; i++)
            {
                CU.Data[i] = (byte)(Math.Round(Math.Sin(Math.PI / 128 * i - Math.PI / 2) * 128 + 128, 0));
            }

            C.Curves.Add(CU);

            C.SaveConfigXmlFile("test.xml");

        }
    }
}
