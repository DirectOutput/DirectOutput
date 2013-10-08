using System;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// Abstract base class for named items.<br/>
    /// Implements the name property and the necessary events.
    /// </summary>
    public abstract class NamedItemBase : INamedItem
    {
        #region Name
        private string _Name;
        /// <summary>
        /// Name of the named item.<br />
        /// Triggers BeforeNameChange before a new Name is set.<br />
        /// Triggers AfterNameChanged after a new name has been set.
        /// </summary>
        /// <value>
        /// The name of the item.
        /// </value>
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    string OldName = _Name;
                    if (BeforeNameChange != null)
                    {
                        BeforeNameChange(this, new NameChangeEventArgs(OldName, value));
                    }

                    _Name = value;

                    if (AfterNameChanged != null)
                    {
                        AfterNameChanged(this, new NameChangeEventArgs(OldName, value));
                    }
                }
            }
        }
        /// <summary>
        /// Event is fired after the value of the property Name has changed.
        /// </summary>
        public event EventHandler<NameChangeEventArgs> AfterNameChanged;

        /// <summary>
        /// Event is fired before the value of the property Name is changed.
        /// </summary>
        public event EventHandler<NameChangeEventArgs> BeforeNameChange;
        #endregion



    }
}
