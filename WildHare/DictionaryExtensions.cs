using System.Collections.Generic;
using System.Linq;

namespace WildHare.Extensions
{
    public static class DictionaryExtensions
    {
        public static T Get<T>(this IDictionary<string, object> dictionary, string key)
        {
            return (T)dictionary[key];
        }

        public static bool TryGet<T>(this IDictionary<string, object> dictionary,
                                     string key, out T value)
        {
            object result;
            if (dictionary.TryGetValue(key, out result) && result is T)
            {
                value = (T)result;
                return true;
            }
            value = default(T);
            return false;
        }

        public static void Set(this IDictionary<string, object> dictionary, string key, object value)
        {
            dictionary[key] = value;
        }

        public static IDictionary<string, string> ToQueryDictionary(this string query)
        {
            return query.Split('&').Select(s => s.Split('=')).ToDictionary(k => k[0], v => v[1]);
        }

        public static string ToQueryString(this IDictionary<string, string> dict)
        {
            return string.Join("&", dict.Select(s => string.Join("=", s.Key, s.Value)));
        }

    }
}

// Above from: https ://codereview.stackexchange.com/questions/12291/type-safe-dictionary-for-various-types
