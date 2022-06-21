using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using WildHare.Tests.Models;
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

            Assert.AreEqual("123456", numbersOnlyString);
        }

        [Test]
        public void Test_NumbersOnly_With_Other_Characters()
        {
            string numbersAndWords = "$12345.SomeWord00";
            string numbersOnlyString = numbersAndWords.NumbersOnly("$.");

            Assert.AreEqual("$12345.00", numbersOnlyString);
        }

        [Test]
        public void Test_NumbersAndLettersOnly_With_Other_Characters()
        {
            string str = "<*will22@wildhare.com*/>";
            string filteredStr = str.NumbersAndLettersOnly("@.");

            Assert.AreEqual("will22@wildhare.com", filteredStr);
        }

        [Test]
        public void Test_LettersOnly_With_Other_Characters()
        {
            string str = "<*123will@wildhare.com*/>";
            string filteredStr = str.LettersOnly("@.");

            Assert.AreEqual("will@wildhare.com", filteredStr);
        }

        [Test]
        public void Test_Truncate_Basic()
        {
            string str = "12345678901234567890123456789012345678901234567890";
            string result = str.Truncate(10);

            Assert.AreEqual("1234567890...", result);
        }

        [Test]
        public void Test_Truncate_Custom_More()
        {
            string str = "12345678901234567890123456789012345678901234567890";
            string result = str.Truncate(10, "...(truncated)");

            Assert.AreEqual("1234567890...(truncated)", result);
        }

        [Test]
        public void Test_Truncate_Exact()
        {
            string str = "12345678901234567890123456789012345678901234567890";
            string result = str.Truncate(10, "", 0);

            Assert.AreEqual("1234567890", result);
        }

        [Test]
        public void Test_Truncate_Small_WordCut()
        {
            string str = "12345 67890 1234567890";
            string result = str.Truncate(10);

            Assert.AreEqual("12345...", result);
        }

        [Test]
        public void Test_Truncate_Small_WordCut_Case2()
        {
            string str = "12345 67890 123456789012345";
            string result = str.Truncate(12);

            Assert.AreEqual("12345 67890...", result);
        }

        [Test]
        public void Test_Truncate_Small_WordCut_Case3()
        {
            string str = "12345 78901234567890123456789";
            string result = str.Truncate(20, "...", 12);

            Assert.AreEqual("12345 78901234567890...", result);
        }

        [Test]
        public void Test_RemoveStartFromAllLines_Basic()
        {
            string multiLineText = @"This is a 
			sentence 
			spread across multiple lines.";

            string indentingRemovedText = multiLineText.RemoveStartFromAllLines("\t\t\t");

            Assert.AreEqual($"This is a {NewLine}sentence {NewLine}spread across multiple lines.", indentingRemovedText);
        }

        [Test]
        public void Test_RemoveIndents_Basic()
        {
            string multiLineText = @"This is a 
			        sentence 
			        spread across multiple lines.";

            string indentingRemovedText = multiLineText.RemoveIndents();

            Assert.AreEqual($"This is a {NewLine}sentence {NewLine}spread across multiple lines.", indentingRemovedText);
        }

        [Test]
        public void Test_RemoveIndents_Basic2()
        {
            string multiLineText = @"
                    This is a 
                    sentence 
                    spread across multiple lines.";

            string indentingRemovedText = multiLineText.RemoveIndents();

            Assert.AreEqual($"This is a {NewLine}sentence {NewLine}spread across multiple lines.", indentingRemovedText);
        }

        [Test]
        public void Test_RemoveIndents_Basic_Keep_First_Line_Spaces()
        {
            string multiLineText = @"
                    This is a 
                    sentence 
                    spread across multiple lines.";

            string indentingRemovedText = multiLineText.RemoveIndents(false);

            Assert.AreEqual($"{NewLine}This is a {NewLine}sentence {NewLine}spread across multiple lines.", indentingRemovedText);
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

            Assert.AreEqual($"This is a{NewLine}sentence {NewLine}spread across{NewLine}multiple lines.{NewLine}", indentingRemovedText);
        }

        [Test]
        public void Test_IsWhiteSpace_Characters()
        {
            char newlineChar = '\n';
            char tabChar = '\t';
            char returnChar = '\r';

            Assert.IsTrue(char.IsWhiteSpace(newlineChar));
            Assert.IsTrue(char.IsWhiteSpace(tabChar));
            Assert.IsTrue(char.IsWhiteSpace(returnChar));
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

            Assert.AreEqual(multiLineText, string1);
            Assert.AreNotEqual(multiLineText, string2);
            Assert.AreNotEqual(string1, string2);
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

            Assert.AreEqual(expected, indentingRemovedText);
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

            Assert.AreEqual(expected, sbText);
        }

        [Test]
        public void Test_String_Repeat_Times_Ten()
        {
            string stringRepeated = "x".Repeat(10);

            Assert.AreEqual("xxxxxxxxxx", stringRepeated);

            string stringRepeatedTwice = stringRepeated.Repeat(3);

            Assert.AreEqual("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", stringRepeatedTwice);
        }

        [Test]
        public void Test_String_Repeat_With_Null()
        {
            string nullString = null;
            string stringRepeated = nullString.Repeat(10);

            Assert.AreEqual(null, stringRepeated);
        }

        [Test]
        public void Test_String_Repeat_With_Negative_Number()
        {
            string emptyString = "empty";
            string stringRepeated = emptyString.Repeat(-10);

            Assert.IsNull(stringRepeated);
        }

        [Test]
        public void Test_AddStart_IfEmpty()
        {
            string str = "";
            string strToAdd = "xxx";
            string result = str.AddStart(strToAdd);

            Assert.AreEqual("", result);
        }

        [Test]
        public void Test_AddStart_IfNull()
        {
            string str = null;
            string strToAdd = "xxx";
            string result = str.AddStart(strToAdd);

            Assert.IsNull(result);
        }

        [Test]
        public void Test_AddStart_If_Populated_String()
        {
            string str = "Test";
            string strToAdd = "xxx";
            string result = str.AddStart(strToAdd);

            Assert.AreEqual("xxxTest", result);
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

            Assert.AreEqual(expected, urlWithQuerystring);
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

            Assert.AreEqual(expected, url + querystring.RemoveStart("&").AddStart("?"));
        }


        [Test]
        public void Test_EnsureStart_IfEmpty()
        {
            string str = "";
            string strToAdd = "xxx";
            string result = str.EnsureStart(strToAdd);

            Assert.AreEqual("xxx", result);
        }

        [Test]
        public void Test_EnsureStart_IfNull()
        {
            string str = null;
            string strToAdd = "xxx";
            string result = str.EnsureStart(strToAdd);

            Assert.IsNull(result);
        }

        [Test]
        public void Test_EnsureStart_If_String_Does_Not_Start_With_AddToStart()
        {
            string str = "Test";
            string strToAdd = "xxx";
            string result = str.EnsureStart(strToAdd);

            Assert.AreEqual("xxxTest", result);
        }

        [Test]
        public void Test_EnsureStart_If_String_Starts_With_AddToStart()
        {
            string str = "xxxTest";
            string strToAdd = "xxx";
            string result = str.EnsureStart(strToAdd);

            Assert.AreEqual("xxxTest", result);
        }

        //========================

        [Test]
        public void Test_EnsureEnd_IfEmpty()
        {
            string str = "";
            string strToAdd = "xxx";
            string result = str.EnsureEnd(strToAdd);

            Assert.AreEqual("xxx", result);
        }

        [Test]
        public void Test_EnsureEnd_IfNull()
        {
            string str = null;
            string strToAdd = "xxx";
            string result = str.EnsureEnd(strToAdd);

            Assert.IsNull(result);
        }

        [Test]
        public void Test_EnsureEnd_If_String_Does_Not_End_With_StrToAdd()
        {
            string str = "Test";
            string strToAdd = "xxx";
            string result = str.EnsureEnd(strToAdd);

            Assert.AreEqual("Testxxx", result);
        }

        [Test]
        public void Test_EnsureEnd_If_String_End_With_StrToAdd()
        {
            string str = "Testxxx";
            string strToAdd = "xxx";
            string result = str.EnsureEnd(strToAdd);

            Assert.AreEqual("Testxxx", result);
        }

        //========================

        [Test]
        public void Test_EnsureStartEnd_IfEmpty()
        {
            string str = "";
            string addToStart = "xxx";
            string addToEnd = "xxx";
            string result = str.EnsureStartEnd(addToStart, addToEnd);

            Assert.AreEqual("xxxxxx", result);
        }

        [Test]
        public void Test_EnsureStartEnd_IfNull()
        {
            string str = null;
            string addToStart = "xxx";
            string addToEnd = "xxx";
            string result = str.EnsureStartEnd(addToStart, addToEnd);

            Assert.IsNull(result);
        }

        [Test]
        public void Test_EnsureStartEnd_If_String_Does_Not_Start_With_AddToStart_Or_AddToEnd()
        {
            string str = "Test";
            string addToStart = "xxx";
            string addToEnd = "xxx";
            string result = str.EnsureStartEnd(addToStart, addToEnd);

            Assert.AreEqual("xxxTestxxx", result);
        }

        [Test]
        public void Test_EnsureStartEnd_If_String_Starts_With_AddToStart_And_AddToEnd()
        {
            string str = "xxxTestxxx";
            string addToStart = "xxx";
            string addToEnd = "xxx";
            string result = str.EnsureStartEnd(addToStart, addToEnd);

            Assert.AreEqual("xxxTestxxx", result);
        }

        [Test]
        public void Test_EnsureStartEnd_If_String_Starts_With_AddToStart_And_AddToEnd_IsNull()
        {
            string str = "xxxTestxxx";
            string addToStart = "xxx";
            string result = str.EnsureStartEnd(addToStart);

            Assert.AreEqual("xxxTestxxx", result);
        }

        [Test]
        public void Test_GetStart_Basic()
        {
            string str = "Begin_Finish";
            string start = str.GetStartBefore("_");

            Assert.AreEqual("Begin", start);
        }

        [Test]
        public void Test_GetStart_Basic_IncludeSeparator()
        {
            string str = "Begin_Finish";
            string start = str.GetStartBefore("_", true);

            Assert.AreEqual("Begin_", start);
        }

        [Test]
        public void Test_GetStart_No_Separator()
        {
            string str = "Begin_Finish";
            string start = str.GetStartBefore("x");

            Assert.AreEqual(str, start);
        }

        [Test]
        public void Test_GetStart_Null()
        {
            string str = null;
            string start = str.GetStartBefore("_");

            Assert.IsNull(start);
        }

        [Test]
        public void Test_GetEnd_Basic()
        {
            string str = "Begin_Finish";
            string end = str.GetEndAfter("_");

            Assert.AreEqual("Finish", end);
        }

        [Test]
        public void Test_GetEnd_Basic_IncludeSeparator()
        {
            string str = "Begin_Finish";
            string end = str.GetEndAfter("_", true);

            Assert.AreEqual("_Finish", end);
        }

        [Test]
        public void Test_StartsWith_Array_Overload_If_Start_Is_True()
        {
            string str = "StartString";
            string[] values = new string[] { "A", "Start" };
            bool result = str.StartsWith(values);

            Assert.IsTrue(result);
        }

        [Test]
        public void Test_StartsWith_Array_Overload_If_Inline_Start_Is_True()
        {
            string str = "StartString";
            bool result = str.StartsWith(new string[] { "Start" });

            Assert.IsTrue(result);
        }

        [Test]
        public void Test_StartsWith_Array_Overload_If_Shortcut_Inline_Start_Is_True()
        {
            string str1 = "Start1";
            string str2 = "Start2";
            string str3 = "Start3";

            bool result1 = str1.StartsWith(new[] { "Start1", "Start2" });
            bool result2 = str2.StartsWith(new[] { "Start1", "Start2" });
            bool result3 = str3.StartsWith(new[] { "Start1", "Start2" });

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
        }


        [Test]
        public void Test_StartsWith_Array_Overload_With_String_Array_Start_Is_True()
        {
            string str = "StartString";
            string[] values = { "a", "Start" };
            bool result = str.StartsWith(values);

            Assert.IsTrue(result);
        }

        [Test]
        public void Test_StartsWith_Array_Overload_With_String_Array_Start_Is_False()
        {
            string str = "StartString";
            string[] values = { "a", "b" };
            bool result = str.StartsWith(values);

            Assert.IsFalse(result);
        }

        [Test]
        public void Test_EndsWith_Array_Overload_With_String_Array_End_Is_True()
        {
            string str = "StringEnd";
            string[] values = { "a", "End" };
            bool result = str.EndsWith(values);

            Assert.IsTrue(result);
        }

        [Test]
        public void Test_EndsWith_Array_Overload_With_String_Array_End_Is_False()
        {
            string str = "StringEnd";
            string[] values = { "a", "b" };
            bool result = str.EndsWith(values);

            Assert.IsFalse(result);
        }

        [Test]
        public void Test_Parse()
        {
            string str = "Parse.The.Contents.Of.The.(Inner)Parenthesis";
            int startIndex = str.IndexOf('(');
            int endIndex = str.IndexOf(')');

            int length = endIndex - startIndex;
            string result = str.Substring(startIndex + 1, length - 1);

            Assert.AreEqual("Inner", result);
        }

        [Test]
        public void Test_Replace_With_Two_Array_Overload()
        {
            string str = "Favorite animals: cat dog rabbit.";
            string[] old = { "cat", "dog", "rabbit" };
            var result = str.Replace(old, new[] { "platypus", "cheetah", "emu" });

            Assert.AreEqual("Favorite animals: platypus cheetah emu.", result);
        }

        [Test]
        public void Test_Replace_With_Array_and_string_Overload()
        {
            string str = "Favorite animals: cat dog rabbit.";
            string[] oldArray = { "cat", "dog", "rabbit" };
            var result = str.Replace(oldArray, "dog");

            Assert.AreEqual("Favorite animals: dog dog dog.", result);
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

            Assert.AreEqual("Favorite animals: platypus cheetah emu.", result);
        }

        [Test]
        public void Test_Replace_Dictionary_Overload()
        {
            string str = "Favorite animals: cat dog rabbit.";
            var dictionary = GetAnimalList();

            Assert.AreEqual("Favorite animals: platypus cheetah emu.", str.Replace(dictionary));
        }

        [Test]
        public void Test_Replace_Dictionary_Overload_Reverse()
        {
            string str = "Favorite animals: platypus cheetah emu.";
            var dictionary = GetAnimalList();

            Assert.AreEqual("Favorite animals: cat dog rabbit.", str.Replace(dictionary, true));
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

            Assert.AreEqual(result, message);
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

            Assert.AreEqual(result, message);
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

            Assert.AreEqual(result, message);
        }

        [Test]
        public void Test_Left_Mid_Right_Basic()
        {
            string str = "LeftMiddleRight";

            Assert.AreEqual("Left", str.Left(4));
            Assert.AreEqual("Middle", str.Mid(4, 6));
            Assert.AreEqual("MiddleRight", str.Mid(4));
            Assert.AreEqual("Right", str.Right(5));
        }

        [Test]
        public void Test_Left_Mid_Right_When_Null()
        {
            string str = null;

            Assert.AreEqual(null, str.Left(4));
            Assert.AreEqual(null, str.Mid(4, 6));
            Assert.AreEqual(null, str.Mid(4));
            Assert.AreEqual(null, str.Right(5));
        }

        [Test]
        public void Test_Left_Mid_Right_When_Empty()
        {
            string str = "";

            Assert.AreEqual("", str.Left(4));
            Assert.AreEqual("", str.Mid(4, 6));
            Assert.AreEqual("", str.Mid(4));
            Assert.AreEqual("", str.Right(5));
        }

        [Test]
        public void Test_Left_Mid_Right_When_Shorter_Than_String()
        {
            string str = "string";

            Assert.AreEqual("stri", str.Left(4));
            Assert.AreEqual("ring", str.Mid(2, 6));
            Assert.AreEqual("ring", str.Mid(2));
            Assert.AreEqual("ing", str.Right(3));
        }

        [Test]
        public void Test_Left_Mid_Right_When_One_Character_Each()
        {
            // start is zero-based position

            string str = "LMR";

            Assert.AreEqual("L", str.Left(1));   // get 1
            Assert.AreEqual("M", str.Mid(1, 1));  // start at 1 (position 2) get 1
            Assert.AreEqual("R", str.Mid(2));    // start at 2 (position 3) get 1
            Assert.AreEqual("R", str.Right(1));  // get 1 from right end   
            Assert.AreEqual("MR", str.Right(2)); // get 2 from right end   

        }

        [Test]
        public void Test_ApplyToAllLines_Basic()
        {
            string str = $"line1\r\nline2\r\nline3";
            string expected = $"x_line1_x\r\nx_line2_x\r\nx_line3_x";

            Assert.AreEqual(expected, str.ForEachLine(a => "x_" + a + "_x"));
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

            Assert.AreEqual("https://www.test.com/", example.Format(subDomain1));
            Assert.AreEqual("https://admin.test.com/", example.Format(subDomain2));
            Assert.AreEqual("https://shop.test.com/", example.Format(subDomain3));

            Assert.AreEqual("https://www.test.com/", "https://{0}.test.com/".Format(subDomain1));
            Assert.AreEqual("https://admin.test.com/", "https://{0}.test.com/".Format(subDomain2));
            Assert.AreEqual("https://shop.test.com/", "https://{0}.test.com/".Format(subDomain3));

            Assert.AreEqual("https://www.test.com/", "https://{0}.{1}.{2}/".Format(subDomain1, domain, extension));
        }

        [Test]
        public void Test_Format_With_Multiple_Args()
        {
            string example = "https://{0}.{1}.{2}/";
            string subDomain1 = "www";
            string domain = "test";
            string extension = "com";

            Assert.AreEqual("https://www.test.com/", example.Format(subDomain1, domain, extension));
            Assert.AreEqual("https://www.test.com/", "https://{0}.{1}.{2}/".Format(subDomain1, domain, extension));
        }

        [Test]
        public void Test_ReplaceLineReturns_Basic()
        {
            string example = @"line 1
            line 2
            line 3
            line 4
            line 5";

            Assert.AreEqual("line 1x            line 2x            line 3x            line 4x            line 5",
                            example.ReplaceLineReturns("x"));
        }

        [Test]
        public void Test_Bool_IfTrue_Basic()
        {
            var items = new List<Item>
            {
                 new Item() { ItemId = 1, ItemName = "cart"},
                 new Item() { ItemId = 2, ItemName = "box",
                     Stuff = new[] { "pork","steak","fish" }.ToList(),
                 }
            };

            var first = items.First();
            var last = items.Last();

            Assert.AreEqual("cart", $"{first.ItemName}{first.HasStuff.IfTrue("*")}");
            Assert.AreEqual("cart has no stuff", $"{first.ItemName}{first.HasStuff.IfTrue(" has stuff", " has no stuff")}");

            Assert.AreEqual("box*", $"{last.ItemName}{last.HasStuff.IfTrue("*")}");
            Assert.AreEqual("box has stuff.", $"{last.ItemName}{last.HasStuff.IfTrue(" has stuff").AddEnd(".")}");

            Assert.AreEqual("box has stuff.", $"{last.ItemName}{(last.HasStuff ? " has stuff" : "").AddEnd(".")}");  // Comparable inline
        }

        [Test]
        public void Test_CombineSpaces_Basic()
        {
            string str = "This            is           a test.";

            Assert.AreEqual("This is a test.", str.CombineSpaces());
            Assert.AreEqual(15, str.CombineSpaces().Length);
        }

        [Test]
        public void Test_CombineSpaces_With_Line_Breaks()
        {
            string str = "This is      " + NewLine + NewLine + "     a test.";

            Assert.AreEqual("This is a test.", str.CombineSpaces());
            Assert.AreEqual(15, str.CombineSpaces().Length);
        }


        [Test]
        public void Test_CombineSpaces_With_Ignore_Returns()
        {
            string str = "This is      " + NewLine + "   " + NewLine + "     a test.";
            string combinedStr = str.CombineSpaces(ignoreReturns: true);

            Assert.AreEqual("This is " + NewLine + NewLine + "a test.", combinedStr);
            Assert.AreEqual(19, combinedStr.Length);
        }

        [Test]
        public void Test_Reverse_string()
        {
            string str = "The quick brown fox jumped over the lazy dog";
            string rev = "dog lazy the over jumped fox brown quick The";

            Assert.AreEqual(rev, Reverse(str));
            Assert.AreEqual("", Reverse(""));
            Assert.AreEqual(null, Reverse(null));
            Assert.AreEqual(rev, Reverse2(str));
            Assert.AreEqual("", Reverse2(""));
            Assert.AreEqual(null, Reverse2(null));
        }

        [Test]
        public void Test_Reverse_string3()
        {
            string str = "The quick brown fox jumped over the lazy dog";
            string rev = "dog lazy the over jumped fox brown quick The";

            Assert.AreEqual(rev, Reverse3(str));
            Assert.AreEqual("", Reverse3(""));
            Assert.AreEqual(null, Reverse3(null));
        }

        private string Reverse(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var s = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Array.Reverse(s);

            return string.Join(' ', s);
        }

        private string Reverse2(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var s = str.Split(new char[] { ' ' }).Reverse();

            return string.Join(' ', s);
        }

        private string Reverse3(string str)
        {
            return str.IsNullOrEmpty() ? str : string.Join(' ', str.Split(new char[] { ' ' }).Reverse());
        }

        [Test]
        public void Test_Reverse_Letters_In_Each_Word()
        {
            string str = "quick brown fox jumped";
            string rev = "kciuq nworb xof depmuj";

            Assert.AreEqual(rev, ReverseLetters(str));
            Assert.AreEqual("", ReverseLetters(""));
            Assert.AreEqual(null, ReverseLetters(null));
        }

        private string ReverseLetters(string str)
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
    }
}