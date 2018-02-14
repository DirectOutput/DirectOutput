using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Extends the byte type with additional functionality.
/// </summary>
public static class byteExtensions
{
    /// <summary>
    /// Limits the value to the supplied Min- and MaxValues
    /// </summary>
    /// <param name="MinValue">Minimum Value</param>
    /// <param name="MaxValue">Maximum Value</param>
    /// <returns>Double limited the to specified Min- and MaxValues</returns>
    public static byte Limit(this byte d, byte MinValue, byte MaxValue)
    {
        if (d < MinValue) return MinValue;
        if (d > MaxValue) return MaxValue;
        return d;
    }

    /// <summary>
    /// Indicates whether the value of the byte within a specified range.
    /// </summary>
    /// <param name="MinValue">Minimum Value</param>
    /// <param name="MaxValue">>Maximum Value</param>
    /// <returns>true if the byte is between <paramref name="MinValue"/> and <paramref name="MaxValue"/>, otherwise false.</returns>
    public static bool IsBetween(this byte i, byte MinValue, byte MaxValue)
    {
        return (i >= MinValue && i <= MaxValue);
    }


    /// <summary>
    /// Determines whether the specified bit is set.
    /// </summary>
    /// <param name="b">The byte to check.</param>
    /// <param name="BitNr">The number of the bit to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified bit is set; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBitSet(this byte b, int BitNr)
    {
        return (b & (1 << BitNr)) != 0;
    }

    /// <summary>
    /// Inverts all bits of the byte.
    /// </summary>
    /// <param name="d">The byte.</param>
    /// <returns>Inverted byte.</returns>
    public static byte Invert(this byte d)
    {
        return (byte)~d;
    }
}

