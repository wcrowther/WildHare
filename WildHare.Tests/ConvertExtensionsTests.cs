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

            string newStr = file.IncrementString(ignoreEnd: ".txt");
            Assert.AreEqual("file1.txt", newStr);

            newStr = newStr.IncrementString(ignoreEnd: ".txt");
            Assert.AreEqual("file2.txt", newStr);

            newStr = newStr.IncrementString(ignoreEnd: ".txt");
            Assert.AreEqual("file3.txt", newStr);
        }

        [Test]
        public void Test_IncrementString_With_Number_And_Extension()
        {
            string file = "file1.txt";

            string newStr = file.IncrementString(ignoreEnd: ".txt");
            Assert.AreEqual("file2.txt", newStr);

            newStr = newStr.IncrementString(ignoreEnd: ".txt");
            Assert.AreEqual("file3.txt", newStr);

            newStr = newStr.IncrementString(ignoreEnd: ".txt");
            Assert.AreEqual("file4.txt", newStr);
        }
    }
}