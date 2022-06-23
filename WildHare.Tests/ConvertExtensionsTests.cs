using NUnit.Framework;
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
            Assert.AreEqual(true, boolStringTrue.ToBool());

            string boolStringFalse = "false";
            Assert.AreEqual(false, boolStringFalse.ToBool());

            string boolStringFred = "Fred";
            Assert.AreEqual(false, boolStringFred.ToBool());

            string boolStringNull = null;
            Assert.AreEqual(false, boolStringNull.ToBool());
        }

        [Test]
        public void Test_ToBoolNullable_Basic()
        {
            string boolStringTrue = "true";
            Assert.AreEqual(true, boolStringTrue.ToBoolNullable());

            string boolStringFalse = "false";
            Assert.AreEqual(false, boolStringFalse.ToBoolNullable());

            string boolStringFred = "Fred";
            Assert.AreEqual(null, boolStringFred.ToBoolNullable());

            string boolStringNull = null;
            Assert.AreEqual(null, boolStringNull.ToBoolNullable());
            Assert.AreEqual(true, boolStringNull.ToBoolNullable(true));
        }

        [Test]
        public void Test_ToBool_With_trueValue()
        {
            string boolStringTrue = "Yes";
            Assert.AreEqual(true, boolStringTrue.ToBool("Yes"));

            string boolStringFalse = "No";
            Assert.AreEqual(false, boolStringFalse.ToBool("Yes"));

            string boolStringFred = "Fred";
            Assert.AreEqual(false, boolStringFred.ToBool("Yes"));

            string boolStringNull = null;
            Assert.AreEqual(false, boolStringNull.ToBool("Yes"));
            Assert.AreEqual(false, boolStringNull.ToBool("No"));

            string boolStringTrueLowercase = "yes";
            Assert.AreEqual(true, boolStringTrueLowercase.ToBool("Yes", true));

            string boolStringFalseLowercase = "yes";
            Assert.AreEqual(false, boolStringFalseLowercase.ToBool("Yes", false));
        }


        [Test]
        public void Test_IncrementString_Without_Number_And_Extension()
        {
            string file = "file";

            string newStr = file.IncrementString();
            Assert.AreEqual("file1", newStr);

            newStr = newStr.IncrementString();
            Assert.AreEqual("file2", newStr);

            newStr = newStr.IncrementString();
            Assert.AreEqual("file3", newStr);
        }

        [Test]
        public void Test_IncrementString_Without_Number_But_With_Extension()
        {
            string file = "file.txt";

            string newStr = file.IncrementString(1, ".txt");
            Assert.AreEqual("file1.txt", newStr);

            newStr = newStr.IncrementString(null, ".txt");
            Assert.AreEqual("file2.txt", newStr);

            newStr = newStr.IncrementString(ignoreExtension: ".txt");
            Assert.AreEqual("file3.txt", newStr);
        }

        [Test]
        public void Test_IncrementString_With_Number_And_Extension()
        {
            string file = "file1.txt";

            string newStr = file.IncrementString(ignoreExtension: ".txt");
            Assert.AreEqual("file2.txt", newStr);

            newStr = newStr.IncrementString(ignoreExtension: ".txt");
            Assert.AreEqual("file3.txt", newStr);

            newStr = newStr.IncrementString(ignoreExtension: ".txt");
            Assert.AreEqual("file4.txt", newStr);
        }

        [Test]
        public void Test_ToIntArray_Not_Strict()
        {
            string intString = "-1, 0,1,02,3,sss4,,6,Seven,8, 9 , 10";

            var array = intString.ToIntArray();
            Assert.AreEqual(10, array.Length);
        }

        [Test]
        public void Test_ToIntArray_Not_Strict_With_Space_Separator()
        {
            string intString = "-1 12 abc 6    8";

            var array = intString.ToIntArray(separator: " ");

            Assert.AreEqual(4, array.Length);
            Assert.AreEqual(-1, array[0]);
            Assert.AreEqual(12, array[1]);
            Assert.AreEqual(6, array[2]);
            Assert.AreEqual(8, array[3]);
        }

        [Test]
        public void Test_ToIntArray_With_Default_Comma_Separator_Using_Period_Separator()
        {
            // Because default separator is "," the "." characters are removed and the number
            // 72002012000 overflows int and returns null so no elements returned

            string intString = "7.200.201.2000"; 

            var array = intString.ToIntArray();

            Assert.AreEqual(0, array.Length);
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
            Assert.AreEqual(errorMessage, ex.Message);
        }

        [Test]
        public void Test_ToIntArray_Strict_Empty_Values()
        {
            string intString = "1,2,3,,5";

            var ex = Assert.Throws<Exception>
            (
                () => intString.ToIntArray(true)
            );

            string errorMessage = "ToIntArray() cannot have null or invalid values when in strict mode.";
            Assert.AreEqual(errorMessage, ex.Message);
        }


        [Test]
        public void Test_ToIntArray_Strict_Invalid_Values()
        {
            string intString = "sss4,Seven";

            var ex = Assert.Throws<Exception>
            (
                () => intString.ToIntArray(true)
            );

            string errorMessage = "ToIntArray() cannot have null or invalid values when in strict mode.";
            Assert.AreEqual(errorMessage, ex.Message);
        }


        [Test]
        public void Test_IntArray_AsString_Basic()
        {
            int[] intArray = { 1,2,3,4,9 };

            string intArrayString = intArray.AsString();  

            Assert.AreEqual("1,2,3,4,9", intArrayString);
        }

        [Test]
        public void Test_IntArray_AsString_Empty()
        {
            int[] intArray = new int[0];

            string intArrayString = intArray.AsString();

            Assert.AreEqual("", intArrayString);
        }

        [Test]
        public void Test_IntArray_AsString_Null()
        {
            int[] intArray = null;

            string intArrayString = intArray.AsString();

            Assert.AreEqual(null, intArrayString);
        }

        [Test]
        public void Test_IEnumerable_Int_AsString_Basic()
        {
            IEnumerable<int> intList = new List<int>{ 1, 2, 3, 4, 9 };

            string intListString = intList.AsString();

            Assert.AreEqual("1,2,3,4,9", intListString);
        }

        [Test]
        public void Test_IEnumerable_Int_AsString_Empty()
        {
            IEnumerable<int> intList = new int[0];

            string intListString = intList.AsString();

            Assert.AreEqual("", intListString);
        }

        [Test]
        public void Test_IEnumerable_Int_AsString_Null()
        {
            IEnumerable<int> intList = null;

            string intListString = intList.AsString();

            Assert.AreEqual(null, intListString);
        }
    }
}
