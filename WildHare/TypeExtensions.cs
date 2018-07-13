using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WildHare.Extensions
{
    public static class TypeExtensions
    {
        public static MetaModel GetMetaModel(this Type type)
        {
            return new MetaModel(type);
        }

        public static MetaModel GetMetaModel<T>(this IEnumerable<T> enumerable)
        {
            Type type = enumerable.GetType().GetGenericArguments()[0];
            return new MetaModel(type);
        }

        public static MetaModel GetMetaModel<T>(this T instance) where T : class, new()
        {
            if (instance is IEnumerable)
            {
                Type type = instance.GetType().GetGenericArguments()[0];
                return new MetaModel(type);
            }
            return new MetaModel(instance.GetType(), instance);
        }

        public static MetaModel GetMetaModel<K, V>(this Dictionary<K, V> dictionary)
        {
            return new MetaModel(dictionary.GetType(), dictionary);
        }

        public static List<MetaProperty> GetMetaProperties(this Type type)
        {
            return new MetaModel(type).GetMetaProperties();
        }

        public static List<MetaProperty> GetMetaProperties<T>(this IEnumerable<T> enumerable, string exclude = null, string include = null)
        {
            Type type = enumerable.GetType().GetGenericArguments()[0];

            return new MetaModel(type).GetMetaProperties(exclude, include);
        }

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

        public static Type[] GetTypesInNamespace(this Assembly assembly, string nameSpace = null)
        {

            Type[] types = assembly.GetTypes().Where(t => !t.Name.StartsWith("<")).ToArray();

            if (!nameSpace.IsNullOrEmpty())
                types = types.Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();

            return types.OrderBy(t => t.Name).ToArray();
        }
    }
}
