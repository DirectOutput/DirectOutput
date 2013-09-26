using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Toys.Basic
{

    /// <summary>
    /// Lamp toy.<br/>
    /// Inherits from <see cref="AnalogToy"/>, implements <see cref="IToy"/>.
    /// </summary>
    public class Lamp : AnalogToy, IToy,ILampToy
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
           protected set
            {
                SetValue( value);
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
