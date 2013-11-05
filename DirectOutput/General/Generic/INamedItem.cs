using System;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// Interface for items which can be added the the NamedItemList.
    /// </summary>
    public interface INamedItem
    {
        /// <summary>
        /// Gets or sets the name of the item.<br/>
        /// Must fire the BeforeNameChange and AfterNameChange events when the value of the property is changed.
        /// </summary>
        /// <value>
        /// The name of the item.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Occurs before the name of the item changes.
        /// </summary>
        event EventHandler<NameChangeEventArgs> BeforeNameChange;
        /// <summary>
        /// Occurs when after the name of the item has changed.
        /// </summary>
        event EventHandler<NameChangeEventArgs> AfterNameChanged;
    }

}
