using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General.Generic
{
    public interface INamedItem
    {
        string Name { get; set; }

        event EventHandler<NameChangeEventArgs> BeforeNameChange;
        event EventHandler<NameChangeEventArgs> AfterNameChanged;
    }

}
