using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Attributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=true)]
    public class ToyNameAttribute: System.Attribute
    {
        public Type AllowedToyType { get; private set; }

        public ToyNameAttribute(Type AllowedToyType)
        {
            this.AllowedToyType = AllowedToyType;
        }
    }
}
