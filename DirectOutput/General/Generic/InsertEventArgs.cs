using System;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// Eventargs of BeforeInsert and AfterInsert events.
    /// </summary>
    public class InsertEventArgs<Ty> : EventArgs
    {

        /// <summary>
        /// The zero-based index at which to insert value.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The new value of the element at Index.
        /// </summary>
        public Ty Item { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertEventArgs{Ty}"/> class.
        /// </summary>
        public InsertEventArgs() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertEventArgs{Ty}"/> class.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <param name="Item">The item.</param>
        public InsertEventArgs(int Index, Ty Item)
        {
            this.Index = Index;
            this.Item = Item;
        }
    }
}
