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
}

