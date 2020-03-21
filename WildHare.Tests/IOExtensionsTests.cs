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

        [Test]
        public void Test_GetAll_CsHtml_Files_And_Parse_With_AngleSharp()
        {
            //string pathToWriteTo = $@"{pathRoot}\AllCsHtmlFiles.txt";
            //string pathRoot = @"C:\Code\Trunk\WildHare\WildHare.Web";
            //var allFiles = $@"{pathRoot}\Pages".GetAllFiles().Where(w => w.Extension == ".cshtml");

            string pathToWriteTo = @"C:\Code\Trunk\WildHare\WildHare.Web\SeedPacketCss.txt";
            string pathRoot = @"C:\Code\Trunk\SeedPacket\Examples\Views";
            
            var allFiles = $@"{pathRoot}".GetAllFiles("*.cshtml");

            var sb = new StringBuilder();

            foreach (var file in allFiles)
            {
                GetStyleInfoForFile(sb, file);
            }

            if (allFiles.Count() > 0)
                sb.AppendLine("=".Repeat(100));

            sb.ToString().WriteToFile(pathToWriteTo, true);

            Assert.AreEqual(30, allFiles.Count());  // 10 for WildHare
        }

        // =======================================================================
        // PRIVATE FUNCTIONS
        // =======================================================================

        private static void GetStyleInfoForFile(StringBuilder sb, FileInfo file)
        {
            string start = "\t\t   ";
            string source = File.ReadAllText(file.FullName);

            var parser = new HtmlParser();
            var doc = parser.ParseDocument(source);
            var styles = doc.QuerySelectorAll("*[style]");

            sb.AppendLine("=".Repeat(100));
            sb.AppendLine($"{file.Directory.Name,-10} {file.Name} - ({styles.Count()} inline styles)");

            // ---------------------------------------------------

            if (styles.Count() > 0)
                sb.AppendLine(start + "-".Repeat(90));

            foreach (var style in styles)
            {
                sb.AppendLine($"{start}style: {style.GetAttribute("style")}");
            }

            // ---------------------------------------------------

            var styleImports = doc.QuerySelectorAll("link[rel=stylesheet]");

            if (styleImports.Count() > 0)
                sb.AppendLine(start + "-".Repeat(90));

            foreach (var import in styleImports)
            {
                sb.AppendLine($"{start}stylesheet: {import.GetAttribute("href")}");
            }

            // ---------------------------------------------------

            var textLines = source.Split('\n')
                            .Select((x, lineNum) => $"line {lineNum}: {x.TrimStart()}")
                            .Where(w => w.Contains("@Styles.Render"))
                            .Select(s => $"{start}{s.Truncate(150).EnsureEnd("\n")}");

            if (textLines.Count() > 0)
                sb.AppendLine(start + "-".Repeat(90));

            sb.Append(string.Join("", textLines));

            // ---------------------------------------------------

            var scriptRenderLines = source.Split('\n')
                            .Select((x, lineNum) => $"line {lineNum}: {x.TrimStart()}")
                            .Where(w => w.Contains("@Scripts.Render"))
                            .Select(s => $"{start}{s.Truncate(150).EnsureEnd("\n")}");

            if (scriptRenderLines.Count() > 0)
                sb.AppendLine(start + "-".Repeat(90));

            sb.Append(string.Join("", scriptRenderLines));
        }
    }
}
