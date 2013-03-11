using System;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// Event args for Set events.
    /// </summary>
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
        public SetEventArgs() { }
        public SetEventArgs(int Index, Ty OldItem, Ty NewItem)
        {
            this.Index = Index;
            this.OldItem = OldItem;
            this.NewItem = NewItem;
        }
    }

}
