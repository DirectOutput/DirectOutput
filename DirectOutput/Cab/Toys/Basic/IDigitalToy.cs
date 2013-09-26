using System;
namespace DirectOutput.Cab.Toys.Basic
{
    /// <summary>
    /// Generic interface for Toys having only 2 states.<br/>
    /// The main purpose of this interface is to facilitate the development of effects. AssignedEffects using this interface for toy references are able to call the common methods of these toys.
    /// </summary>
    public interface IDigitalToy : IToy
    {

        /// <summary>
        /// Gets or sets the name of the outputof the <see cref="IDigitalToy"/>.
        /// </summary>
        /// <value>
        /// The name of the output of the <see cref="IDigitalToy"/>.
        /// </value>
        string OutputName { get; set; }

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
