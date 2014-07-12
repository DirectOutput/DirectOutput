using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.Layer;
using System.Xml.Serialization;
using DirectOutput.Cab.Toys;
using DirectOutput.General.Color;

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
                    RGBAToy = null;
                    Layer = null;
                }
            }
        }


        [XmlIgnoreAttribute]
        protected RGBAColor Layer { get; private set; }



        private int _LayerNr=0;

        /// <summary>
        /// Gets or sets the number of the layer for the RGBA effect (Default=0).
        /// </summary>
        /// <value>
        /// The layer number.
        /// </value>
        public int LayerNr
        {
            get { return _LayerNr; }
            set { _LayerNr = value; }
        }
        


        private IRGBAToy _RGBAToy;

        /// <summary>
        /// Refrence to the RGBA Toy specified in the RGBAToyName property.<br/>
        /// If the RGBAToyName property is empty or contains a unknown name or the name of a toy which is not a IRGBAToy this property will return null.
        /// </summary>
        [XmlIgnoreAttribute]
        public IRGBAToy RGBAToy
        {
            get
            {
                return _RGBAToy;
            }
            private set
            {
                _RGBAToy = value;
            }
        }


        /// <summary>
        /// Initializes the RGBA effect.<br/>
        /// Resolves the name of the RGBA toy.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            this.Table = Table;
            if (!ToyName.IsNullOrWhiteSpace() && Table.Pinball.Cabinet.Toys.Contains(ToyName))
            {
                if (Table.Pinball.Cabinet.Toys[ToyName] is IRGBAToy)
                {
                    RGBAToy = (IRGBAToy)Table.Pinball.Cabinet.Toys[ToyName];
                    Layer = RGBAToy.Layers[LayerNr];
                }

            }
        }

        /// <summary>
        /// Finishes the RGBA effect.
        /// </summary>
        public override void Finish()
        {
            this.Layer = null;
            this.RGBAToy = null;
            this.Table = null;
            
            base.Finish();
        }
    
    }
}
