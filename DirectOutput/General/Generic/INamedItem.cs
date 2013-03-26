using System;

namespace DirectOutput.General.Generic
{
    public interface INamedItem
    {
        string Name { get; set; }

        event EventHandler<NameChangeEventArgs> BeforeNameChange;
        event EventHandler<NameChangeEventArgs> AfterNameChanged;
    }

}
