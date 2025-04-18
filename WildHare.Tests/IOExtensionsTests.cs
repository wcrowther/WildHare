using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using WildHare.Xtra;
using static System.Environment;

namespace WildHare.Tests;

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
        string testRoot = XtraExtensions.GetApplicationRoot();
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

        string pathRoot			= XtraExtensions.GetApplicationRoot();
        string directoryPath	= $@"{pathRoot}\Directory0";
        string outputPath		= $@"{pathRoot}\Analytics\TextFiles\TestDirectories.txt";
        var files				= new StringBuilder();
        var folders				= new StringBuilder();

        var allFilesAndFolders	= new DirectoryInfo(directoryPath).GetFileSystemInfos();

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

        $"{NewLine}{folders}{NewLine}{files}".WriteToFile(outputPath, true);

        Assert.AreEqual(9, allFilesAndFolders.Length);
    }

    [Test]
    public void Test_GetFileSystemInfos_WithSearchOption()
    {
        // Gets multiple levels

        string pathRoot				= XtraExtensions.GetApplicationRoot();
        string directoryPath		= $@"{pathRoot}\Directory0";
        string outputPath			= $@"{pathRoot}\TextFiles\TestDirectories.txt";
        var files					= new StringBuilder();
        var folders					= new StringBuilder();

        var allFilesAndFolders		= new DirectoryInfo(directoryPath).GetFileSystemInfos("*", SearchOption.AllDirectories);

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

        Assert.AreEqual(16, allFilesAndFolders.Count());
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
			string infoType = info.IsDirectory() ? $"{NewLine}Directory" : "\t File";

			sb.AppendLine($"{infoType} {info.Name,-35} {info.FullName}");
		}

		sb.ToString()
		  .WriteToFile(outputPath, true);

		Assert.AreEqual(14, list.Count);

		var outputToRead = new FileInfo(outputPath);
		var outputLines = outputToRead.ReadFile().ToLineArray();

		Assert.AreEqual(19, outputLines.Length);
	}

    [Test]
    public void Test_FileInfo_GetString_Found()
    {
        string pathRoot = XtraExtensions.GetApplicationRoot();
        string directoryPath = $@"{pathRoot}\Directory0\TextFile0.txt";

        var fileToRead = new FileInfo(directoryPath);

        Assert.AreEqual("This is TextFile0.txt.\r\n", fileToRead.ReadFile());
    }

    [Test]
    public void Test_FileInfo_GetString_Not_Found()
    {
        string pathRoot = XtraExtensions.GetApplicationRoot();
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
        string pathRoot = XtraExtensions.GetApplicationRoot();
        string directoryPath = $@"{pathRoot}\Directory0\DoesNotExist.txt";

        var fileToRead = new FileInfo(directoryPath);

        Assert.IsNull(fileToRead.ReadFile(false));
    }

    [Test]
    public void TestContext_CurrentContext_TestDirectory()
    {
        string testDirectory = TestContext.CurrentContext.TestDirectory;

        Assert.AreEqual(@"C:\Git\WildHare\WildHare.Tests\bin\Debug\net9.0", testDirectory);
        Assert.AreEqual(@"C:\Git\WildHare\WildHare.Tests\", testDirectory.GetStartBefore("bin"));
    }

    [Test]
    public void Get_FileInfo_Child_FileInfo()
    {
        string testDirectory    = TestContext.CurrentContext.TestDirectory;
        var helpersDirectory    = new DirectoryInfo(testDirectory.GetStartBefore("bin")).Child("Helpers");

        Assert.IsNotNull(helpersDirectory);
        Assert.AreEqual("Helpers", helpersDirectory.Name);
    }

    [Test]
    public void Get_FileInfo_Sibling_FileInfo()
    {
        string testDirectory    = TestContext.CurrentContext.TestDirectory;
        var wildHareDirectory   = new DirectoryInfo(testDirectory.GetStartBefore("bin")).Sibling("WildHare");

        Assert.IsNotNull(wildHareDirectory);
        Assert.AreEqual("WildHare", wildHareDirectory.Name);
    }

	[Test]
	public void Test_GetAllFiles_GetProj_Extensions()
	{
		string pathRoot = $@"{approot}\WildHare\WildHare.Web";
		var projFiles   = pathRoot.GetAllFiles([".csproj",".user"]);

		Assert.AreEqual(2, projFiles.Count);
	}

	// [Test]
	// public void Test_GetAllImageFiles()
	// {
	// 	string testDirectory = TestContext.CurrentContext.TestDirectory;
	// 	var imagesDirectory = new DirectoryInfo(testDirectory.GetStartBefore("bin"))
	// 								   .Sibling("WildHare.Web")
	// 								   .Child("wwwroot")
	// 								   .Child("images");
	// 
	// 	var projFiles = pathRoot.GetAllFiles([".png", ".jpg", ".gif", ".webp"]);
	// 
	// 	Assert.AreEqual(10, projFiles.Count());
	// }


}
