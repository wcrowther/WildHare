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
            Assert.IsFalse(letterChar.IsNumber());
		}

    }
}