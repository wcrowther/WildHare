using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WildHare.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>Takes the element in the list at the position of {index} looping around to the beginning
        /// of the list if the (zero-base) index is outside of the number of items in the list. Will always return an element
        /// unless there are no elements in the list, in which case it returns an exception.</summary>
        public static TSource ElementIn<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (source == null)
            {
                throw new NullReferenceException("ElementIn() source cannnot be null.");
            }

            if (source.Count() == 0)
                throw new Exception("ElementIn() source contains no elements.");

            index = Math.Abs(index);

            int mod = index % source.Count();
            int position = mod;

            if (source is IList<TSource> list)
            {
                return list[position];
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        if (position == 0)
                        {
                            return e.Current;
                        }
                        index--;
                    }
                }
            }
            throw new Exception("ElementIn() no elements to return.");
        }

        /// <summary>Takes the element in the list at the position of {index} looping around to the beginning
        /// of the list if the (zero-based) index is outside of the number of items in the list. Will always return an element
        /// unless there are no elements in the list, in which case it returns the {defaultItem} of type TSource.</summary>
        public static TSource ElementInOrDefault<TSource>(this IEnumerable<TSource> source, int index, TSource defaultItem)
        {
            if (source == null)
            {
                throw new NullReferenceException("ElementIn() source cannnot be null.");
            }

            if (source.Count() == 0)
                return defaultItem;

            index = Math.Abs(index);


            int mod = index % source.Count();
            int position = mod;

            if (source is IList<TSource> list)
            {
                return list[position];
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        if (position == 0)
                        {
                            return e.Current;
                        }
                        index--;
                    }
                }
            }
            return defaultItem;
        }

        /// <summary>Takes the element in the list at the position of {index} looping around to the beginning
        /// of the list if the (zero-based) index is outside of the number of items in the list. Will always return an element
        /// unless there are no elements in the list, in which case it returns the type default value.</summary>
        public static TSource ElementInOrDefault<TSource>(this IEnumerable<TSource> source, int index)
        {
            var defaultItem = default(TSource);

            return source.ElementInOrDefault(index, defaultItem);
        }

        /// <summary>Given two lists it returns values from first if the {func} is true. If {consecutive} 
        /// is false, continues returning values until one of the lists has no more elements.</summary>
        public static IEnumerable<T1> MatchList<T1, T2>(this IEnumerable<T1> list1, IEnumerable<T2> list2,
                                                             Func<T1, T2, bool> func, bool consecutive = true)
        {
            var ie1 = list1.GetEnumerator();
            var ie2 = list2.GetEnumerator();
            bool match = true; // initialize as true
            bool next = true;

            while (ie1.MoveNext() && next)
            {
                if (consecutive || match)
                {
                    next = ie2.MoveNext();
                }
                if (next && func(ie1.Current, ie2.Current))
                {
                    match = true;
                    yield return ie1.Current;
                }
                else
                {
                    match = false;
                }
            }
        }

        /// <summary>Given two lists returns the values from first if func is true. 
        /// If {consecutive} is false continues until one of the lists has no  more elements.</summary>
        [Obsolete("Renamed to MatchList. Will remove in a future version.")]
        public static IEnumerable<TFirst> Sequence<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,
                                                                         Func<TFirst, TSecond, bool> func, bool consecutive = true)
        {
            return first.MatchList(second, func, consecutive);
        }

        /// <summary>Compares 2 lists and using the {comparer} to return an array of int positions
        /// where the items match.</summary>
        public static int[] InList<TList, TItems>(this IList<TList> list,
                                                       IList<TItems> items,
                                                       Func<TList, TItems, bool> comparer)
        {
            var indexes = new List<int>();

            if (list == null || items == null || list.Count < items.Count)
                return indexes.ToArray();

            for (int x = 0; x < list.Count; x++)
            {
                int matches = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    var a = list.ElementAtOrDefault(x + i);
                    var b = items.ElementAtOrDefault(i);
                    bool match = comparer(a, b);

                    if (match)
                    {
                        matches++;
                    }
                    else
                    {
                        matches = 0;
                        break;
                    }
                }
                if (matches == items.Count)
                {
                    indexes.Add(x);
                }
            }

            if (indexes.Count == 0)
            {
                indexes.Add(-1);
            }

            return indexes.ToArray();
        }

        /// <summary>Converts IEnumerable to Collection of type parameter.
        /// Returns an empty Collection rather than failing if enumerable is null.</summary>
        public static Collection<T> ToCollection<T>(this IEnumerable<T> enumerable)
        {
            var collection = new Collection<T>();

            if (enumerable == null)
                enumerable = new Collection<T>();

            foreach (T i in enumerable)
            {
                collection.Add(i);
            }
            return collection;
        }

        /// <summary>Provides a 0-based index to a foreach loop using a tuple.</summary>
        /// <example>
        /// foreach (var (item, index) in collection.WithIndex() )
        /// {
        ///     Debug.WriteLine($"{index} : {item}");
        /// }
        /// </example>
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> list)
        {
            return list?.Select((item, index) => (item, index)) ?? new Collection<(T, int)>();
        }

        /// <summary>Converts an IEnumerable of ints to a string. The method will return null if the {intList} parameter is null 
        /// when {strict} is false. When {strict} is true the methods will throw an exception.</summary>
        public static string AsString(this IEnumerable<int> intList, bool strict = false)
        {
            if (intList is null && strict)
                throw new Exception("IntList.AsString() cannot be null when in strict mode.");

            if (intList is null)
                return null;

            return string.Join(",", intList);
        }

        /// <summary>Given a {list} enumerable and a {pattern} enumerable, it enumerates the list and
        /// returns the list item if the {func} returns true for the match to the pattern item.
        /// The list enumerates to the next item regardless of a match or not, but pattern does not
        /// enumerate to the next pattern unless it matched.</summary>
        public static IEnumerable<T> PatternMatch<T,P>(this IEnumerable<T> list,
                                                          IEnumerable<P> pattern,
                                                          Func<T, P, bool> func
                                                     )
        {
            var listEnumerator = list.GetEnumerator();
            var patternEnumerator = pattern.GetEnumerator();

            bool patternHasMoreItems = patternEnumerator.MoveNext(); // init outside loop

            while (listEnumerator.MoveNext() && patternHasMoreItems)
            {
                if (func(listEnumerator.Current, patternEnumerator.Current))
                {
                    yield return listEnumerator.Current;

                    patternHasMoreItems = patternEnumerator.MoveNext();
                }
            }
        }

        /// <summary>Will return the {singular} form of a word if the count of list is equal to 1, otherwise returns {plural}. If the parmeter 
        /// {plural} is omitted, it will add an "s" to the end, or an "es" if {singular} ends in "s","x","ch","sh","z", or "o").</summary>
        /// <example>When a list contains 1 element: list.Pluralize("clown") returns "clown";</example>
        /// <example>When a list contains 0 element: list.Pluralize("clown") returns "clowns";</example>
        /// <example>When a list contains 3 element: list.Pluralize("fox") returns "foxes";</example>
        /// <example>When a list contains 5 element: list.Pluralize("child", "children") returns "children";</example>
        public static string Pluralize<T>(this IEnumerable<T> list, string singular, string plural = null)
        {
            int count = list?.Count() ?? default;

            return count.Pluralize(singular, plural);
        }

        public static bool AnyEquals<T>(this IEnumerable<T> list, T value)
        {
            return list.Any(a => a.Equals(value));
        }

        /// <summary>Compact version of OrderBy / OrderDescending where you can pass in a bool.</summary>
        public static IOrderedEnumerable<T> OrderBy<T,TKey>(this IEnumerable<T> list, Func<T,TKey> keySelector, bool isDescending)
        {
            return isDescending ? list.OrderByDescending(keySelector) : list.OrderBy(keySelector);
        }
    }
}
