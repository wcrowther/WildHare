using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using WildHare.Extensions;

namespace WildHare.Tests
{
    [TestFixture]
    public class ConvertExtensionsTests
    {
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

            string newStr = file.IncrementString(ignoreExtension: ".txt");
            Assert.AreEqual("file1.txt", newStr);

            newStr = newStr.IncrementString(ignoreExtension: ".txt");
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
