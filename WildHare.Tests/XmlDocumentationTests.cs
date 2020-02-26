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

namespace WildHare.Tests
{
    [TestFixture]
    public class XmlDocumentationTests
    {
        [Test]
        public void Test_Getting_XML_Documentation()
        {
            string pathToDocumentation = @"C:\Code\Trunk\WildHare\WildHare\WildHare.xml";

            var docXml = XElement.Load(pathToDocumentation);
            var assemblyName = docXml.Element("assembly").Element("name").Value;
            var memberList = docXml.Element("members").Elements()
                .Select(g => (
                    name: g.Attribute("name").Value,
                    summary: g.Element("summary").Value
                ));

            Assert.AreEqual(@"C:\Code\Trunk\WildHare\WildHare\WildHare.xml", pathToDocumentation);
            Assert.IsNotNull(docXml);
        }
    }
}
