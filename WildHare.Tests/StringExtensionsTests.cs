using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using WildHare.Extensions;

namespace WildHare.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void Test_WriteToFile_Get_Base_Directory_Alternatives()
        {
            string codeBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            string localPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string applicationRoot = StringExtensions.GetApplicationRoot();

            Assert.AreEqual(@"file:\C:\Code\Trunk\WildHare\WildHare.Tests\bin\Debug\netcoreapp2.0", codeBase);
            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare.Tests\bin\Debug\netcoreapp2.0", localPath);
            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare.Tests\bin\Debug\netcoreapp2.0", location);
            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare.Tests", applicationRoot);
        }

        [Test]
        public void Test_WriteToFile_ToApplictionPath()
        {
            // Thanks to Tim Brown
            // http://codebuckets.com/2017/10/19/getting-the-root-directory-path-for-net-core-applications/

            string string1 = @"\TestFile.txt".ToMapPath();
            string string2 = @"\Helpers\TestFile.txt".ToMapPath();
            string string3 = @"~\Helpers\TestFile.txt".ToMapPath();

            string fileName1 = @"C:\Code\Trunk\WildHare\WildHare.Tests\TestFile.txt";
            string fileName2 = @"C:\Code\Trunk\WildHare\WildHare.Tests\Helpers\TestFile.txt";
            string fileName3 = @"C:\Code\Trunk\WildHare\WildHare.Tests\Helpers\TestFile.txt";

            Assert.AreEqual(fileName1, string1);
            Assert.AreEqual(fileName2, string2);
            Assert.AreEqual(fileName3, string3);
        }
    }
}