using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace WildHare.Extensions.Xtra
{
    public static partial class XtraExtensions
    {

        /// <summary>(EXPERIMENTAL) Returns a list Xml Documentation elements.
        /// Enable in VS by checking the Properties/Build/Enable Xml Documentation checkbox. On build,
        /// an XML file is generated to the file location with all the /// comments (like this one)
        /// that have been written in the your code.</summary>
        /// <example>@"C:Git\WildHare\WildHare\WildHare.xml"</example>
        public static List<DocMember> GetXmlDocumentation(this string xmlDocPath, Assembly assemblyToDocument = null)
        {
            var docXml = XElement.Load(xmlDocPath);
            var assemblyName = docXml.Element("assembly").Element("name").Value;
            var assembly = assemblyToDocument ?? Assembly.Load(assemblyName);
            var mList = docXml.Element("members").Elements();

            var memberList = docXml.Element("members").Elements()
                .Select(g => new DocMember(g.Attribute("name").Value, assembly)
                {
                    Documentation = g.Element("documentation")?.Value,
                    Summary = g.Element("summary")?.Value
                    //Example = g.Element("example")?.Value,
                    //Params = g.Elements("param")?.Select(s => s.Value).ToList()
                }).ToList();

            return memberList.ToList();
        }

    }
}
