using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WildHare.Extensions;

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
        public void Test_RemoveStartFromAllLines_Basic()
        {
            string multiLineText = @"This is a 
			sentence 
			spread across multiple lines.";

            string indentingRemovedText = multiLineText.RemoveStartFromAllLines("\t\t\t");

            Assert.AreEqual("This is a \r\nsentence \r\nspread across multiple lines.", indentingRemovedText);
        }

        [Test]
        public void Test_RemoveStartFromAllLines_Using_String_Array()
        {
            string multiLineText = @"This is a 
            sentence 
            spread across 
            multiple lines.";

            string indentingRemovedText = multiLineText.RemoveStartFromAllLines(new[] { "\t\t\t", "            " });

            Assert.AreEqual("This is a \nsentence \nspread across \nmultiple lines.", indentingRemovedText);
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
        public void Test_EnsureEnd_If_String_Does_Not_Start_With_AddToStart()
        {
            string str = "Test";
            string strToAdd = "xxx";
            string result = str.EnsureEnd(strToAdd);

            Assert.AreEqual("Testxxx", result);
        }

        [Test]
        public void Test_EnsureEnd_If_String_Starts_With_AddToStart()
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
            string start = str.GetStart("_");

            Assert.AreEqual("Begin", start);
        }

        [Test]
        public void Test_GetStart_Basic_IncludeSeparator()
        {
            string str = "Begin_Finish";
            string start = str.GetStart("_", true);

            Assert.AreEqual("Begin_", start);
        }

        [Test]
        public void Test_GetStart_No_Separator()
        {
            string str = "Begin_Finish";
            string start = str.GetStart("x");

            Assert.IsNull(start);
        }

        [Test]
        public void Test_GetStart_Null()
        {
            string str = null;
            string start = str.GetStart("_");

            Assert.IsNull(start);
        }

        [Test]
        public void Test_GetEnd_Basic()
        {
            string str = "Begin_Finish";
            string end = str.GetEnd("_");

            Assert.AreEqual("Finish", end);
        }

        [Test]
        public void Test_GetEnd_Basic_IncludeSeparator()
        {
            string str = "Begin_Finish";
            string end = str.GetEnd("_", true);

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
        public void Test_Parse()
        {
            string str = "Get.Parse.The.Contents.Of.The.(Inner)Parenthesis";
            int startIndex = str.IndexOf('(');
            int endIndex = str.IndexOf(')');

            int length = endIndex - startIndex;
            string result = str.Substring(startIndex + 1, length - 1);

            Assert.AreEqual("Inner", result);
        }

        [Test]
        public void Test__Replace_With_Two_Array_Overload()
        {
            string str = "Favorite animals: cat dog rabbit.";
            string[] old = { "cat", "dog", "rabbit" };
            var result = str.Replace(old, new[]{ "platypus", "cheetah", "emu" });

            Assert.AreEqual("Favorite animals: platypus cheetah emu.", result);
        }


        [Test]
        public void Test_Dictionary_Aggregate_Replace()
        {
            string str = "Favorite animals: cat dog rabbit.";
            var dict = new Dictionary<string, string>{ { "cat", "platypus" }, {"dog", "cheetah"},{"rabbit", "emu" } };
            var result = dict.Aggregate(str, (current, value) => current.Replace(value.Key, value.Value));

            Assert.AreEqual("Favorite animals: platypus cheetah emu.", result);
        }
    }
}
