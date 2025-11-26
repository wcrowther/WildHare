using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using static System.Environment;
using NUnit.Framework.Legacy;

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

			ClassicAssert.IsTrue(numberChar.IsNumber());
			ClassicAssert.IsFalse(numberChar.IsLetter());

			ClassicAssert.IsTrue(letterChar.IsLetter());
            ClassicAssert.IsFalse(letterChar.IsNumber());
        }

        [Test]
        public void Test_Char_IsWhiteSpace_Basic()
        {
            string str = " \n \r";

            ClassicAssert.AreEqual(4, str.ToCharArray().Length);

            ClassicAssert.IsTrue(str[0].IsWhiteSpace());
            ClassicAssert.IsTrue(str[1].IsWhiteSpace());
            ClassicAssert.IsTrue(str[2].IsWhiteSpace());
			ClassicAssert.IsTrue(str[3].IsWhiteSpace());
        }

        [Test]
        public void Test_Char_IsReturn_Basic()
        {
            string str = " \n \r";

            ClassicAssert.AreEqual(4, str.ToCharArray().Length);

            ClassicAssert.IsFalse(str[0].IsReturn());
            ClassicAssert.IsTrue (str[1].IsReturn());
            ClassicAssert.IsFalse(str[2].IsReturn());
            ClassicAssert.IsTrue (str[3].IsReturn());
        }

        [Test]
        public void Test_Char_IsReturn_With_Newline()
        {
            string str = " " + NewLine;

            ClassicAssert.AreEqual(3, str.ToCharArray().Length);

            ClassicAssert.IsTrue (str[0].IsWhiteSpace());
            ClassicAssert.IsFalse(str[0].IsReturn());
            ClassicAssert.IsTrue (str[1].IsWhiteSpace());
            ClassicAssert.IsTrue (str[1].IsReturn());
            ClassicAssert.IsTrue (str[2].IsWhiteSpace());
            ClassicAssert.IsTrue (str[2].IsReturn());
        }
    }
}
