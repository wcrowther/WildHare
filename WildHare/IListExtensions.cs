using System;
using System.Collections.Generic;
using System.Linq;

namespace WildHare.Extensions
{
    public static class IListExtensions
    {
        /// <summary>Will randomly return a list of items from the {sourceList} equal to the {count} (up to the number in the list).<br />
        /// If the {count} is not specified, it will return one. If {remove} is true, the items are removed from the {sourceList}. </summary>
        public static IList<T> TakeRandom<T>(this IList<T> sourceList, int count = 1, Random random = null, bool remove = true)
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

        /// <summary>Will return a sequencial list of items from the {sourceList} equal to the {count}(up to the number<b />
        /// remaining in the list).<br/>If the {count} is not specified, it will return one. If {remove} is true, the items
        /// are removed from the {sourceList}. If {offset} > 0, it will skip this number of records
        /// but will loop back to the beginning if necessary and elements exist.
        /// </summary>
        public static IList<T> TakeNext<T>(this IList<T> sourceList, int count = 1, int offset = 0, bool remove = true)
        {
            if (sourceList == null)
            {
                throw new Exception("TakeNext Error. The Datasource list is null.");
            }

            var destinationList = new List<T>();

            for (int i = 0; i < count; i++)
            {
                T element = sourceList.ElementInOrDefault(i + offset); // ElementIn - wraps list

                if (element == null)
                    break;
                else
                    destinationList.Add(element);
            }

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
