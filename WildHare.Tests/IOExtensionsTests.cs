using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using WildHare.Extensions;

namespace WildHare.Tests
{
    [TestFixture]
    public class IOExtensionsTests
    {


        [Test]
        public void Test_WriteToFile_TextFile1()
        {
            string testRoot = IOExtensions.GetApplicationRoot();
            string pathToWriteTo = $@"{testRoot}\Helpers\TestFile1.txt";

            bool fileAlreadyExists = File.Exists(pathToWriteTo);
            Debug.WriteLine($"{fileAlreadyExists}");

            File.Delete(pathToWriteTo);
            bool notAbleToDeleteExistingFile = File.Exists(pathToWriteTo);

            string sentenceToWrite = "This is the sentence to save to write to file";
            sentenceToWrite.WriteToFile(pathToWriteTo, false);

            var fileAllText = File.ReadAllText(pathToWriteTo);

            Assert.IsFalse(notAbleToDeleteExistingFile);
            Assert.AreEqual(sentenceToWrite, fileAllText);
        }

        [Test]
        public void Test_WriteToFile_Get_Base_Directory_Alternatives()
        {
            string codeBase			= Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            string localPath		= Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            string location			= Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string applicationRoot	= IOExtensions.GetApplicationRoot();
			string entryAssembly	= Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

			Assert.AreEqual(@"file:\C:\Code\Trunk\WildHare\WildHare.Tests\bin\Debug\netcoreapp2.0", codeBase);
            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare.Tests\bin\Debug\netcoreapp2.0", localPath);
            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare.Tests\bin\Debug\netcoreapp2.0", location);
            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare.Tests", applicationRoot);
            Assert.AreEqual(@"C:\Users\Will Crowther\.nuget\packages\microsoft.testplatform.testhost\15.7.2\lib\netstandard1.5", entryAssembly);
        }

        [Test]
        public void Test_WriteToFile_ToMapPath()
        {
            // Thanks to Tim Brown
            // http://codebuckets.com/2017/10/19/getting-the-root-directory-path-for-net-core-applications/

            string source1 = @"\TestFile.txt".ToMapPath();
            string source2 = @"\Helpers\TestFile.txt".ToMapPath();
            string source3 = @"~\Helpers\TestFile.txt".ToMapPath();
            string source4 = @"~/SourceFiles/xmlSeedSourcePlus.xml".ToMapPath();

            string fileName1 = @"C:\Code\Trunk\WildHare\WildHare.Tests\TestFile.txt";
            string fileName2 = @"C:\Code\Trunk\WildHare\WildHare.Tests\Helpers\TestFile.txt";
            string fileName3 = @"C:\Code\Trunk\WildHare\WildHare.Tests\Helpers\TestFile.txt";
            string fileName4 = @"C:\Code\Trunk\WildHare\WildHare.Tests\SourceFiles\xmlSeedSourcePlus.xml";
            string fileName4a = "C:\\Code\\Trunk\\WildHare\\WildHare.Tests\\SourceFiles\\xmlSeedSourcePlus.xml";

            Assert.AreEqual(fileName1, source1);
            Assert.AreEqual(fileName2, source2);
            Assert.AreEqual(fileName3, source3);
            Assert.AreEqual(fileName4, source4);
            Assert.AreEqual(fileName4a, source4);
        }
    }
}
