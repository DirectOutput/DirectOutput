using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Extends the double type with additional functionality.
/// </summary>
public static class DoubleExtensions
{
    /// <summary>
    /// Limits the value to the supplied Min- and MaxValues
    /// </summary>
    /// <param name="MinValue">Minimum Value</param>
    /// <param name="MaxValue">Maximum Value</param>
    /// <returns>Double limited the to specified Min- and MaxValues</returns>
    public static double Limit(this double d, double MinValue, double MaxValue)
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
    public static bool IsBetween(this double i, double MinValue, double MaxValue)
    {
        return (i >= MinValue && i <= MaxValue);
    }

}

