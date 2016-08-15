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

    /// <summary>
    /// Determines whether the type has the specified property.
    /// </summary>
    /// <param name="obj">The type.</param>
    /// <param name="PropertyName">Name of the property.</param>
    /// <returns>
    ///   <c>true</c> if the specified has the specified property; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasProperty(this Type obj, string PropertyName)
    {
        return obj.GetProperty(PropertyName) != null;
    }

    /// <summary>
    /// Determines whether the type is a builtin type (e.g. bool, int, string).
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    ///   <c>true</c> if it is a built in type; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBuiltinType(this Type type)
    {
        return type.Namespace == "System";
    }


    /// <summary>
    /// Gets the full name of the type.
    /// Does also output generic types in proper writing.
    /// </summary>
    /// <param name="t">The full name of the type</param>
    /// <returns></returns>
    public static string GetFullName(this Type t)
    {
        if (!t.IsGenericType)
            return t.Name;
        StringBuilder sb = new StringBuilder();

        sb.Append(t.Name.Substring(0, t.Name.LastIndexOf("`")));
        sb.Append(t.GetGenericArguments().Aggregate("<",

            delegate(string aggregate, Type type)
            {
                return aggregate + (aggregate == "<" ? "" : ",") + GetFullName(type);
            }
            ));
        sb.Append(">");

        return sb.ToString();
    }


    public static string GetDisplayName(this Type t)
    {
        if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            return string.Format("{0}?", GetDisplayName(t.GetGenericArguments()[0]));
        if (t.IsGenericType)
            return string.Format("{0}<{1}>",
                                 t.Name.Remove(t.Name.IndexOf('`')),
                                 string.Join(",", t.GetGenericArguments().Select(at => at.GetDisplayName())));
        if (t.IsArray)
            return string.Format("{0}[{1}]",
                                 GetDisplayName(t.GetElementType()),
                                 new string(',', t.GetArrayRank() - 1));
        return t.Name;
    }
}
