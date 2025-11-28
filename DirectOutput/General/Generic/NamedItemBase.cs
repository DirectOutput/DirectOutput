using System;
using System.ComponentModel;

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
                    OnBeforeNameChanged(OldName, value);
                    BeforeNameChange(OldName, value);
                    _Name = value;
                    AfterNameChange(OldName, value);
                    OnAfterNameChanged(OldName, value);
                }
            }
        }

        protected virtual void AfterNameChange(string OldName, string NewName)
        {

        }

        protected virtual void BeforeNameChange(string OldName, string NewName)
        {

        }

        private void OnAfterNameChanged(string OldName,String NewName)
        {
            if (AfterNameChanged != null)
            {
                AfterNameChanged(this, new NameChangeEventArgs(OldName, NewName));
            }
        }

        private void OnBeforeNameChanged(string OldName, String NewName)
        {
            if (BeforeNameChanged != null)
            {
                BeforeNameChanged(this, new NameChangeEventArgs(OldName, NewName));
            }
        }

        /// <summary>
        /// Event is fired after the value of the property Name has changed.
        /// </summary>
        public event EventHandler<NameChangeEventArgs> AfterNameChanged;

        /// <summary>
        /// Event is fired before the value of the property Name is changed.
        /// </summary>
        public event EventHandler<NameChangeEventArgs> BeforeNameChanged;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;


        protected virtual void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }

        }

    }
}
