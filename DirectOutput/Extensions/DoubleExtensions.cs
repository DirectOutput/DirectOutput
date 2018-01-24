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
    /// <param name="d">The value to clamp</param>
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
    /// <param name="i">The value to test</param>
    /// <param name="MinValue">Minimum Value</param>
    /// <param name="MaxValue">>Maximum Value</param>
    /// <returns>true if the double is between <paramref name="MinValue"/> and <paramref name="MaxValue"/>, otherwise false.</returns>
    public static bool IsBetween(this double i, double MinValue, double MaxValue)
    {
        return (i >= MinValue && i <= MaxValue);
    }



    /// <summary>
    /// Rounds the value to the next full number.
    /// </summary>
    /// <param name="d">The value.</param>
    /// <returns>Rounded value</returns>
    public static double Round(this double d)
    {
        return Math.Round(d, 0);
    }

    /// <summary>
    /// Rounds the value to the next full number.
    /// </summary>
    /// <param name="d">The value.</param>
    /// <returns>Rounded value</returns>
    public static int RoundToInt(this double d)
    {
        return (int)Math.Round(d, 0);
    }

    /// <summary>
    /// Rounds the value to the specified number of digits.
    /// </summary>
    /// <param name="d">The value.</param>
    /// <param name="Digits">The number of digits.</param>
    /// <returns>Value round to the specified number of digits.</returns>
    public static double Round(this double d, int Digits)
    {
        return Math.Round(d, Digits.Limit(0, 15));
    }

    /// <summary>
    /// Returns the absolute value of the double.
    /// </summary>
    /// <param name="d">The value.</param>
    /// <returns>The absolute value of the double.</returns>
    public static double Abs(this double d)
    {
        return Math.Abs(d);
    }

    /// <summary>
    /// Returns the largest integer less than or equal to the specified floating-point number.
    /// </summary>
    /// <param name="d">The value.</param>
    /// <returns>The largest integer less than or equal to the float value.</returns>
    public static double Floor(this double d)
    {
        return Math.Floor(d);
    }


    /// <summary>
    /// Returns the smallest integral value that is greater than or equal to the specified floating-point number.
    /// </summary>
    /// <param name="d">The value.</param>
    /// <returns>The smallest integral value that is greater than or equal to a. If a is equal to NaN, NegativeInfinity, or PositiveInfinity, that value is returned.</returns>
    public static double Ceiling(this double d)
    {
        return Math.Ceiling(d);
    }

    /// <summary>
    /// Determines whether the specified double is integral.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns>
    ///   <c>true</c> if the double is integral; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsIntegral(this double d)
    {
        return d == Math.Floor(d);

    }

}

