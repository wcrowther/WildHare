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
        public void GetCSharpTypes_Basic()
        {
			string binaryStr	= "binary";
			string datetimeStr	= "datetime";
			string numericStr	= "numeric";
			string intStr		= "int";
			string unknownStr	= "fred";

			Assert.AreEqual("byte[]", binaryStr.FromTSqlTypeToCSharpType());
			Assert.AreEqual("DateTime", datetimeStr.FromTSqlTypeToCSharpType());
			Assert.AreEqual("DateTime", datetimeStr.FromTSqlTypeToCSharpType(false));
			Assert.AreEqual("DateTime?", datetimeStr.FromTSqlTypeToCSharpType(true));
			Assert.AreEqual("decimal", numericStr.FromTSqlTypeToCSharpType());
			Assert.AreEqual("decimal", numericStr.FromTSqlTypeToCSharpType(false));
			Assert.AreEqual("decimal?", numericStr.FromTSqlTypeToCSharpType(true));
			Assert.AreEqual("int", intStr.FromTSqlTypeToCSharpType());
			Assert.AreEqual("int?", intStr.FromTSqlTypeToCSharpType(true));
			Assert.AreEqual("UNKNOWN", unknownStr.FromTSqlTypeToCSharpType());
		}
	}
}
