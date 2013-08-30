using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Out
{
    /// <summary>
    /// Basic IOutputNumbered implementation.
    /// </summary>
    public class OutputNumbered: Output, IOutputNumbered
    {
        #region IOutputNumbered Member

        /// <summary>
        /// Gets or sets the number of the OutputNumbered object.
        /// </summary>
        /// <value>
        /// The number of the OutputNumbered object.
        /// </value>
        public int Number{get;set;}

        #endregion
    }
}
