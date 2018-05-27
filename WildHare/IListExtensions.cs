using System;
using System.Collections.Generic;
using System.Linq;

namespace WildHare.Extensions.ForIList
{
    public static class IListExtensions
    {
        /// <summary>Will randomly take up to {maxCount} number of items from the sourceList and return them in the new destinationList</summary>
        public static IList<T> TakeFromListRandom<T>(this IList<T> sourceList, Random random, int maxCount = 1, bool remove = true)
        {
            if (sourceList == null)
            {
                throw new Exception("TakeRandomItems Error. The Datasource list is null.");
            }

            var destinationList = new List<T>();
            for (int i = 0; i < maxCount; i++)
            {
                int index = random.Next(sourceList.Count);
                T element = sourceList.ElementAtOrDefault(index);
                if (element == null)
                {
                    break;
                }
                else
                {
                    destinationList.Add(element);
                    if (remove)
                    {
                        sourceList.Remove(element);
                    }
                }
            }
            return destinationList;
        }
    }
}
