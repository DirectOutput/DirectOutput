using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

/// <summary>
/// Extends the Type object with additional functionality.
/// </summary>
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


    /// <summary>
    /// Determines whether the type is a generic list.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    ///   <c>true</c> if the type is a generic list; otherwise, <c>false</c>.
    /// </returns>
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


    /// <summary>
    /// Determines whether the type is a generic dictionary.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    ///   <c>true</c> if the type is a generic dictionary; otherwise, <c>false</c>.
    /// </returns>
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


    /// <summary>
    /// Gets the get generic collection type arguments.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>Array of the types of the generic collection.</returns>
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
