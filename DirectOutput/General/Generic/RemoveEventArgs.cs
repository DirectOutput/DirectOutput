using System;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// EventErgs for remove events
    /// </summary>
    public class RemoveEventArgs<Ty> : EventArgs
    {

        /// <summary>
        /// The zero-based index at which the item can be found.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///  The value of the Item to remove from index.
        /// </summary>  
        public Ty Item { get; set; }
        public RemoveEventArgs() { }
        public RemoveEventArgs(int Index, Ty Item)
        {
            this.Index = Index;
            this.Item = Item;
        }
    }
}
