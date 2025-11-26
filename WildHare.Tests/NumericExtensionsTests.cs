using NUnit.Framework;
using NUnit.Framework.Legacy;
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

            ClassicAssert.IsTrue  ( four.EqualsAny(numberSet1) );
            ClassicAssert.IsFalse ( four.EqualsAny(numberSet2) );
            ClassicAssert.IsTrue  ( four.EqualsAny(numberSet3) );
            ClassicAssert.IsFalse ( four.EqualsAny(numberSet4) );
			ClassicAssert.IsTrue  ( four.EqualsAny([4, 100])   );

            var ex = Assert.Throws<ArgumentNullException>
            (
                () => four.EqualsAny(numberSet5)
            );

			ClassicAssert.AreEqual("Value cannot be null. (Parameter 'source')", ex.Message);
		}

		[Test]
        public void Test_EqualsAny_With_Params()
        {
            int four                = 4;
            decimal onePointFive    = 1.5m;
            bool trueVal            = true;
            
            ClassicAssert.IsTrue  ( four.EqualsAny(1,2,3,4) );
            ClassicAssert.IsFalse ( four.EqualsAny(0,-1,12) );
            ClassicAssert.IsTrue  ( onePointFive.EqualsAny(1.4m, 1.5m, 1.6m) );
            ClassicAssert.IsFalse ( onePointFive.EqualsAny(0m) );
            ClassicAssert.IsTrue  ( trueVal.EqualsAny(false, false, true) );
            ClassicAssert.IsFalse ( trueVal.EqualsAny(false, false, false) );
			ClassicAssert.IsFalse ( four.EqualsAny() );
        }
    }
}
