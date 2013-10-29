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
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveEventArgs{Ty}"/> class.
        /// </summary>
        public RemoveEventArgs() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveEventArgs{Ty}"/> class.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <param name="Item">The item.</param>
        public RemoveEventArgs(int Index, Ty Item)
        {
            this.Index = Index;
            this.Item = Item;
        }
    }
}
