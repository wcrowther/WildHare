using System;

namespace WildHare.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>Given a DateTime like {date}, get the next {DayOfWeek}, like Sunday, Monday, etc. as DateTime.</summary>
        public static DateTime GetNextDayOfWeek(DateTime date, DayOfWeek dayOfWeek)
        {
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

        /// <summary>Returns a boolean true if the {datetime} and {target} at in the same month.</summary>
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
    }
}
