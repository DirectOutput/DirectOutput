using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.Cab.Toys
{

    /// <summary>
    /// Base class for IToy implementations
    /// </summary>
    public abstract class ToyBase : NamedItemBase, IToy
    {
        /// <summary>
        /// Must initialize the IToy.
        /// Method must be overwritten.
        /// </summary>
        /// <param name="Cabinet">Cabinet to which the IToy belongs.</param>
        public abstract void Init(Cabinet Cabinet);


        /// <summary>
        /// Must reset the state of the IToy to its default state (off).
        /// Method must be overwritten.
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// Must finish the IToy, do all necessary clean up work and reset to IToy to its default state (off). 
        /// Method is virtual and can be overwritten.
        /// </summary>
        public virtual void Finish() { Reset(); }



    }
}
