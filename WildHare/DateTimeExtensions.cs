using System;
using System.Globalization;

namespace WildHare.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>Given a DateTime like {date}, gets the next {DayOfWeek}, like Sunday, Saturday, etc. as DateTime.
        /// If {includeDate} is true and {date} is that day of the week, then return that {date}.</summary>
        /// <example>Given {date} is 1/1/2020 (Wednesday) then date.GetNextDayOfWeek(DayOfTheWeek.Wednesday)
        /// returns the next Wednesday which is 1/8/2020. If {includeDate} is true then 1/1/2020.</example>
        public static DateTime GetNextDayOfWeek(DateTime date, DayOfWeek dayOfWeek, bool includeDate = false)
        {
            if (includeDate && date.DayOfWeek == dayOfWeek)
                return date;

            int daysFromDate = ((int)dayOfWeek - (int)date.DayOfWeek + 7) % 7;

            return date.AddDays(daysFromDate);
        }

        /// <summary>Gets a DateTime for the first day of the month for the given {datetime}.</summary>
        public static DateTime FirstDayOfTheMonth(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, 1);
        }

        /// <summary>Gets a DateTime for the first day of the next month for the given {datetime}.</summary>
        public static DateTime FirstDayOfNextMonth(this DateTime datetime)
        {
            return datetime.LastDayOfTheMonth().AddDays(1);
        }

        /// <summary>Gets a DateTime for the last day of the next month for the given {datetime}.</summary>
        public static DateTime LastDayOfTheMonth(this DateTime datetime)
        {
            var daysInMonth = DateTime.DaysInMonth(datetime.Year, datetime.Month);
            return new DateTime(datetime.Year, datetime.Month, daysInMonth);
        }

        /// <summary>Returns a boolean true if the {datetime} and {target} are in the same month.</summary>
        public static bool IsInTheSameMonth(this DateTime datetime, DateTime target)
        {
            return datetime >= target.FirstDayOfTheMonth() && datetime < target.FirstDayOfNextMonth();
        }

        /// <summary>Returns a boolean true if the {datetime} is today or before.</summary>
        public static bool TodayOrBefore(this DateTime datetime)
        {
            return datetime.Date <= DateTime.Today;
        }

        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek dayOfWeek, bool includeCurrentDate = false)
        {
            if (includeCurrentDate && date.DayOfWeek == dayOfWeek)
                return date;

            return date.AddDays(7 - (int)date.DayOfWeek);
        }

        public static string MonthName(this DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
        }

        public static string MonthName(this int monthInt)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthInt);
        }

        public static string ShortMonthName(this DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(date.Month);
        }

        public static string ShortMonthName(this int monthInt)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthInt);
        }

        public static string YearMonth(this DateTime date)
        {
            return $"{date.Year}{date.Month:00}";
        }

        public static string YearMonthDay(this DateTime date)
        {
            return $"{date.Year}{date.Month:00}{date.Day:00}";
        }
    }
}
