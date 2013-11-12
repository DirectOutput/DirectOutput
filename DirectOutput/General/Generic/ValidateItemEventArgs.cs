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
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateItemEventArgs{Ty}"/> class.
        /// </summary>
        public ValidateItemEventArgs() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateItemEventArgs{Ty}"/> class.
        /// </summary>
        /// <param name="Item">The item.</param>
        public ValidateItemEventArgs(Ty Item)
        {
            this.Item = Item;
        }
    }

}
