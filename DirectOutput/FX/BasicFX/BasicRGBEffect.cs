using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys;
using DirectOutput.Table;

namespace DirectOutput.FX.BasicFX
{
    /// <summary>
    /// The BasicRGBToyEffect is used to turn on (/set a color) and off RGB toys based on the value of a TableElement.<br/>
    /// If the value of the table element is >0, the assigned IRGBToy will be set to the value specified in the Color property, for 0 the IRGBToy is set to black. <br/>
    /// If this effect is used as a static effect, the value of Color will be set on table start.
    /// </summary>
    public class BasicRGBEffect : EffectBase, IEffect
    {
        private string _RGBToyName;


        /// <summary>
        /// Name of the RGB toy.
        /// </summary>
        /// <value>
        /// The name of the RGB toy.
        /// </value>
        public string RGBToyName
        {
            get { return _RGBToyName; }
            set
            {
                if (_RGBToyName != value)
                {
                    _RGBToyName = value;
                    _RGBToy = null;
                }
            }
        }

        /// <summary>
        /// Color for the RGB toy.
        /// </summary>
        /// <value>
        /// The color for the RGB toy.
        /// </value>
        public string Color { get; set; }


        private IRGBToy _RGBToy;

        /// <summary>
        /// Refrence to the RGB Toy specified in the RGBToyName property.<br/>
        /// If the RGBToyName property is empty or contains a unknown name or the name of a toy which does not implement IRGBToy this property will return null.
        /// </summary>
        public IRGBToy RGBToy
        {
            get
            {

                return _RGBToy;
            }
        }

        private void ResolveName(Table.Table Table)
        {

            if (!RGBToyName.IsNullOrWhiteSpace() && Table.Pinball.Cabinet.Toys.Contains(RGBToyName))
            {
                if (Table.Pinball.Cabinet.Toys[RGBToyName] is IRGBToy)
                {
                    _RGBToy = (IRGBToy)Table.Pinball.Cabinet.Toys[RGBToyName];
                }

            }

        }

        /// <summary>
        /// Triggers the effect.<br />
        /// If the Value property of the TableElement is 0 the RGB toy will be turned off resp. set to color #000000, if the value is not 0 the RGB toy will be set to the color specified in the Color property.
        /// If TableElement is null, the IRGBToy will be set to the value of Color.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(TableElementData TableElementData)
        {
            if (RGBToy != null && !Color.IsNullOrWhiteSpace())
            {
                if (TableElementData != null)
                {
                    if (TableElementData.Value == 0)
                    {
                        RGBToy.SetColor("#000000");
                    }
                    else
                    {
                        RGBToy.SetColor(Color);
                    }
                }
            }
            else
            {
                RGBToy.SetColor(Color);
            }
        }

        /// <summary>
        /// Initializes the BasicRGBEffect.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {

            ResolveName(Table);
           
        }
        /// <summary>
        /// Finishes the BasicRGBEffect.
        /// </summary>
        public override void Finish()
        {
            if (RGBToy != null) RGBToy.Reset();

            _RGBToy = null;

        }





    }
}
