using System;
namespace DirectOutput.Cab.Toys
{
    /// <summary>
    /// Generic interface for Toys having only 2 states.<br/>
    /// The main purpose of this interface is to facilitate the development of effects. AssignedEffects using this interface for toy references are able to call the common methods of these toys.
    /// </summary>
    public interface IDigitalToy : IToy
    {
        /// <summary>
        /// Gets or sets the name of the see <cref="IDigitalToy"/>.
        /// </summary>
        /// <value>
        /// The name if the <see cref="IDigitalToy"/>.
        /// </value>
        new string Name { get; set; }
        /// <summary>
        /// Must initialize the <see cref="IDigitalToy"/>.
        /// </summary>
        /// <param name="Pinball"><see cref="Pinball"/> object containing the <see cref="Cabinet"/> to which the <see cref="IDigitalToy"/> belongs.</param>
        new void Init(Pinball Pinball);

        /// <summary>
        /// Gets or sets the name of the outputof the <see cref="IDigitalToy"/>.
        /// </summary>
        /// <value>
        /// The name of the output of the <see cref="IDigitalToy"/>.
        /// </value>
        string OutputName { get; set; }
        /// <summary>
        /// Must reset the state of the IToy to its default state (off).
        /// </summary>
        new void Reset();
        /// <summary>
        /// Sets the state of the <see cref="IDigitalToy"/>.
        /// </summary>
        /// <param name="State">State of the <see cref="IDigitalToy"/>.</param>
        void SetState(bool State);
        /// <summary>
        /// Gets a value indicating the state of the <see cref="IDigitalToy"/> .
        /// </summary>
        /// <value>
        ///   <c>true</c> or <c>false</c>.
        /// </value>
        bool State { get; }
    }
}
