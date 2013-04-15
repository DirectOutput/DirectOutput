using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.General.Generic;

namespace DirectOutput.Cab.Out
{

    /// <summary>
    /// Abstract OutputController base class.<br/>
    /// Implements IOutputController.
    /// </summary>
    /// <remarks>Remember that IOutputController doeas inherit INamedItem, so the members of that interface have to be implemented as well. The easiest way to achiev this is to inherit the NamedItemBase class.</remarks>
    public abstract class OutputControllerBase : NamedItemBase, IOutputController
    {
        
        /// <summary>
        /// OutputList for the IOutputcontroller object.
        /// </summary>
        public virtual OutputList Outputs { get; set; }
        
        /// <summary>
        /// Init must be overwritten and must initialize the ouput controller.<br/>
        /// This method is called after the objects haven been instanciated.
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// Finish must be overwritten and must finish the ouput controller.<br/>
        /// All necessary cleanup tasks have to be implemented here und all physical outputs have to be turned off.
        /// </summary>
        public abstract void Finish();

        /// <summary>
        /// Update must update the physical outputs to the values defined in the Outputs list. 
        /// </summary>
        public abstract void Update();

    }
}
