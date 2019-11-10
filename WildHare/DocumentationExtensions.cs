using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace WildHare.Extensions.Xtra
{
    public static class DocumentationExtensions
    {

        /// <summary>(EXPERIMENTAL) Returns a list the Visual Studio Build Xml Documentation.
        /// Enable in VS by checking the Properties/Build/Enable Xml Documentation checkbox. On build,
        /// an XML file is generated to the file location with all the /// comments (like this one)
        /// that have been written in the your code.</summary>
        /// <example>@"C:\Code\Trunk\WildHare\WildHare\WildHare.xml"</example>
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
