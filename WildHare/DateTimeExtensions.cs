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

        /// <summary>Returns a boolean true if the {datetime} is before DateTime.Today with time component set to 00.00.00 
        /// (the C# definition). When {includeAllOfToday} is true, any time in today is included.
        /// </summary>
        public static bool TodayOrBefore(this DateTime datetime, bool includeAllOfToday = false)
        {
            if (includeAllOfToday)
                return datetime.Date < DateTime.Today.AddDays(1);

            return datetime.Date <= DateTime.Today;
        }

        /// <summary>Given a DateTime {date}, returns the first instance of the day of the week. 
        /// If {includeCurrentDate} is true, then {date} is included. 
        /// ex. NextDayOfWeek for dayOfWeek "Wednesday" for {date} Sunday Aug 21, 2022 would be Aug 24, 2022.
        /// ex2. For the same {date}, for dayOfWeek "Sunday", and {includeCurrentDate} is true, 
        /// it the would return Aug 21, 2022, as it is a Sunday.</summary>
        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek dayOfWeek, bool includeCurrentDate = false)
        {
            if (includeCurrentDate && date.DayOfWeek == dayOfWeek)
                return date;

            return date.AddDays(7 - (int)date.DayOfWeek);
        }

        // Full name of the month for the given {date}
        public static string MonthName(this DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
        }

        // Full name of the month for the given int from 1 to 12. Throws an error if it is outside this range.
        public static string MonthName(this int monthInt)
        {
            // Throws and exception for any ints out of this range, including 13 (which returns an empty string in c#)
            if (monthInt < 1 || monthInt > 12)
                throw new ArgumentOutOfRangeException();
            
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthInt);
        }

        // Short name of the month for the given {date}
        public static string ShortMonthName(this DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(date.Month);
        }

        // Short name of the month for the given int from 1 to 12. Throws an error if it is outside this range.
        public static string ShortMonthName(this int monthInt)
        {
            // Throws and exception for any ints out of this range, including 13 (which returns an empty string in c#)
            if (monthInt < 1 || monthInt > 12)
                throw new ArgumentOutOfRangeException();

            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthInt);
        }

        // Returns a string with the four digit year and two digit month combined with no spaces.
        public static string YearMonth(this DateTime date)
        {
            return $"{date.Year}{date.Month:00}";
        }

        // Returns a string with the four digit year, two digit month, and two digit day combined with no spaces.
        public static string YearMonthDay(this DateTime date)
        {
            return $"{date.Year}{date.Month:00}{date.Day:00}";
        }

        /// <summary>Given a DateTime {date}, returns the starting day of the week. Typically {startOfWeek} 
        /// would be either a Sunday or Monday. This version is not culture aware.</summary>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;

            return dt.AddDays(-1 * diff).Date;
        }

        // SOURCE: https ://stackoverflow.com/questions/38039/how-can-i-get-the-datetime-for-the-start-of-the-week
    }
}
