using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Out
{
    /// <summary>
    /// This interface defines additional methods for output controllers which allow for direct modification of the outputs.
    /// </summary>
    public interface ISupportsSetValues:IOutputController
    {
        /// <summary>
        /// Sets the values for one or several outputs of the controller.
        /// </summary>
        /// <param name="FirstOutput">The first output to be updated with a new value (zero based).</param>
        /// <param name="Values">The values to be used.</param>
        void SetValues(int FirstOutput, byte[] Values);


    }
}
