using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WildHare.Extensions.ForObject;
using WildHare.Extensions.ForString;

namespace WildHare.Models
{
    public class MetaModel
    {

        // --------------------------------------------------------------
        // Private properties
        // --------------------------------------------------------------

        private Type _type;
        private List<MetaProperty> properties = null;
        private object _instance;

        private List<MetaProperty> MetaProperties
        {
            get
            {
                if (_type == null)
                    throw new Exception("MetaModel type cannot be null.");

                if (properties == null)
                    properties = new List<MetaProperty>();

                if (properties.Count == 0)
                {
                    foreach (var propertyInfo in _type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {

                        properties.Add(new MetaProperty(propertyInfo, _instance)
                        {
                            Name = propertyInfo.Name
                        });
                    }
                }
                return properties;
            }
        }

        // --------------------------------------------------------------
        // Constructors
        // --------------------------------------------------------------

        public MetaModel(Type type, object instance = null)
        {
            _type = type;
            _instance = instance;
        }

        // --------------------------------------------------------------
        // Public Properties
        // --------------------------------------------------------------

        public string TypeName
        {
            get
            {
                if (_type == null)
                    throw new Exception("MetaModel type cannot be null.");

                return _type.Name;
            }
        }

        public string PrimaryKeyName
        {
            get
            {
                return PrimaryKeyMeta.Is() ? PrimaryKeyMeta.Name : "";
            }
        }

        public bool IsDictionary
        {
            get
            {
                return _type.IsGenericType && _type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
            }
        }

        public Type DictionaryKeyType
        {
            get
            {
                return IsDictionary ? _type.GetGenericArguments()[0] : null;
            }
        }

        public Type DictionaryValueType
        {
            get
            {
                return IsDictionary ? _type.GetGenericArguments()[1] : null;
            }
        }

        public MetaProperty PrimaryKeyMeta
        {
            get
            {
                return properties.FirstOrDefault(a => a.IsKey == true);
            }
        }


        // --------------------------------------------------------------
        // Public Methods
        // --------------------------------------------------------------

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

        public bool Implements(string interfaceName)
        {
            return _type.GetInterfaces().Any(a => a.Name == interfaceName);
        }

        public override string ToString()
        {
            return $"MetaModel for {TypeName} ({MetaProperties.Count} MetaProperties)";
        }

    }
}
