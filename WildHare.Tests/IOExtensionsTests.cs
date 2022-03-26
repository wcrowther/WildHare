using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using static WildHare.Extensions.Xtra.XtraExtensions;

namespace WildHare.Tests
{
    [TestFixture]
    public class IOExtensionsTests
    {
        string approot = "";

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appSettings.json")
                            .Build();

            approot = config["App:Root"];
        }

        [Test]
        public void Test_WriteToFile_TextFile1()
        {
            string testRoot = GetApplicationRoot();
            string fileName = "TestFile1.txt";
            string pathToWriteTo = $@"{testRoot}\TextFiles\{fileName}";

            bool fileAlreadyExists = File.Exists(pathToWriteTo);
            Debug.WriteLine($"{fileAlreadyExists}");

            File.Delete(pathToWriteTo);
            bool notAbleToDeleteExistingFile = File.Exists(pathToWriteTo);

            string sentenceToWrite = $"This is the sentence to write to file '{fileName}'.";
            sentenceToWrite.WriteToFile(pathToWriteTo, false);

            var fileAllText = File.ReadAllText(pathToWriteTo);

            Assert.IsFalse(notAbleToDeleteExistingFile);
            Assert.AreEqual(sentenceToWrite, fileAllText);
        }

        [Test]
        public void Test_WriteToFile_WithCreateFolder()
        {
            string pathRoot = GetApplicationRoot();
            string directoryPath = $@"{pathRoot}\TestFolder";
            string fileName = "TestFile.txt";

            string pathToWriteTo = $@"{directoryPath}\{fileName}";
            string testText = $"Write to file {fileName}.";

            testText.WriteToFile(pathToWriteTo);

            string fileContents = File.ReadAllText(pathToWriteTo);

            Assert.AreEqual(fileContents, testText);

            // Cleanup by deleting directory and existing files
            var directory = new DirectoryInfo(directoryPath);
            directory.Delete(true);

            var fileInfo = new FileInfo(pathToWriteTo);

            Assert.IsFalse(fileInfo.Exists);
        }

        [Test]
        public void Test_GetAllCssFiles()
        {
            string pathRoot = $@"{approot}\WildHare\WildHare.Web";
            string pathToWriteTo = $@"{pathRoot}\Analytics\AllCssFiles.txt";
            var sb = new StringBuilder();

            var allFiles = pathRoot.GetAllFiles("*.css");

            foreach (var file in allFiles)
            {
                sb.AppendLine($"{file.Name} length: {file.Length}");
            }

            sb.ToString().WriteToFile(pathToWriteTo, true);

            Assert.AreEqual(4, allFiles.Count());
        }

        [Test]
        public void Test_GetFileSystemInfos()
        {
            // Only gets 1 level of hierarchy

            string pathRoot = GetApplicationRoot();
            string directoryPath = $@"{pathRoot}\Directory0";
            string outputPath = $@"{pathRoot}\Analytics\TextFiles\TestDirectories.txt";
            var files = new StringBuilder();
            var folders = new StringBuilder();

            var allFilesAndFolders = new DirectoryInfo(directoryPath).GetFileSystemInfos();

            foreach (var info in allFilesAndFolders)
            {
                if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    folders.AppendLine($"Directory: {info.Name}");
                }
                else
                {
                    files.AppendLine($"File: {info.Name}");
                }
            }
            string filesAndFolders = folders.ToString() + files.ToString();
            filesAndFolders.WriteToFile(outputPath, true);

            Assert.AreEqual(8, allFilesAndFolders.Count());
        }

        [Test]
        public void Test_GetFileSystemInfos_WithSearchOption()
        {
            // Gets multiple levels

            string pathRoot = GetApplicationRoot();
            string directoryPath = $@"{pathRoot}\Directory0";
            string outputPath = $@"{pathRoot}\TextFiles\TestDirectories.txt";
            var files = new StringBuilder();
            var folders = new StringBuilder();

            var allFilesAndFolders = new DirectoryInfo(directoryPath).GetFileSystemInfos("*", SearchOption.AllDirectories);

            foreach (var info in allFilesAndFolders)
            {
                if (info.IsDirectory())
                {
                    folders.AppendLine($"Directory: {info.Name}");
                }
                else
                {
                    files.AppendLine($"File: {info.Name}");
                }
            }
            string filesAndFolders = folders.ToString() + files.ToString();
            filesAndFolders.WriteToFile(outputPath, true);

            Assert.AreEqual(15, allFilesAndFolders.Count());
        }

        [Test]
        public void Test_GetAllDirectoriesAndFiles()
        {
            string pathRoot = GetApplicationRoot();
            string directoryPath = $@"{pathRoot}\Directory0";
            string outputPath = $@"{pathRoot}\TextFiles\FileSystemInfos.txt";

            var allFilesAndFolders = new DirectoryInfo(directoryPath);
            List<FileSystemInfo> list = allFilesAndFolders.GetAllDirectoriesAndFiles();

            var sb = new StringBuilder();

            foreach (var info in list)
            {
                sb.AppendLine($"{(info.IsDirectory() ? "Directory" : "\t File")} {info.Name} {info.FullName}");
            }

            sb.ToString().WriteToFile(outputPath, true);

            Assert.AreEqual(13, list.Count());
        }

        [Test]
        public void Test_FileInfo_GetString_Found()
        {
            string pathRoot = GetApplicationRoot();
            string directoryPath = $@"{pathRoot}\Directory0\TextFile0.txt";

            var fileToRead = new FileInfo(directoryPath);

            Assert.AreEqual("This is TextFile0.txt.\r\n", fileToRead.ReadFile());
        }

        [Test]
        public void Test_FileInfo_GetString_Not_Found()
        {
            string pathRoot = GetApplicationRoot();
            string directoryPath = $@"{pathRoot}\Directory0\DoesNotExist.txt";

            var fileToRead = new FileInfo(directoryPath);

            var ex = Assert.Throws<FileNotFoundException>
            (
                () => fileToRead.ReadFile()
            );

            Assert.IsTrue(ex.Message.StartsWith("Could not find file"));
        }

        [Test]
        public void Test_FileInfo_GetString_Not_Found_But_Not_Strict_Should_Return_Null()
        {
            string pathRoot = GetApplicationRoot();
            string directoryPath = $@"{pathRoot}\Directory0\DoesNotExist.txt";

            var fileToRead = new FileInfo(directoryPath);

            Assert.IsNull(fileToRead.ReadFile(false));
        }
    }
}
