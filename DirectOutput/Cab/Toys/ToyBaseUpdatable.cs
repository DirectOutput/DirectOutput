using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.Cab.Toys
{

    /// <summary>
    /// Base class for toys which need update calls to update the state of their assigned outputs.<br/>
    /// Toys which are directly updating their outputs when their state changes dont need to implement this method and should use the ToyBase class instead..
    /// </summary>
    public abstract class ToyBaseUpdatable : ToyBase, IToyUpdatable
    {

        #region IToyUpdatable Member

        /// <summary>
        /// Toys implementing this method, should use it to update their assosiated outputs.
        /// </summary>
        public abstract void UpdateOutputs();

        #endregion
    }
}
