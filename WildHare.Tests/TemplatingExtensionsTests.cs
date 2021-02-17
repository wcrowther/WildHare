using NUnit.Framework;
using System;
using System.Collections.Generic;
using WildHare.Extensions;
using WildHare.Tests.Models;
using WildHare.Extensions.ForTemplating;
using System.IO;
using WildHare.Extensions.Xtra;
using static System.Environment;
using System.Text;

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
            string template = "InvoiceId [InvoiceId] for AccountId [AccountId].";

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

        [Test]
        public void Test_TemplateList_With_String_Basic()
        {
            var invoice = new Invoice
            {
                InvoiceId = 1000,
                AccountId = 9999,
                InvoiceItems = new List<InvoiceItem>
                {
                    new InvoiceItem { InvoiceItemId = 1, Product = "DooHickey",      Fee = 1.00m },
                    new InvoiceItem { InvoiceItemId = 2, Product = "WhatZit",        Fee = 2.00m  },
                    new InvoiceItem { InvoiceItemId = 3, Product = "WhatDoYaCallIt", Fee = 3.00m  },
                    new InvoiceItem { InvoiceItemId = 4, Product = "ThingaMaGig",    Fee = 4.00m  }
                }
            };
            string lineTemplate = "    <div>ItemId: [InvoiceItemId] Product: [Product] Fee: [Fee]</div>";
            string docTemplate = $@"
            <div>InvoiceId: [InvoiceId] AccountId: [AccountId]
            {invoice.InvoiceItems.TemplateList(lineTemplate, "\n")}
            </div>"
            .RemoveIndents();

            var expected = @"
            <div>InvoiceId: 1000 AccountId: 9999
                <div>ItemId: 1 Product: DooHickey Fee: 1.00</div>
                <div>ItemId: 2 Product: WhatZit Fee: 2.00</div>
                <div>ItemId: 3 Product: WhatDoYaCallIt Fee: 3.00</div>
                <div>ItemId: 4 Product: ThingaMaGig Fee: 4.00</div>
            </div>"
            .RemoveIndents();

            // GET .RemoveIndents() working correctly with this sample
            string getInvoiceHtml = invoice.Template(docTemplate);

            Assert.AreEqual(expected, getInvoiceHtml);
        }

    }
}
