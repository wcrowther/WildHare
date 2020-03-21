using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
            string fileName = "TestFile1.txt";
            string pathToWriteTo = $@"{testRoot}\TestFiles\{fileName}";

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
            string pathRoot = XtraExtensions.GetApplicationRoot();
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
            string pathRoot = @"C:\Code\Trunk\WildHare\WildHare.Web";
            string pathToWriteTo = $@"{pathRoot}\AllCssFiles.txt";
            var sb = new StringBuilder();

            var allFiles = pathRoot.GetAllFiles().Where(w => w.Extension == ".css");

            foreach (var file in allFiles)
            {
                sb.AppendLine($"{file.Name} length: {file.Length}");
            }

            sb.ToString().WriteToFile(pathToWriteTo, true);

            Assert.AreEqual(4, allFiles.Count());
        }


        [Test]
        public void Test_GetAll_CsHtml_Files_And_Parse_With_AngleSharp()
        {
            string pathRoot = @"C:\Code\Trunk\WildHare\WildHare.Web";
            string pathToWriteTo = $@"{pathRoot}\AllCsHtmlFiles.txt";
            var sb = new StringBuilder();

            var allFiles = $@"{pathRoot}\Pages".GetAllFiles().Where(w => w.Extension == ".cshtml");

            // Uses AngleSharp to parse HTML
            var config = Configuration.Default;

            foreach (var file in allFiles)
            {
                string source = File.ReadAllText(file.FullName);

                var parser = new HtmlParser();
                var doc = parser.ParseDocument(source);
                var styles = doc.QuerySelectorAll("*[style]");

                sb.AppendLine("=".Repeat(100));
                sb.AppendLine($"{file.Name} - inline styles({styles.Count()})");

                foreach (var style in styles)
                {
                    sb.AppendLine($"\t\tstyle: {style.GetAttribute("style")}");
                }

                var styleImports = doc.QuerySelectorAll("link[rel=stylesheet]");

                if(styleImports.Count() > 0)
                    sb.AppendLine("-".Repeat(100));

                foreach (var import in styleImports)
                {
                    sb.AppendLine($"\t\tstylesheet: {import.GetAttribute("href")}");
                }
            }

            if(allFiles.Count() > 0)
                sb.AppendLine("=".Repeat(100));

            sb.ToString().WriteToFile(pathToWriteTo, true);

            Assert.AreEqual(10, allFiles.Count());
        }


        [Test]
        public void Test_GetFileSystemInfos()
        {
            // Only gets 1 level of hierarchy

            string pathRoot = XtraExtensions.GetApplicationRoot();
            string directoryPath = $@"{pathRoot}\Directory0";
            string outputPath = $@"{pathRoot}\TextFiles\TestDirectories.txt";
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

            Assert.AreEqual(3, allFilesAndFolders.Count());
        }

        [Test]
        public void Test_GetFileSystemInfos_WithSearchOption()
        {
            // Gets multiple levels

            string pathRoot = XtraExtensions.GetApplicationRoot();
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

            Assert.AreEqual(10, allFilesAndFolders.Count());
        }

        [Test]
        public void Test_GetAllDirectoriesAndFiles()
        {
            string pathRoot = XtraExtensions.GetApplicationRoot();
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

            Assert.AreEqual(8, list.Count());
        }

    }
}
