using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Environment;

namespace WildHare.Extensions
{
    public static class StringExtensions
    {
        /// <summary>Inline version of string.IsNullOrEmpty()</summary>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>A null string returns {replacement} if given, else an empty string.</summary>
        public static string IfNull(this string s, string replacement = "")
        {
            return s ?? replacement;
        }

        /// <summary>A null or empty string returns {replacement} if given, else an empty string.</summary>
        public static string IfNullOrEmpty(this string s, string replacement = "")
        {
            return s.IsNullOrEmpty() ? replacement : s;
        }

        /// <summary>Inline version of string.IsNullOrWhiteSpace()</summary>
        public static bool IsNullOrSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>A Null or whitespace string returns {replacement} if given, else an empty string.</summary>
        public static string IfNullOrSpace(this string s, string replacement = "")
        {
            return s.IsNullOrSpace() ? replacement : s;
        }

        /// <summary>Remove the start of a string if it exactly matches {start}.</summary>
        public static string RemoveStart(this string input, string start)
        {
            string s = input.IfNullOrEmpty();
            string t = start.IfNullOrEmpty();

            return s.StartsWith(t) ? s.Remove(0, t.Length) : s; 
        }

        /// <summary>Remove the end of a string if it exactly matches {end}.</summary>
        public static string RemoveEnd(this string input, string end)
        {
            string s = input.IfNullOrEmpty();
            string e = end.IfNullOrEmpty();

            return s.EndsWith(e) ? s.Remove(s.Length - e.Length, e.Length) : s;
        }

        /// <summary>Remove the start of a string if it matches {start} and end of a string if it matches {end}.
        /// If {end} is not specified use {start} for both values.</summary>
        public static string RemoveStartEnd(this string input, string start, string end =  null)
        {
            return input.RemoveStart(start).RemoveEnd(end ?? start);
        }

        /// <summary>Removes the start of a string if it exactly matches any of the strings in the {startArray}.</summary>
        public static string RemoveStart(this string input, string[] startArray)
        {
            string s = input.IfNullOrEmpty();
            foreach (string start in startArray)
            {
                s = s.RemoveStart(start);
            }
            return s;
        }

        /// <summary>Removes the end of a string if it exactly matches any of the strings in the {endArray}.</summary>
        public static string RemoveEnd(this string input, string[] endArray)
        {
            string s = input.IfNullOrEmpty();
            foreach (string end in endArray)
            {
                s = s.RemoveEnd(end);
            }
            return s;
        }

        /// <summary>Removes the start of a string if it exactly matches any of the strings in the {startArray} 
        /// and removse the end of a string if it exactly matches any of the strings in the {endArray}.</summary>
        public static string RemoveStartEnd(this string input, string[] startArray, string[] endArray = null)
        {
            string s = input.IfNullOrEmpty();
            foreach (string start in startArray)
            {
                s = s.RemoveStart(start);
            }

            if (endArray == null)
                endArray = startArray;

            foreach (string end in endArray)
            {
                s = s.RemoveEnd(end);
            }
            return s;
        }

        /// <summary>Remove the start of line if it exactly matches {start} for all lines in the string. 
        /// (This can be useful for programmatically removing indents from a long string.)</summary>
        public static string RemoveStartFromAllLines(this string input, string start)
        {
            return string.Join(NewLine, input.Split(NewLine).Select(a => a.RemoveStart(start)));
        }

        /// <summary>Remove the start of line if it exactly matches any string in the {startArray} for all lines in the string. 
        /// (This can be useful for programmatically removing indents from a long string.)</summary>
        public static string RemoveStartFromAllLines(this string input, string[] startArray)
        {
            return string.Join("\n", input.Split('\n').Select(a => a.RemoveStart(startArray)));
        }

        /// <summary>Remove the end of line if it exactly matches {end} for all lines in the string.</summary>
        public static string RemoveEndFromAllLines(this string input, string end)
        {
            return string.Join("\n", input.Split('\n').Select(a => a.RemoveEnd(end)));
        }

        /// <summary>Remove the end of line if it exactly matches any string in the {endArray} for all lines in the string.</summary>
        public static string RemoveEndFromAllLines(this string input, string[] endArray)
        {
            return string.Join("\n", input.Split('\n').Select(a => a.RemoveEnd(endArray)));
        }

