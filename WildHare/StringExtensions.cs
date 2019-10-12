using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WildHare.Extensions
{
    public static class StringExtensions
    {
        /// <summary>Inline version of string.IsNullOrEmpty()</summary>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
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

        /// <summary>Remove the start of a string if it exactly matches {start} and remove 
        /// and the end of a string if it exactly matches {end}.</summary>
        public static string RemoveStartEnd(this string input, string start, string end =  null)
        {
            return input.RemoveStart(start).RemoveEnd(end ?? start);
        }

        /// <summary>Remove the start of a string if it exactly matches any of the strings in the {startArray}.</summary>
        public static string RemoveStart(this string input, string[] startArray)
        {
            string s = input.IfNullOrEmpty();
            foreach (string start in startArray)
            {
                s = s.RemoveStart(start);
            }
            return s;
        }

        /// <summary>Remove the end of a string if it exactly matches any of the strings in the {endArray}.</summary>
        public static string RemoveEnd(this string input, string[] endArray)
        {
            string s = input.IfNullOrEmpty();
            foreach (string end in endArray)
            {
                s = s.RemoveEnd(end);
            }
            return s;
        }

        /// <summary>Remove the start of a string if it exactly matches any of the strings in the {startArray} 
        /// and remove the end of a string if it exactly matches any of the strings in the {endArray}.</summary>
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
            return string.Join("\n", input.Split('\n').Select(a => a.RemoveStart(start)));
        }

        /// <summary>Remove the end of line if it exactly matches {end} for all lines in the string.</summary>
        public static string RemoveEndFromAllLines(this string input, string end)
        {
            return string.Join("\n", input.Split('\n').Select(a => a.RemoveEnd(end)));
        }

        /// <summary>If a string if it is not null or empty, adds {addToEnd} to the end.
        public static string IfNotEmpty(this string s, string addToEnd)
        {
            s = s ?? "";
            return (s.Trim().Length > 0) ? (s += addToEnd) : s;
        }

        /// <summary>If a string if it is not null or empty, adds {addToStart} to the start and {addToEnd} to the end. 
        public static string IfNotEmpty(this string s, string addToStart, string addToEnd)
        {
            s = s ?? "";
            return (s.Trim().Length > 0) ? (addToStart + s + addToEnd) : s;
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
        ///  to the end with '...' as the default, if not specified. It will attempt to make the truncation 
        /// at a space or line break, but will search {wordcut} characters before forcing the wordcut.</summary>
        public static string Truncate(this string input, int maxCharacters, string more = "... ", int wordcut = 12)
        {
            int strEnd;
            if (input.Length > maxCharacters)
            {
                strEnd = input.Trim().LastIndexOfAny(new char[] { ' ', '\n', '\r' }, maxCharacters);
                if ((maxCharacters - strEnd) > wordcut) strEnd = maxCharacters - wordcut;
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
			string indentation = string.Join("", Enumerable.Repeat(indent, number));
			string trimAllLines = string.Join("\n", output.Split('\n').Select(a => a.RemoveStart(indentation)));

			return trimAllLines;
		}

		/// <summary>Increments integer +1 on the end of a string</summary>
		/// <example>'File.txt'.IncrementString(".txt") = 'File1.txt'</example>
		/// <example>'File6.txt'.IncrementString(1,".txt") = 'File1.txt'</example>
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
    }
}
