using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.MatrixFX
{
    public abstract class MatrixPlasmaEffectBase<MatrixElementType> : MatrixEffectBase<MatrixElementType>
    {

        private int _PlasmaSpeed = 100;

        public int PlasmaSpeed
        {
            get { return _PlasmaSpeed; }
            set { _PlasmaSpeed = value.Limit(1, int.MaxValue); }
        }


        private int _PlasmaScale = 100;

        public int PlasmaScale
        {
            get { return _PlasmaScale; }
            set { _PlasmaScale = value; }
        }


        private int _PlasmaDensity = 100;

        public int PlasmaDensity
        {
            get { return _PlasmaDensity; }
            set { _PlasmaDensity = value; }
        }




        private const int RefreshIntervalMs = 30;

        int CurrentTrigerrValue = 0;

        DateTime PlasmaStartDateTime = DateTime.Now;

        public bool Active { get; private set; }

        private void DoPlasma()
        {
            int V = CurrentTrigerrValue.Limit(0, 255);
            if (V > 0)
            {

                if (!Active)
                {
                    PlasmaStartDateTime = DateTime.Now;
                    Table.Pinball.Alarms.RegisterIntervalAlarm(RefreshIntervalMs, DoPlasma);
                    Active = true;
                }
                DrawFrame();
            }
            else
            {
                Table.Pinball.Alarms.UnregisterIntervalAlarm(DoPlasma);
                Active = false;
                ClearFrame();
            }
        }

        double Time = 0;


        private void DrawFrame()
        {
            int F = (FadeMode == FadeModeEnum.OnOff ? (CurrentTrigerrValue > 0 ? 255 : 0) : CurrentTrigerrValue.Limit(0, 255));


            // double Time = (DateTime.Now - PlasmaStartDateTime).TotalMilliseconds * ((double)PlasmaSpeed/10000);
            Time += ((double)PlasmaSpeed / 2000);
            int W = AreaWidth;
            int H = AreaHeight;



            double SX;
            double SY;

            if (W >= H)
            {
                SX = (double)PlasmaScale / 100 / W;
                SY = (double)SX;
            }
            else
            {
                SY = (double)PlasmaScale / 100 / H;
                SX = (double)SY;
            }

            //SX = (double)1 / W;
            //SY = (double)1 / H;
            for (int Y = 0; Y < H; Y++)
            {
                double YY = (double)SY * Y;
                for (int X = 0; X < W; X++)
                {
                    double XX = SX * X;
                    double V = CalcPositionValue(XX, YY, Time).Limit(0, 1);

                    MatrixLayer[AreaLeft + X, AreaTop + Y] = GetEffectValue(F, Time, V, XX, YY);
                    //   Console.Write("{0} ", ((int)V).ToString("X2"));
                }
                //Console.WriteLine();
            }
            //Console.WriteLine();

        }


        private void ClearFrame()
        {
            int W = AreaWidth;
            int H = AreaHeight;

            double KX = (double)W / H;
            for (int Y = 0; Y < H; Y++)
            {
                for (int X = 0; X < W; X++)
                {
                    MatrixLayer[AreaLeft + X, AreaTop + Y] = GetEffectValue(0, 0, 0, 0, 0);
                }

            }

        }


        private double CalcPositionValue(double X, double Y, double Time)
        {
            double V = 0;

            //  V = X / Math.PI;

            V += Math.Sin(X * Math.PI * (PlasmaDensity / 28) + Time);

            V += Math.Sin(Math.PI * (PlasmaDensity / 28) * (X * Math.Sin(Time / 2.567) + Y * Math.Cos(Time)) + Time);


            //          V += Math.Sin((Y*8  + Time)) / 2;
            //V += Math.Sin((X*8  + Y*8 + Time)) / 2;

            double cx = X + .5 * Math.Sin(Time / 1.1767);
            double cy = Y + .5 * Math.Cos(Time / 1.833371);
            V += Math.Sin(Math.Sqrt((Math.PI * (PlasmaDensity / 56)) * (Math.PI * (PlasmaDensity / 56)) * (cx * cx + cy * cy) + 1) + Time);
            V = ((V + 3) / 6.0);
            if (V < 0 || V > 1)
            {

            }
            return V;
        }

        protected abstract MatrixElementType GetEffectValue(int TriggerValue, double Time, double Value, double X, double Y);

        public override void Trigger(Table.TableElementData TableElementData)
        {

            if (MatrixLayer != null)
            {
                CurrentTrigerrValue = TableElementData.Value;
                if (CurrentTrigerrValue > 0 && !Active)
                {
                    DoPlasma();
                }
            }

        }


    }
}
