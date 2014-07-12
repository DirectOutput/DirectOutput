using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General.Analog
{
    /// <summary>
    /// Object containing a analog value (0-255) and a alpha value (0-255).
    /// </summary>
    public class AnalogAlpha
    {
        /// <summary>
        /// The analog value (0-255).
        /// </summary>
        public int Value;
        /// <summary>
        /// The alpha value (0-255).
        /// </summary>
        public int Alpha;


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of the AnalogAlphaValue instance.</returns>
        public AnalogAlpha Clone()
        {
            return new AnalogAlpha(Value, Alpha);
        }

        /// <summary>
        /// Sets the specified values.
        /// </summary>
        /// <param name="Value">The analog value.</param>
        /// <param name="Alpha">The alpha value.</param>
        public void Set(int Value, int Alpha)
        {
            this.Value = Value;
            this.Alpha = Alpha;
        }

        /// <summary>
        /// Sets the analog value.<br/>
        /// If Value is 0, the alpha value will be set to 0, otherwise it will be set to 255.
        /// </summary>
        /// <param name="Value">The analog value.</param>
        public void Set(int Value)
        {
            this.Value = Value;
            this.Alpha = (Value != 0 ? 255 : 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogAlpha"/> class.
        /// </summary>
        /// <param name="Value">The analog value.</param>
        /// <param name="Alpha">The alpha value.</param>
        public AnalogAlpha(int Value, int Alpha)
        {
            this.Value = Value;
            this.Alpha = Alpha;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogAlpha"/> class.
        /// If Value is 0, the alpha value will be set to 0, otherwise it will be set to 255.
        /// </summary>
        /// <param name="Value">The analog value.</param>
        public AnalogAlpha(int Value)
        {
            this.Value = Value;
            this.Alpha = (Value != 0 ? 255 : 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogAlpha"/> class.
        /// </summary>
        public AnalogAlpha() { }

    }
}