using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Extends the float type with additional functionality.
/// </summary>
public static class floatExtensions
{
    /// <summary>
    /// Limits the value to the supplied Min- and MaxValues
    /// </summary>
    /// <param name="MinValue">Minimum Value</param>
    /// <param name="MaxValue">Maximum Value</param>
    /// <returns>Float limited the to specified Min- and MaxValues</returns>
    public static float Limit(this float d, float MinValue, float MaxValue)
    {
        if (d < MinValue) return MinValue;
        if (d > MaxValue) return MaxValue;
        return d;
    }


    /// <summary>
    /// Indicates wheter the value of the double within a specified range.
    /// </summary>
    /// <param name="MinValue">Minimum Value</param>
    /// <param name="MaxValue">>Maximum Value</param>
    /// <returns>true if the double is between <paramref name="MinValue"/> and <paramref name="MaxValue"/>, otherwise false.</returns>
    public static bool IsBetween(this float i, float MinValue, float MaxValue)
    {
        return (i >= MinValue && i <= MaxValue);
    }

    /// <summary>
    /// Rounds the value to the next full number.
    /// </summary>
    /// <param name="d">The value.</param>
    /// <returns>Rounded value</returns>
    public static float Round(this float d)
    {
        return (float)Math.Round(d, 0);
    }

    /// <summary>
    /// Rounds the value to the specified number of digits.
    /// </summary>
    /// <param name="d">The value.</param>
    /// <param name="Digits">The number of digits.</param>
    /// <returns>Value round to the specified number of digits.</returns>
    public static float Round(this float d, int Digits)
    {
        return (float)Math.Round(d, Digits.Limit(0,15));
    }

    /// <summary>
    /// Rounds the value to the next full number.
    /// </summary>
    /// <param name="d">The value.</param>
    /// <returns>Rounded value</returns>
    public static int RoundToInt(this float d)
    {
        return (int)Math.Round(d, 0);
    }

    /// <summary>
    /// Returns the absolute value of the float.
    /// </summary>
    /// <param name="d">The value.</param>
    /// <returns>The absolute value of the float.</returns>
    public static float Abs(this float d)
    {
        return Math.Abs(d);
    }
}

