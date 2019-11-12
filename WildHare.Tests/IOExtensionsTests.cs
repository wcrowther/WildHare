using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Extensions.Xtra;

namespace WildHare.Tests
{
    [TestFixture]
    public class IOExtensionsTests
    {
        [Test]
        public void Test_WriteToFile_TextFile1()
        {
            string testRoot = XtraExtensions.GetApplicationRoot();
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
        public void Test_WriteToFile_WithCreateFolder()
        {
            string testRoot = XtraExtensions.GetApplicationRoot();
            string filePath = $@"{testRoot}\TestFolder\TestFile.txt";
            string testText = "Write to file.";

            testText.WriteToFile(filePath);

            string fileContents = File.ReadAllText(filePath);

            Assert.AreEqual(fileContents, testText);
        }
    }
}
