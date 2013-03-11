using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys
{
    public class Flasher : GenericDigitalToy, IToy
    {
        public void Fire()
        {
            Fire(1);
        }

        public void Fire(int NumberOfFlashes)
        {
            Fire(NumberOfFlashes, 150);
        }

        public void Fire(int NumberOfFlashes, int IntervallMs)
        {
            throw new System.NotImplementedException();
        }
    }
}
