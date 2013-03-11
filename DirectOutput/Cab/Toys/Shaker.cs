using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Toys
{
    /// <summary>
    /// Shaker toy.
    /// Inherits from GenericAnalogToy, implements IToy.
    /// </summary>
    public class Shaker : GenericAnalogToy, IToy
    {
        /// <summary>
        /// Power of the shaker.
        /// </summary>
        [XmlIgnoreAttribute]
        public int Power
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
        /// Sets the power of the shaker.
        /// </summary>
        /// <param name="Power">Power of the shaker.</param>
        public void SetPower(int Power)
        {
            SetValue(Power);
        }


    }
}
