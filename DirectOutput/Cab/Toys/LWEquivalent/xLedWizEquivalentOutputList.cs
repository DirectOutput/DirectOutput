using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.LWEquivalent
{
    /// <summary>
    /// List of LedWizEquivalentOutput objects.
    /// </summary>
    public class xLedWizEquivalentOutputList:List<xLedWizEquivalentOutput>
    {

        /// <summary>
        /// Initializes all LedWizEquivalentOutput objects in the list.
        /// </summary>
        /// <param name="Cabinet">The cabinet to which the outputs belong.</param>
        public void Init(Cabinet Cabinet)
        {
            foreach (xLedWizEquivalentOutput O in this)
            {
                O.Init(Cabinet);
            }
        }

        /// <summary>
        /// Finishes all LedWizEquivalentOutput objects in the list.
        /// </summary>
        public void Finish()
        {
            foreach (xLedWizEquivalentOutput O in this)
            {
                O.Finish();
            }
        }

        /// <summary>
        /// Resets all LedWizEquivalentOutput objects in the list.
        /// </summary>
        public void Reset()
        {
            foreach (xLedWizEquivalentOutput O in this)
            {
                O.Reset();
            }
        }
    }
}
