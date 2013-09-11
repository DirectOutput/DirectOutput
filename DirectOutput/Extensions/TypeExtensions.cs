using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

public static class TypeExtensions
{
    /// <summary>
    /// Determines whether the type is a number.
    /// </summary>
    /// <param name="t">The type.</param>
    /// <returns>
    ///   <c>true</c> if the type is a number; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNumber(this Type t)
    {
        return t == typeof(sbyte)
        || t == typeof(byte)
        || t == typeof(short)
        || t == typeof(ushort)
        || t == typeof(int)
        || t == typeof(uint)
        || t == typeof(long)
        || t == typeof(ulong)
        || t == typeof(float)
        || t == typeof(double)
        || t == typeof(decimal);
    }

    /// <summary>
    /// Gets the XML serializable properties.
    /// </summary>
    /// <returns>List of XML serializable properties.</returns>
    public static List<PropertyInfo> GetXMLSerializableProperties(this Type t)
    {
        return t.GetProperties().Where(PI => PI.IsXMLSerializeable()).ToList();
    }


    public static bool IsGenericList(this Type type)
    {
        if (type == null) return false;

        foreach (Type @interface in type.GetInterfaces())
        {
            if (@interface.IsGenericType)
            {
                if (@interface.GetGenericTypeDefinition() == typeof(ICollection<>))
                {

                    return true;
                }
            }
        }
        return false;
    }


    public static bool IsGenericDictionary(this Type type)
    {
        if (type == null) return false;

        foreach (Type @interface in type.GetInterfaces())
        {
            if (@interface.IsGenericType)
            {
                if (@interface.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {

                    return true;
                }
            }
        }
        return false;
    }


    public static Type[] GetGetGenericCollectionTypeArguments(this Type type)
    {
        if (type == null) return null;
        if (!type.IsGenericList() && !type.IsGenericDictionary()) return null;

        foreach (Type @interface in type.GetInterfaces())
        {
            if (@interface.IsGenericType)
            {
                if (@interface.GetGenericTypeDefinition() == typeof(ICollection<>) | @interface.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {

                    return @interface.GetGenericArguments();
                }
            }
        }
        return null;
    }


}
