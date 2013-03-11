using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys
{

    /// <summary>
    /// EventArgs for events of IToy objects
    /// </summary>
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

        public ToyEventArgs(IToy Toy)
        {
            this.Toy = Toy;
        }

        public ToyEventArgs() { }
    }
}
