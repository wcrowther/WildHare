using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace WildHare.Extensions
{

    /*  NOTE: 'ExpandoObject's are generally used as a 'dynamic' that is evaluated at runtime,
        but 'dynamic' does not support extension methods. Therefore to use this collection of 
        extension methods, the 'dynamic' expression must be cast to an ExpandoObject. These 
        methods can also be invoked by calling them directly using non-extension syntax.
        >> See the unit tests for examples. <<
    */

    public static class ExpandoExtensions
    {

        /// <summary> Gets item by string {name} but does not throw an exception if it does not exist.
        /// If an ExpandoObject is cast to dynamic, be sure to cast back to ExpandoObject to see this extension method.</summary>
        public static T Get<T>(this ExpandoObject expando, string name)
        {
            var dictionary = (IDictionary<string, object>)expando;

            return dictionary != null && dictionary.ContainsKey(name) ? (T)dictionary[name] : default(T);
        }

        /// <summary> Gets a string by string {name} but does not throw an exception if it does not exist.
        /// If an ExpandoObject is cast to dynamic, be sure to cast back to ExpandoObject to see this extension method.</summary>
        public static string Get(this ExpandoObject expando, string name)
        {
            var dictionary = (IDictionary<string, object>) expando;

            return dictionary != null && dictionary.ContainsKey(name) ? dictionary[name]?.ToString() : default;
        }

        /// <summary>Add a {value} of (T) to the Expando using a string {name}. </summary>
        public static void Add<T>(this ExpandoObject expando, string name, T value)
        {
            var dictionary = (IDictionary<string, object>)expando;

            if (dictionary == null || value == null)
                return;

            if (dictionary.ContainsKey(name))
            {
                dictionary[name] = value;
            }
            else
            {
                dictionary.Add(name, value);
            }
        }

        /// <summary>Add a string to the Expando using a string {name}. </summary>
        public static void Add(this ExpandoObject expando, string name, string value)
        {
            var dictionary = (IDictionary<string, object>) expando;

            if (dictionary == null || value == null)
                return;

            if (dictionary.ContainsKey(name))
            {
                dictionary[name] = value;
            }
            else
            {
                dictionary.Add(name, value);
            }
        }

        /// <summary>Remove a value from the Expando using a string {name}. </summary>
        public static void Remove(this ExpandoObject expando, string name)
        {
            var dictionary = (IDictionary<string, object>) expando;

            if (dictionary.ContainsKey(name))
            {
                dictionary.Remove(name);
            }
        }

        /// <summary>Gets a value from an ExpandoObject by the specified {key}. If it does not exist, the
        ///  method returns the {defaultVal}. If this is not specified, it is the default for that type.</summary>
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
