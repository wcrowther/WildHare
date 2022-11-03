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
    public class NumericExtensionsTests
    {
		[Test]
		public void Test_EqualsAny_Basic()
		{
            int four = 4;

            int[] numberSet1 = { 1, 2, 3, 4 };
            int[] numberSet2 = { 0, -1, 12 };
            int[] numberSet3 = { 4, 4 };
            int[] numberSet4 = {  };
            int[] numberSet5 = null;

            Assert.IsTrue (four.EqualsAny(numberSet1));
            Assert.IsFalse(four.EqualsAny(numberSet2));
            Assert.IsTrue (four.EqualsAny(numberSet3));
            Assert.IsFalse(four.EqualsAny(numberSet4));
            Assert.IsTrue(four.EqualsAny(new int[]{4, 100}));

            Assert.Throws<ArgumentNullException>
            (
                () => four.EqualsAny(numberSet5)
            );
        }

        [Test]
        public void Test_EqualsAny_With_Params()
        {
            int four = 4;
            decimal onePointFive = 1.5m;
            bool trueVal = true;
            
            Assert.IsTrue (four.EqualsAny(1,2,3,4));
            Assert.IsFalse(four.EqualsAny(0,-1,12));
            Assert.IsTrue (onePointFive.EqualsAny(1.4m, 1.5m, 1.6m));
            Assert.IsFalse(onePointFive.EqualsAny(0m));
            Assert.IsTrue (trueVal.EqualsAny(false, false, true));
            Assert.IsFalse (trueVal.EqualsAny(false, false, false));

            Assert.IsFalse(four.EqualsAny<int>());

            // Assert.IsFalse(four.EqualsAny());
            // Does not compile as nothing to get the type of T from
            // Explicitly define T as <int> as in example above
        }
    }
}
