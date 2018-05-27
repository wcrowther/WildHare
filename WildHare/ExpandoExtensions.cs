using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace WildHare.Extensions.ForExpando
{
    public static class ExpandoExtensions
    {
        public static T GetByItemName<T>(this ExpandoObject expando, string name)
        {
            var dictionary = (IDictionary<string, object>) expando;

            return dictionary.ContainsKey(name) ? (T)dictionary[name] : default(T);
        }

        public static void AddItemByName<T>(this ExpandoObject expando, string name, T value)
        {
            var dictionary = (IDictionary<string, object>)expando;

            if (value == null)
                value = default(T);

            if (dictionary.ContainsKey(name))
            {
                dictionary[name] = value;
            }
            else
            {
                dictionary.Add(name, value);
            }
        }

        public static void RemoveItemByName(this ExpandoObject expando, string name)
        {
            var dictionary = (IDictionary<string, object>) expando;

            if (dictionary.ContainsKey(name))
            {
                dictionary.Remove(name);
            }
        }

        // Useful extension as Expandos can be cast as IDictionary<TKey, TVal> and this extension makes inlining easier

        public static TVal Get<TKey, TVal>(this Dictionary<TKey, TVal> dictionary, TKey key, TVal defaultVal = default(TVal))
        {
            if (dictionary.TryGetValue(key, out TVal val))
            {
                return val;
            }
            return defaultVal;
        }
    }
}
