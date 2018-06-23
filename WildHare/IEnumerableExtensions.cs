using System;
using System.Collections.Generic;
using System.Linq;

namespace WildHare.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>Given two lists returns to values from first if func is true. 
        /// If consecutive is false continues until one of the lists has no more elements.</summary>
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
