using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using WildHare.Extensions;

namespace WildHare.Tests
{
    [TestFixture]
    public class ConvertExtensionsTests
    {
        [Test]
        public void Test_ToBool_Basic()
        {
            string boolStringTrue = "true";
            ClassicAssert.AreEqual(true, boolStringTrue.ToBool());

            string boolStringFalse = "false";
            ClassicAssert.AreEqual(false, boolStringFalse.ToBool());

            string boolStringFred = "Fred";
            ClassicAssert.AreEqual(false, boolStringFred.ToBool());

            string boolStringNull = null;
            ClassicAssert.AreEqual(false, boolStringNull.ToBool());
        }

        [Test]
        public void Test_ToBoolNullable_Basic()
        {
            string boolStringTrue = "true";
            ClassicAssert.AreEqual(true, boolStringTrue.ToBoolNullable());

            string boolStringFalse = "false";
            ClassicAssert.AreEqual(false, boolStringFalse.ToBoolNullable());

            string boolStringFred = "Fred";
            ClassicAssert.AreEqual(null, boolStringFred.ToBoolNullable());

            string boolStringNull = null;
            ClassicAssert.AreEqual(null, boolStringNull.ToBoolNullable());
            ClassicAssert.AreEqual(true, boolStringNull.ToBoolNullable(true));
        }

        [Test]
        public void Test_ToBool_With_trueValue()
        {
            string boolStringTrue = "Yes";
            ClassicAssert.AreEqual(true, boolStringTrue.ToBool("Yes"));

            string boolStringFalse = "No";
            ClassicAssert.AreEqual(false, boolStringFalse.ToBool("Yes"));

            string boolStringFred = "Fred";
            ClassicAssert.AreEqual(false, boolStringFred.ToBool("Yes"));

            string boolStringNull = null;
            ClassicAssert.AreEqual(false, boolStringNull.ToBool("Yes"));
            ClassicAssert.AreEqual(false, boolStringNull.ToBool("No"));

            string boolStringTrueLowercase = "yes";
            ClassicAssert.AreEqual(true, boolStringTrueLowercase.ToBool("Yes", true));

            string boolStringFalseLowercase = "yes";
            ClassicAssert.AreEqual(false, boolStringFalseLowercase.ToBool("Yes", false));
        }


        [Test]
        public void Test_IncrementString_Without_Number_And_Extension()
        {
            string file = "file";

            string newStr = file.IncrementString();
            ClassicAssert.AreEqual("file1", newStr);

            newStr = newStr.IncrementString();
            ClassicAssert.AreEqual("file2", newStr);

            newStr = newStr.IncrementString();
            ClassicAssert.AreEqual("file3", newStr);
        }

        [Test]
        public void Test_IncrementString_Without_Number_But_With_Extension()
        {
            string file = "file.txt";

            string newStr = file.IncrementString(1, ".txt");
            ClassicAssert.AreEqual("file1.txt", newStr);

            newStr = newStr.IncrementString(null, ".txt");
            ClassicAssert.AreEqual("file2.txt", newStr);

            newStr = newStr.IncrementString(ignoreExtension: ".txt");
            ClassicAssert.AreEqual("file3.txt", newStr);
        }

        [Test]
        public void Test_IncrementString_With_Number_And_Extension()
        {
            string file = "file1.txt";

            string newStr = file.IncrementString(ignoreExtension: ".txt");
            ClassicAssert.AreEqual("file2.txt", newStr);

            newStr = newStr.IncrementString(ignoreExtension: ".txt");
            ClassicAssert.AreEqual("file3.txt", newStr);

            newStr = newStr.IncrementString(ignoreExtension: ".txt");
            ClassicAssert.AreEqual("file4.txt", newStr);
        }

        [Test]
        public void Test_ToIntArray_Not_Strict()
        {

            string intString = "-1, 0,1,02,3,sss4,,6,Seven,8, 9 , 10";
            var array = intString.ToIntArray();

            // ignores letters in "sss4" to make it 4, ignores empty entry and 'Seven'
            ClassicAssert.AreEqual(10, array.Length);
        }

        [Test]
        public void Test_ToIntArray_Not_Strict_With_Space_Separator()
        {
            string intString = "-1 12 abc 6    8";

            var array = intString.ToIntArray(separator: " ");

            ClassicAssert.AreEqual(4, array.Length);
            ClassicAssert.AreEqual(-1, array[0]);
            ClassicAssert.AreEqual(12, array[1]);
            ClassicAssert.AreEqual(6, array[2]);
            ClassicAssert.AreEqual(8, array[3]);
        }

        [Test]
        public void Test_ToIntArray_With_Default_Comma_Separator_Using_Period_Separator()
        {
            // Because default separator is "," the "." characters are removed and the number
            // 72002012000 overflows int and returns null so no elements returned

            string intString = "7.200.201.2000"; 

            var array = intString.ToIntArray();

            ClassicAssert.AreEqual(0, array.Length);
        }

        [Test]
        public void Test_ToIntArray_With_Default_Comma_Separator_Using_Period_Separator_Strict()
        {
            // Because default separator is "," and not ".", the "." characters are removed
            // and the number 72002012000 overflows max value of int and throws an Exception

            string intString = "7.200.201.2000";

            var ex = Assert.Throws<Exception>
            (
                () => intString.ToIntArray(true)
            );

            string errorMessage = "ToIntArray() cannot have null or invalid values when in strict mode.";
            ClassicAssert.AreEqual(errorMessage, ex.Message);
        }
    }
}
