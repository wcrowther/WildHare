using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace WildHare.Extensions
{
    public static class DocumentationExtensions
    {

        /// <summary>Gets the Xml Documentation like: @"C:\Code\Trunk\WildHare\WildHare\WildHare.xml"</summary>

        public static List<DocMember> GetXmlDocumentation(this string xmlDocPath, bool enhanced = true)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var docXml = XElement.Load(xmlDocPath);
            var assemblyName = docXml.Element("assembly").Element("name").Value;

            var memberList = docXml.Element("members").Elements()
                .Select(g => new DocMember(currentAssembly)
                {
                    Documentation = g.Element("documentation")?.Value,
                    RawName = g.Attribute("name").Value,
                    Summary = g.Element("summary").Value,
                    IsExtensionMethod = g.Element("extensionmethod") != null ? g.Element("extensionmethod").Value.ToBool() : false
                }).ToList();

            return memberList;
        }

    }
}
