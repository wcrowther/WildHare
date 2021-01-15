using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WildHare.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>Takes the element in the list at the position of {index} looping around to the beginning
        /// of the list if the index is outside of the number of items in the list. Will always return an element
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
        /// of the list if the index is outside of the number of items in the list. Will always return an element
        /// unless there are no elements in the list, in which case it returns the type default value.</summary>
        public static TSource ElementInOrDefault<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (source == null)
            {
                throw new NullReferenceException("ElementIn() source cannnot be null.");
            }

            if (source.Count() == 0)
                return default;

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
            return default;
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
            var ie1 = first.GetEnumerator();
            var ie2 = second.GetEnumerator();
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

        public static int[] InList<T>(this IList<T> list,
                                           IList<T> items,
                                           Func<T, T, bool> func)
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
                    bool match = func(a,b); 

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

    }
}
