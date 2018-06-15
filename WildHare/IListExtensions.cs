using System;
using System.Collections.Generic;
using System.Linq;

namespace WildHare.Extensions.ForIList
{
    public static class IListExtensions
    {
        /// <summary>Will randomly take up to {maxCount} number of items from the sourceList and return them in the new destinationList</summary>
        public static IList<T> TakeFromListRandom<T>(this IList<T> sourceList, Random random = null, int count = 1, bool remove = true)
        {
            if (sourceList == null)
            {
                throw new Exception("TakeRandomItems Error. The Datasource list is null.");
            }

            if (random == null)
                random = new Random();

            var destinationList = new List<T>();
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(sourceList.Count);

                T element = sourceList.ElementAtOrDefault(index);
                if (element == null)
                    break;
                else
                    destinationList.Add(element);

                if (remove)
                {
                    foreach (var item in destinationList)
                    {
                        sourceList.Remove(item);
                    }
                }
            }
            return destinationList;
        }

        /// <summary>Will take up to {maxCount} number of items from the sourceList and return them in the new destinationList</summary>
        public static IList<T> TakeFromListNext<T>(this IList<T> sourceList, int count = 1, int offset = 0, bool remove = true)
        {
            if (sourceList == null)
            {
                throw new Exception("TakeFromListNext Error. The Datasource list is null.");
            }

            var destinationList = sourceList.Skip(offset).Take(count).ToList();

            if (remove)
            {
                foreach (var item in destinationList)
                {
                    sourceList.Remove(item);
                }
            }

            return destinationList;
        }
    }
}
