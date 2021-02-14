using System;
using System.Globalization;

namespace WildHare.Extensions
{
    public static class CharExtensions
    {
        /// <summary>Returns the character at the position {i}.</summary>
        public static char CharAt(this string s, int i)
        {
            return Convert.ToChar(s.Substring(i, 1));
        }

        /// <summary>Inline version of char.IsLetterOrDigit(ch). ie: A-Z, a-z, and letters of other alphabets</summary>
        public static bool IsLetter(this char ch) { return char.IsLetter(ch); }

        /// <summary>Inline version of char.IsLetterOrDigit(ch). ie: Letters plus digits</summary>
        public static bool IsLetterOrDigit(this char ch) { return char.IsLetterOrDigit(ch); }

        /// <summary>Inline version of char.IsUpper(ch). ie: Uppercase letters</summary>
        public static bool IsUpper(this char ch) { return char.IsUpper(ch); }

        /// <summary>Inline version of char.IsLower(ch). ie: Lowercase letters</summary>
        public static bool IsLower(this char ch) { return char.IsLower(ch); }

        /// <summary>Inline version of char.IsPunctuation(ch). ie: Symbols used for punctuation in Western and other alphabets</summary>
        public static bool IsPunctuation(this char ch) { return char.IsPunctuation(ch); }

        /// <summary>Inline version of char.IsNumber(ch). ie: All digits plus Unicode fractions and Roman numeral symbols</summary>
        public static bool IsNumber(this char ch) { return char.IsNumber(ch); }

        /// <summary>Inline version of char.IsDigit(ch). ie: 0-9 plus digits of other alphabets</summary>
        public static bool IsDigit(this char ch) { return char.IsDigit(ch); }

        /// <summary>Inline version of char.IsWhiteSpace(ch). ie: All separators plus \n, \r, \t, \f, and \v </summary>
        public static bool IsWhiteSpace(this char ch) { return char.IsWhiteSpace(ch); }

        /// <summary>Inline version of char.IsSymbol(ch). ie: Most other printable symbols</summary>
        public static bool IsSymbol(this char ch) { return char.IsSymbol(ch); }

        /// <summary>Inline version of char.IsControl(ch). ie: Non-printable "control" characters below 0x20, such as \r, \n, \r, and \0,
        /// and characters between 0x7F and 0x9A</summary>
        public static bool IsControl(this char ch) { return char.IsControl(ch); }

        /// <summary>Inline version of char.IsSeparator(ch). in: Space plus all Unicode separator characters</summary>
        public static bool IsSeparator(this char ch) { return char.IsSeparator(ch); }

        /// <summary>Inline version of char.GetUnicodeCategory(ch)</summary>
        public static UnicodeCategory GetUnicodeCategory(this char ch) { return char.GetUnicodeCategory(ch); }

    }
}
