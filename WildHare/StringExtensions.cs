using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WildHare.Extensions

{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static string IfNullOrEmpty(this string s, string replacement = "")
        {
            return s.IsNullOrEmpty() ? replacement : s;
        }

        public static bool IsNullOrSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static string IfNullOrSpace(this string s, string replacement = "")
        {
            return s.IsNullOrSpace() ? replacement : s;
        }

        public static string RemoveStart(this string input, string start)
        {
            string s = input.IfNullOrEmpty();
            string t = start.IfNullOrEmpty();

            return s.StartsWith(t) ? s.Remove(0, t.Length) : s;
        }

        public static string RemoveEnd(this string input, string end)
        {
            string s = input.IfNullOrEmpty();
            string e = end.IfNullOrEmpty();

            return s.EndsWith(e) ? s.Remove(s.Length - e.Length, e.Length) : s;
        }

        public static string RemoveStartEnd(this string input, string start, string end =  null)
        {

            return input.RemoveStart(start).RemoveEnd(end ?? start);
        }

        public static string RemoveStart(this string input, string[] startArray)
        {
            string s = input.IfNullOrEmpty();
            foreach (string start in startArray)
            {
                s = s.RemoveStart(start);
            }
            return s;
        }

        public static string RemoveEnd(this string input, string[] endArray)
        {
            string s = input.IfNullOrEmpty();
            foreach (string end in endArray)
            {
                s = s.RemoveEnd(end);
            }
            return s;
        }

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

        public static string IfNotEmpty(this string s, string addToEnd)
        {
            s = s ?? "";
            return (s.Trim().Length > 0) ? (s += addToEnd) : s;
        }

        public static string IfNotEmpty(this string s, string addToStart, string addToEnd)
        {
            s = s ?? "";
            return (s.Trim().Length > 0) ? (addToStart + s + addToEnd) : s;
        }

        public static string NumbersOnly(this string input, string additionalChars)
        {
            var output = new StringBuilder("");
            for (int i = 0; i < input.IfNullOrEmpty().Length; i++)
            {
                if (("0123456789" + additionalChars).IndexOf(input[i]) != -1)
                {
                    output.Append(input[i]);
                }
            }
            return output.ToString();
        }

        public static string Truncate(this string input, int maxCharacters, string more = "... ", int wordcut = 12)
        {
            int strEnd = 0;
            if (input.Length > maxCharacters)
            {
                strEnd = input.Trim().LastIndexOfAny(new char[] { ' ', '\n', '\r' }, maxCharacters);
                if ((maxCharacters - strEnd) > wordcut) strEnd = maxCharacters - wordcut;
                input = input.Substring(0, strEnd).Trim() + more;
            }
            return input;
        }

        public static string ProperCase(this string input)
        {
            var sb = new StringBuilder();
            bool emptyBefore = true;
            foreach (char ch in input)
            {
                char chThis = ch;
                if (Char.IsWhiteSpace(chThis))
                    emptyBefore = true;
                else
                {
                    if (Char.IsLetter(chThis) && emptyBefore)
                        chThis = Char.ToUpper(chThis);
                    else
                        chThis = Char.ToLower(chThis);
                    emptyBefore = false;
                }
                sb.Append(chThis);
            }
            return sb.ToString();
        }

        public static char CharAt(this string s, int i)
        {
            return Convert.ToChar(s.Substring(i, 1));
        }

        public static bool WriteToFile(this string stringToWrite, string fileName, bool overWriteExisting = false)
        {
            var file = new FileInfo(fileName);

            return stringToWrite.WriteToFile(file, overWriteExisting);
        }

        public static bool WriteToFile(this string stringToWrite, FileInfo file, bool overWriteExisting = false)
        {
            if (file.Exists && overWriteExisting != true)
            {
                return false;
            }
            using (var tw = new StreamWriter(file.Create()))
            {
                tw.Write(stringToWrite);
            }
            return true;
        }

        public static bool AppendToFile(this string stringToWrite, string fileName)
        {
            var file = new FileInfo(fileName);

            return stringToWrite.AppendToFile(file);
        }

        public static bool AppendToFile(this string stringToWrite, FileInfo file)
        {
            using (var tw = file.AppendText())
            {
                tw.Write(stringToWrite);
            }
            return true;
        }

        public static string ToMapPath(this string fileName)
        {
            var appRoot = GetApplicationRoot();
            string file = fileName.Replace(@"/", @"\").RemoveStart(@"~").RemoveStart(@"/");
            var path = Path.Combine(appRoot, file);

            return path;
        }

        public static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;

            return appRoot;
        }
    }
}
