using NUnit.Framework;
using System;
using System.IO;
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
    }
}
