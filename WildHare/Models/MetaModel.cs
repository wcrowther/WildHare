using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Extensions.Xtra;

namespace WildHare
{
    public class MetaModel
    {
        public MetaModel(Type type, object instance = null)
        {
            if (type == null)
                throw new Exception("MetaModel type cannot be null.");

            _type = type;
            _instance = instance;
        }

        private readonly object _instance;
        private readonly Type _type;
        private string xmlDocMemberName = null;

        private List<MetaField> fields = null;
        private List<MetaProperty> properties = null;
        private List<MetaMethod> methods = null;

        public string TypeName => _type.Name; 

        public string TypeFullName => _type.FullName;

        public string TypeNamespace => _type.Namespace; 

        public string PrimaryKeyName => PrimaryKeyMeta != null ? PrimaryKeyMeta.Name : ""; 

        public bool IsDictionary => _type.IsGenericType && _type.GetGenericTypeDefinition() == typeof(Dictionary<,>); 

        public Type DictionaryKeyType => IsDictionary ? _type.GetGenericArguments()[0] : null; 

        public Type DictionaryValueType => IsDictionary ? _type.GetGenericArguments()[1] : null; 

        public bool IsAnonymousType => TypeName.StartsWith(new[] { "<", "_" }); 

        public bool IsStaticType => _type.IsAbstract && _type.IsSealed; 

        public MetaProperty PrimaryKeyMeta => properties.FirstOrDefault(a => a.IsKey == true); 

        public bool Implements(string interfaceName) => _type.GetInterfaces().Any(a => a.Name == interfaceName);

        public List<MetaMethod> GetMetaMethods(string exclude = null, string include = null, bool includeInherited = false)
        {
            if (!exclude.IsNullOrEmpty() && !include.IsNullOrEmpty())
            {
                throw new Exception("The GetMetaMethods method only accepts the exclude OR the include list.");
            }

            var metaMethodList = includeInherited ? MetaMethods : MetaMethods.Where(w => w.DeclaringType.Name == this.TypeName);

            if (!exclude.IsNullOrEmpty())
            {
                var excludeList = exclude.Split(',').Select(a => a.Trim());
                return metaMethodList.Where(w => !excludeList.Any(e => w.Name == e)).ToList();
            }

            if (!include.IsNullOrEmpty())
            {
                var includeList = include.Split(',').Select(a => a.Trim());
                return metaMethodList.Where(w => includeList.Any(e => w.Name == e)).ToList(); // returns fields not in list;
            }

            return metaMethodList.ToList();
        }

        public List<MetaProperty> GetMetaProperties(string exclude = null, string include = null)
        {
            if (!exclude.IsNullOrEmpty() && !include.IsNullOrEmpty())
            {
                throw new Exception("The GetMetaProperties method only accepts the exclude OR the include list.");
            }

            if (!exclude.IsNullOrEmpty())
            {
                var excludeList = exclude.Split(',').Select(a => a.Trim());
                return MetaProperties.Where(w => !excludeList.Any(e => w.Name == e)).ToList();
            }

            if (!include.IsNullOrEmpty())
            {
                var includeList = include.Split(',').Select(a => a.Trim());
                return MetaProperties.Where(w => includeList.Any(e => w.Name == e)).ToList(); // returns fields not in list;
            }

            return MetaProperties.ToList();
        }

        public string Summary { get; set; }

        public string XmlDocMemberName => xmlDocMemberName ?? GetXmlDocMemberName();

        public override string ToString() => $"{TypeNamespace.AddEnd(".")}{TypeName} Properties: {MetaProperties.Count} Methods: {MetaMethods.Count} Fields: {MetaFields.Count}";


        // =========================================================================================================
        // Private
        // =========================================================================================================

        private List<MetaProperty> MetaProperties
        {
            get
            {
                properties = properties ?? new List<MetaProperty>();

                if (properties.Count == 0)
                {
                    foreach (var propertyInfo in _type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {

                        properties.Add(new MetaProperty(propertyInfo, _instance));
                    }
                }
                return properties;
            }
        }

        private List<MetaMethod> MetaMethods
        {
            // Includes inherited Methods but they are excluded by default in public GetMetaMethods()
            // Getters and Setters are encapulated in MetaProperties and are excluded here.

            get
            {
                methods = methods ?? new List<MetaMethod>();

                if (methods.Count == 0)
                {
                    foreach (var methodInfo in _type.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public))
                    {
                        var metaMethod = new MetaMethod(methodInfo);

                        if (!metaMethod.IsGetter && !metaMethod.IsSetter)
                        {
                            Debug.WriteLine("Name: " + metaMethod.Name);
                            Debug.WriteLine("DeclaringType.Name: " + metaMethod.DeclaringType.Name);
                            Debug.WriteLine("TypeName: " + this.TypeName);
                            Debug.WriteLine("methodInfo.DeclaringType.FullName: " + methodInfo.DeclaringType.FullName);
                            Debug.WriteLine("=".Repeat(25));

                            methods.Add(metaMethod);
                        }
                    }
                }
                return methods;
            }
        }

        private List<MetaField> MetaFields
        {
            get
            {
                fields = fields ?? new List<MetaField>();

                if (fields.Count == 0)
                {
                    foreach (var fieldInfo in _type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    {
                        fields.Add(new MetaField(fieldInfo));
                    }
                }
                return fields;
            }
        }

        private List<MetaParameter> GetMetaParameters(MethodInfo methodInfo)
        {
            var metaParmeters = new List<MetaParameter>();

            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                metaParmeters.Add(new MetaParameter(parameterInfo));
            }

            return metaParmeters;
        }

        private string GetXmlDocMemberName()
        {
            return TypeFullName;
        }

    }
}
