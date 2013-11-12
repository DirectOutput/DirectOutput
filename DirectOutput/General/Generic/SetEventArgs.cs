using System;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// Event args for Set events.
    /// </summary>
    /// <typeparam name="Ty">The type of the y.</typeparam>
    public class SetEventArgs<Ty> : EventArgs
    {

        /// <summary>
        /// The zero-based index at which OldItem can be found.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Item to be replaced.
        /// </summary>
        public Ty OldItem { get; set; }

        /// <summary>
        /// New item for the replacement.
        /// </summary>
        public Ty NewItem { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="SetEventArgs{Ty}"/> class.
        /// </summary>
        public SetEventArgs() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SetEventArgs{Ty}"/> class.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <param name="OldItem">The old item.</param>
        /// <param name="NewItem">The new item.</param>
        public SetEventArgs(int Index, Ty OldItem, Ty NewItem)
        {
            this.Index = Index;
            this.OldItem = OldItem;
            this.NewItem = NewItem;
        }
    }

}
