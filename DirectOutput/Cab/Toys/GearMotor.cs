using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectOutput.Cab.Toys
{

    /// <summary>
    /// Gear motor toy.
    /// Inherits from GenericAnalogToy, implements IToy.
    /// </summary>
    public class GearMotor : GenericAnalogToy, IToy
    {

        /// <summary>
        /// Power of the gear motor
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
        /// Sets the power of the gear motor 
        /// </summary>
        /// <param name="Power">Power of the gear motor.</param>
        public void SetPower(int Power)
        {
            SetValue(Power);
        }


    }
}
