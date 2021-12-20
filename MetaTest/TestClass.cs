using System;
using System.Collections.Generic;

namespace MetaTest
{
    /// <summary>This TestClass is used for testing of the MetaAssembly.GetMetaModels and MetaAssembly.GetMetaModelsGroupedByNamespace</summary>
    public static class TestClass
    {
        /// <summary>SimpleMethod Summary Comment.</summary>
        public static int SimpleMethod(this int item, int step = 1)
        {
            return item;
        }

        /// <summary>ReturnIListMethod Summary Comment.</summary>
        public static IList<string> ReturnIListMethod(this string item, int step = 1)
        {
            return new List<string>();
        }

        /// <summary>TestThree Summary Comment.</summary>
        public static IList<(string, string, string)> TestThree(this string item, Func<string, bool> func)
        {
            var list = new List<(string, string, string)>();

            list.Add(("one", "two", "three"));

            return list;
        }

        /// <summary>TestOneGeneric Summary Comment.</summary>
        public static T TestOneGeneric<T>(this T item, int step = 1)
        {
            return item;
        }

        /// <summary>TestTwoGenericGeneric Summary Comment.</summary>
        public static IList<T> TestTwoGeneric<T>(this T item, IList<T> itemList, Func<T, bool> func, int step = 1)
        {
            return itemList;
        }

        /// <summary>TestThree Summary Comment.</summary>
        public static IList<(T, T2, T3)> TestThreeGeneric<T, T2, T3>(this T2 item, IList<(T, T2, T3)> itemList, Func<T, bool> func, int step = 1)
        {
            return itemList;
        }

    }
}
