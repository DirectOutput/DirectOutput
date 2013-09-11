using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


public static class PropertyInfoExtension
{
    /// <summary>
    /// Determines whether the property is XML serializeable.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the property is XML serializeable; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsXMLSerializeable(this PropertyInfo PI)
    {

        return !PI.IsDefined(typeof(System.Xml.Serialization.XmlIgnoreAttribute), false) && PI.CanRead && PI.CanWrite && PI.GetGetMethod(false)!=null && PI.GetSetMethod(false) != null && !PI.GetGetMethod(false).IsStatic;
    }
}


