using System;
namespace DirectOutput.Cab.Toys.Basic
{
    /// <summary>
    /// Common interface for lamp like toys.<br/>
    /// Implement this interface if a toy resembles some kind of lamp so all effects working with lamps can use the toy.
    /// </summary>
    public interface ILampToy:IAnalogToy
    {
        /// <summary>
        /// Gets the brightness of the lamp.
        /// </summary>
        /// <value>
        /// The brightness of the lamp.
        /// </value>
        int Brightness { get;  }
        /// <summary>
        /// Sets the brightness of the lamp.
        /// </summary>
        /// <param name="Brightness">The brightness of the lamp.</param>
        void SetBrightness(int Brightness);
    }
}
