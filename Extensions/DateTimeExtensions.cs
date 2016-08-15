using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensions;


public static class DateTimeExtensions
{
    public static DateTime Limit(this DateTime DateToLimit, DateTime MinDate, DateTime MaxDate)
    {
        if (DateToLimit < MinDate) return MinDate;
        if (DateToLimit > MaxDate) return MaxDate;
        return DateToLimit;
    }

    public static DateTime? Limit(this DateTime? DateToLimit, DateTime MinDate, DateTime MaxDate)
    {
        if (!DateToLimit.HasValue) return null;
        if (DateToLimit < MinDate) return MinDate;
        if (DateToLimit > MaxDate) return MaxDate;
        return DateToLimit;
    }


    public static bool IsBetween(this DateTime DateToCheck, DateTime MinDate, DateTime MaxDate)
    {
        return DateToCheck >= MinDate && DateToCheck <= MaxDate;
    }


    public static bool IsBetween(this DateTime? DateToCheck, DateTime MinDate, DateTime MaxDate)
    {
        if (!DateToCheck.HasValue) return false;
        return DateToCheck.Value >= MinDate && DateToCheck.Value <= MaxDate;
    }

    public static DateTime FirstDayOfMonth(this DateTime DateTime)
    {
        return new DateTime(DateTime.Year, DateTime.Month, 1);

    }

    public static DateTime LastDayOfMonth(this DateTime DateTime)
    {
        return new DateTime(DateTime.Year, DateTime.Month, DateTime.DaysInMonth(DateTime.Year, DateTime.Month));

    }

    

}
