using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;
using WildHare.Tests.Models;
using WildHare.Xtra;
using static System.Environment;

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
        public void BasicTypeNameFromValue_Basic()
        {
            string string1 = "frog";
            string string2 = "";
            string string3 = null;

            Assert.AreEqual("string", string1.BasicTypeNameFromValue());
            Assert.AreEqual("string", string2.BasicTypeNameFromValue());
            Assert.AreEqual("string", string3.BasicTypeNameFromValue());
        }

        [Test]
        public void BasicTypeNameFromValue_Bool()
        {
            string string1 = "true";
            string string2 = "false";
            string string3 = "True";
            string string4 = "False";

            Assert.AreEqual("bool", string1.BasicTypeNameFromValue());
            Assert.AreEqual("bool", string2.BasicTypeNameFromValue());
            Assert.AreEqual("bool", string3.BasicTypeNameFromValue());
            Assert.AreEqual("bool", string4.BasicTypeNameFromValue());
        }

        [Test]
        public void BasicTypeNameFromValue_Numbers()
        {
            string string1 = "0";
            string string2 = "1";
            string string3 = "12000";
            string string4 = "12,000";
            string string5 = "-1,000,000";
            string string6 = "2,147,483,647";
            string string7 = "2,147,483,648";
            string string8 = "1.0";
            string string9 = "1.333";
            string string10 = "-1.333";
            string string11 = "giraffe";
            string string12 = "";
            string string13 = null;

            Assert.AreEqual("int",      string1.BasicTypeNameFromValue());
            Assert.AreEqual("int",      string2.BasicTypeNameFromValue());
            Assert.AreEqual("int",      string3.BasicTypeNameFromValue());
            Assert.AreEqual("int",      string4.BasicTypeNameFromValue());
            Assert.AreEqual("int",      string5.BasicTypeNameFromValue());
            Assert.AreEqual("int",      string6.BasicTypeNameFromValue());
            Assert.AreEqual("long",     string7.BasicTypeNameFromValue());
            Assert.AreEqual("decimal",  string8.BasicTypeNameFromValue());
            Assert.AreEqual("decimal",  string9.BasicTypeNameFromValue());
            Assert.AreEqual("decimal",  string10.BasicTypeNameFromValue());
            Assert.AreEqual("string",   string11.BasicTypeNameFromValue());
            Assert.AreEqual("string",   string12.BasicTypeNameFromValue());
            Assert.AreEqual("string",   string13.BasicTypeNameFromValue());
        }

        [Test]
        public void BasicTypeNameFromValue_Strict_Throws_For_Empty()
        {
            string string1 = "";

            var ex = Assert.Throws<Exception>
            (
                () => string1.BasicTypeNameFromValue(true)
            );

            string expectedMessage = "The BasicTypeNameFromValue value cannot be empty or null when in strict mode.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void BasicTypeNameFromValue_Strict_Throws_For_Null()
        {
            string string1 = null;
            string customErrorMessage = "Something went wrong...";

            var ex = Assert.Throws<Exception>
            (
                () => string1.BasicTypeNameFromValue(true, customErrorMessage)
            );

            Assert.AreEqual("Something went wrong...", ex.Message);
        }

        [Test]
        public void Test_Template_For_Object_With_String_Template()
        {
            string template = "InvoiceId [InvoiceId] for AccountId [AccountId]." + NewLine;

            var invoice = new Invoice
            {
                AccountId = 1000,
                InvoiceId = 222
            };

            string result = invoice.Template(template);

            Assert.AreEqual("InvoiceId 222 for AccountId 1000." + NewLine, result);
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

            Assert.AreEqual("InvoiceId 222 for AccountId 1000." + NewLine, result);
        }

        [Test]
        public void Test_TemplateList_With_String_Basic()
        {
            string pathRoot = XtraExtensions.GetApplicationRoot();
            string outputPath = $@"{pathRoot}\Directory0\SimpleOutput.txt";

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
            {invoice.InvoiceItems.TemplateList(lineTemplate, NewLine)}
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

            string getInvoiceHtml = invoice.Template(docTemplate);

            Assert.AreEqual(expected, getInvoiceHtml);

            getInvoiceHtml.WriteToFile(outputPath);

            string outputString = new FileInfo(outputPath).ReadFile();

            Assert.AreEqual(getInvoiceHtml, getInvoiceHtml);

        }

        [Test]
        public void Test_Raw_String_LineBreaks()
        {
            // For line ends in Raw String to be CR/LF same as NewLine 
            // See Advanced Save Options (may have to be added to File menu)
            
            var expected = @"
            <div>InvoiceId: 1000 AccountId: 9999
                <div>ItemId: 1 Product: DooHickey Fee: 1.00</div>
                <div>ItemId: 2 Product: WhatZit Fee: 2.00</div>
                <div>ItemId: 3 Product: WhatDoYaCallIt Fee: 3.00</div>
                <div>ItemId: 4 Product: ThingaMaGig Fee: 4.00</div>
            </div>"
            .RemoveIndents();

            var actual = """
            <div>InvoiceId: 1000 AccountId: 9999
                <div>ItemId: 1 Product: DooHickey Fee: 1.00</div>
                <div>ItemId: 2 Product: WhatZit Fee: 2.00</div>
                <div>ItemId: 3 Product: WhatDoYaCallIt Fee: 3.00</div>
                <div>ItemId: 4 Product: ThingaMaGig Fee: 4.00</div>
            </div>
            """;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Template_With_Dictionary()
        {
            var dictionary = new Dictionary<string, object>
            {
                { "AccountNumber", "ABC12345" },
                { "NameOne", "John" },
                { "NameTwo", "Jean" },
                { "NameThree", "Joan" },
                { "NameFour", "Jeez" },
            };

            var template = @"
            <div>Account Number: {{AccountNumber}}
                <div>NameOne:   {{NameOne}}</div>
                <div>NameTwo:   {{NameTwo}}</div>
                <div>NameThree: {{NameThree}}</div>
                <div>NameFour:  {{NameFour}}</div>
            </div>"
            .RemoveIndents();

            string actual = dictionary.Template(template,"{{","}}");

            var expected = @"
            <div>Account Number: ABC12345
                <div>NameOne:   John</div>
                <div>NameTwo:   Jean</div>
                <div>NameThree: Joan</div>
                <div>NameFour:  Jeez</div>
            </div>"
            .RemoveIndents();

            Assert.AreEqual(expected, actual);
        }
    }
}
