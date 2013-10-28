using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys
{

    /// <summary>
    /// EventArgs for events of IToy objects
    /// </summary>
    // TODO: Check if this class is necessary (no references to the class yet).
    public class ToyEventArgs : EventArgs
    {

        /// <summary>
        /// IToy which has triggered the event 
        /// </summary>
        public IToy Toy { get; set; }

        /// <summary>
        /// Name of the IToy which has triggered the event 
        /// </summary>
        public string Name { get { return Toy.Name; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToyEventArgs"/> class.
        /// </summary>
        /// <param name="Toy">The toy.</param>
        public ToyEventArgs(IToy Toy)
        {
            this.Toy = Toy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToyEventArgs"/> class.
        /// </summary>
        public ToyEventArgs() { }
    }
}
