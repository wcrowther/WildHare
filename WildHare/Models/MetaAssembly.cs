using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using WildHare.Extensions;
using WildHare.Extensions.Xtra;
using static System.Environment;


namespace WildHare
{
    public class MetaAssembly
    {
        private readonly Assembly _assembly;
        private List<MetaModel> metaTypes = null;


        public MetaAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new Exception("MetaAssembly assembly cannot be null.");
            _assembly = assembly;
        }

        public string AssemblyName { get => _assembly.GetName().Name; }

        public List<MetaModel> GetMetaModels()
        {
            if (metaTypes == null)
                metaTypes = new List<MetaModel>();

            if (metaTypes.Count == 0)
            {
                foreach (var type in _assembly.GetTypes())
                {
                    if (!type.Name.StartsWith(new[]{"<", "_"}))
                    { 
                        metaTypes.Add(new MetaModel(type));                    
                    }
                }
            }
            return metaTypes;

        }

        public override string ToString() => $"MetaAssembly: {AssemblyName} Type count: {GetMetaModels().Count}";

        public bool WriteMetaAssemblyToFile(string outputDirectory, string xmlDocPath = null, bool overwrite = false)
        {
            if (!xmlDocPath.IsNullOrSpace())
            {
                var docXml = XElement.Load(xmlDocPath);

                if (docXml == null)
                    throw new Exception($"Not able to find XML Document at the supplied 'xmlDocPath'.");

                var memberList = docXml.Element("members").Elements()
                                .Select(g => new MetaDocumentation(g.Attribute("name").Value)
                                {
                                    Documentation = g.Element("documentation")?.Value,
                                    Summary = g.Element("summary")?.Value
                                }).ToList();
            }

            string title = 
                    $@"{this.AssemblyName} Assembly
                    {"-".Repeat(25)}
                    {NewLine.Repeat(2)}";

            var sb = new StringBuilder(title.RemoveIndents());
            var metaModels = GetMetaModels();

            foreach (var model in metaModels)
            {
                sb.Append(model.TypeNamespace.AddEnd(": ") + model.TypeName + NewLine);
                foreach (var method in model.GetMetaMethods(includeInherited: false).OrderBy(o => o.Name))
                {
                    sb.AppendLine($"{method.Name.AddStart("   ")}{GetParamString(method)}");
                }
            }

            string path = $"{outputDirectory}/{AssemblyName}.txt";
            bool isSuccess = sb.ToString().WriteToFile(path, overwrite);

            return isSuccess;
        }

        private string GetParamString(MetaMethod method)
        {
            string commaSpace = ", ";
            var sb = new StringBuilder("(");

            foreach (var param in method.Parameters)
            {
                sb.Append($"{param.ParameterType.Name} {param.Name}{commaSpace}");
            }

            return sb.ToString().RemoveEnd(commaSpace).EnsureStartEnd("(", ")");
        }
    }
}
