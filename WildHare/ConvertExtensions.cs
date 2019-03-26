using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WildHare.Extensions;

namespace WildHare
{
    public static class ConvertExtensions
    {
        /// <summary>Converts strings to Int.</summary>
        /// <returns>An int value</returns>
        public static int ToInt(this string value, int defaultValue = 0)
        {
            int result = defaultValue;
            int.TryParse(value, out result);

            return result;
        }

        /// <summary>Converts strings to Int if possible</summary>
        /// <returns>An int value or null</returns>
        public static int? ToIntNullable(this string value, int? defaultValue = null)
        {
            int result;
            return (int.TryParse(value, out result)) ? result : defaultValue;
        }

        /// <summary>Converts strings to long</summary>
        /// <returns>A long value</returns>
        public static long ToLong(this string value, Int64 defaultValue = 0)
        {
            long result = defaultValue;
            long.TryParse(value, out result);

            return result;
        }

        /// <summary>Converts strings to double.</summary>
        /// <returns>An double value</returns>
        public static double ToDouble(this string value, double defaultValue = 0D)
        {
            double result = defaultValue;
            double.TryParse(value, out result);
            return result;
        }

        /// <summary>Converts strings to double if possible</summary>
        /// <returns>An double value or null</returns>
        public static double? ToDoubleNullable(this string value, double? defaultValue = null)
        {
            double result;
            return (double.TryParse(value, out result)) ? (double?) result : defaultValue;
        }

		/// <summary>Converts strings to Decimal without without having to use an explicit try/catch</summary>
        /// <returns>A Decimal value</returns>
        public static decimal ToDecimal(this string value, decimal defaultValue = 0.0M)
        {
            decimal result = defaultValue;
            decimal.TryParse(value, out result);

            return result;
        }

        /// <summary>Converts strings to Decimal if possible</summary>
        /// <returns>An Decimal value or null</returns>
        public static Decimal? ToDecimalNullable(this string value, decimal? defaultValue = 0.0M)
        {
            return (Decimal.TryParse(value, out decimal result)) ? (Decimal?)result : defaultValue;
        }

		/// <summary>Converts strings to DateTime with default if unsuccessful</summary>
		/// <returns>A DateTime value</returns>
		public static DateTime ToDateTime(this string value, DateTime defaultValue)
		{
			DateTime result = defaultValue;
			DateTime.TryParse(value, out result);

			return result;
		}

		/// <summary>Converts strings to DateTime with default value of nullable (datetime or null) 
        /// if unsuccessful</summary>
		/// <returns>A nullable DateTime value</returns>
		public static DateTime? ToDateTime(this string value, DateTime? defaultValue = null)
		{
            return (DateTime.TryParse(value, out DateTime result)) ? result : defaultValue;
        }

        /// <summary>Increments integer +1 on the end of a string</summary>
        /// <example>'File.txt'.IncrementString("txt") = 'File1.txt'</example>
        /// <example>'File6.txt'.IncrementString(1,"txt") = 'File1.txt'</example>
        /// <returns>A string with end int incremented +1</returns>
        public static string IncrementString(this string str, int? seedIfEmpty = 1, string ignoreEnd = "")
        {
            str = str.RemoveEnd(ignoreEnd);
            int? currentnumber = seedIfEmpty;
            string numberstring = "";

            numberstring += Regex.Match(str, @"[0-9,-]*$");
            if (numberstring.ToIntNullable() != null)
            {
                str = str.Remove(str.Length - numberstring.Length);
                currentnumber = numberstring.ToInt() + 1;
            }
            return str + (currentnumber.HasValue ? currentnumber.Value.ToString() : "") + ignoreEnd;
        }
    }
}
