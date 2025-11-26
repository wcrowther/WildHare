using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using WildHare.Tests.Models;
using WildHare.Tests.SourceFiles;
using WildHare.Xtra;
using static System.Environment;

namespace WildHare.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void Test_NumbersOnly()
        {
            string numbersAndWords = "123SomeWord456";
            string numbersOnlyString = numbersAndWords.NumbersOnly();

            ClassicAssert.AreEqual("123456", numbersOnlyString);
        }

        [Test]
        public void Test_NumbersOnly_With_Other_Characters()
        {
            string numbersAndWords = "$12345.SomeWord00";
            string numbersOnlyString = numbersAndWords.NumbersOnly("$.");

            ClassicAssert.AreEqual("$12345.00", numbersOnlyString);
        }

        [Test]
        public void Test_NumbersAndLettersOnly_With_Other_Characters()
        {
            string str = "<*will22@wildhare.com*/>";
            string filteredStr = str.NumbersAndLettersOnly("@.");

            ClassicAssert.AreEqual("will22@wildhare.com", filteredStr);
        }

        [Test]
        public void Test_LettersOnly_With_Other_Characters()
        {
            string str = "<*123will@wildhare.com*/>";
            string filteredStr = str.LettersOnly("@.");

            ClassicAssert.AreEqual("will@wildhare.com", filteredStr);
        }

        [Test]
        public void Test_CharactersOnly_Basic()
        {
            string str = "<*will22@wildhare.com* />";
            string filteredStr = str.CharactersOnly("</>");

            ClassicAssert.AreEqual("</>", filteredStr);
        }

        [Test]
        public void Test_IsNumbersOnly()
        {
            string numbers = "123456";
            string numbersAndWords = "123SomeWord456";

            ClassicAssert.IsTrue(numbers.IsNumbersOnly());
            ClassicAssert.False(numbersAndWords.IsNumbersOnly());
        }

        [Test]
        public void Test_IsNumbersOnly_With_Other_Characters()
        {
            string numbers = "$123456.00";
            string numbersAndWords = "$123SomeWord456.00";

            ClassicAssert.IsTrue(numbers.IsNumbersOnly("$."));
            ClassicAssert.False(numbersAndWords.IsNumbersOnly("$."));
        }

        [Test]
        public void Test_IsLettersOnly_With_Other_Characters()
        {
            string trueStr = "wildhare.com";
            string falseStr = "22@wildhare.com";

            ClassicAssert.IsTrue(trueStr.IsLettersOnly("."));
            ClassicAssert.False(falseStr.IsLettersOnly("@."));
        }

        [Test]
        public void Test_IsLettersOnly_With_Null_And_Empty()
        {
            string falseStr = null;
            string trueStr = "";

            ClassicAssert.False(falseStr.IsLettersOnly("."));
            ClassicAssert.IsTrue (trueStr.IsLettersOnly("."));
        }

        [Test]
        public void Test_IsNumbersAndLettersOnly_With_Other_Characters()
        {
            string trueStr = "will22@wildhare.com";
            string falseStr = "<*will22@wildhare.com*/>";

            ClassicAssert.IsTrue(trueStr.IsNumbersAndLettersOnly("@."));
            ClassicAssert.False(falseStr.IsNumbersAndLettersOnly("@."));
        }

        [Test]
        public void Test_IsLettersAndNumbersOnly_With_Null_And_Empty()
        {
            string falseStr = null;
            string trueStr  = "";

            ClassicAssert.False(falseStr.IsNumbersAndLettersOnly("@."));
            ClassicAssert.IsTrue (trueStr.IsNumbersAndLettersOnly("@."));
        }

        [Test]
        public void Test_IsCharactersOnly_Basic()
        {
            string trueStr = "<!-- -->";
            string falseStr = "<*will22@wildhare.com*/>";

            ClassicAssert.IsTrue(trueStr.IsCharactersOnly("<!- >"));
            ClassicAssert.False(falseStr.IsCharactersOnly("<!- >"));
        }

        [Test]
        public void Test_IsCharactersOnly_With_Null_And_Empty()
        {
            string falseStr = null;
            string trueStr = "";

            ClassicAssert.False(falseStr.IsCharactersOnly("@."));
            ClassicAssert.IsTrue (trueStr.IsCharactersOnly("@."));
        }

        [Test]
        public void Test_Truncate_Basic()
        {
            string str = "12345678901234567890123456789012345678901234567890";
            string result = str.Truncate(10);

            ClassicAssert.AreEqual("1234567890...", result);
        }

        [Test]
        public void Test_Truncate_Custom_More()
        {
            string str = "12345678901234567890123456789012345678901234567890";
            string result = str.Truncate(10, "...(truncated)");

            ClassicAssert.AreEqual("1234567890...(truncated)", result);
        }

        [Test]
        public void Test_Truncate_Exact()
        {
            string str = "12345678901234567890123456789012345678901234567890";
            string result = str.Truncate(10, "", 0);

            ClassicAssert.AreEqual("1234567890", result);
        }

        [Test]
        public void Test_Truncate_Small_WordCut()
        {
            string str = "12345 67890 1234567890";
            string result = str.Truncate(10);

            ClassicAssert.AreEqual("12345...", result);
        }

        [Test]
        public void Test_Truncate_Small_WordCut_Case2()
        {
            string str = "12345 67890 123456789012345";
            string result = str.Truncate(12);

            ClassicAssert.AreEqual("12345 67890...", result);
        }

        [Test]
        public void Test_Truncate_Small_WordCut_Case3()
        {
            string str = "12345 78901234567890123456789";
            string result = str.Truncate(20, "...", 12);

            ClassicAssert.AreEqual("12345 78901234567890...", result);
        }

        [Test]
        public void Test_RemoveStartFromAllLines_Basic()
        {
            string multiLineText = @"This is a 
			sentence 
			spread across multiple lines.";

            string indentingRemovedText = multiLineText.RemoveStartFromAllLines("\t\t\t");

            ClassicAssert.AreEqual($"This is a {NewLine}sentence {NewLine}spread across multiple lines.", indentingRemovedText);
        }

        [Test]
        public void Test_RemoveIndents_Basic()
        {
            string multiLineText = @"This is a 
			        sentence 
			        spread across multiple lines.";

            string indentingRemovedText = multiLineText.RemoveIndents();

            ClassicAssert.AreEqual($"This is a {NewLine}sentence {NewLine}spread across multiple lines.", indentingRemovedText);
        }

        [Test]
        public void Test_RemoveIndents_Basic2()
        {
            string multiLineText = @"
                    This is a 
                    sentence 
                    spread across multiple lines.";

            string indentingRemovedText = multiLineText.RemoveIndents();

            ClassicAssert.AreEqual($"This is a {NewLine}sentence {NewLine}spread across multiple lines.", indentingRemovedText);
        }

        [Test]
        public void Test_RemoveIndents_Basic_Keep_First_Line_Spaces()
        {
            string multiLineText = @"
                    This is a 
                    sentence 
                    spread across multiple lines.";

            string indentingRemovedText = multiLineText.RemoveIndents(false);

            ClassicAssert.AreEqual($"{NewLine}This is a {NewLine}sentence {NewLine}spread across multiple lines.", indentingRemovedText);
        }

        [Test]
        public void Test_RemoveIndents_StringBuilder()
        {
            var sb = new StringBuilder();
            sb.AppendLine("This is a");
            sb.AppendLine("            sentence ");
            sb.AppendLine("            spread across");
            sb.AppendLine("            multiple lines.");

            string indentingRemovedText = sb.ToString().RemoveIndents();

            ClassicAssert.AreEqual($"This is a{NewLine}sentence {NewLine}spread across{NewLine}multiple lines.{NewLine}", indentingRemovedText);
        }

        [Test]
        public void Test_IsWhiteSpace_Characters()
        {
            char newlineChar = '\n';
            char tabChar = '\t';
            char returnChar = '\r';

            ClassicAssert.IsTrue(char.IsWhiteSpace(newlineChar));
            ClassicAssert.IsTrue(char.IsWhiteSpace(tabChar));
            ClassicAssert.IsTrue(char.IsWhiteSpace(returnChar));
        }

        [Test]
        public void Test_WriteLine_And_Verbatim_String_NewLine_Equality()
        {
            string multiLineText = @"This is a 
            sentence 
            spread across 
            multiple lines.";

            var sb1 = new StringBuilder();
            sb1.Append("This is a \n");
            sb1.Append("            sentence \n");
            sb1.Append("            spread across \n");
            sb1.Append("            multiple lines.");

            var sb2 = new StringBuilder();
            sb2.AppendLine("This is a ");
            sb2.AppendLine("            sentence ");
            sb2.AppendLine("            spread across ");
            sb2.AppendLine("            multiple lines.");

            string string1 = sb1.ToString();
            string string2 = sb2.ToString();

            ClassicAssert.AreEqual(multiLineText, string1);
			ClassicAssert.AreNotEqual(multiLineText, string2);
			ClassicAssert.AreNotEqual(string1, string2);
        }


        [Test]
        public void Test_RemoveStartFromAllLines_Using_String_Array()
        {
            string multiLineText = @"This is a 
            sentence 
            spread across 
            multiple lines.";

            string indentingRemovedText = multiLineText.RemoveStartFromAllLines(new[] { "\t\t\t", "            " });
            string expected = $"This is a {NewLine}sentence {NewLine}spread across {NewLine}multiple lines.";

            ClassicAssert.AreEqual(expected, indentingRemovedText);
        }

        [Test]
        public void Test_RemoveStartFromAllLines_Using_String_Array_2()
        {
            var sb = new StringBuilder();
            sb.AppendLine("This is a ");
            sb.AppendLine("\t\tsentence ");
            sb.AppendLine("\t\tspread across ");
            sb.Append("\t\tmultiple lines.");

            string sbText = sb.ToString().RemoveStartFromAllLines(new[] { "\t\t", "            " });
            string expected = $"This is a {NewLine}sentence {NewLine}spread across {NewLine}multiple lines.";

            ClassicAssert.AreEqual(expected, sbText);
        }

        [Test]
        public void Test_String_Repeat_Times_Ten()
        {
            string stringRepeated = "x".Repeat(10);

            ClassicAssert.AreEqual("xxxxxxxxxx", stringRepeated);

            string stringRepeatedTwice = stringRepeated.Repeat(3);

            ClassicAssert.AreEqual("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", stringRepeatedTwice);
        }

        [Test]
        public void Test_String_Repeat_With_Null()
        {
            string nullString = null;
            string stringRepeated = nullString.Repeat(10);

            ClassicAssert.AreEqual(null, stringRepeated);
        }

        [Test]
        public void Test_String_Repeat_With_Negative_Number()
        {
            string emptyString = "empty";
            string stringRepeated = emptyString.Repeat(-10);

            ClassicAssert.IsNull(stringRepeated);
        }

        [Test]
        public void Test_AddStart_IfEmpty()
        {
            string str = "";
            string strToAdd = "xxx";
            string result = str.AddStart(strToAdd);

            ClassicAssert.AreEqual("", result);
        }

        [Test]
        public void Test_AddStart_IfNull()
        {
            string str = null;
            string strToAdd = "xxx";
            string result = str.AddStart(strToAdd);

            ClassicAssert.IsNull(result);
        }

        [Test]
        public void Test_AddStart_If_Populated_String()
        {
            string str = "Test";
            string strToAdd = "xxx";
            string result = str.AddStart(strToAdd);

            ClassicAssert.AreEqual("xxxTest", result);
        }

        [Test]
        public void Test_AddStart_Optional_QueryString()
        {
            string expected = "https://www.will.com?one=1&three=3";

            string url = "https://www.will.com";
            string qs1 = "1";
            string qs2 = "";
            string qs3 = "3";
            string querystring = $"{qs1.AddStartEnd("one=", "&")}{qs2.AddStartEnd("two=", "&")}{qs3.AddStart("three=")}".RemoveEnd("&");

            string urlWithQuerystring = $"{url}{querystring.AddStart("?")}";

            ClassicAssert.AreEqual(expected, urlWithQuerystring);
        }

        [Test]
        public void Test_AddStart_Optional_QueryString_Version_2()
        {
            string expected = "https://www.will.com?one=1&three=3";

            string url = "https://www.will.com";
            string qs1 = "1";
            string qs2 = "";
            string qs3 = "3";

            string querystring = qs1.AddStart("&one=") + qs2.AddStart("&two=") + qs3.AddStart("&three=");

            ClassicAssert.AreEqual(expected, url + querystring.RemoveStart("&").AddStart("?"));
        }


        [Test]
        public void Test_EnsureStart_IfEmpty()
        {
            string str = "";
            string strToAdd = "xxx";
            string result = str.EnsureStart(strToAdd);

            ClassicAssert.AreEqual("xxx", result);
        }

        [Test]
        public void Test_EnsureStart_IfNull()
        {
            string str = null;
            string strToAdd = "xxx";
            string result = str.EnsureStart(strToAdd);

            ClassicAssert.IsNull(result);
        }

        [Test]
        public void Test_EnsureStart_If_String_Does_Not_Start_With_AddToStart()
        {
            string str = "Test";
            string strToAdd = "xxx";
            string result = str.EnsureStart(strToAdd);

            ClassicAssert.AreEqual("xxxTest", result);
        }

        [Test]
        public void Test_EnsureStart_If_String_Starts_With_AddToStart()
        {
            string str = "xxxTest";
            string strToAdd = "xxx";
            string result = str.EnsureStart(strToAdd);

            ClassicAssert.AreEqual("xxxTest", result);
        }

        //========================

        [Test]
        public void Test_EnsureEnd_IfEmpty()
        {
            string str = "";
            string strToAdd = "xxx";
            string result = str.EnsureEnd(strToAdd);

            ClassicAssert.AreEqual("xxx", result);
        }

        [Test]
        public void Test_EnsureEnd_IfNull()
        {
            string str = null;
            string strToAdd = "xxx";
            string result = str.EnsureEnd(strToAdd);

            ClassicAssert.IsNull(result);
        }

        [Test]
        public void Test_EnsureEnd_If_String_Does_Not_End_With_StrToAdd()
        {
            string str = "Test";
            string strToAdd = "xxx";
            string result = str.EnsureEnd(strToAdd);

            ClassicAssert.AreEqual("Testxxx", result);
        }

        [Test]
        public void Test_EnsureEnd_If_String_End_With_StrToAdd()
        {
            string str = "Testxxx";
            string strToAdd = "xxx";
            string result = str.EnsureEnd(strToAdd);

            ClassicAssert.AreEqual("Testxxx", result);
        }

        //========================

        [Test]
        public void Test_EnsureStartEnd_IfEmpty()
        {
            string str = "";
            string addToStart = "xxx";
            string addToEnd = "xxx";
            string result = str.EnsureStartEnd(addToStart, addToEnd);

            ClassicAssert.AreEqual("xxxxxx", result);
        }

        [Test]
        public void Test_EnsureStartEnd_IfNull()
        {
            string str = null;
            string addToStart = "xxx";
            string addToEnd = "xxx";
            string result = str.EnsureStartEnd(addToStart, addToEnd);

            ClassicAssert.IsNull(result);
        }

        [Test]
        public void Test_EnsureStartEnd_If_String_Does_Not_Start_With_AddToStart_Or_AddToEnd()
        {
            string str = "Test";
            string addToStart = "xxx";
            string addToEnd = "xxx";
            string result = str.EnsureStartEnd(addToStart, addToEnd);

            ClassicAssert.AreEqual("xxxTestxxx", result);
        }

        [Test]
        public void Test_EnsureStartEnd_If_String_Starts_With_AddToStart_And_AddToEnd()
        {
            string str = "xxxTestxxx";
            string addToStart = "xxx";
            string addToEnd = "xxx";
            string result = str.EnsureStartEnd(addToStart, addToEnd);

            ClassicAssert.AreEqual("xxxTestxxx", result);
        }

        [Test]
        public void Test_EnsureStartEnd_If_String_Starts_With_AddToStart_And_AddToEnd_IsNull()
        {
            string str = "xxxTestxxx";
            string addToStart = "xxx";
            string result = str.EnsureStartEnd(addToStart);

            ClassicAssert.AreEqual("xxxTestxxx", result);
        }

        [Test]
        public void Test_GetStart_Basic()
        {
            string str = "Begin_Finish";
            string start = str.GetStartBefore("_");

            ClassicAssert.AreEqual("Begin", start);
        }

        [Test]
        public void Test_GetStart_Basic_IncludeSeparator()
        {
            string str = "Begin_Finish";
            string start = str.GetStartBefore("_", true);

            ClassicAssert.AreEqual("Begin_", start);
        }

        [Test]
        public void Test_GetStart_No_Separator()
        {
            string str = "Begin_Finish";
            string start = str.GetStartBefore("x");

            ClassicAssert.AreEqual(str, start);
        }

        [Test]
        public void Test_GetStart_Null()
        {
            string str = null;
            string start = str.GetStartBefore("_");

            ClassicAssert.IsNull(start);
        }

        [Test]
        public void Test_GetEnd_Basic()
        {
            string str = "Begin_Finish";
            string end = str.GetEndAfter("_");

            ClassicAssert.AreEqual("Finish", end);
        }

        [Test]
        public void Test_GetEnd_Basic_IncludeSeparator()
        {
            string str = "Begin_Finish";
            string end = str.GetEndAfter("_", true);

            ClassicAssert.AreEqual("_Finish", end);
        }

        [Test]
        public void Test_StartsWith_Array_Overload_If_Start_Is_True()
        {
            string str = "A";
            string[] values = ["A", "IsNotNullString"];
            bool result = str.StartsWith(values);

            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void Test_StartsWith_Array_Overload_If_Inline_Start_Is_True()
        {
            string str = "IsNotNullString";
            bool result = str.StartsWith(["IsNotNullString"]);

            ClassicAssert.IsTrue(result);
        }

		private static readonly string[] valuesArray = ["Start1", "Start2"];

		[Test]
        public void Test_StartsWith_Array_Overload_If_Shortcut_Inline_Start_Is_True()
        {
            string str1 = "Start1";
            string str2 = "Start2";
            string str3 = "Start3";

            bool result1 = str1.StartsWith(["Start1", "Start2"]);
            bool result2 = str2.StartsWith(["Start1", "Start2"]);
            bool result3 = str3.StartsWith(valuesArray);

            ClassicAssert.IsTrue(result1);
            ClassicAssert.IsTrue(result2);
            ClassicAssert.False(result3);
        }


        [Test]
        public void Test_StartsWith_Array_Overload_With_String_Array_Start_Is_True()
        {
            string str = "a";
            string[] values = ["a", "IsNotNullString"];
            bool result = str.StartsWith(values);

            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void Test_StartsWith_Array_Overload_With_String_Array_Start_Is_False()
        {
            string str = "StartString";
            string[] values = { "a", "b" };
            bool result = str.StartsWith(values);

            ClassicAssert.False(result);
        }

        [Test]
        public void Test_EndsWith_Array_Overload_With_String_Array_End_Is_True()
        {
            string str = "StringEnd";
            string[] values = { "a", "End" };
            bool result = str.EndsWith(values);

            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void Test_EndsWith_Array_Overload_With_String_Array_End_Is_False()
        {
            string str = "StringEnd";
            string[] values = { "a", "b" };
            bool result = str.EndsWith(values);

            ClassicAssert.False(result);
        }

        [Test]
        public void Test_Parse()
        {
            string str = "Parse.The.Contents.Of.The.(Inner)Parenthesis";
            int startIndex = str.IndexOf('(');
            int endIndex = str.IndexOf(')');

            int length = endIndex - startIndex;
            string result = str.Substring(startIndex + 1, length - 1);

            ClassicAssert.AreEqual("Inner", result);
        }

        [Test]
        public void Test_Contains_Overload_Basic()
        {
            string str = "=====In=====";

            bool result1 = str.Contains("In", true);
            bool result2 = str.Contains("in", true);
            bool result3 = str.Contains("in", false);
            bool result4 = str.Contains("in");

            ClassicAssert.IsTrue(result1);
            ClassicAssert.IsTrue(result2);
            ClassicAssert.False(result3);
            ClassicAssert.False(result4);
        }

        [Test]
        public void Test_Contains_ValuesArray_Basic()
        {
            string str = "====hide===In=====";

            string[] array1 = { "In" };
            string[] array2 = { "in" };
            string[] array3 = { "bob", "hide" };
            string[] array4 = { "hide", "BOB", "Jean" };
            string[] array5 = { "fred", "BOB", "Jean" };
            string[] array6 = { "will", "Joe", "in" };

            bool result1 = str.Contains(array1, true);
            bool result2 = str.Contains(array2, true);
            bool result3 = str.Contains(array3, true);
            bool result4 = str.Contains(array4, true);
            bool result5 = str.Contains(array5);
            bool result6 = str.Contains(array6);

            ClassicAssert.IsTrue(result1);
            ClassicAssert.IsTrue(result2);
            ClassicAssert.IsTrue(result3);
            ClassicAssert.IsTrue(result4);

            ClassicAssert.False(result5);
            ClassicAssert.False(result6);
        }


        [Test]
        public void Test_Contains_Params_Basic()
        {
            string str = "====hide===In=====";

            bool result1 = str.Contains(true, "In");
            bool result2 = str.Contains(true, "in");
            bool result3 = str.Contains(true, "bob", "hide");
            bool result4 = str.Contains(true, "hide", "BOB", "Jean");
            bool result5 = str.Contains(false, "fred", "BOB", "Jean");
            bool result6 = str.Contains(false, "will", "Joe", "in");

            ClassicAssert.IsTrue(result1);
            ClassicAssert.IsTrue(result2);
            ClassicAssert.IsTrue(result3);
            ClassicAssert.IsTrue(result4); 

            ClassicAssert.False(result5);
            ClassicAssert.False(result6);
        }

        [Test]
        public void Test_Replace_With_Two_Array_Overload()
        {
            string str = "Favorite animals: cat dog rabbit.";
            string[] old = { "cat", "dog", "rabbit" };
            var result = str.Replace(old, new[] { "platypus", "cheetah", "emu" });

            ClassicAssert.AreEqual("Favorite animals: platypus cheetah emu.", result);
        }

        [Test]
        public void Test_Replace_With_Array_and_string_Overload()
        {
            string str = "Favorite animals: cat dog rabbit.";
            string[] oldArray = { "cat", "dog", "rabbit" };
            var result = str.Replace(oldArray, "dog");

            ClassicAssert.AreEqual("Favorite animals: dog dog dog.", result);
        }

        private static Dictionary<string, string> GetAnimalList()
        {
            return new Dictionary<string, string>
                {
                    {"cat", "platypus"},
                    {"dog", "cheetah"},
                    {"rabbit", "emu"}
                };
        }

        [Test]
        public void Test_Dictionary_Aggregate_Replace_Using_Linq()
        {
            string str = "Favorite animals: cat dog rabbit.";
            var dictionary = GetAnimalList();
            var result = dictionary.Aggregate(str, (current, value) => current.Replace(value.Key, value.Value));

            ClassicAssert.AreEqual("Favorite animals: platypus cheetah emu.", result);
        }

        [Test]
        public void Test_Replace_Dictionary_Overload()
        {
            string str = "Favorite animals: cat dog rabbit.";
            var dictionary = GetAnimalList();

            ClassicAssert.AreEqual("Favorite animals: platypus cheetah emu.", str.Replace(dictionary));
        }

        [Test]
        public void Test_Replace_Dictionary_Overload_Reverse()
        {
            string str = "Favorite animals: platypus cheetah emu.";
            var dictionary = GetAnimalList();

            ClassicAssert.AreEqual("Favorite animals: cat dog rabbit.", str.Replace(dictionary, true));
        }

        [TestCase(-1, "Item", "-1 Items in the list.")]
        [TestCase(0, "Item", "0 Items in the list.")]
        [TestCase(1, "Item", "1 Item in the list.")]
        [TestCase(10, "Item", "10 Items in the list.")]
        [TestCase(1, "dog", "1 dog in the list.")]
        [TestCase(10, "dog", "10 dogs in the list.")]
        [TestCase(1, "fox", "1 fox in the list.")]
        [TestCase(10, "fox", "10 foxes in the list.")]
        [TestCase(1, "clown", "1 clown in the list.")]
        [TestCase(3, "clown", "3 clowns in the list.")]
        [TestCase(3, "fox", "3 foxes in the list.")]
        public void Test_SingularOrPlural_With_One_Parameter(int count, string singular, string result)
        {
            string message = $"{count} {count.Pluralize(singular)} in the list.";

            ClassicAssert.AreEqual(result, message);
        }

        [TestCase(10, "dog", null, "10 dogs in the list.")]
        [TestCase(1, "wolf", "wolves", "1 wolf in the list.")]
        [TestCase(10, "wolf", "wolves", "10 wolves in the list.")]
        [TestCase(1, "octopus", "octopi", "1 octopus in the list.")]
        [TestCase(10, "octopus", "octopi", "10 octopi in the list.")]
        [TestCase(5, "child", "children", "5 children in the list.")]
        public void Test_SingularOrPlural_With_Two_Parameters(int count, string singular, string plural, string result)
        {
            string message = $"{count} {count.Pluralize(singular, plural)} in the list.";

            ClassicAssert.AreEqual(result, message);
        }

        [TestCase("Item", -1, "-1 Items in the list.")]
        [TestCase("Item", 0, "0 Items in the list.")]
        [TestCase("Item", 1, "1 Item in the list.")]
        [TestCase("Item", 10, "10 Items in the list.")]
        [TestCase("dog", 1, "1 dog in the list.")]
        [TestCase("dog", 10, "10 dogs in the list.")]
        [TestCase("fox", 1, "1 fox in the list.")]
        [TestCase("fox", 10, "10 foxes in the list.")]
        [TestCase("clown", 1, "1 clown in the list.")]
        [TestCase("clown", 3, "3 clowns in the list.")]
        [TestCase("fox", 3, "3 foxes in the list.")]
        public void Test_IfPlural_With_One_Parameter(string singular, int count, string result)
        {
            string message = $"{count} {count.Pluralize(singular)} in the list.";

            ClassicAssert.AreEqual(result, message);
        }

        [Test]
        public void Test_Left_Mid_Right_Basic()
        {
            string str = "LeftMiddleRight";

            ClassicAssert.AreEqual("Left", str.Left(4));
            ClassicAssert.AreEqual("Middle", str.Mid(4, 6));
            ClassicAssert.AreEqual("MiddleRight", str.Mid(4));
            ClassicAssert.AreEqual("Right", str.Right(5));
        }

        [Test]
        public void Test_Left_Mid_Right_When_Null()
        {
            string str = null;

            ClassicAssert.AreEqual(null, str.Left(4));
            ClassicAssert.AreEqual(null, str.Mid(4, 6));
            ClassicAssert.AreEqual(null, str.Mid(4));
            ClassicAssert.AreEqual(null, str.Right(5));
        }

        [Test]
        public void Test_Left_Mid_Right_When_Empty()
        {
            string str = "";

            ClassicAssert.AreEqual("", str.Left(4));
            ClassicAssert.AreEqual("", str.Mid(4, 6));
            ClassicAssert.AreEqual("", str.Mid(4));
            ClassicAssert.AreEqual("", str.Right(5));
        }

        [Test]
        public void Test_Left_Mid_Right_When_Shorter_Than_String()
        {
            string str = "string";

            ClassicAssert.AreEqual("stri", str.Left(4));
            ClassicAssert.AreEqual("ring", str.Mid(2, 6));
            ClassicAssert.AreEqual("ring", str.Mid(2));
            ClassicAssert.AreEqual("ing", str.Right(3));
        }

        [Test]
        public void Test_Left_Mid_Right_When_One_Character_Each()
        {
            // start is zero-based position

            string str = "LMR";

            ClassicAssert.AreEqual("L", str.Left(1));   // get 1
            ClassicAssert.AreEqual("M", str.Mid(1, 1));  // start at 1 (position 2) get 1
            ClassicAssert.AreEqual("R", str.Mid(2));    // start at 2 (position 3) get 1
            ClassicAssert.AreEqual("R", str.Right(1));  // get 1 from right end   
            ClassicAssert.AreEqual("MR", str.Right(2)); // get 2 from right end   

        }

        [Test]
        public void Test_ToLineArray_Basic()
        {
            string str = @" line1
                            line2
                            line3";
            string[] lineArray = str.ToLineArray();

            ClassicAssert.AreEqual(3, lineArray.Length);
            ClassicAssert.AreEqual("line1", lineArray[0].Trim());
            ClassicAssert.AreEqual("line2", lineArray[1].Trim());
            ClassicAssert.AreEqual("line3", lineArray[2].Trim());
        }


        [Test]
        public void Test_ApplyToAllLines_Basic1()
        {
            string str = $"line1\r\nline2\r\nline3";
            string expected = $"\t\txxxline1\r\n\t\txxxline2\r\n\t\txxxline3";

            ClassicAssert.AreEqual(expected, str.ForEachLine(a => "\t\t" + "xxx" + a));
        }

        [Test]
        public void Test_ApplyToAllLines_Basic2()
        {
            string str = $"line1\r\nline2\r\nline3";
            string expected = $"x_line1_x\r\nx_line2_x\r\nx_line3_x";

            ClassicAssert.AreEqual(expected, str.ForEachLine(a => "x_" + a + "_x"));
        }

        [Test]
        public void Test_ForEachLine_From_File_To_Remove_Region()
        {
            string pathRoot     = XtraExtensions.GetApplicationRoot();
            string sourcePath   = $@"{pathRoot}\SourceFiles\TestClass.cs";
            string writePath    = $@"{pathRoot}\TextFiles\TrimmedTestClass.cs";

            var fileContent     = new FileInfo(sourcePath)
                                      .ReadFile()
                                      .Replace("TestClass", "TrimmedTestClass");

            var lineArray       = fileContent.ToLineArray();  //
            var trimmedString   = fileContent.ForEachLine(a => a.Trim().StartsWith(false, "#region", "#endregion") ? null : a);

            trimmedString.RemoveExtraLines().WriteToFile(writePath, true);

            ClassicAssert.AreEqual(44, lineArray.Length);
            ClassicAssert.AreEqual(38, trimmedString.ToLineArray().Length);
        }

        [Test]
        public void Test_Format_With_One_Arg()
        {
            string example = "https://{0}.test.com/";

            string subDomain1 = "www";
            string subDomain2 = "admin";
            string subDomain3 = "shop";
            string domain = "test";
            string extension = "com";

            ClassicAssert.AreEqual("https://www.test.com/", example.Format(subDomain1));
            ClassicAssert.AreEqual("https://admin.test.com/", example.Format(subDomain2));
            ClassicAssert.AreEqual("https://shop.test.com/", example.Format(subDomain3));

            ClassicAssert.AreEqual("https://www.test.com/", "https://{0}.test.com/".Format(subDomain1));
            ClassicAssert.AreEqual("https://admin.test.com/", "https://{0}.test.com/".Format(subDomain2));
            ClassicAssert.AreEqual("https://shop.test.com/", "https://{0}.test.com/".Format(subDomain3));

            ClassicAssert.AreEqual("https://www.test.com/", "https://{0}.{1}.{2}/".Format(subDomain1, domain, extension));
        }

        [Test]
        public void Test_Format_With_Multiple_Args()
        {
            string example = "https://{0}.{1}.{2}/";
            string subDomain1 = "www";
            string domain = "test";
            string extension = "com";

            ClassicAssert.AreEqual("https://www.test.com/", example.Format(subDomain1, domain, extension));
            ClassicAssert.AreEqual("https://www.test.com/", "https://{0}.{1}.{2}/".Format(subDomain1, domain, extension));
        }

        [Test]
        public void Test_ReplaceLineReturns_Basic()
        {
            string example = @"line 1
            line 2
            line 3
            line 4
            line 5";

            ClassicAssert.AreEqual("line 1x            line 2x            line 3x            line 4x            line 5",
                            example.ReplaceLineReturns("x"));
        }

        [Test]
        public void Test_Bool_IfTrue_Basic()
        {
            var items = new List<Item>
            {
                 new() { ItemId = 1, ItemName = "cart"},
                 new() { ItemId = 2, ItemName = "box",
                     Stuff = ["pork","steak","fish"],
                 }
            };

            var first = items[0];
            var last = items[^1];

            ClassicAssert.AreEqual("cart", $"{first.ItemName}{first.HasStuff.IfTrue("*")}");
            ClassicAssert.AreEqual("cart has no stuff", $"{first.ItemName}{first.HasStuff.IfTrue(" has stuff", " has no stuff")}");

            ClassicAssert.AreEqual("box*", $"{last.ItemName}{last.HasStuff.IfTrue("*")}");
            ClassicAssert.AreEqual("box has stuff.", $"{last.ItemName}{last.HasStuff.IfTrue(" has stuff").AddEnd(".")}");

            ClassicAssert.AreEqual("box has stuff.", $"{last.ItemName}{(last.HasStuff ? " has stuff" : "").AddEnd(".")}");  // Comparable inline
        }

        [Test]
        public void Test_CombineSpaces_Basic()
        {
            string str = "This            is           a test.";

            ClassicAssert.AreEqual("This is a test.", str.CombineSpaces());
            ClassicAssert.AreEqual(15, str.CombineSpaces().Length);
        }

        [Test]
        public void Test_CombineSpaces_With_Line_Breaks()
        {
            string str = "This is      " + NewLine + NewLine + "     a test.";

            ClassicAssert.AreEqual("This is a test.", str.CombineSpaces());
            ClassicAssert.AreEqual(15, str.CombineSpaces().Length);
        }

        [Test]
        public void Test_CombineSpaces_With_Ignore_Returns()
        {
            string str = "This is      " + NewLine + "   " + NewLine + "     a test.";
            string combinedStr = str.CombineSpaces(ignoreReturns: true);

            ClassicAssert.AreEqual("This is " + NewLine + NewLine + "a test.", combinedStr);
            ClassicAssert.AreEqual(19, combinedStr.Length);
        }

        [Test]
        public void Test_Reverse_string()
        {
            string str = "The quick brown fox jumped over the lazy dog";
            string rev = "dog lazy the over jumped fox brown quick The";

            ClassicAssert.AreEqual(rev,    Example_Reverse_1(str));
            ClassicAssert.AreEqual("",     Example_Reverse_1(""));
            ClassicAssert.AreEqual(null,   Example_Reverse_1(null));
            ClassicAssert.AreEqual(rev,    Example_Reverse_2(str));
            ClassicAssert.AreEqual("",     Example_Reverse_2(""));
            ClassicAssert.AreEqual(null,   Example_Reverse_2(null));
            ClassicAssert.AreEqual(rev,    Example_Reverse_3(str));
            ClassicAssert.AreEqual("",     Example_Reverse_3(""));
            ClassicAssert.AreEqual(null,   Example_Reverse_3(null));
        }

        private string Example_Reverse_1(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var s = str.Split([' '], StringSplitOptions.RemoveEmptyEntries);
            Array.Reverse(s);

            return string.Join(' ', s);
        }

        private string Example_Reverse_2(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var s = str.Split(new char[] { ' ' }).Reverse();

            return string.Join(' ', s);
        }

        private string Example_Reverse_3(string str)
        {
            return str.IsNullOrEmpty() ? str : string.Join(' ', str.Split(new char[] { ' ' }).Reverse());
        }

        [Test]
        public void Test_Reverse_Letters_In_Each_Word()
        {
            string str = "quick brown fox jumped";
            string rev = "kciuq nworb xof depmuj";

            ClassicAssert.AreEqual(rev,    Example_ReverseLetters(str));
            ClassicAssert.AreEqual("",     Example_ReverseLetters(""));
            ClassicAssert.AreEqual(null,   Example_ReverseLetters(null));
        }

        private string Example_ReverseLetters(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var strArr = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] revArr = new string[strArr.Length];

            for (int i = 0; i < strArr.Length; i++)
            {
                string s = strArr[i];
                revArr[i] = new string(s.Reverse().ToArray());
            }
            return string.Join(' ', revArr);
        }

        [Test]
        public void Test_EqualsAny_With_Params_Array()
        {
            string str = "giraffe";

            ClassicAssert.AreEqual(false, str.EqualsAny());
            ClassicAssert.AreEqual(false, str.EqualsAny("lions", "tigers", "bears"));
            ClassicAssert.AreEqual(false, str.EqualsAny("GIRAFFE", "lions", "tigers", "bears"));
            ClassicAssert.AreEqual(true,  str.EqualsAny("giraffe", "lions", "tigers", "bears"));
        }

        [Test]
        public void Test_EqualsAny_With_Params_Array_With_Ignorecase()
        {
            string str = "giraffe";

            ClassicAssert.AreEqual(false, str.EqualsAny("lions", "tigers", "bears"));
            ClassicAssert.AreEqual(false, str.EqualsAny(false, "GIRAFFE", "Lions", "TIGERS", "bears"));
            ClassicAssert.AreEqual(true,  str.EqualsAny(true, "GIRAFFE", "Lions", "TIGERS", "bears"));
        }

        [Test]
        public void Test_EqualsAny_With_Array()
        {
            string str = "giraffe";
            string[] animalArray0 = { };
            string[] animalArray1 = { "lions", "tigers", "bears" };
            string[] animalArray2 = { "giraffe", "lions", "tigers", "bears" };
            string[] animalArray3 = { "GIRAFFE", "lions", "tigers", "bears" };

            ClassicAssert.AreEqual(false, str.EqualsAny(animalArray0));
            ClassicAssert.AreEqual(false, str.EqualsAny(animalArray1));
            ClassicAssert.AreEqual(true,  str.EqualsAny(animalArray2));
            ClassicAssert.AreEqual(false, str.EqualsAny(animalArray3));
            ClassicAssert.AreEqual(true,  str.EqualsAny(true, animalArray3));
        }

        [Test]
        public void Test_AsString_Basic()
        {
            string[] animals = { "lions", "tigers", "bears" };

            ClassicAssert.AreEqual("lions, tigers, bears", animals.AsString());
        }

        [Test]
        public void Test_String_Joins_With_Nulls()
        {
            string[] animals = { "lions", null, "tigers", null, "bears" };

            ClassicAssert.AreEqual("lions, , tigers, , bears", string.Join(", ", animals));
        }

        [Test]
        public void Test_AsString_Remove_Nulls()
        {
            // Removes null

            string[] animals = { "lions", null, "tigers", null, "bears" };

            ClassicAssert.AreEqual("lions, tigers, bears", animals.AsString());
        }

        [Test]
        public void Test_Use_StringBuilder_In_Linq_Aggregate_Overload()
        {
            string[] strings = { "lions", "tigers", "bears" };
            var aggregate = strings.Aggregate(new StringBuilder(), (sb, t) => sb.Append($"{t}*, ") )
                                   .ToString().RemoveEnd(", ");

            ClassicAssert.AreEqual("lions*, tigers*, bears*", aggregate); 
        }


        [Test]
        public void Test_RemoveExtraLines_Basic()
        {
            string text        = @"""
                                    Trim 


                                    this



                                    text.
                                 """;
            string trimmedText = @"""
                                    Trim 

                                    this

                                    text.
                                 """;

            ClassicAssert.AreEqual(trimmedText, text.RemoveExtraLines());
            ClassicAssert.AreEqual(trimmedText, text.RemoveExtraLines(1));
        }

        [Test]
        public void Test_RemoveExtraLines_Zero_Lines()
        {
            string        text = @"""
                                    Trim 


                                    this



                                    text.
                                 """;
            string trimmedText = @"""
                                    Trim 
                                    this
                                    text.
                                 """;

            ClassicAssert.AreEqual(trimmedText, text.RemoveExtraLines(0));
        }

        [Test]
        public void Test_RemoveExtraLines_Disable()
        {
            string text         = @"""
                                    Trim 

                                    this

                                    text.
                                 """;
            string trimmedText  = @"""
                                    Trim 

                                    this

                                    text.
                                 """;

            ClassicAssert.AreEqual(trimmedText, text.RemoveExtraLines(-1));
        }


        [Test]
        public void Test_String_Split()
        {
            string text = "  ; https://www.google.com;https://www.yahoo.com; https://www.willcrowther.com ";
            var strArray = text.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries  );
            
            // intial empty string is removed

            ClassicAssert.AreEqual(3, strArray.Length);
            ClassicAssert.AreEqual("https://www.google.com", strArray[0]);
            ClassicAssert.AreEqual("https://www.yahoo.com", strArray[1]);
            ClassicAssert.AreEqual("https://www.willcrowther.com", strArray[2]);
        }

        [Test]
        public void Test_String_Split_Alternate()
        {
            string text = "9811456789,    ";
            var strArray = text.Split(",", true);

			ClassicAssert.AreEqual(1, strArray.Length);
            ClassicAssert.AreEqual("9811456789", strArray[0]);
        }

		[Test]
		public void Test_String_Split_Bool()
		{
			string text = "  ; https://www.google.com;https://www.yahoo.com; https://www.willcrowther.com ";
			var strArray = text.Split(";", true, true);

			// intial empty string is removed

			ClassicAssert.AreEqual(3, strArray.Length);
			ClassicAssert.AreEqual("https://www.google.com", strArray[0]);
			ClassicAssert.AreEqual("https://www.yahoo.com", strArray[1]);
			ClassicAssert.AreEqual("https://www.willcrowther.com", strArray[2]);
		}

		[Test]
		public void Test_String_Split_Alternate_Bool()
		{
			string text = "9811456789,    ";
			var strArray = text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

			ClassicAssert.AreEqual(1, strArray.Length);
			ClassicAssert.AreEqual("9811456789", strArray[0]);
		}

		[Test]
		public void Test_Contains()
		{
			string text = "Acme East Systems";
			bool contains = text.Contains("East");

			ClassicAssert.IsTrue(contains);
		}

		// [Test]
		// public void Test_String_Get_Last_20_Charaters()
		// {
		// 	string text = "123456789012345678901234567890A234567890B23456789C";

		// 	ClassicAssert.AreEqual("A234567890B23456789C", text[^20..]);
		// 	// Will error if text is less than 20 characters	

		// 	ClassicAssert.AreEqual("A234567890B23456789C", text.Substring(text.Length - 100));
		// }

	}
}