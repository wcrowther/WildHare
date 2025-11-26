using NUnit.Framework;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Xtra;
using WildHare.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NUnit.Framework.Legacy;

namespace WildHare.Tests
{
    [TestFixture]
    public class XtraExtensionsTests
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
        public void Test_WriteToFile_Get_Base_Directory_Alternatives()
        {
            string codeBase			= Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string localPath		= Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath);
            string location			= Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string applicationRoot	= XtraExtensions.GetApplicationRoot();
			string entryAssembly	= Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

			ClassicAssert.AreEqual($@"{approot}\WildHare\WildHare.Tests\bin\Debug\net10.0", codeBase);
            ClassicAssert.AreEqual($@"{approot}\WildHare\WildHare.Tests\bin\Debug\net10.0", localPath);
            ClassicAssert.AreEqual($@"{approot}\WildHare\WildHare.Tests\bin\Debug\net10.0", location);
            ClassicAssert.AreEqual($@"{approot}\WildHare\WildHare.Tests", applicationRoot);
            ClassicAssert.AreEqual($@"{approot}\WildHare\WildHare.Tests\bin\Debug\net10.0", entryAssembly);
        }

        [Test]
        public void Test_WriteToFile_ToMapPath()
        {
            // Thanks to Tim Brown
            // http://codebuckets.com/2017/10/19/getting-the-root-directory-path-for-net-core-applications/

            string source1 = $@"\TestFile.txt".ToMapPath();
            string source2 = $@"\Helpers\TestFile.txt".ToMapPath();
            string source3 = $@"~\Helpers\TestFile.txt".ToMapPath();
            string source4 = $@"~/SourceFiles/xmlSeedSourcePlus.xml".ToMapPath();

            string fileName1 = $@"{approot}\WildHare\WildHare.Tests\TestFile.txt";
            string fileName2 = $@"{approot}\WildHare\WildHare.Tests\Helpers\TestFile.txt";
            string fileName3 = $@"{approot}\WildHare\WildHare.Tests\Helpers\TestFile.txt";
            string fileName4 = $@"{approot}\WildHare\WildHare.Tests\SourceFiles\xmlSeedSourcePlus.xml";

            ClassicAssert.AreEqual(fileName1, source1);
            ClassicAssert.AreEqual(fileName2, source2);
            ClassicAssert.AreEqual(fileName3, source3);
            ClassicAssert.AreEqual(fileName4, source4);
        }


        [Test]
        public void Test_DynamicListOfType()
        {
            dynamic list = XtraExtensions.DynamicListOfType(typeof(Person));// Is IList of object
            list.Add(new Person() { FirstName = "Will", LastName="Crowther"});

            ClassicAssert.AreEqual(1, list.Count);
            ClassicAssert.AreEqual("Person", ((IEnumerable) list).GetMetaModel().TypeName);

            var listOfPerson = new List<Person>()
            {
                new Person{ FirstName="Patricia", LastName="Crowther"}
            };

            list.AddRange(listOfPerson);

            ClassicAssert.AreEqual(2, list.Count);
        }


        [Test]
        public void Test_DynamicArrayOfType()
        {
            var type = typeof(Person);

            // Create list of Abstract

            List<Person> listOfAbstract = new List<Person>
            {
                new Person { FirstName = "Will" }
            };

            // Creates an IList of <dynamic>

            List<dynamic> dynamicArray = XtraExtensions.DynamicArrayOfType(type).ToList();
            dynamicArray.Add(new Person() { FirstName = "Fred" });
            dynamicArray.Add(new Person() { FirstName = "Patricia" });

            ClassicAssert.AreEqual(2, dynamicArray.Count);

            // listOfAbstract.AddRange(listOfDynamicObjects);  DOES NOT WORK SO USE:

            var listOfDynamicAbstracts = dynamicArray.OfType<Person>(); // var is IEnumerable<Abstract>

            listOfAbstract.AddRange(listOfDynamicAbstracts);

            ClassicAssert.AreEqual(3, listOfAbstract.Count);
        }
    }
}
