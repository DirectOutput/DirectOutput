using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Out.DMX
{
    /// <summary>
    /// IOutput implementation for DMX.<br/>
    /// Inherits from Output and adds the \ref DmxChannel property.
    /// </summary>
    public class DMXOutput: Output, IOutput
    {
        private int _DmxChannel;

        /// <summary>
        /// Gets or sets the DMX channel.
        /// </summary>
        /// <value>
        /// The DMX channel.
        /// </value>
        /// <exception cref="System.Exception">Dmx channels numbers must be in the range of 1-512. The supplied number {0} is out of range..Build(value)</exception>
        public int DmxChannel
        {
            get
            {
                return _DmxChannel;
            }
            set
            {
                if (_DmxChannel != value)
                {
                    if (!value.IsBetween(1, 512))
                    {
                        throw new Exception("Dmx channels numbers must be in the range of 1-512. The supplied number {0} is out of range.".Build(value));
                    }
                    if (Name.IsNullOrWhiteSpace() || Name == "DmxChannel {0:000}".Build(_DmxChannel))
                    {
                        Name = "DmxChannel {0:00}".Build(value);
                    }
                    _DmxChannel = value;
                }
            }
        }
    }
}
