using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

}

