using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<MetaDocumentation> _xmDocMemberList = null;
        private string[] anonStartArray = { "<", "_" };

        public MetaAssembly(Assembly assembly, string xmlDocPath = null)
        {
            if (assembly == null)
                throw new Exception("MetaAssembly assembly cannot be null.");

            _assembly   = assembly;
            _xmlDocPath = xmlDocPath;

            _metaModels  = GetAllMetaModels();
            _xmDocMemberList = GetMetaDocumentationList();
        }

        public string AssemblyName { get => _assembly.GetName().Name; }

        public List<MetaModel> GetMetaModels(string exclude = null, string include = null)
        {
            if (!exclude.IsNullOrEmpty() && !include.IsNullOrEmpty())
            {
                throw new Exception("The GetMetaModels method only accepts the exclude OR the include list.");
            }

            var metaModelList = GetAllMetaModels().Where(w => !w.TypeName.StartsWith(anonStartArray)).ToList();

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

            return metaModelList;
        }

        public List<MetaNamespace> GetMetaModelsGroupedByNamespaces(string exclude = null, string include = null)
        {
            var namespaces = GetMetaModels(exclude, include).ToLookup(l => l.TypeNamespace);
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
            if (!_xmlDocPath.IsNullOrSpace() && _xmDocMemberList == null)
            {
                var docXml = XElement.Load(_xmlDocPath);

                if (docXml == null)
                    throw new Exception($"Not able to find XML Document at the supplied 'xmlDocPath'.");

                _xmDocMemberList =  docXml.Element("members").Elements()
                                    .Select(g => new MetaDocumentation(g.Attribute("name").Value)
                                    {
                                        Documentation = g.Element("documentation")?.Value,
                                        Summary = g.Element("summary")?.Value

                                    }).ToList();
            }

            return _xmDocMemberList ?? new List<MetaDocumentation>(); 
        }    

        public override string ToString() => $"MetaAssembly: {AssemblyName} Type count: {GetMetaModels().Count}";

        public bool WriteMetaAssemblyDescriptionToFile(string outputDirectory, string includeNamespaces = null, bool overwrite = false)
        {
            string header = "=".Repeat(150);
            string spacer = "-".Repeat(145);
            string tab = " ".Repeat(5);
            int matches = 0;
            int methodCount = 0;

            var sb = new StringBuilder();
            var metaNamespaces = GetMetaModelsGroupedByNamespaces(include: includeNamespaces);

            foreach (var ns in metaNamespaces)
            {
                sb.AppendLine(header);
                sb.AppendLine(ns.NamespaceName);
                sb.AppendLine();

                foreach (var metaModel in ns.MetaModels)
                {
                    sb.AppendLine($"{tab}{metaModel.TypeFullName}");
                    sb.AppendLine();

                    foreach (var method in metaModel.GetMetaMethods(includeInherited: false).OrderBy(o => o.Name))
                    {
                        // sb.AppendLine($"{tab}{tab}{method.Name}{GetParamString(method)}");

                        string match;

                        var xmlDoc = _xmDocMemberList.FirstOrDefault(f => f.MemberName == $"{ method.XmlDocMemberName}");

                        if (xmlDoc != null && $"{method.XmlDocMemberName}" == xmlDoc.MemberName)
                        {
                            match = $"{tab}  >> ";
                            matches++;
                        }
                        else
                        { 
                            match = $"{tab}{tab}";
                        }

                        sb.AppendLine($"{match}{method.XmlDocMemberName}");  //  : Is MetaMethod.DocMemberName

                        // if (doc != null && !doc.Summary.IsNullOrSpace())
                        // {
                        //     // sb.AppendLine($"{match}{doc.MemberName} : Is MemberName in XMLDoc");
                        //     sb.AppendLine($"{tab}{tab}{tab}   Doc Summary: {doc.Summary.ReplaceLineReturns().CombineSpaces()}");
                        // }

                        // sb.AppendLine($"{tab}{spacer}");

                        methodCount++;
                    }
                    sb.AppendLine();
                }
            }

            string title =
            $@"
            {header}
            {this.AssemblyName} ASSEMBLY - {matches} matches of {methodCount} methods.
            {header}{NewLine}
            ";

            string path = $@"{outputDirectory}\{AssemblyName}AssemblyDescription.txt";
            bool isSuccess = sb.ToString()
                               .AddStart(title.RemoveIndents())
                               .WriteToFile(path, overwrite);

            return isSuccess;
        }

        public bool WriteMetaAssemblyNotesToJsonFile(string outputDirectory, string includedNamespaces, bool overwrite = false)
        {
            string tab = " ".Repeat(5);
            var sb = new StringBuilder();
            var metaNamespaces = GetMetaModelsGroupedByNamespaces(include: includedNamespaces) ;
            int nsCount = 0;

            foreach (var ns in metaNamespaces)
            {
                nsCount++;

                sb.AppendLine($"{tab}\"{ns.NamespaceName}\" : {{");

                foreach (var (mm, index) in ns.MetaModels.WithIndex())
                {
                    sb.AppendLine($"{tab}{tab}\"{mm.TypeName}\" : {{");

                    var methods = mm.GetMetaMethods(includeInherited: false).OrderBy(o => o.Name).ToList();

                    int mCount = 0;

                    foreach (var method in methods)
                    {
                        mCount++;

                        string mComma = (methods.Count == mCount) ? "" : ",";
                        sb.AppendLine($"{tab}{tab}{tab}\"{method.Name}{GetGenericArguments(method)}{GetParamString(method)}\" : \"\"{mComma}");
                    }

                    Debug.WriteLine($"{mm.TypeName} Count: {ns.MetaModels.Count} Index: {index + 1}");

                    string mmComma = (ns.MetaModels.Count == index + 1) ? "" : ","; 
                    sb.AppendLine($"{tab}{tab}}}{mmComma}");
                }

                string nsComma = (metaNamespaces.Count == nsCount) ? "" : ",";
                sb.AppendLine($"{tab}}}{nsComma}");
            }

            string path = $@"{outputDirectory}\{AssemblyName}Notes.json";
            bool isSuccess = sb.ToString().RemoveEnd(",")
                                            .AddStartEnd("{" + NewLine, "}" + NewLine)
                                            .WriteToFile(path, overwrite);

            return isSuccess;
        }

        public bool WriteXMLDocumentMemberNamesToFile(string outputDirectory, bool overwrite = false)
        {
            var documentNameList =  new List<string>();
            var sb = new StringBuilder();

            if (_xmlDocPath.IsNullOrSpace())
            {
                return false;
            }

            var docXml = XElement.Load(_xmlDocPath);

            if (docXml == null)
                throw new Exception($"Not able to find XML Document at the supplied 'xmlDocPath'.");

            documentNameList = docXml.Element("members")
                                     .Elements()
                                     .Select(g => g.Attribute("name").Value)
                                     .ToList();

            foreach (var name in documentNameList)
            {
                sb.AppendLine($"{name}");
            }

            string path = $@"{outputDirectory}\{AssemblyName}XMLDocumentNames.txt";

            bool isSuccess = sb.ToString().AddStartEnd(NewLine + NewLine).WriteToFile(path, overwrite);

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

        private string GetGenericArguments(MetaMethod method)
        {
            var genericArguments = method.GetGenericArguments();

            if(genericArguments.Count() == 0)
                return "";

            return $"``{genericArguments.Count()}";
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
