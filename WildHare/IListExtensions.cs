using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WildHare.Extensions.List;

namespace WildHare.Extensions
{
    /// <summary>Class Summary</summary>
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

        /// <summary>Will randomly return a single item from the {sourceList}.  If {remove} is true, the item
        /// is removed from the {sourceList}.</summary>
        public static T TakeRandomOne<T>(this IList<T> sourceList, Random random = null, bool remove = true)
        {
            return sourceList.TakeRandom(1, random, remove).FirstOrDefault();
        }

        /// <summary>Will return a sequencial list of items from the {sourceList} equal to the {count}(up to the number<b />
        /// remaining in the list).<br/>If the {count} is not specified, it will return one. If {remove} is true, the items
        /// are removed from the {sourceList}. If {offset} > 0, it will skip this number of records
        /// but will loop back to the beginning if necessary and elements exist.</summary>
        public static IList<T> TakeNext<T>(this IList<T> sourceList, int count = 1, int offset = 0, bool remove = true)
        {
            if (sourceList == null)
            {
                throw new Exception("TakeNext Error. The Datasource list is null.");
            }

            if (offset < 0)
            {
                throw new Exception("TakeNext offset values must be 0 or greater.");
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

        /// <summary>Will return a single item from the {sourceList}  If {remove} is true, the item
        /// is removed from the {sourceList}. If {offset} > 0, it will skip this number of records
        /// but will loop back to the beginning if necessary and elements exist.</summary>
        public static T TakeNextOne<T>(this IList<T> sourceList, int offset = 0, bool remove = true)
        {
            return sourceList.TakeNext(1, offset, remove).FirstOrDefault();
        }

        /// <summary>Given an item in a list, it will return the next item in the list or the
        /// default for that type (null if non-numeric). The {distance} is an int with 1 for the
        /// next item (the default). 2 for the next item after that, and so on... </summary>
        public static T NextIn<T>(this T item, IList<T> itemList, int distance = 1)  // Constrain?  where T : class, new()
        {
            if (itemList?.Count == 0 || item == null)
                return default;

            int itemIndex = itemList.IndexOf(item);

            return (itemIndex >= 0) ? itemList.ElementAtOrDefault(itemIndex + distance) : default;
        }

        /// <summary>Given an item in a list, it will return the previous item in the list or the 
        /// default for that type (null if non-numeric). The {distance} is an int with 1 for the 
        /// previous item (the default). 2 for the previous item before that, and so on... </summary>
        public static T PreviousIn<T>(this T item, IList<T> itemList, int distance = 1)
        {
            if (itemList?.Count == 0 || item == null)
                return default;

            int itemIndex = itemList.IndexOf(item);

            return (itemIndex >= 0) ? itemList.ElementAtOrDefault(itemIndex - distance) : default;
        }

        /// <summary>Given an item in a list, it will return the next items in the list while {func} 
        /// returns true. The {step} parameter can be used to take every nth item in a positive 
        /// or a negative direction. The step default is 1 to take the next and subsequent items.</summary>
        public static IList<T> NextInWhile<T>(this T item, IList<T> itemList, Func<T,bool> func, int step = 1)  
        {         
            if (itemList?.Count == 0 || item == null)
                return default;

            int itemIndex = itemList.IndexOf(item);

            if (itemIndex == -1)
                return default;

            bool moreItems = true; // start true
            var returnList = new List<T>();

            while (moreItems)
            {
                var nextItem = itemList.ElementAtOrDefault(itemIndex + step);
                if (nextItem == null)
                {
                    moreItems = false;
                    continue;
                }
                if (func(nextItem))
                {
                    returnList.Add(nextItem);
                    itemIndex += step;
                }
                else 
                {
                    moreItems = false;
                }
            }
            return returnList;
        }

        /// <summary>Given an item in a list, it will return the previous items in the list while {func} 
        /// returns true. This method is a convenience and does the same thing as NextInWhile with a -1 step.</summary>
        public static IList<T> PreviousInWhile<T>(this T item, IList<T> itemList, Func<T, bool> func)
        {
            return item.NextInWhile(itemList, func, -1);
        }

        /// <summary>Given an item in a list, returns true if the item is 
        /// the first element in the list.</summary>
        public static bool IsFirstIn<T>(this T item, IList<T> itemList)
        {
            if (itemList?.Count == 0 || item == null)
                return default;

            return itemList.IndexOf(item) == 0;
        }

        /// <summary>Given an item in a list, returns true if the item is 
        /// the last element in the list.</summary>
        public static bool IsLastIn<T>(this T item, IList<T> itemList)
        {
            if (itemList?.Count == 0 || item == null)
                return default;

            return itemList.IndexOf(item) == itemList.Count - 1;
        }

        /// <summary>Adds a AddRange to the IList&gt;T&gt; interface. List&gt;T&gt; already has a version,
        /// which this method will use if available.</summary>
        public static void AddRange<T>(this IList<T> itemList, IEnumerable<T> newItems)
        {
            if (itemList is null)
                throw new ArgumentNullException(nameof(itemList), "AddRange requires a non-null itemList parameter");

            if (newItems is null)
                throw new ArgumentNullException(nameof(newItems), "AddRange requires a non-null items parameter.");

            if (itemList is List<T> itemListAsList)
            {
                itemListAsList.AddRange(newItems);
            }
            else
            {
                foreach (var item in itemList)
                {
                    itemList.Add(item);
                }
            }
        }

        /// <summary>Removes an item at the {index} in the {itemList} and replaces with the {newItem}. </summary>
        public static void ReplaceItem<T>(this IList<T> itemList, int index, T newItem)
        {
            if (itemList == null)
                throw new ArgumentNullException(nameof(itemList), "ReplaceItem requires a non-null itemList parameter.");

            if (newItem == null)
                throw new ArgumentNullException(nameof(newItem), "ReplaceItem requires a non-null newItem parameter.");

            if (index < 0 || index >= itemList.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "The ReplaceItem() index is outside of the valid range for the itemList.");

            itemList.RemoveAt(index);

            itemList.Insert(index, newItem);
        }

        /// <summary>Replaces a list of {newItems} starting with the {index} with the same number of items in the {itemList}.</summary>
        public static void ReplaceItems<T>(this IList<T> itemList, int index, IList<T> newItems)
        {
            if (itemList == null)
                throw new ArgumentNullException(nameof(itemList), "ReplaceItem requires a non-null itemList parameter.");

            if (newItems == null)
                throw new ArgumentNullException(nameof(newItems), "ReplaceItems requires a non-null newItems parameter.");

            if (index < 0 || index >= itemList.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "The ReplaceItem() index is outside of the valid range for the itemList.");

            int counter = 0;

            itemList.RemoveAt(index);

            foreach (var item in newItems)
            {
                itemList.Insert(index + counter, item);
                counter++;
            }
        }

        /// <summary>Combines a {count} of items at the {index} in the {itemList} into a single {newItem}.</summary>
        public static void CombineItems<T>(this IList<T> itemList, int index, int count, T newItem)
        {
            if (itemList == null)
                throw new Exception("ReplaceItem() requires a valid non-null itemList parameter.");

            if (newItem == null)
                throw new Exception("ReplaceItem() requires a valid non-null  newItem parameter.");

            if (index < 0 || index >= itemList.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "The ReplaceItem() index is outside of the valid range for the itemList.");

            if (itemList.Count - index < count)
                throw new ArgumentOutOfRangeException(nameof(count), count, "The ReplaceItem() count is out of range of the itemList");

            if (itemList is List<T> list)
            {
                list.RemoveRange(index, count);
                itemList.Insert(index, newItem);

                return;
            }

            for (int i = count; i > 0; i--)
            {
                itemList.RemoveAt(index);
            }

            itemList.Insert(index, newItem);
        }

    }
}
