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
        private string _xmlDocPath = null;
        private List<MetaModel> _metaModels = null;
        private List<MetaDocumentation> _documentMemberList = null;
        private string[] anonStartArray = { "<", "_" };

        public MetaAssembly(Assembly assembly, string xmlDocPath = null)
        {
            if (assembly == null)
                throw new Exception("MetaAssembly assembly cannot be null.");

            _assembly   = assembly;
            _xmlDocPath = xmlDocPath;

            _metaModels  = GetAllMetaModels();
            _documentMemberList = GetMetaDocumentationList();
        }

        public string AssemblyName { get => _assembly.GetName().Name; }

        public List<MetaModel> GetMetaModels()
        {
            return GetAllMetaModels().Where(w => !w.TypeName.StartsWith(anonStartArray)).ToList();
        }

        public List<MetaNamespace> GetMetaModelsInNamespaces()
        {
            var namespaces = GetMetaModels().ToLookup(l => l.TypeNamespace);
            var metaNamespaces = new List<MetaNamespace>();

            foreach (var ns in namespaces)
            {
                metaNamespaces.Add(new MetaNamespace(ns.Key) { MetaModels = ns.ToList()});
            }

            return metaNamespaces.OrderBy(o => o.NamespaceName).ToList();
        }

        public List<MetaModel> GetMetaModelsForAnonymousTypes()
        {
            return GetAllMetaModels().Where(w => w.TypeName.StartsWith(anonStartArray)).ToList();
        }

        public List<MetaDocumentation> GetMetaDocumentationList()
        {
            if (!_xmlDocPath.IsNullOrSpace() && _documentMemberList == null)
            {
                var docXml = XElement.Load(_xmlDocPath);

                if (docXml == null)
                    throw new Exception($"Not able to find XML Document at the supplied 'xmlDocPath'.");

                _documentMemberList =  docXml.Element("members").Elements()
                                    .Select(g => new MetaDocumentation(g.Attribute("name").Value)
                                    {
                                        Documentation = g.Element("documentation")?.Value,
                                        Summary = g.Element("summary")?.Value

                                    }).ToList();
            }

            return _documentMemberList ?? new List<MetaDocumentation>(); 
        }    

        public override string ToString() => $"MetaAssembly: {AssemblyName} Type count: {GetMetaModels().Count}";

        public bool WriteMetaAssemblyToFile(string outputDirectory, bool overwrite = false)
        {
            string spacer = "-".Repeat(100);
            string tab = " ".Repeat(5);
            int matches = 0;
            int methodCount = 0;

            string title = 
                    $@"
                    {spacer}
                    {this.AssemblyName} ASSEMBLY
                    {spacer}
                    {NewLine.Repeat(2)}";

            var sb = new StringBuilder(title.RemoveIndents());
            var metaModels = GetMetaModels();

            foreach (var model in metaModels)
            {
                sb.AppendLine(model.TypeFullName);
                sb.AppendLine(spacer);

                foreach (var method in model.GetMetaMethods(includeInherited: false).OrderBy(o => o.Name))
                {
                    sb.AppendLine($"{tab}{method.Name}{GetParamString(method)}");
                    sb.AppendLine($"{tab}{tab}{model.TypeFullName}.{method.DocMemberName} : Is Meta.DocMemberName");
                    var doc = this._documentMemberList.FirstOrDefault(f => f.MemberName == $"{ model.TypeFullName}.{ method.DocMemberName}");

                    if (doc != null)
                    {
                        if ($"{model.TypeFullName}.{method.DocMemberName}" == doc.MemberName)
                        {
                            sb.AppendLine($"{tab} >>> Match");
                            matches++;
                        }

                        sb.AppendLine($"{tab}{tab}{doc.MemberName} : Is MemberName in XMLDoc");
                        sb.AppendLine($"{tab}{tab}***  {doc.Summary.ReplaceLineReturns().CombineSpaces()}");
                    }

                    sb.AppendLine(spacer);

                    methodCount++;
                }
            }

            string path = $@"{outputDirectory}\{AssemblyName}.txt";
            bool isSuccess =  sb.ToString()
                                .AddStart($"{matches} matches of {methodCount} methods.{NewLine}")
                                .WriteToFile(path, overwrite);

            return isSuccess;
        }

        // ==============================================================================================
        // PRIVATE
        // ==============================================================================================

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

        private List<MetaModel> GetAllMetaModels()
        {
            _metaModels = _metaModels ?? new List<MetaModel>();

            if (_metaModels.Count == 0)
            {
                foreach (var type in _assembly.GetTypes())
                {
                    _metaModels.Add(new MetaModel(type));
                }
            }

            return _metaModels;
        }
    }
}
