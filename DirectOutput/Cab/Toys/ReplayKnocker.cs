using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys
{

    /// <summary>
    /// Replayknocker toy.
    /// Iherits GenericDigitalToy, implements IToy.
    /// </summary>
    public class ReplayKnocker : GenericDigitalToy, IToy
    {


        /// <summary>
        /// Fires the replay knocker once.
        /// </summary>
        public void Knock()
        {
            Knock(1);
        }


        /// <summary>
        /// Fires the replay knocker several times.
        /// </summary>
        /// <param name="NumberOfKnocks">Number of knocks</param>
        public void Knock(int NumberOfKnocks)
        {
            Knock(NumberOfKnocks, 50);
        }

        /// <summary>
        /// Fires the replay knocker several times.
        /// </summary>
        /// <param name="NumberOfKnocks">Number of knocks.</param>
        /// <param name="IntervallMs">Intervall in milliseconds between knocks.</param>
        public void Knock(int NumberOfKnocks, int IntervallMs)
        {
            throw new System.NotImplementedException();
        }
    }
}
