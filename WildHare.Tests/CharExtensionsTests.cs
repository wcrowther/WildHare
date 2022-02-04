using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using static System.Environment;

namespace WildHare.Tests
{
    [TestFixture]
    public class CharExtensionsTests
    {
		[Test]
		public void Test_Char_IsNumber()
		{
            char numberChar = 1234.ToString().ToCharArray()[0];  // char '1'
            char letterChar = "Fred".ToCharArray()[0];  // char 'F'

            Assert.IsTrue(numberChar.IsNumber());
            Assert.IsFalse(numberChar.IsLetter());

            Assert.IsTrue(letterChar.IsLetter());
            Assert.IsFalse(letterChar.IsNumber());
        }

        [Test]
        public void Test_Char_IsWhiteSpace_Basic()
        {
            string str = " \n \r";

            Assert.AreEqual(4, str.ToCharArray().Length);

            Assert.IsTrue(str[0].IsWhiteSpace());
            Assert.IsTrue(str[1].IsWhiteSpace());
            Assert.IsTrue(str[2].IsWhiteSpace());
            Assert.IsTrue(str[3].IsWhiteSpace());
        }

        [Test]
        public void Test_Char_IsReturn_Basic()
        {
            string str = " \n \r";

            Assert.AreEqual(4, str.ToCharArray().Length);

            Assert.IsFalse(str[0].IsReturn());
            Assert.IsTrue (str[1].IsReturn());
            Assert.IsFalse(str[2].IsReturn());
            Assert.IsTrue (str[3].IsReturn());
        }

        [Test]
        public void Test_Char_IsReturn_With_Newline()
        {
            string str = " " + NewLine;

            Assert.AreEqual(3, str.ToCharArray().Length);

            Assert.IsTrue (str[0].IsWhiteSpace());
            Assert.IsFalse(str[0].IsReturn());
            Assert.IsTrue (str[1].IsWhiteSpace());
            Assert.IsTrue (str[1].IsReturn());
            Assert.IsTrue (str[2].IsWhiteSpace());
            Assert.IsTrue (str[2].IsReturn());
        }
    }
}
