using NUnit.Framework;
using System;
using System.Collections.Generic;
using WildHare.Extensions;
using WildHare.Tests.Models;
using WildHare.Extensions.ForTemplating;

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
    }
}
