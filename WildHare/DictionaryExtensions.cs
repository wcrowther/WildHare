using System.Collections.Generic;

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

        public static void Set(this IDictionary<string, object> dictionary,
                               string key, object value)
        {
            dictionary[key] = value;
        }

    }
}

// Above from: https ://codereview.stackexchange.com/questions/12291/type-safe-dictionary-for-various-types

// ==========================================================================================
// NATIVE GetValueOrDefault on Dictionary already provides this functionality
// ==========================================================================================
// /<summary>Returns the Dictionary value from the supplied key or an optional {defaultValue}.
// /If a {defaultValue} is not specified, it will default to the TValue default.</summary>
//public static TValue GetOrDefault<TKey, TValue>(
//                this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default)
//{
//    if (dict.TryGetValue(key, out TValue val))
//        return val;

//    return defaultValue;
//}
