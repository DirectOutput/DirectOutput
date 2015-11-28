using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectOutput.FX.MatrixFX
{
    public abstract class MatrixPlasmaEffectBase<MatrixElementType> : MatrixEffectBase<MatrixElementType>
    {

        private int _PlasmaSpeed = 100;

        /// <summary>
        /// Gets or sets the plasma speed.
        /// Default is 100.
        /// </summary>
        /// <value>
        /// The plasma speed.
        /// </value>
        public int PlasmaSpeed
        {
            get { return _PlasmaSpeed; }
            set { _PlasmaSpeed = value.Limit(1, int.MaxValue); }
        }


        private int _PlasmaScale = 100;

        /// <summary>
        /// Gets or sets the plasma scale.
        /// Defaults to 100. Higher values will give plasma effects with a higher density/more ripples.
        /// </summary>
        /// <value>
        /// The plasma scale.
        /// </value>
        public int PlasmaScale
        {
            get { return _PlasmaScale; }
            set { _PlasmaScale = value; }
        }


        private int _PlasmaDensity = 100;

        /// <summary>
        /// Gets or sets the plasma density.
        /// Default is 100.
        /// </summary>
        /// <value>
        /// The plasma density.
        /// </value>
        public int PlasmaDensity
        {
            get { return _PlasmaDensity; }
            set { _PlasmaDensity = value; }
        }




        private const int RefreshIntervalMs = 30;

        [XmlIgnore]
        int CurrentTrigerrValue = 0;

        [XmlIgnore]
        DateTime PlasmaStartDateTime = DateTime.Now;

        [XmlIgnore]
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

        [XmlIgnore]
        double Time = 0;


        private void DrawFrame()
        {
            int F = (FadeMode == FadeModeEnum.OnOff ? (CurrentTrigerrValue > 0 ? 255 : 0) : CurrentTrigerrValue.Limit(0, 255));

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

            PrecalcTimeValues(Time);

            for (int X = 0; X < W; X++)
            {
                double XX = SX * X;
                PrecalcXValues(XX, Time);
                
                for (int Y = 0; Y < H; Y++)
                {
                    double YY = (double)SY * Y;

                    double V = CalcPositionValue(XX, YY, Time).Limit(0, 1);

                    MatrixLayer[AreaLeft + X, AreaTop + Y] = GetEffectValue(F, Time, V, XX, YY);
                    
                }
               
            }
           
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

        double CosTime = 0;
        double SinTimeDiv11767Mult05 = 0;
        double CosTimeDiv1833371Mult05=0;
        private void PrecalcTimeValues(double Time)
        {
            CosTime = Math.Cos(Time);
            SinTimeDiv11767Mult05 = .5 * Math.Sin(Time / 1.1767);
            CosTimeDiv1833371Mult05 = .5 * Math.Cos(Time / 1.833371);
        }

        double XWaveValue1 = 0;
        double XWaveValue2 = 0;
        private void PrecalcXValues(double X, double Time)
        {
            XWaveValue1 = Math.Sin(X * Math.PI * (PlasmaDensity / 28) + Time);
            XWaveValue2 = X * Math.Sin(Time / 2.567);
        }


        private double CalcPositionValue(double X, double Y, double Time)
        {
            double V = XWaveValue1;

            V += Math.Sin(Math.PI * (PlasmaDensity / 28) * (XWaveValue2 + Y * CosTime) + Time);

            double cx = X + SinTimeDiv11767Mult05;
            double cy = Y + CosTimeDiv1833371Mult05;
            V += Math.Sin(Math.Sqrt((Math.PI * (PlasmaDensity / 56)) * (Math.PI * (PlasmaDensity / 56)) * (cx * cx + cy * cy) + 1) + Time);
            V = ((V + 3) / 6.0);

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
