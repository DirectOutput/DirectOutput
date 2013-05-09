using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DirectOutput.Cab.Out
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
   public class OutputNameAttribute: System.Attribute
    {

        public OutputNameAttribute()
        {
            
        }
    }
}
