using NUnit.Framework;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Extensions.Xtra;
using WildHare.Tests.Models;
using System.Collections.Generic;

namespace WildHare.Tests
{
    [TestFixture]
    public class XtraExtensionsTests
    {
        [Test]
        public void Test_WriteToFile_Get_Base_Directory_Alternatives()
        {
            string codeBase			= Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            string localPath		= Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            string location			= Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string applicationRoot	= XtraExtensions.GetApplicationRoot();
			string entryAssembly	= Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

			Assert.AreEqual(@"file:\C:\Code\Trunk\WildHare\WildHare.Tests\bin\Debug\netcoreapp3.1", codeBase);
            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare.Tests\bin\Debug\netcoreapp3.1", localPath);
            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare.Tests\bin\Debug\netcoreapp3.1", location);
            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare.Tests", applicationRoot);
            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare.Tests\bin\Debug\netcoreapp3.1", entryAssembly);
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


        [Test]
        public void Test_CreateListOfType()
        {
            dynamic list = XtraExtensions.CreateListOfType(typeof(Person));// Is IList of object
            list.Add(new Person() { FirstName = "Will", LastName="Crowther"});

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Person", ((IEnumerable) list).GetMetaModel().TypeName);

            var listOfPerson = new List<Person>()
            {
                new Person{ FirstName="Patricia", LastName="Crowther"}
            };

            list.AddRange(listOfPerson);

            Assert.AreEqual(2, list.Count);
        }
    }
}
