using System;
using System.Collections.Generic;
using System.Linq;

namespace WildHare.Extensions
{
    public static class DictionaryExtensions
    {
        // ==============================================================================================================
        // IDictionary<string, object>
        // ==============================================================================================================

        public static T Get<T>(this IDictionary<string, object> dictionary, string key, T defaultVal = default) 
        {
            if (dictionary.TryGetValue(key, out object val))
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }
            return defaultVal;
        }

        public static bool TryGet<T>(this IDictionary<string, object> dictionary, string key, out T value)
        {
            if (dictionary.TryGetValue(key, out object result) && result is T)
            {
                value = (T)result;
                return true;
            }
            value = default(T);

            return false;
        }

        public static string Get(this IDictionary<string, object> dictionary, string key, string defaultVal = default)
        {
            if (dictionary.TryGetValue(key, out object val))
            {
                return (string)Convert.ChangeType(val, typeof(string));
            }
            return defaultVal;
        }

        public static void Set(this IDictionary<string, object> dictionary, string key, object value)
        {
            dictionary[key] = value;
        }

        public static void Set(this IDictionary<string, object> dictionary, string key, string value)
        {
            dictionary[key] = value;
        }

        // ==============================================================================================================
        // IDictionary<string, string>
        // ==============================================================================================================

        public static T Get<T>(this IDictionary<string, string> dictionary, string key, T defaultVal = default )
        {
            if (dictionary.TryGetValue(key, out string str))
            {
                return (T) Convert.ChangeType(str, typeof(T));
            }
            return defaultVal;
        }

        public static string Get(this IDictionary<string, string> dictionary, string key, string defaultVal = default)
        {
            if (dictionary.TryGetValue(key, out string str))
            {
                return str;
            }
            return defaultVal;
        }

        public static bool TryGet<T>(this IDictionary<string, string> dictionary, string key, out T value)
        {
            if (dictionary.TryGetValue(key, out string str))
            {
                value = (T)Convert.ChangeType(str, typeof(T));

                return true;
            }
            value = default(T);

            return false;
        }

        public static void Set(this IDictionary<string, string> dictionary, string key, object value)
        {
            string str = (string) Convert.ChangeType(value, typeof(string));
            dictionary[key] = str;
        }

        public static void Set(this IDictionary<string, string> dictionary, string key, DateTime value)
        {
            //  This overload is need or the DateTime value will lose it's milliseconds on ChangeType
            dictionary[key] = value.ToString("o");
        }

        public static void Set(this IDictionary<string, string> dictionary, string key, string value)
        {
            dictionary[key] = value;
        }

        public static bool IsValid<T>(this IDictionary<string, string> dictionary, string key)
        {
            if (dictionary.TryGetValue(key, out string str))
            {
                var value = (T)Convert.ChangeType(str, typeof(T));

                return value is null ? false : true;
            }
            return false;
        }

        // ==============================================================================================================


        public static IDictionary<string, string> ToQueryDictionary(this string query)
        {
            return query.Split('&').Select(s => s.Split('=')).ToDictionary(k => k[0], v => v[1]);
        }

        public static string ToQueryString(this IDictionary<string, string> dict)
        {
            return string.Join("&", dict.Select(s => string.Join("=", s.Key, s.Value)));
        }

        // /// <summary>Gets a value from {dictionary} by the specified {key}. If it does not exist, the
        // /// method returns the {defaultVal}. If this is not specified, it is the default for that type.</summary>
        // public static TVal Get<TKey, TVal>(this Dictionary<TKey, TVal> dictionary, TKey key, TVal defaultVal = default)
        // {
        //     if (dictionary.TryGetValue(key, out TVal val))
        //     {
        //         return val;
        //     }
        //     return defaultVal;
        // }

    }
}

// Above from: https ://codereview.stackexchange.com/questions/12291/type-safe-dictionary-for-various-types
