using NUnit.Framework;
using System;
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
    }
}
