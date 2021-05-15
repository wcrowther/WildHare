using System;
using System.Linq;

namespace WildHare.Extensions
{
    public static class ConvertExtensions
    {
        /// <summary>Converts string to bool. (case insensitive, defaults to false if string does not parse to true)</summary>
        /// <returns>An bool value</returns>
        public static bool ToBool(this string value)
		{
            if (value is null)
                return false;

            bool.TryParse(value.ToLower(), out bool result);

			return result;
		}

        /// <summary>Converts string to bool by comparing to the trueValue. Ignores case by default.</summary>
        /// <example>"yes".ToBool("yes") return true.</example>
        /// <example>"Yes".ToBool("yes", false) return false.</example>
        /// <returns>An bool value</returns>
        public static bool ToBool(this string value, string trueValue, bool ignoreCase = true)
        {
            if (value is null || trueValue is null)
                return false;

            return (ignoreCase ? value.ToLower() : value) == (ignoreCase ? trueValue.ToLower() : trueValue);
        }

        /// <summary>Converts string to bool. Can be null. (case insensitive)</summary>
        /// <returns>An bool value</returns>
        /// <documentation>Test1</documentation>
        public static bool? ToBoolNullable(this string value, bool? defaultValue = default)
		{
            if (value is null)
                return defaultValue;

            return bool.TryParse(value.ToLower(), out bool result) ? result : defaultValue;
		}

        /// <summary>Converts strings to Int.</summary>
        /// <returns>An int value</returns>
        public static int ToInt(this string value, int defaultValue = 0)
        {
            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        /// <summary>Converts strings to Int if possible</summary>
        /// <returns>An int value or null</returns>
        public static int? ToIntNullable(this string value, int? defaultValue = null)
        {
			return int.TryParse(value, out int result) ? result : defaultValue;
		}

        /// <summary>Converts strings to long</summary>
        /// <returns>A long value</returns>
        public static long ToLong(this string value, long defaultValue = 0)
        {
            return long.TryParse(value, out long result) ? result : defaultValue;
        }

        /// <summary>Converts strings to long</summary>
        /// <returns>A long value or null</returns>
        public static long? ToLongNullable(this string value, long? defaultValue = null)
        {
            return long.TryParse(value, out long result) ? result : defaultValue;
        }

        /// <summary>Converts strings to double.</summary>
        /// <returns>An double value</returns>
        public static double ToDouble(this string value, double defaultValue = 0D)
        {
            return double.TryParse(value, out double result ) ? result : defaultValue;
        }

        /// <summary>Converts strings to double if possible</summary>
        /// <returns>An double value or null</returns>
        public static double? ToDoubleNullable(this string value, double? defaultValue = null)
        {
			return double.TryParse(value, out double result) ? result : defaultValue;
		}

        /// <summary>Converts strings to Decimal without without having to use an explicit try/catch</summary>
        /// <returns>A Decimal value</returns>
        public static decimal ToDecimal(this string value, decimal defaultValue = 0.0M)
        {
            return decimal.TryParse(value, out decimal result) ? result : defaultValue;
        }

        /// <summary>Converts strings to Decimal if possible</summary>
        /// <returns>An Decimal value or null</returns>
        public static decimal? ToDecimalNullable(this string value, decimal? defaultValue = null)
        {
            return decimal.TryParse(value, out decimal result) ? result : defaultValue;
        }

        /// <summary>Converts strings to DateTime with default if unsuccessful</summary>
        /// <returns>A DateTime value</returns>
        public static DateTime ToDateTime(this string value, DateTime defaultValue)
		{
			return DateTime.TryParse(value, out DateTime result) ? result : defaultValue;
		}

        /// <summary>Converts strings to DateTime with default value of nullable (datetime or null) 
        /// if unsuccessful</summary>
        /// <returns>A nullable DateTime value</returns>
        public static DateTime? ToDateTime(this string value, DateTime? defaultValue = null)
		{
            return DateTime.TryParse(value, out DateTime result) ? result : defaultValue;
        }

        /// <summary>Converts a string to an array of ints. With {strict} equals false, the default, 
        /// the method will ignore any characters except for numbers, the negative symbol, or commas.
        /// With {strict} equals true, empty entries, alphabetic characters, etc. will cause exceptions.</summary>
        public static int[] ToIntArray(this string str, bool strict = false)
        {
            string intStr = strict ? str : str.NumbersOnly(",-");
            var options = strict ? StringSplitOptions.None : StringSplitOptions.RemoveEmptyEntries;
            var intArray = intStr.Split(",", options).Select(s => s.Trim().ToIntNullable()).ToArray();

            if (strict && intArray.Any(a => !a.HasValue))
                throw new Exception("ToIntArray() cannot have null or invalid values when in strict mode.");

            return intArray.Select(w => w.Value).ToArray();
        }

        /// <summary>Converts an array of ints to a string. The method will return null if the {intArray} parameter is null 
        /// when {strict} is false. When {strict} is true the methods will throw an exception.</summary>
        public static string AsString(this int[] intArray, bool strict = false)
        {
            if(intArray is null && strict)
                throw new Exception("IntArray.AsString() cannot be null when in strict mode.");

            if (intArray is null)
                return null;

            return string.Join(",", intArray);
        }
    }
}
