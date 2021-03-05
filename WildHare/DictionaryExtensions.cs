using System.Collections.Generic;

namespace WildHare.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>Returns the Dictionary value from the supplied key or an optional {defaultValue}.
        /// If a {defaultValue} is not specified, it will default to the TValue default.</summary>
        public static TValue GetOrDefault<TKey, TValue>(
                        this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default)
        {
            if (dict.TryGetValue(key, out TValue val))
                return val;

            return defaultValue;
        }

    }
}
