using System;
namespace DirectOutput.Cab.Toys
{
    /// <summary>
    /// Common interface of analog toys supporting values from 0-255.<br/>
    /// The main purpose of this interface is to facilitate the development of effects. AssignedEffects using this interface for toy references are able to call the common methods of these toys.
    /// </summary>
    public interface IAnalogToy:IToy
    {

        /// <summary>
        /// Gets or sets the name of the see <cref="IAnalogToy"/>.
        /// </summary>
        /// <value>
        /// The name if the <see cref="IAnalogToy"/>.
        /// </value>
        new string Name { get; set; }
        /// <summary>
        /// Must initialize  the <see cref="IAnalogToy"/>.
        /// </summary>
        /// <param name="Cabinet">Cabinet to which the <see cref="IAnalogToy"/> belongs.</param>
        new void  Init(DirectOutput.Cab.Cabinet Cabinet);
        /// <summary>
        /// Gets or sets the name of the output of the <see cref="IAnalogToy"/>.
        /// </summary>
        /// <value>
        /// The name of the output of the <see cref="IAnalogToy"/>.
        /// </value>
        string OutputName { get; set; }
        /// <summary>
        /// Must reset the state of the <see cref="IAnalogToy"/> to its default state (off).
        /// </summary>
        new void Reset();
        /// <summary>
        /// Sets the value of the <see cref="IAnalogToy"/>.
        /// </summary>
        /// <param name="Value">The value of the of the <see cref="IAnalogToy"/>.</param>
        void SetValue(int Value);
        /// <summary>
        /// Gets or sets the value of the <see cref="IAnalogToy"/>.
        /// </summary>
        /// <value>
        /// The value of the <see cref="IAnalogToy"/>.
        /// </value>
        int Value { get; set; }
    }
}
