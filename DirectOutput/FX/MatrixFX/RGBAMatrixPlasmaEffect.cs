using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Color;

namespace DirectOutput.FX.MatrixFX
{
    public class RGBAMatrixPlasmaEffect : MatrixPlasmaEffectBase<RGBAColor>
    {

        private RGBAColor _ActiveColor1 = new RGBAColor(0x00, 0x00, 0xff, 0xff);

        /// <summary>
        /// Gets or sets the active color.
        /// </summary>
        /// <value>
        /// The active color.
        /// </value>
        public RGBAColor ActiveColor1
        {
            get { return _ActiveColor1; }
            set { _ActiveColor1 = value; }
        }


        private RGBAColor _ActiveColor2 = new RGBAColor(0x80, 0x80, 0x00, 0xff);

        /// <summary>
        /// Gets or sets the active color 2.
        /// </summary>
        /// <value>
        /// The active color 2.
        /// </value>
        public RGBAColor ActiveColor2
        {
            get { return _ActiveColor2; }
            set { _ActiveColor2 = value; }
        }


        private RGBAColor _InactiveColor = new RGBAColor(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the inactive color.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The inactive color.
        /// </value>
        public RGBAColor InactiveColor
        {
            get { return _InactiveColor; }
            set { _InactiveColor = value; }
        }


        protected override RGBAColor GetEffectValue(int TriggerValue, double Time, double Value, double X, double Y)
        {

            double BlendVal = (Math.Sin(Value * Math.PI * 2 + Time) + 1) / 2;
            RGBAColor Blended = new RGBAColor(
                (int)(BlendVal * ActiveColor1.Red + (1 - BlendVal) * ActiveColor2.Red),
                (int)(BlendVal * ActiveColor1.Green + (1 - BlendVal) * ActiveColor2.Green),
                (int)(BlendVal * ActiveColor1.Blue + (1 - BlendVal) * ActiveColor2.Blue),
                (int)((BlendVal * ActiveColor1.Alpha + (1 - BlendVal) * ActiveColor2.Alpha) * Value)
                );


            RGBAColor D = new RGBAColor();

            int V = TriggerValue.Limit(0, 255);
            D.Red = InactiveColor.Red + (int)((float)(Blended.Red - InactiveColor.Red) * V / 255).Limit(0, 255);
            D.Green = InactiveColor.Green + (int)((float)(Blended.Green - InactiveColor.Green) * V / 255).Limit(0, 255);
            D.Blue = InactiveColor.Blue + (int)((float)(Blended.Blue - InactiveColor.Blue) * V / 255).Limit(0, 255);
            D.Alpha = InactiveColor.Alpha + (int)((float)(Blended.Alpha - InactiveColor.Alpha) * V / 255).Limit(0, 255);
            return D;


            //return new RGBAColor(00, 0, 255, (int)(Value / 255 * TriggerValue));


            // return new RGBAColor((byte)((Math.Sin(Value / 255 * Math.PI) + 1) * 127.5), (byte)((Math.Cos(Value / 255 * Math.PI) + 1) * 127.5), (byte)((Math.Sin(Time * 22.987) + 1) * 127.5), 255);

            //            return new RGBAColor((byte)((Math.Sin(Value / 255 * Math.PI) + 1) * 127.5), (byte)((Math.Cos(Value / 255 * Math.PI) + 1) * 127.5), (byte)((Math.Sin(Time * 22.987) + 1) * 127.5), (int)(Value / 255 * TriggerValue));

            //            return new RGBAColor((byte)((Math.Sin(X * 3.27 + Y * 1.789 + Time * 24.321) + 1) * 127.5), (byte)((Math.Sin(X * 1.27 + Y * 2.659 + Time * 23.371) + 1) * 127.5), (byte)((Math.Sin(X * 2.1798+ Y*2.001327 + Time * 22.987) + 1) * 127.5), (int)(Value / 255 * TriggerValue));
        }
    }
}