        /// <summary>Adds {addToStart} to the beginning of the string if it is not NULL or EMPTY.</summary>
        public static string AddStart(this string s, string addToStart)
        {
            string str = s ?? "";
            return (str.Trim().Length > 0) ? (addToStart + str) : s;
        }

        /// <summary>Adds {addToEnd} to the end of the string if it is not NULL or EMPTY.</summary>
        public static string AddEnd(this string s, string addToEnd)
        {
            string str = s ?? "";
            return (str.Trim().Length > 0) ? (str + addToEnd) : s;
        }

        /// <summary>Adds {addToEnd} to the beginning of the string if it does not start with that string AND
        /// adds {addToEnd} to the end of the string if it is not NULL or EMPTY.</summary>
        public static string AddStartEnd(this string s, string addToStart, string addToEnd = null)
        {
            string str = s ?? "";
            return (str.Trim().Length > 0) ? (addToStart + str + (addToEnd ?? addToStart) ) : s;
        }

        /// <summary>Adds {addToStart} to the beginning of the string UNLESS it already starts with that string.</summary>
        public static string EnsureStart(this string s, string addToStart)
        {
            if (s == null) return null;

            return s.StartsWith(addToStart) ? s : addToStart + s;
        }

        /// <summary>Adds {addToEnd} to the end of the string UNLESS it already starts with that string.</summary>
        public static string EnsureEnd(this string s, string addToEnd)
        {
            if (s == null) return null;

            return s.EndsWith(addToEnd) ? s : s + addToEnd;
        }

        /// <summary>Adds {addToStart} to the beginning of the string if it does not start with that string AND
        /// adds {addToEnd} to the end of the string if it does not end with that string. If {addToEnd}
        /// is null, adds {addToStart} to both the start and end.</summary>
        public static string EnsureStartEnd(this string s, string addToStart, string addToEnd = null)
        {
            if (s == null) return null;

            addToEnd = addToEnd ?? addToStart;
            string str = s.StartsWith(addToStart) ? s : addToStart + s;

            return s.EndsWith(addToEnd) ? str : str + addToEnd;
        }

        /// <summary>Splits string into an array based on {separator} and returns the start element.
        /// Includes the separator if {includeSeparator} is true and it is contained in the string.</summary>
        public static string GetStart(this string s, string separator, bool includeSeparator = false)
        {
            if (s == null || s.IndexOf(separator) == -1)
                return null;

            var sepArray = new string[1] { separator }; // .Split requires string array
            var array = s.Split(sepArray, StringSplitOptions.None);

            return includeSeparator && s.Contains(separator) ? array[0] + separator : array[0];
        }

        /// <summary>Splits string into an array based on {separator} and returns the end element.
        /// Includes the separator if {includeSeparator} is true and it is contained in the string.</summary>
        public static string GetEnd(this string s, string separator, bool includeSeparator = false)
        {
            if (s == null || s.IndexOf(separator) == -1)
                return null;

            var sepArray = new string[1] { separator }; // .Split requires string array
            var array = s.Split(sepArray, StringSplitOptions.None);
            int last = array.Length - 1;

            return includeSeparator && s.Contains(separator) ? separator + array[last] : array[last];
        }

        /// <summary>Returns a string with only numbers and any additional characters in {otherCharacters}.</summary>
        public static string NumbersOnly(this string input, string otherCharacters = "")
        {
            var output = new StringBuilder("");
            for (int i = 0; i < input.IfNullOrEmpty().Length; i++)
            {
                if (("0123456789" + otherCharacters).IndexOf(input[i]) != -1)
                {
                    output.Append(input[i]);
                }
            }
            return output.ToString();
        }

        /// <summary>Truncates a string down if it is over {maxcharacters}. If truncated it adds {more} parameter
        /// to the end with '...' as the default. It will attempt to make the truncation 
        /// at a space or line break, but will search {wordcut} characters before forcing the wordcut.</summary>
        public static string Truncate(this string input, int maxCharacters, string more = "...", int wordcut = 8)
        {
            int strEnd;
            if (input.Length > maxCharacters)
            {
                strEnd = input.Trim().LastIndexOfAny(new char[] { ' ', '\n', '\r' }, maxCharacters);

                if (strEnd == -1)  // if no space or line found
                    return input.Substring(0, maxCharacters) + more;

                var charsFromEndOfLastWord = maxCharacters - strEnd;

                if (charsFromEndOfLastWord > wordcut)
                    strEnd = maxCharacters;

                input = input.Substring(0, strEnd).Trim() + more;
            }
            return input;
        }

