using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WildHare.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>Gets a MetaModel that describes the {type} for use in code generation.</summary>
        public static MetaModel GetMetaModel(this Type type)
        {
            return new MetaModel(type);
        }

        /// <summary>Gets a MetaModel that describes the {type} for (T) for use in code generation.</summary>
        public static MetaModel GetMetaModel<T>(this IEnumerable<T> enumerable)
        {
            Type type = enumerable.GetType().GetGenericArguments()[0];
            return new MetaModel(type);
        }

        /// <summary>Gets a MetaModel that describes the {type} for the current instance for use in code generation.</summary>
        public static MetaModel GetMetaModel<T>(this T instance) where T : class
        {
            if (instance is IEnumerable)
            {
                Type type = instance.GetType();
                Type[] genericTypes = type.GetGenericArguments();
                type = genericTypes.Count() > 0 ? genericTypes[0] : type;

                return new MetaModel(type);
            }
            return new MetaModel(instance.GetType(), instance);
        }

        /// <summary>Gets a MetaModel that describes the {type} a the current dictionary for use in code generation.</summary>
        public static MetaModel GetMetaModel<K, V>(this Dictionary<K, V> dictionary)
        {
            return new MetaModel(dictionary.GetType(), dictionary);
        }

        /// <summary>Gets a list of MetaProperties for the current {type}.</summary>
        public static List<MetaProperty> GetMetaProperties(this Type type)
        {
            return new MetaModel(type).GetMetaProperties();
        }

        /// <summary>Gets a list of MetaProperties for the current {type}.</summary>
        public static List<MetaProperty> GetMetaProperties(this Type type, string exclude = null, string include = null)
        {
            return new MetaModel(type).GetMetaProperties(exclude, include);
        }

        /// <summary>Gets a list of MetaProperties for the {type} for (T) for use in code generation. You can 
        /// include either an include or exclude list (but not both) to filter in/out these properties.</summary>
        public static List<MetaProperty> GetMetaProperties<T>(this IEnumerable<T> enumerable, string exclude = null, string include = null)
        {
            Type type = enumerable.GetType().GetGenericArguments()[0];

            return new MetaModel(type).GetMetaProperties(exclude, include);
        }

        /// <summary>Gets a list of MetaProperties for the Type for the current instance for use in code generation. You can 
        /// include either an include or exclude list (but not both) to filter in/out these properties.</summary>
        public static List<MetaProperty> GetMetaProperties<T>(this T instance, string exclude = null, string include = null)
                where T : class, new()
        {
            if (instance is IEnumerable)
            {
                Type type = instance.GetType().GetGenericArguments()[0];
                return new MetaModel(type).GetMetaProperties(exclude, include);
            }
            return new MetaModel(instance.GetType(), instance).GetMetaProperties(exclude, include);
        }

        /// <summary>Given an Assembly, returns a Type array of the types in the {namspace}.</summary>
        public static Type[] GetTypesInNamespace(this Assembly assembly, string nameSpace = null)
        {

            Type[] types = assembly.GetTypes().Where(t => !t.Name.StartsWith("<")).ToArray();

            if (!nameSpace.IsNullOrEmpty())
                types = types.Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();

            return types.OrderBy(t => t.Name).ToArray();
        }

        /// <summary></summary>
        public static MetaAssembly GetMetaAssembly(this Assembly assembly, string xmlDocumentationPath =  null)
        {
            return new MetaAssembly(assembly, xmlDocumentationPath);
        }

        /// <summary>[OBSOLETE] GetDerivedClasses has been renamed to GetDerivedTypes
        /// and will be removed in a future version.</summary>
        [Obsolete("GetDerivedClasses() has been renamed to GetDerivedTypes() and will be removed in a future version.")]
        public static Type[] GetDerivedClasses(this Type type, string[] ignoreTypeNames = null)
        {
            return type.GetDerivedClasses(ignoreTypeNames);
        }

        /// <summary>Gets an array of derived Types that are a subclass of {type}, excluding any types named in
        /// the {ingnoreTypeNames} list. Set {includeBaseType} to true to include the baseType in Type array.
        /// Use {otherAssembly} when the derived classes to find are in another Assembly.</summary>
        /// <example>Called like:  var subClassesOfTeam = typeof(Team).GetDerivedTypes();</example>
        public static Type[] GetDerivedTypes(this Type type, string[] ignoreTypeNames = null, 
                                                  bool includeBaseType = false, Assembly otherAssembly = null)
        {
            ignoreTypeNames = ignoreTypeNames ?? Array.Empty<string>();

            var assembly = otherAssembly ?? Assembly.GetAssembly(type);
            
            var types =  assembly
                        .GetTypes()
                        .Where (t => t.IsSubclassOf(type) && (!ignoreTypeNames?.Any(t.Name.Contains) ?? false) )
                        .ToList();

            if (includeBaseType)
                types.Add(type);

            return types.OrderBy(o => o.Name).ToArray();
        }

        public static Type GetCommonBaseType(this Type[] types)
        {
            // From: https: //stackoverflow.com/questions/353430/easiest-way-to-get-a-common-base-class-from-a-collection-of-types

            if (types.Length == 0)
            {
                return (typeof(object));
            }
            else if (types.Length == 1)
            { 
                return (types[0]);            
            }

            bool hasBeenChecked = false;
            Type hasBeenTested = null;

            var typeList = new Type[types.Length];

            for (int i = 0; i < types.Length; i++)
            {
                typeList[i] = types[i];
            }

            while (!hasBeenChecked)
            {
                hasBeenTested = typeList[0];
                hasBeenChecked = true;

                for (int i = 1; i < typeList.Length; i++)
                {
                    if (hasBeenTested.Equals(typeList[i]))
                        continue;
                    else
                    {
                        if (hasBeenTested.Equals(typeList[i].BaseType))
                        {
                            typeList[i] = typeList[i].BaseType;
                            continue;
                        }
                        else if (hasBeenTested.BaseType.Equals(typeList[i]))
                        {
                            for (int j = 0; j <= i - 1; j++)
                            {
                                typeList[j] = typeList[j].BaseType;
                            }

                            hasBeenChecked = false;
                            break;
                        }
                        else
                        {
                            for (int j = 0; j <= i; j++)
                            {
                                typeList[j] = typeList[j].BaseType;
                            }

                            hasBeenChecked = false;
                            break;
                        }
                    }
                }
            }
            return hasBeenTested;
        }

        public static Type[] GetCommonInterfaces(this object[] objects)
        {
            var interfaces = new Type[0];
            
            foreach (var o in objects)
            {
                var typeInterfaces = o.GetType().GetInterfaces();

                if (typeInterfaces.Length == 0)
                    return new Type[0];

                if (interfaces.Length == 0)
                {
                    interfaces = typeInterfaces;
                }
                else
                {
                    interfaces = typeInterfaces.Intersect(interfaces).ToArray();
                }
            }
            return interfaces;
        }

    }
}
