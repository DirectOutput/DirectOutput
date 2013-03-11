using System;
using DirectOutput.General.Generic;
namespace DirectOutput.Cab.Toys
{

    /// <summary>
    /// Common interface for all <see cref="IToy"/>.<br/>
    /// The abstract class <see cref="Toy"/> implements this interface.
    /// </summary>
    public interface IToy:INamedItem
    {
        /// <summary>
        /// Gets or sets the Name of the <see cref="IToy"/>.
        /// </summary>
        /// <value>
        /// The name of the <see cref="IToy"/>.
        /// </value>
        new string Name { get; set; }


        /// <summary>
        /// Must initialize the <see cref="IToy"/>.
        /// </summary>
        /// <param name="Cabinet"> to which the <see cref="IToy"/> belongs.</param>
        void Init(DirectOutput.Cab.Cabinet Cabinet);




        /// <summary>
        /// Must reset the state of the <see cref="IToy"/> to its default state (off).
        /// </summary>
        void Reset();

        /// <summary>
        /// Must finish the <see cref="IToy"/>, do all necessary clean up work and reset the <see cref="IToy"/> to its default state (off). 
        /// </summary>
        void Finish();
    }
}
