using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class EffectNameAttribute : System.Attribute
    {
        public Type AllowedEffectType { get; private set; }

        public EffectNameAttribute(Type AllowedEffectType)
        {
            this.AllowedEffectType = AllowedEffectType;
        }
    }
}
