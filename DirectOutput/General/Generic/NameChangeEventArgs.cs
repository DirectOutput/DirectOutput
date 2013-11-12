using System;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// EventArgs for name change events.
    /// </summary>
    public class NameChangeEventArgs:EventArgs
    {
        /// <summary>
        /// Gets or sets the old name.
        /// </summary>
        /// <value>
        /// The old name.
        /// </value>
        public string OldName { get; set; }
        /// <summary>
        /// Gets or sets the new name.
        /// </summary>
        /// <value>
        /// The new name.
        /// </value>
        public string NewName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameChangeEventArgs"/> class.
        /// </summary>
        public NameChangeEventArgs() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NameChangeEventArgs"/> class.
        /// </summary>
        /// <param name="OldName">The old name.</param>
        /// <param name="NewName">The new name.</param>
        public NameChangeEventArgs(string OldName, string NewName)
        {
            this.OldName = OldName;
            this.NewName = NewName;
        }


    }
}
