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
        public void TestGetMethodsFromClass()
        {
            var testMethods = new TestMethods(1, 2);
            var metaModel = testMethods.GetMetaModel();

            var metaMethods = metaModel.GetMetaMethods(); // includeInherited: false

            Assert.AreEqual(4, metaMethods.Count);
            Assert.AreEqual("Add", metaMethods.ElementAt(0).Name);
            Assert.AreEqual("Subtract", metaMethods.ElementAt(1).Name);
            Assert.AreEqual("Multiply", metaMethods.ElementAt(2).Name);
            Assert.AreEqual("first", metaMethods.ElementAt(2).Parameters[0].Name);
            Assert.AreEqual("Int32", metaMethods.ElementAt(2).Parameters[0].ParameterMetaType.Name);
            Assert.AreEqual("second", metaMethods.ElementAt(2).Parameters[1].Name);
            Assert.AreEqual("Int32", metaMethods.ElementAt(2).Parameters[1].ParameterMetaType.Name);
            Assert.AreEqual("Divide", metaMethods.ElementAt(3).Name);

            var allMetaMethods = metaModel.GetMetaMethods(includeInherited: true);

            Assert.AreEqual(8, allMetaMethods.Count);
        }

        [Test]
        public void TestGetMetaAssembly()
        {
            string outputPathRoot = XtraExtensions.GetApplicationRoot();
            string outputPath = $@"{outputPathRoot}\Directory0";

            string xmlDocumentationPath = $@"c:\Git\WildHare\WildHare\WildHare.xml";

            var metaAssembly = Assembly.Load("WildHare").GetMetaAssembly(xmlDocumentationPath);
            metaAssembly.WriteMetaAssemblyToFile(outputPath, true);

            Assert.AreEqual(29, metaAssembly.GetMetaModels().Count);
        }

        [Test]
        public void TestGetMetaModelsGroupedByNamespace_Basic()
        {
            var metaAssembly = Assembly.Load("WildHare").GetMetaAssembly();
            var metaNamespaces = metaAssembly.GetMetaModelsGroupedByNamespace();

            Assert.AreEqual(6, metaNamespaces.Count);
        }

        [Test]
        public void TestGetMetaModelsGroupedByNamespace_With_Excluded_Namespaces()
        {
            string excludedNamespaces = "WildHare.Properties, WildHare.Extensions.Xtra";
            var metaAssembly = Assembly.Load("WildHare").GetMetaAssembly();
            var metaNamespaces = metaAssembly.GetMetaModelsGroupedByNamespace(exclude: excludedNamespaces);

            Assert.AreEqual(4, metaNamespaces.Count);
        }

        [Test]
        public void TestWriteMetaAssemblyToFile()
        {
            string outputPath = $@"c:\Git\WildHare\MetaTest\";

            var metaAssembly = Assembly.Load("MetaTest").GetMetaAssembly();
            bool success = metaAssembly.WriteMetaAssemblyToFile(outputPath, true);

            Assert.IsTrue(success);
        }

        [Test]
        public void Test_Using_MetaTest_Example_Basic()
        {
            GetMetaTestExample(out List<MetaModel> metaModels, out MetaModel firstModel);

            Assert.AreEqual(1, metaModels.Count);
            Assert.AreEqual("TestClass", firstModel.TypeName);

            Assert.AreEqual(6, firstModel.GetMetaMethods().Count);
            Assert.AreEqual("T:MetaTest.TestClass",  firstModel.DocMemberName);
        }

        [Test]
        public void Test_Using_MetaTest_Example_SimpleMethod()
        {
            GetMetaTestExample(out List<MetaModel> metaModels, out MetaModel firstModel);

            var metaMethod = firstModel.GetMetaMethods().Single(f => f.Name == "SimpleMethod");

            Assert.AreEqual("SimpleMethod(System.Int32 item, System.Int32 step)",
                            metaMethod.Signature);

            //Assert.AreEqual("M:MetaTest.TestClass.SimpleMethod(System.Int32,System.Int32)",
            //                metaMethod.DocMemberName);
        }

        [Test]
        public void Test_Using_MetaTest_Example_ReturnIListMethod()
        {
            GetMetaTestExample(out List<MetaModel> metaModels, out MetaModel firstModel);

            var metaMethod = firstModel.GetMetaMethods().Single(f => f.Name == "ReturnIListMethod");

            Assert.AreEqual("ReturnIListMethod(System.String item, System.Int32 step)",
                            metaMethod.Signature);

            //Assert.AreEqual("M:MetaTest.TestClass.ReturnIListMethod(System.String,System.Int32)",
            //                metaMethod.DocMemberName);
        }

        [Test]
        public void Test_Using_MetaTest_Example_TestThree()
        {
            GetMetaTestExample(out List<MetaModel> metaModels, out MetaModel firstModel);

            var metaMethod = firstModel.GetMetaMethods().Single(f => f.Name == "TestThree");

            Assert.AreEqual("TestThree(System.String item, System.Func`2 func)",
                            metaMethod.Signature);

            //Assert.AreEqual("M:MetaTest.TestClass.TestThree(System.String,System.Func{System.String,System.Boolean})",
            //                firstModel.GetMetaMethods()[2].DocMemberName);
        }

        [Test]
        public void Test_Using_MetaTest_Example_TestOneGeneric()
        {
            GetMetaTestExample(out List<MetaModel> metaModels, out MetaModel firstModel);

            var metaMethod = firstModel.GetMetaMethods().Single(f => f.Name == "TestOneGeneric");

            Assert.AreEqual("TestOneGeneric(T item, System.Int32 step)",
                            metaMethod.Signature);

            //Assert.AreEqual("M:MetaTest.TestClass.TestOneGeneric``1(``0,System.Int32)",
            //                firstModel.GetMetaMethods()[3].DocMemberName);
        }

        [Test]
        public void Test_Using_MetaTest_Example_TestTwoGeneric()
        {
            GetMetaTestExample(out List<MetaModel> metaModels, out MetaModel firstModel);

            var metaMethod = firstModel.GetMetaMethods().Single(f => f.Name == "TestTwoGeneric");

            Assert.AreEqual("TestTwoGeneric(T item, System.IList`1 itemList, System.Func`2 func, Int32 step)",
                            metaMethod.Signature);

            //Assert.AreEqual("M:MetaTest.TestClass.TestTwoGeneric``1(``0,System.Collections.Generic.IList{``0},System.Func{``0,System.Boolean},System.Int32)",
            //                firstModel.GetMetaMethods()[4].DocMemberName);
        }

        [Test]
        public void Test_Using_MetaTest_Example_TestThreeGeneric()
        {
            GetMetaTestExample(out List<MetaModel> metaModels, out MetaModel firstModel);

            var metaMethod = firstModel.GetMetaMethods().Single(f => f.Name == "TestThreeGeneric");

            Assert.AreEqual("TestThreeGeneric(T2 item, System.IList`1 itemList, System.Func`2 func, System.Int32 step)",
                            metaMethod.Signature);

            //Assert.AreEqual("M: MetaTest.TestClass.TestThreeGeneric``3(``1, System.Collections.Generic.IList{ System.ValueTuple{``0,``1,``2} },System.Func{``0,System.Boolean},System.Int32)",
            //                firstModel.GetMetaMethods()[5].DocMemberName);
        }

        // ======================================================================================================

        private static void GetMetaTestExample(out List<MetaModel> metaModels, out MetaModel firstModel)
        {
            var metaAssembly = Assembly.Load("MetaTest").GetMetaAssembly();
            metaModels = metaAssembly.GetMetaModels();
            firstModel = metaModels.First();
        }
    }
}
