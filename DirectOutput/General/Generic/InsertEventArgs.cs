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
        public InsertEventArgs() { }
        public InsertEventArgs(int Index, Ty Item)
        {
            this.Index = Index;
            this.Item = Item;
        }
    }
}
