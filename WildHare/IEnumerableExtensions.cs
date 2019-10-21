using System;
using System.Collections.Generic;
using System.Linq;

namespace WildHare.Extensions
{
    public static class IEnumerableExtensions
    {

        public static TSource ElementIn<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (source == null)
            {
                throw new NullReferenceException("ElementIn() source cannnot be null.");
            }

            if(source.Count() == 0)
                throw new Exception("ElementIn() source contains no elements.");

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

        public static TSource ElementInOrDefault<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (source == null)
            {
                throw new NullReferenceException("ElementIn() source cannnot be null.");
            }

            if (source.Count() == 0)
                return default;

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
        public static IEnumerable<T1> MatchList<T1, T2>(this IEnumerable<T1> list1,
                                                             IEnumerable<T2> list2,
                                                             Func<T1, T2, bool> func,
                                                             bool consecutive = true )
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
    }
}
