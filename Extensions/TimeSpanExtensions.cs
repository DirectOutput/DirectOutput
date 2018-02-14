using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Extends the TimeSPan object with additional functionality.
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>
    /// Returns a formated string of the TimeSpan objects value.
    /// </summary>
    /// <returns>String of formatted TimeSpan value.</returns>
    public static string Format(this TimeSpan TS)
    {
        if (TS.Ticks < 10)
        {
            //Nanoseconds
            return "{0}ns".Build(TS.Ticks * 100);
        }
        else if (TS.Ticks < 10000) //<1 millisecond
        {
            //Microseconds
            return "{0:0.0}µs".Build(TS.Ticks / 10);
        }
        else if (TS.TotalMilliseconds < 1000) //<1 second
        {
            return "{0:0.0}ms".Build(TS.TotalMilliseconds);
        }
        else if (TS.TotalSeconds < 60) //<1 minute
        {
            return "{0:0.000}s".Build(TS.TotalSeconds);
        }
        else if (TS.TotalMinutes < 60) //<1 hour
        {
            return "{0:#0}m {1:#0}s".Build(Math.Floor(TS.TotalMinutes), TS.Seconds);
        }
        else if (TS.TotalHours < 24)
        {
            return "{0:#0}h {1:#0}m {2:#0}s".Build(Math.Floor(TS.TotalHours), TS.Minutes, TS.Seconds);
        }
        return TS.ToString();
    }

    /// <summary>
    /// Indicates wheter the value of the TimeSpan within a specified range.
    /// </summary>
    /// <param name="MinValue">Minimum Value</param>
    /// <param name="MaxValue">>Maximum Value</param>
    /// <returns>true if the TimeSpan is between <paramref name="MinValue"/> and <paramref name="MaxValue"/>, otherwise false.</returns>
    public static bool IsBetween(this TimeSpan i, TimeSpan MinValue, TimeSpan MaxValue)
    {
        return (i >= MinValue && i <= MaxValue);
    }


    /// <summary>
    /// Limits the value to the supplied Min- and MaxValues
    /// </summary>
    /// <param name="MinValue">Minimum Value</param>
    /// <param name="MaxValue">Maximum Value</param>
    /// <returns>TimeSpan limited the to specified Min- and MaxValues</returns>
    public static TimeSpan Limit(this TimeSpan i, TimeSpan MinValue, TimeSpan MaxValue)
    {
        if (i < MinValue) return MinValue;
        if (i > MaxValue) return MaxValue;
        return i;
    }
}

