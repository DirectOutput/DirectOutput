using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
