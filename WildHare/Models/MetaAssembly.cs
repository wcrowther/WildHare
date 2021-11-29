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
        private List<MetaModel> _metaTypes = null;
        private List<MetaDocumentation> _documentMemberList = null;

        public MetaAssembly(Assembly assembly, string xmlDocPath = null)
        {
            if (assembly == null)
                throw new Exception("MetaAssembly assembly cannot be null.");

            _assembly   = assembly;
            _xmlDocPath = xmlDocPath;

            _metaTypes  = GetAllMetaModels();
            _documentMemberList = GetMetaDocumentationList();
        }

        public string AssemblyName { get => _assembly.GetName().Name; }

        public List<MetaModel> GetMetaModels()
        {
            return GetAllMetaModels().Where(w => !w.TypeName.StartsWith("<")).ToList();
        }

        public List<MetaModel> GetMetaModelsForAnonymousTypes()
        {
            return GetAllMetaModels().Where(w => w.TypeName.StartsWith("<")).ToList();
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
            int spacerLength = 50;
            int tab = 5;

            string title = 
                    $@"{this.AssemblyName} Assembly
                    {"-".Repeat(25)}
                    {NewLine.Repeat(2)}";

            var sb = new StringBuilder(title.RemoveIndents());
            var metaModels = GetMetaModels();

            foreach (var model in metaModels)
            {
                sb.AppendLine("-".Repeat(spacerLength));
                sb.AppendLine(model.TypeNamespace.AddEnd(": ") + model.TypeName);
                sb.AppendLine("-".Repeat(spacerLength));

                foreach (var method in model.GetMetaMethods(includeInherited: false).OrderBy(o => o.Name))
                {
                    sb.AppendLine($"{method.Name.AddStart(" ".Repeat(tab))}{GetParamString(method)}");
                    sb.AppendLine("-".Repeat(spacerLength - tab));
                }
            }

            string path = $"{outputDirectory}/{AssemblyName}.txt";
            bool isSuccess = sb.ToString().WriteToFile(path, overwrite);

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
            _metaTypes = _metaTypes ?? new List<MetaModel>();

            if (_metaTypes.Count == 0)
            {
                foreach (var type in _assembly.GetTypes())
                {
                    _metaTypes.Add(new MetaModel(type));
                }
            }

            return _metaTypes;
        }
    }
}
