using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{
    public class WS2811StripController : NamedItemBase, IOutputController
    {

        private byte[] LedData = new byte[0];


        private int _NumberOfLeds=0;

        /// <summary>
        /// Gets or sets the number of leds on the WS2811 based led strip.
        /// </summary>
        /// <value>
        /// The number of leds on the WS2811 based led strip.
        /// </value>
        public int NumberOfLeds
        {
            get { return _NumberOfLeds; }
            set { _NumberOfLeds = value; }
        }
        
        private void Setup() {

            LedData = new byte[NumberOfLeds];

            Outputs.Clear();
            for (int i = 0; i < NumberOfLeds; i++)
            {
                
            }



        }


        private object UpdateLocker = new object();
        private bool UpdateRequired = true;

        

        public void SetRGBValues(int FirstLedNumber, int[,] Data) {
            if (FirstLedNumber <= NumberOfLeds && Data.GetUpperBound(1) == 2)
            {
                int End = (FirstLedNumber + Data.GetUpperBound(0)+1).Limit(0, NumberOfLeds);
                int DataPointer = (FirstLedNumber * 3).Limit(0, NumberOfLeds * 3);
                lock (UpdateLocker)
                {
                    for (int i = FirstLedNumber; i < End; i++)
                    {
                        LedData[DataPointer] = (byte)Data[i, 1];
                        DataPointer++;
                        LedData[DataPointer] = (byte)Data[i, 0];
                        DataPointer++;
                        LedData[DataPointer] = (byte)Data[i, 2];
                    }
                    UpdateRequired = true;
                }
            }
        }




        #region IOutputController Member

        public void Init(Cabinet Cabinet)
        {
            throw new NotImplementedException();
        }

        public void Finish()
        {
            throw new NotImplementedException();
        }

        public OutputList Outputs
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
