using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using WildHare.Extensions;
using WildHare.Extensions.Xtra;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
    [TestFixture]
    public class MetaDocumentationTests
    {
        // string approot = "";
           
        // [SetUp]
        // public void Setup()
        // {
        //     var config = new ConfigurationBuilder()
        //                     .AddJsonFile("appSettings.json")
        //                     .Build();
           
        //     approot = config["App:Root"];
        // }


        [Test]
        public void GetMethodsFromClass()
        {
            var testMethods = new TestMethods(1, 2);
            var metaModel = testMethods.GetMetaModel();

            var metaMethods = metaModel.GetMetaMethods(); // includeInherited: false

            Assert.AreEqual(4, metaMethods.Count);
            Assert.AreEqual("Add", metaMethods.ElementAt(0).Name);
            Assert.AreEqual("Subtract", metaMethods.ElementAt(1).Name);
            Assert.AreEqual("Multiply", metaMethods.ElementAt(2).Name);
            Assert.AreEqual("first", metaMethods.ElementAt(2).Parameters[0].Name);
            Assert.AreEqual("Int32", metaMethods.ElementAt(2).Parameters[0].ParameterType.Name);
            Assert.AreEqual("second", metaMethods.ElementAt(2).Parameters[1].Name);
            Assert.AreEqual("Int32", metaMethods.ElementAt(2).Parameters[1].ParameterType.Name);
            Assert.AreEqual("Divide", metaMethods.ElementAt(3).Name);

            var allMetaMethods = metaModel.GetMetaMethods(includeInherited: true);

            Assert.AreEqual(8, allMetaMethods.Count);
        }

        [Test]
        public void GetMetaAssembly()
        {
            string outputPathRoot = XtraExtensions.GetApplicationRoot();
            string outputPath = $@"{outputPathRoot}\Directory0";

            string xmlDocumentationPath = $@"c:\Git\WildHare\WildHare\WildHare.xml";

            var metaAssembly = Assembly.Load("WildHare").GetMetaAssembly(xmlDocumentationPath);
            metaAssembly.WriteMetaAssemblyToFile(outputPath, true);

            Assert.AreEqual(29, metaAssembly.GetMetaModels().Count);
        }
    }
}
