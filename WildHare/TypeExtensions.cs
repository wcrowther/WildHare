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

        public static Type[] GetDerivedClasses(this Type type, string[] ignoreTypeNames = null) 
        {
            ignoreTypeNames = ignoreTypeNames ?? new string[0];

            return Assembly.GetAssembly(type)
                            .GetTypes()
                            .Where
                            (
                                t => t.IsSubclassOf(type) &&
                                (!ignoreTypeNames?.Any(t.Name.Contains) ?? false)
                            )
                            .OrderBy(o => o.Name)
                            .ToArray();
        }
    }
}
