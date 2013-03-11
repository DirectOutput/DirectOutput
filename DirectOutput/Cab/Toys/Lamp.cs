using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Toys
{

    /// <summary>
    /// Lamp toy.<br/>
    /// Inherits from <see cref="GenericAnalogToy"/>, implements <see cref="IToy"/>.
    /// </summary>
    public class Lamp : GenericAnalogToy, IToy
    {

        /// <summary>
        /// Brightsness of the lamp.
        /// </summary>
        [XmlIgnoreAttribute]
        public int Brightness
        {
            get
            {
                return Value;
            }
            set
            {
                this.Value = value;
            }
        }


        /// <summary>
        /// Sets the brightness of the lamp.
        /// </summary>
        /// <param name="Brightness">Brightness of the lamp.</param>
        public void SetBrightness(int Brightness)
        {
            SetValue(Brightness);
        }


    }
}
