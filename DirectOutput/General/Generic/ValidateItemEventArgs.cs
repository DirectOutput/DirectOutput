using System;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// EventArgs for ValidateItem event.
    /// </summary>
    public class ValidateItemEventArgs<Ty> : EventArgs
    {

        /// <summary>
        /// Item to validate. 
        /// </summary>
        public Ty Item { get; set; }
        public ValidateItemEventArgs() { }
        public ValidateItemEventArgs(Ty Item)
        {
            this.Item = Item;
        }
    }

}