		/// <summary>Capitalizes the first letter and all first letters after whitespace in a string.
		/// If {underscoreCountsAsSpace} is true then also capitalizes the first letter after an underscore '_'.
		/// </summary>
		public static string ProperCase(this string input, bool underscoreCountsAsSpace = false)
        {
            var sb = new StringBuilder();
            bool emptyBefore = true;
            foreach (char ch in input)
            {
                char chThis = ch;
                if (char.IsWhiteSpace(chThis))
				{
                    emptyBefore = true;
				}
				else if (underscoreCountsAsSpace && chThis == '_')
				{
					emptyBefore = true;
				}
				else
                {
					if (char.IsLetter(chThis) && emptyBefore)
					{
						chThis = char.ToUpper(chThis);
					}
					else
					{
						chThis = char.ToLower(chThis);
					}
                    emptyBefore = false;
                }
                sb.Append(chThis);
            }
            return sb.ToString();
        }

        /// <summary>Returns the character at the position {i}.</summary>
        public static char CharAt(this string s, int i)
        {
            return Convert.ToChar(s.Substring(i, 1));
        }

		/// <summary>Removes the indent from a block of text. Defaults to am empty space (' ') indent.</summary>
		public static string RemoveLineIndents(this string output, int number, string indent = " ")
		{
			string trimAllLines = string.Join( "\n", output.Split('\n').Select( a => a.RemoveStart(indent.Repeat(number))) );

			return trimAllLines;
		}

		/// <summary>Increments integer +1 on the end of a string</summary>
		/// <example>'File.txt'.IncrementString(".txt") = 'File1.txt'</example>
		/// <example>'File6.txt'.IncrementString(1,".txt") = 'File7.txt'</example>
		/// <returns>A string with end int incremented +1</returns>
		public static string IncrementString(this string str, int? seedIfEmpty = 1, string ignoreExtension = "", int increment = 1)
		{
			str = str.RemoveEnd(ignoreExtension);
			int? currentnumber = seedIfEmpty;
			string numberstring = "";

			numberstring += Regex.Match(str, @"[0-9,-]*$");
			if (numberstring.ToIntNullable() != null)
			{
				str = str.Remove(str.Length - numberstring.Length);
				currentnumber = numberstring.ToInt() + increment;
			}
			return str + (currentnumber.HasValue ? currentnumber.Value.ToString() : "") + ignoreExtension;
		}

        /// <summary>Returns a string {str} x {number} of times</summary>
        public static string Repeat(this string str, int number)
        {
            if (str == null || number < 1)
                return null;

            return new StringBuilder(str.Length * number).Insert(0, str, number).ToString();
        }

        /// <summary>An overload of StartsWith that accepts a string array.
        /// Will return true if any of the values in the {valuesArray} is true.</summary>
        public static bool StartsWith(this string str, string[] valuesArray, bool ignoreCase = false, CultureInfo culture = null)
        {
            foreach (string value in valuesArray)
            {
                if (str.StartsWith(value, ignoreCase, culture ?? CultureInfo.CurrentCulture))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>An overload of Replace that accepts a string array.
        /// For the supplied string, replaces all values in the {oldValues} array with the {newValue} string.</summary>
        /// <example>Shortcut for y.Replace("cat", "").Replace("dog", "") etc...</example>.
        public static string Replace(this string str, string[] oldValues, string newValue)
        {
            string result = str;

            foreach (string oldValue in oldValues)
            {
                result = result.Replace(oldValue, newValue);
            }
            return result;
        }

        /// <summary>An overload of Replace that accepts a string array.
        /// For the supplied string, replaces all values in the {oldValues} array with those in {newValues} array.</summary>
        /// <example>Shortcut for y.Replace("cat", "frog").Replace("dog", "bird") etc...</example>.
        public static string Replace(this string str, string[] oldValues, string[] newValues)
        {
            if (oldValues == null || newValues == null)
                return str;

            if (oldValues.Length != newValues.Length)
                throw new Exception("To use this overload of Replace, the oldValues array must contain the same number of elements as the newValues array.");

            string result = str;

            for (int i = 0; i < oldValues.Length; i++)
            {
                result = result.Replace(oldValues[i], newValues[i]);
            }
            return result;
        }

        /// <summary>An overload of Split that accepts a single string as separator.</summary>
        public static string[] Split(this string str, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            string[] s = new string[] { separator };
            
            return str.Split(s, options);
        }
    }
}
