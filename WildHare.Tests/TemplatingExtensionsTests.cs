using NUnit.Framework;
using System;
using System.Collections.Generic;
using WildHare.Extensions;
using WildHare.Tests.Models;
using WildHare.Extensions.ForTemplating;
using System.IO;
using WildHare.Extensions.Xtra;

namespace WildHare.Tests
{
    [TestFixture]
    public class TemplatingExtensionsTests
    {
        [Test]
        public void GetTSqlTypesToCSharpTypes_Basic()
        {
			string binaryStr	= "binary";
			string datetimeStr	= "datetime";
			string numericStr	= "numeric";
			string intStr		= "int";
			string unknownStr	= "fred";

			Assert.AreEqual("byte[]",       binaryStr.TSqlTypeToCSharpType());
			Assert.AreEqual("DateTime",     datetimeStr.TSqlTypeToCSharpType());
			Assert.AreEqual("DateTime",     datetimeStr.TSqlTypeToCSharpType(false));
			Assert.AreEqual("DateTime?",    datetimeStr.TSqlTypeToCSharpType(true));
			Assert.AreEqual("decimal",      numericStr.TSqlTypeToCSharpType());
			Assert.AreEqual("decimal",      numericStr.TSqlTypeToCSharpType(false));
			Assert.AreEqual("decimal?",     numericStr.TSqlTypeToCSharpType(true));
			Assert.AreEqual("int",          intStr.TSqlTypeToCSharpType());
			Assert.AreEqual("int?",         intStr.TSqlTypeToCSharpType(true));
			Assert.AreEqual("UNKNOWN",      unknownStr.TSqlTypeToCSharpType());
		}

        [Test]
        public void GetDotNetTypeToCSharpType_Basic()
        {
            string boolString1 = "Boolean";
            string boolString2 = "Boolean?";
            string boolString3 = "System.Nullable{Boolean}";

            Assert.AreEqual("bool", boolString1.DotNetTypeToCSharpType());
            Assert.AreEqual("bool?", boolString1.DotNetTypeToCSharpType(true));
            Assert.AreEqual("bool?", boolString2.DotNetTypeToCSharpType());
            Assert.AreEqual("bool?", boolString3.DotNetTypeToCSharpType());

        }

        [Test]
        public void Test_Template_For_Object_With_String_Template()
        {
            string template = "InvoiceId {{InvoiceId}} for AccountId {{AccountId}}.";

            var invoice = new Invoice
            {
                AccountId = 1000,
                InvoiceId = 222
            };

            string result = invoice.Template(template);

            Assert.AreEqual("InvoiceId 222 for AccountId 1000.", result);
        }

        [Test]
        public void Test_Template_For_Object_With_FileInfo_Template()
        {

            string pathRoot = XtraExtensions.GetApplicationRoot();
            string directoryPath = $@"{pathRoot}\Directory0\SimpleTemplate.txt";
            var fileToRead = new FileInfo(directoryPath);

            var invoice = new Invoice
            {
                AccountId = 1000,
                InvoiceId = 222
            };

            string result = invoice.Template(fileToRead);

            Assert.AreEqual("InvoiceId 222 for AccountId 1000.\n", result);
        }
    }
}
