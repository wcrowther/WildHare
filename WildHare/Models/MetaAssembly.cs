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

        public List<MetaModel> GetMetaModels(string exclude = null, string include = null, bool hideAnonymousModels = true)
        {
            if (!exclude.IsNullOrEmpty() && !include.IsNullOrEmpty())
            {
                throw new Exception("The GetMetaModels method only accepts the exclude OR the include list.");
            }

            var metaModelList = hideAnonymousModels ? GetAllMetaModels().Where(w => !w.TypeName.StartsWith(anonStartArray)) : GetAllMetaModels();

            if (!exclude.IsNullOrEmpty())
            {
                var excludeList = exclude.Split(',').Select(a => a.Trim());
                return metaModelList.Where(w => !excludeList.Any(e => w.TypeNamespace == e)).ToList();
            }

            if (!include.IsNullOrEmpty())
            {
                var includeList = include.Split(',').Select(a => a.Trim());
                return metaModelList.Where(w => includeList.Any(e => w.TypeNamespace == e)).ToList(); // returns fields not in list;
            }

            return metaModelList.ToList();
        }

        public List<MetaNamespace> GetMetaModelsGroupedByNamespace(string exclude = null, string include = null, bool hideAnonymousModels = true)
        {
            var namespaces = GetMetaModels(exclude, include, hideAnonymousModels).ToLookup(l => l.TypeNamespace);
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
            string header = "=".Repeat(150);
            string spacer = "-".Repeat(145);
            string tab = " ".Repeat(5);
            int matches = 0;
            int methodCount = 0;

            string title = 
                    $@"
                    {header}
                    {this.AssemblyName} ASSEMBLY
                    ";

            var sb = new StringBuilder(title.RemoveIndents());
            var metaNamespaces = GetMetaModelsGroupedByNamespace();

            foreach (var ns in metaNamespaces)
            {
                sb.AppendLine(header);
                sb.AppendLine(ns.NamespaceName);
                sb.AppendLine(header);

                foreach (var metaModel in ns.MetaModels)
                {
                    sb.AppendLine($"{tab}{metaModel.TypeFullName}");
                    sb.AppendLine($"{tab}{spacer}");

                    foreach (var method in metaModel.GetMetaMethods(includeInherited: false).OrderBy(o => o.Name))
                    {
                        sb.AppendLine($"{tab}{tab}{method.Name}{GetParamString(method)}");
                        sb.AppendLine($"{tab}{tab}{tab}{metaModel.TypeFullName}.{method.DocMemberName} : Is Meta.DocMemberName");
                        
                        var doc = this._documentMemberList.FirstOrDefault(f => f.MemberName == $"{ metaModel.TypeFullName}.{ method.DocMemberName}");

                        if (doc != null)
                        {
                            if ($"{metaModel.TypeFullName}.{method.DocMemberName}" == doc.MemberName)
                            {
                                sb.AppendLine($"{tab} >>> Match");
                                matches++;
                            }

                            sb.AppendLine($"{tab}{tab}{doc.MemberName} : Is MemberName in XMLDoc");
                            sb.AppendLine($"{tab}{tab}***  {doc.Summary.ReplaceLineReturns().CombineSpaces()}");
                        }

                        sb.AppendLine($"{tab}{spacer}");

                        methodCount++;
                    }
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
                sb.Append($"{param.ParameterMetaType.Name} {param.Name}{commaSpace}");
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
