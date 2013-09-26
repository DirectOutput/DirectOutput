using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.RGBAFX
{
    /// <summary>
    /// Base class for effects using RGBA toys
    /// </summary>
    public abstract class RGBAEffectBase:EffectBase
    {
        protected Table.Table Table;

        private string _ToyName;


        /// <summary>
        /// Name of the RGBAToy.
        /// </summary>
        /// <value>
        /// The name of the RGBAToy.
        /// </value>
        public string ToyName
        {
            get { return _ToyName; }
            set
            {
                if (_ToyName != value)
                {
                    _ToyName = value;
                    _RGBAToy = null;
                }
            }
        }


        private int _Layer=0;

        /// <summary>
        /// Gets or sets the number of the layer for the RGBA effect (Default=0).
        /// </summary>
        /// <value>
        /// The layer number.
        /// </value>
        public int Layer
        {
            get { return _Layer; }
            set { _Layer = value; }
        }
        


        private IRGBAToy _RGBAToy;

        /// <summary>
        /// Refrence to the RGBA Toy specified in the RGBAToyName property.<br/>
        /// If the RGBAToyName property is empty or contains a unknown name or the name of a toy which is not a IRGBAToy this property will return null.
        /// </summary>
        public IRGBAToy RGBAToy
        {
            get
            {
                return _RGBAToy;
            }
        }

        private void ResolveName(Table.Table Table)
        {

            if (!ToyName.IsNullOrWhiteSpace() && Table.Pinball.Cabinet.Toys.Contains(ToyName))
            {
                if (Table.Pinball.Cabinet.Toys[ToyName] is IRGBAToy)
                {
                    _RGBAToy = (IRGBAToy)Table.Pinball.Cabinet.Toys[ToyName];
                }

            }
        }

        public override void Init(Table.Table Table)
        {
            this.Table = Table;
            ResolveName(Table);
        }

        public override void Finish()
        {
            base.Finish();
            this.Table = null;
        }
    
    }
}
