using System;
using System.Collections.Generic;
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

        /// <summary>Inline version of string.IsNullOrWhiteSpace()</summary>
        public static bool IsNullOrSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>Inline version of to test for null</summary>
        public static bool IsNull(this string s)
        {
            return (s == null);
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

        /// <summary>Remove the start of line if it exactly matches {start} for all lines in the string. Normalizes all
        /// line returns to Environment.NewLine. This can be useful for programmatically removing indents from long strings.</summary>
        public static string RemoveStartFromAllLines(this string input, string start)
        {
            return string.Join(NewLine, input.Split("\n").Select(a => a.RemoveStartEnd(start, "\r")));
        }

        /// <summary>Remove the start of line if it exactly matches any string in the {startArray} for all lines in the string. Normalizes all line 
        /// returns to Environment.NewLine. This can be useful for programmatically removing indents from long strings.</summary>
        public static string RemoveStartFromAllLines(this string input, string[] startArray)
        {
            return string.Join(NewLine, input.Split("\n").Select(a => a.RemoveStartEnd(startArray, new[] { "\r" }) ));
        }

        /// <summary>Remove the end of line if it exactly matches {end} for all lines in the string.
        /// Normalizes all line returns to Environment.NewLine.</summary>
        public static string RemoveEndFromAllLines(this string input, string end)
        {
            return string.Join(NewLine, input.Split('\n').Select(a => a.RemoveEnd("\r").RemoveEnd(end)));
        }

        /// <summary>Remove the end of line if it exactly matches any string in the {endArray} for all lines in the string.
        /// Normalizes all line returns to Environment.NewLine.</summary>
        public static string RemoveEndFromAllLines(this string input, string[] endArray)
        {
            return string.Join(NewLine, input.Split('\n').Select(a => a.RemoveEnd("\r").RemoveEnd(endArray)));
        }

        /// <summary>Removes the indent from all lines of text in a string using the indent from the second line
        /// of text in the string as the indent to remove. The first line is typically where the verbatim and/or
        /// interpolation is declared and does not have an indent and so is skipped. If {removeInitialSpaces} is true
        /// (the default), it removes the first line if it only contains whitespace.</summary>
        public static string RemoveIndents(this string input, bool removeInitialSpaces = true)
        {
            var lines = input.Split("\n",StringSplitOptions.None).ToList();
            var start = lines.Count >= 2 ? lines[1].GetStartWhitespaces() : "";

            if (removeInitialSpaces && lines[0].IsNullOrSpace())
                lines.RemoveAt(0);

            return string.Join(NewLine, lines.Select(a => a.RemoveStartEnd(start, "\r")));
        }

        /// <summary>Gets the all the whitespace at the beginning of a string.</summary>
        public static string GetStartWhitespaces(this string input)
        {
            if (input.IsNullOrEmpty())
                return input;

            char[] characters = input.ToCharArray(); //.Where(w => char.IsWhiteSpace(w)).ToArray();
            var sb = new StringBuilder();

            foreach (char character in characters)
            {
                if (char.IsWhiteSpace(character))
                    sb.Append(character);
                else
                    break;
            }
            return sb.ToString();
        }

        /// <summary>Adds {addToStart} to the beginning of the string if string {s} is not NULL or EMPTY.</summary>
        public static string AddStart(this string s, string addToStart)
        {
            string str = s ?? "";
            return (str.Trim().Length > 0) ? (addToStart + str) : s;
        }

        /// <summary>Adds {addToEnd} to the end of the string if string {s} is not NULL or EMPTY.</summary>
        public static string AddEnd(this string s, string addToEnd)
        {
            string str = s ?? "";
            return (str.Trim().Length > 0) ? (str + addToEnd) : s;
        }

        /// <summary>Adds {addToStart} to the beginning of the string and {addToEnd} to the end of the string {s} if is not NULL or EMPTY.
        /// If {addToEnd} is NULL, adds {addToStart} to both the start and end..</summary>
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

        /** <summary>Adds {addToStart} to the beginning of the string if it does not start with that string AND
            adds {addToEnd} to the end of the string if it does not end with that string. If {addToEnd}
            is NULL, adds {addToStart} to both the start and end.</summary>
            <cref></cref>*/
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
            if (s == null)
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

        /// <summary>Returns a string with only letters and any additional characters in {otherCharacters}.</summary>
        public static string LettersOnly(this string input, string otherCharacters = "")
        {
            var output = new StringBuilder("");
            for (int i = 0; i < input.IfNullOrEmpty().Length; i++)
            {
                if (("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" + otherCharacters).IndexOf(input[i]) != -1)
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

        /// <summary>Increments integer +1 on the end of a string. IF the string does not contain an integer then
        /// will use {seedIfEmpty}.  You can specify a file extension at the end to ignore using {ignoreExtension}.
        /// If needed, you can set a value for {increment} to be other than 1.</summary>
        public static string IncrementString(this string str, int? seedIfEmpty = null, string ignoreExtension = "", int increment = 1)
		{
			str = str.RemoveEnd(ignoreExtension);
			int? currentnumber = seedIfEmpty ?? 1;
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

        /// <summary>An overload of Replace that accepts a Dictionary of string,string.
        /// For the supplied string, replaces all instances in the matching the dictionary key with the dictionary value
        /// If {reverse} is true, replaces the matching dictionary value with the dictionary key.</summary>
        public static string Replace(this string str, Dictionary<string,string> dictionary, bool reverse = false)
        {
            if(reverse)
                return dictionary.Aggregate(str, (current, value) => current.Replace(value.Value, value.Key)); ;

            return dictionary.Aggregate(str, (current, value) => current.Replace(value.Key, value.Value)); ;
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

        /// <summary>Ignores string by returning null if the string {str} equals {str2}.</summary>
        /// <example>string field = "*"; var f = field.Ignore("*") ?? "Empty" ex: return "Empty"</example>
        public static string Ignore(this string str, string str2)
        {
            return str != str2 ? str : null;
        }

        /// <summary>Will return the {singular} form of a word if {number} is equal to 1, otherwise returns {plural}. For simple cases,
        /// it will add an "s" to the end  (or an "es" if {singular} ends in "s","x","ch","sh","z", or "o") if {plural} is omitted.</summary>
        /// <example>1.SingularOrPlural("clown") returns "clown";</example>
        /// <example>3.SingularOrPlural("clowns") returns "clowns";</example>
        /// <example>3.SingularOrPlural("fox") returns "foxes";</example>
        /// <example>5.SingularOrPlural("child","children") returns "children";</example>
        public static string SingularOrPlural(this int number, string singular, string plural = null)
        {
            string[] endings = { "s", "x", "ch", "sh", "z", "o" }; 

            if (number == 1)
                return singular;

            if (plural is null)
            {
                return endings.Any(x => singular.EndsWith(x, StringComparison.OrdinalIgnoreCase) ) ? singular + "es": singular + "s";
            }
            return plural;
        }

        public static string Left(this string str, int length)
        {
            if (str.IsNullOrEmpty())
                return "";

            return str.Substring(0, Math.Min(str.Length, length));
        }

        public static string Mid(this string str, int start, int? count = null)
        {
            if (str.IsNullOrEmpty())
                return "";

            int strRemainder = str.Length - start;
            int trimmedCount = count.HasValue && count <= strRemainder ? count.Value : strRemainder;

            return str.Substring(Math.Min(start, str.Length), trimmedCount);
        }

        public static string Right(this string str, int length)
        {
            if (str.IsNullOrEmpty())
                return "";

            return (str.Length > length) ? str.Substring(str.Length - length, length) : str;
        }

        public static string ForEachLine(this string input, Func<string, string> func)
        {
            var lines = input.Split("\n", StringSplitOptions.None);
            var returnChars = new[] { "\n", "\r", "\n\r" };

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].RemoveEnd(returnChars);
                lines[i] = func(line);
            }
            return string.Join(NewLine, lines);
        }

        /// Succinct overload of Equals() that compares the {str} to the {compareTo} and returns a bool true if equal.
        /// Takes an optional bool {ignoreCase} which uses StringComparison.Ordinal if
        /// false (the default) or StringComparison.OrdinalIgnoreCase if true.
        public static bool Equals(this string str, string compareTo, bool ignoreCase = false)
        {
            var strComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            return str.Equals(compareTo, strComparison);
        }
    }
}


/*  OLD CODE
    ----------------------------------------------------------
    var i = input.Split("\n");
    var s = i.Select(a => a.RemoveStartEnd(startArray, new[] { "\r" }));
    var j = string.Join(NewLine, s);

    return j;
*/
