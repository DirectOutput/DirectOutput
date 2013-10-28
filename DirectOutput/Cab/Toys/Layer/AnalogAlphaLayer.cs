using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// This class implements a layer object with alpha support for toys working analog values. 
    /// </summary>
    public class AnalogAlphaLayer
    {

        /// <summary>
        /// The analog value of the layer.
        /// </summary>
        public int Value;
        /// <summary>
        /// The alpha value of the layer.
        /// </summary>
        public int Alpha;

        /// <summary>
        /// Sets the value and the alpha value of the layer.
        /// </summary>
        /// <param name="AnalogAlphaValue">The analog alpha value object containing the values to be set.</param>
        public void Set(AnalogAlphaValue AnalogAlphaValue)
        {
            this.Value = AnalogAlphaValue.Value;
            this.Alpha = AnalogAlphaValue.Alpha;
        }


        /// <summary>
        /// Sets the value of the layer to the specified value. The alpha value will be set to 0 if Value is 0, otherwise it will be sett to 255.
        /// </summary>
        /// <param name="Value">The value.</param>
        public void Set(int Value)
        {
            this.Value=Value;
            this.Alpha = (Value == 0 ? 0 : 255);
        }

        /// <summary>
        /// Sets the value and the alpha value of the layer.
        /// </summary>
        /// <param name="Value">The value for the layer.</param>
        /// <param name="Alpha">The alpha value for the layer.</param>
        public void Set(int Value, int Alpha)
        {
            this.Value=Value;
            this.Alpha=Alpha;
        }


        /// <summary>
        /// Gets a AnalogAlphaValue object representing the values for the layer.
        /// </summary>
        /// <returns></returns>
        public AnalogAlphaValue GetAnalogAlphaValue() {
            return new AnalogAlphaValue(Value, Alpha);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogAlphaLayer"/> class.
        /// </summary>
        public AnalogAlphaLayer() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogAlphaLayer"/> class.
        /// </summary>
        /// <param name="Value">The value for the layer. The alpha value will be set to 0 if Value is 0, otherwise it will be sett to 255.</param>
        public AnalogAlphaLayer(int Value) { Set(Value); }
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogAlphaLayer"/> class.
        /// </summary>
        /// <param name="Value">The value for the layer.</param>
        /// <param name="Alpha">The alpha value for the layer.</param>
        public AnalogAlphaLayer(int Value, int Alpha) { Set(Value, Alpha); }
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogAlphaLayer"/> class.
        /// </summary>
        /// <param name="AnalogAlphaValue">The AnalogAlphaValue object containing the values to be set for the layer.</param>
        public  AnalogAlphaLayer(AnalogAlphaValue AnalogAlphaValue) {Set(AnalogAlphaValue);}


    }


}
