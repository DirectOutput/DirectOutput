using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.LWEquivalent
{
    /// <summary>
    /// List of LedWizEquivalentOutput objects.
    /// </summary>
    public class LedWizEquivalentOutputList:List<LedWizEquivalentOutput>
    {

        /// <summary>
        /// Initializes all LedWizEquivalentOutput objects in the list.
        /// </summary>
        /// <param name="Cabinet">The cabinet to which the outputs belong.</param>
        public void Init(Cabinet Cabinet)
        {
            foreach (LedWizEquivalentOutput O in this)
            {
                O.Init(Cabinet);
            }
        }

        /// <summary>
        /// Finishes all LedWizEquivalentOutput objects in the list.
        /// </summary>
        public void Finish()
        {
            foreach (LedWizEquivalentOutput O in this)
            {
                O.Finish();
            }
        }

        /// <summary>
        /// Resets all LedWizEquivalentOutput objects in the list.
        /// </summary>
        public void Reset()
        {
            foreach (LedWizEquivalentOutput O in this)
            {
                O.Reset();
            }
        }
    }
}
