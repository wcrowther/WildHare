using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using WildHare.Extensions.ForType;
using WildHare.Extensions.ForString;
using WildHare.Tests.Models;

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

            string fileName = @"C:\Code\Trunk\WildHare\WildHare.Tests\TestFile.txt";
            string fileName2 = @"C:\Code\Trunk\WildHare\WildHare.Tests\Helpers\TestFile.txt";
            string appPathFileName = @"\TestFile.txt".ToApplicationPath();
            string appPathFileName2 = @"\Helpers\TestFile.txt".ToApplicationPath();

            Assert.AreEqual(fileName, appPathFileName);
            Assert.AreEqual(fileName2, appPathFileName2);
        }
    }
}