using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Out
{
    public abstract class OutputControllerFlexCompleteBase:OutputControllerCompleteBase
    {

        private int _NumberOfOutputs = 1;

        public int NumberOfOutputs
        {
            get { return _NumberOfOutputs; }
            set
            {
                _NumberOfOutputs = value;
                base.SetupOutputs();
            }
        }


        protected override int GetNumberOfConfiguredOutputs()
        {
            return NumberOfOutputs;
        }
    }
}
