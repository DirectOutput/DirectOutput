using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    public class LedStripLayer
    {


        private int _NumberOfLeds;

        public int NumberOfLeds
        {
            get { return _NumberOfLeds; }
            set
            {
                _NumberOfLeds = value;
                RGBALedData = new int[value,4];
            }
        }

        private int[,] _RGBALedData = new int[0,4];

        public int[,] RGBALedData
        {
            get { return _RGBALedData; }
            private set { _RGBALedData = value;  }
        }






    }
}
