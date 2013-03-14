using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys;
using DirectOutput.Table;

namespace DirectOutput.FX.BasicFX
{
    /// <summary>
    /// The BasicRGBLedEffect is used to turn on and off RGBLeds based on the value of a TableElement.<br/>
    /// This effect is also used when a classical ledcontrol.ini File is parsed for this framework.
    /// </summary>
    public class BasicRGBLedEffect : EffectBase, IEffect
    {


        private string _RGBLedName;


        /// <summary>
        /// Name of the RGBLed toy.
        /// </summary>
        /// <value>
        /// The name of the RGB led toy.
        /// </value>
        public string RGBLedName
        {
            get { return _RGBLedName; }
            set
            {
                if (_RGBLedName != value)
                {
                    _RGBLedName = value;
                    _RGBLed = null;
                }
            }
        }


        /// <summary>
        /// Color of the RGBLed.
        /// </summary>
        /// <value>
        /// The color of the RGBLed.
        /// </value>
        public string Color { get; set; }


        private IRGBToy _RGBLed;

        /// <summary>
        /// Refrence to the RGBLed Toy specified in the RGBLedName property.<br/>
        /// If the RGBLedname property is empty or contains a unknown name or the name of a toy which is not of type RGBLed this property will return null.
        /// </summary>
        public IRGBToy RGBLed
        {
            get
            {

                return _RGBLed;
            }
        }

        private void ResolveName(Pinball Pinball)
        {

            if (!RGBLedName.IsNullOrWhiteSpace() && Pinball.Cabinet.Toys.Contains(RGBLedName))
            {
                if (Pinball.Cabinet.Toys[RGBLedName] is IRGBToy)
                {
                    _RGBLed = (IRGBToy)Pinball.Cabinet.Toys[RGBLedName];
                }

            }

        }

        /// <summary>
        /// Triggers the effect.<br />
        /// If the Value property of the TableElement is 0 the RGBLed will be turned off resp. set to color #000000, if the value is not 0 the RGBLEd will be set to the color specified in the Color property.
        /// If TableElement is null, the RGBLed toy will be set to the value of Color.
        /// </summary>
        /// <param name="TableElement">TableElement which has triggered the effect.</param>
        public override void Trigger(TableElement TableElement)
        {
            if (RGBLed != null && !Color.IsNullOrWhiteSpace())
            {
                if (TableElement != null)
                {
                    if (TableElement.Value == 0)
                    {
                        RGBLed.SetColor("#000000");
                    }
                    else
                    {
                        RGBLed.SetColor(Color);
                    }
                }
            }
            else
            {
                RGBLed.SetColor(Color);
            }
        }

        /// <summary>
        /// Initializes the BasicContactorEffect
        /// </summary>
        public override void Init(Pinball Pinball)
        {

            ResolveName(Pinball);
            if (RGBLed != null) RGBLed.Reset();
        }
        /// <summary>
        /// Finishes the BasicContactorEffect
        /// </summary>
        public override void Finish()
        {
            if (RGBLed != null) RGBLed.Reset();

            _RGBLed = null;

        }





    }
}
