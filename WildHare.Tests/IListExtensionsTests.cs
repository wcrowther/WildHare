using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using WildHare.Extensions;
using WildHare.Tests.Helpers;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
    [TestFixture]
    public class IListExtensionsTests
    {
        private List<Item> GetTestList()
        {
            // Produces list of 1-10
            return Enumerable.Range(1, 10)
                    .Select(n => new Item
                    {
                        ItemId = n,
                        ItemName = $"FirstName{n}"
                    })
                    .ToList();
        }

        [Test]
        public void Test_CurrentDomain_BaseDirectory_GetStart_For_Tests()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory.GetStartBefore("bin");

            ClassicAssert.AreEqual(@"C:\Git\WildHare\WildHare.Tests\", baseDirectory);
        }

        [Test]
        public void Test_TakeRandom_With_Remove_True()
        {
            var random = new Random(123456);

            var itemList = GetTestList();
            var destinationList = itemList.TakeRandom(5, random);

            ClassicAssert.AreEqual(5, destinationList.Count);
            ClassicAssert.AreEqual(5, itemList.Count);

            // 'Randomly' selected - but repeatable as 
            // random seed is passed in.

            ClassicAssert.AreEqual(3, destinationList[0].ItemId);
            ClassicAssert.AreEqual(1, destinationList[1].ItemId);
            ClassicAssert.AreEqual(2, destinationList[2].ItemId);
            ClassicAssert.AreEqual(6, destinationList[3].ItemId);
            ClassicAssert.AreEqual(9, destinationList[4].ItemId);

            ClassicAssert.AreEqual(4, itemList[0].ItemId);
            ClassicAssert.AreEqual(5, itemList[1].ItemId);
            ClassicAssert.AreEqual(7, itemList[2].ItemId);
            ClassicAssert.AreEqual(8, itemList[3].ItemId);
            ClassicAssert.AreEqual(10, itemList[4].ItemId);
        }

        [Test]
        public void Test_TakeRandomOne_With_Remove_True()
        {
            var random = new Random(123456);

            var itemList = GetTestList();

            ClassicAssert.AreEqual(10, itemList.Count);

            var randomItem = itemList.TakeRandomOne(random);

            ClassicAssert.AreEqual(9, itemList.Count);
            ClassicAssert.AreEqual(3, randomItem.ItemId);
        }

        [Test]
        public void Test_TakeRandomOne_With_Remove_False()
        {
            var random = new Random(123456);

            var itemList = GetTestList();
            var randomItem = itemList.TakeRandomOne(random, false);

            itemList.Debugger();

            ClassicAssert.AreEqual(10, itemList.Count);
            ClassicAssert.AreEqual(3, randomItem.ItemId);
        }

        [Test]
        public void Test_TakeNext_With_Remove_True()
        {
            var itemList = GetTestList();
            var destinationList = itemList.TakeNext(5);

            ClassicAssert.AreEqual(5, destinationList.Count);
            ClassicAssert.AreEqual(5, itemList.Count);

            ClassicAssert.AreEqual(1, destinationList[0].ItemId);
            ClassicAssert.AreEqual(2, destinationList[1].ItemId);
            ClassicAssert.AreEqual(3, destinationList[2].ItemId);
            ClassicAssert.AreEqual(4, destinationList[3].ItemId);
            ClassicAssert.AreEqual(5, destinationList[4].ItemId);

            ClassicAssert.AreEqual(6, itemList[0].ItemId);
            ClassicAssert.AreEqual(7, itemList[1].ItemId);
            ClassicAssert.AreEqual(8, itemList[2].ItemId);
            ClassicAssert.AreEqual(9, itemList[3].ItemId);
            ClassicAssert.AreEqual(10, itemList[4].ItemId);
        }

        [Test]
        public void Test_TakeNext_With_Remove_True_And_Offset()
        {
            var itemList = GetTestList();
            var destinationList = itemList.TakeNext(5, 3);

            ClassicAssert.AreEqual(5, destinationList.Count);
            ClassicAssert.AreEqual(5, itemList.Count);

            ClassicAssert.AreEqual(4, destinationList[0].ItemId);
            ClassicAssert.AreEqual(5, destinationList[1].ItemId);
            ClassicAssert.AreEqual(6, destinationList[2].ItemId);
            ClassicAssert.AreEqual(7, destinationList[3].ItemId);
            ClassicAssert.AreEqual(8, destinationList[4].ItemId);

            ClassicAssert.AreEqual(1, itemList[0].ItemId);
            ClassicAssert.AreEqual(2, itemList[1].ItemId);
            ClassicAssert.AreEqual(3, itemList[2].ItemId);
            ClassicAssert.AreEqual(9, itemList[3].ItemId);
            ClassicAssert.AreEqual(10, itemList[4].ItemId);
        }

        [Test]
        public void Test_TakeNext_With_Remove_True_And_Large_Offset()
        {
            var itemList = GetTestList();
            var destinationList = itemList.TakeNext(5, 2022);

            ClassicAssert.AreEqual(5, destinationList.Count);
            ClassicAssert.AreEqual(5, itemList.Count);

            ClassicAssert.AreEqual(3, destinationList[0].ItemId);
            ClassicAssert.AreEqual(4, destinationList[1].ItemId);
            ClassicAssert.AreEqual(5, destinationList[2].ItemId);
            ClassicAssert.AreEqual(6, destinationList[3].ItemId);
            ClassicAssert.AreEqual(7, destinationList[4].ItemId);

            ClassicAssert.AreEqual(1, itemList[0].ItemId);
            ClassicAssert.AreEqual(2, itemList[1].ItemId);
            ClassicAssert.AreEqual(8, itemList[2].ItemId);
            ClassicAssert.AreEqual(9, itemList[3].ItemId);
            ClassicAssert.AreEqual(10, itemList[4].ItemId);
        }

        [Test]
        public void Test_TakeNext_With_Integers_Remove_True_1()
        {
            var numbers = new List<int> { 1, 2, 3, 4 };
            int offset = 2;

            int first = numbers.TakeNext().Single();
            int second = numbers.TakeNext(offset: offset).Single();
            int third = numbers.TakeNext().Single();
            int fourth = numbers.TakeNext().Single();
            int fifth = numbers.TakeNext().Single();

            ClassicAssert.AreEqual(1, first);
            ClassicAssert.AreEqual(4, second);
            ClassicAssert.AreEqual(2, third);
            ClassicAssert.AreEqual(3, fourth);
            ClassicAssert.AreEqual(0, fifth);
        }

        [Test]
        public void Test_TakeNext_With_Integers_Remove_True_2()
        {
            var numbers = new List<int> { 1, 2, 3, 4 };
            int offset = 1;

            int first = numbers.TakeNext(offset: offset).Single();
            int second = numbers.TakeNext(offset: offset).Single();
            int third = numbers.TakeNext(offset: offset).Single();
            int fourth = numbers.TakeNext(offset: offset).Single();

            ClassicAssert.AreEqual(2, first);
            ClassicAssert.AreEqual(3, second);
            ClassicAssert.AreEqual(4, third);
            ClassicAssert.AreEqual(1, fourth);
        }

        [Test]
        public void Test_TakeNext_With_Integers_Remove_True_3()
        {
            var numbers = new List<int> { 1, 2, 3, 4 };
            int offset = 61;

            var taken = numbers.TakeNext(4, offset);

            ClassicAssert.AreEqual(4, taken.Count);

            ClassicAssert.AreEqual(2, taken[0]);
            ClassicAssert.AreEqual(3, taken[1]);
            ClassicAssert.AreEqual(4, taken[2]);
            ClassicAssert.AreEqual(1, taken[3]);
        }

        [Test]
        public void Test_TakeNext_With_Remove_True_And_Offset_Is_Close_To_End_But_Wraps()
        {
            var itemList = GetTestList();
            var destinationList = itemList.TakeNext(5, 8);

            ClassicAssert.AreEqual(5, destinationList.Count);
            ClassicAssert.AreEqual(5, itemList.Count);

            ClassicAssert.AreEqual(4, itemList[0].ItemId);
            ClassicAssert.AreEqual(5, itemList[1].ItemId);
            ClassicAssert.AreEqual(6, itemList[2].ItemId);
            ClassicAssert.AreEqual(7, itemList[3].ItemId);
            ClassicAssert.AreEqual(8, itemList[4].ItemId);

            // Takes five elements as it wraps back to the beginning
            ClassicAssert.AreEqual(9, destinationList[0].ItemId);
            ClassicAssert.AreEqual(10, destinationList[1].ItemId);
            ClassicAssert.AreEqual(1, destinationList[2].ItemId);
            ClassicAssert.AreEqual(2, destinationList[3].ItemId);
            ClassicAssert.AreEqual(3, destinationList[4].ItemId);
        }

        [Test]
        public void Test_TakeNextOne_With_Remove_True()
        {
            var itemList = GetTestList();
            var nextItem = itemList.TakeNextOne();

            ClassicAssert.AreEqual(9, itemList.Count);
            ClassicAssert.AreEqual(1, nextItem.ItemId);

            var nextItem2 = itemList.TakeNextOne();

            ClassicAssert.AreEqual(8, itemList.Count);
            ClassicAssert.AreEqual(2, nextItem2.ItemId);
        }

        [Test]
        public void Test_TakeNextOne_With_Remove_False()
        {
            var itemList = GetTestList();
            var nextItem = itemList.TakeNextOne(remove: false);

            ClassicAssert.AreEqual(10, itemList.Count);
            ClassicAssert.AreEqual(1, nextItem.ItemId);

            var nextItem2 = itemList.TakeNextOne();

            ClassicAssert.AreEqual(9, itemList.Count);
            ClassicAssert.AreEqual(1, nextItem2.ItemId);
        }

        [Test]
        public void Test_TakeNextOne_With_Offset_Remove_True()
        {
            var itemList = GetTestList();
            var nextItem = itemList.TakeNextOne(3, true);

            ClassicAssert.AreEqual(9, itemList.Count);
            ClassicAssert.AreEqual(4, nextItem.ItemId);

            var nextItem2 = itemList.TakeNextOne();

            ClassicAssert.AreEqual(8, itemList.Count);
            ClassicAssert.AreEqual(1, nextItem2.ItemId);
        }

        [Test]
        public void Test_GroupBy_And_ToLookup()
        {
            var people = new List<Person>
            {
                new Person { PersonId = 1, FirstName = "Will", LastName= "Smith" },
                new Person { PersonId = 2, FirstName = "Joe", LastName= "Jones" },
                new Person { PersonId = 3, FirstName = "Patty", LastName= "Smith" },
                new Person { PersonId = 4, FirstName = "Jeff", LastName= "Jones" },
                new Person { PersonId = 5, FirstName = "Fred", LastName= "Jones" }
            };

            // =======================================================
            // GroupBy() THEN ToDictionary()
            // =======================================================

            var familiesList = people.GroupBy(g => g.LastName);
            // familiesList Type is: System.Linq.GroupedEnumerable<Person,string> (not public?)
            // familiesList Interface is: IEnumerable<IGrouping<string, Person>>

            var famDictionary = familiesList.ToDictionary(g => g.Key, g => g.ToList());

            ClassicAssert.AreEqual(5, people.Count);
            ClassicAssert.AreEqual(2, familiesList.Count());

            ClassicAssert.AreEqual(2, famDictionary.Count);
            ClassicAssert.AreEqual(2, famDictionary.Values.Count);
            ClassicAssert.AreEqual(2, famDictionary.Keys.Count);
            ClassicAssert.AreEqual(2, famDictionary["Smith"].Count);
            ClassicAssert.AreEqual(3, famDictionary["Jones"].Count);

            // =======================================================
            // ToLookup(): simplest, best performance(?)
            // =======================================================

            var famLookUp = people.ToLookup(l => l.LastName);


            foreach (var family in famLookUp)
            {
                Debug.WriteLine("-".Repeat(20));
                Debug.WriteLine($"family: {family.Key} (count: {family.Count()})" );

                foreach (var person in family)
                {
                    Debug.WriteLine($"{person}");
                }
            }
            Debug.WriteLine("-".Repeat(20));

            ClassicAssert.AreEqual(2, famLookUp.Count);
            ClassicAssert.AreEqual(2, famLookUp["Smith"].Count());
            ClassicAssert.AreEqual(3, famLookUp["Jones"].Count());
            ClassicAssert.AreEqual(0, famLookUp["Crowther"].Count()); // Empty does not throw

            string famLookUpJson = famLookUp.ToJson();

			ClassicAssert.IsNotNull(famLookUp);

            // =======================================================
            // OVERLOAD: ToLookup(l => l.LastName, l => l.FirstName)
            // only brings back single value for FirstName
            // =======================================================

            var famLookUp2 = people.ToLookup(l => l.LastName, l => l.FirstName);

			ClassicAssert.IsNotNull(famLookUp2);
        }

        [Test]
        public void Test_NextIn_PreviousIn_Basic_Numeric()
        {
            var itemList = new[]{ 2, 3, 4, 2, 6 };

            // Next
            ClassicAssert.AreEqual(3, 2.NextIn(itemList));
            ClassicAssert.AreEqual(4, 3.NextIn(itemList));
            ClassicAssert.AreEqual(0, 6.NextIn(itemList));

            // Previous
            ClassicAssert.AreEqual(0, 2.PreviousIn(itemList));
            ClassicAssert.AreEqual(2, 3.PreviousIn(itemList));
            ClassicAssert.AreEqual(2, 6.PreviousIn(itemList));
        }

        [Test]
        public void Test_NextIn_PreviousIn_Basic_Objects()
        {
            var people = new List<Person>
            {
                new Person { PersonId = 1, FirstName = "Will", LastName= "Smith" },
                new Person { PersonId = 2, FirstName = "Joe", LastName= "Jones" },
                new Person { PersonId = 3, FirstName = "Patty", LastName= "Smith" },
                new Person { PersonId = 1, FirstName = "Will", LastName= "Smith" }
            };

            // Next 
            ClassicAssert.AreEqual(null, people[3].NextIn(people));
            ClassicAssert.AreEqual(2, people[0].NextIn(people).PersonId);
            ClassicAssert.AreEqual("Patty", people.First(f => f.FirstName == "Joe").NextIn(people).FirstName);

            // Previous
            ClassicAssert.AreEqual(null, people[0].PreviousIn(people));
            ClassicAssert.AreEqual(2, people[2].PreviousIn(people).PersonId);
            ClassicAssert.AreEqual("Will", people.First(f => f.FirstName == "Joe").PreviousIn(people).FirstName);
        }


        [Test]
        public void Test_NextWhile_Basic_Ints()
        {
            var numbers = new List<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 9, 10, 440, 322 };

            var four = numbers.ElementAt(3); // zero-based
            var nums = four.NextInWhile(numbers, p => p <= 8);

            ClassicAssert.AreEqual(6, nums.Count); // stops when it gets to last 8
            ClassicAssert.AreEqual(42, nums.Sum()); // sum of 5, 6, 7, 8, 8, 8
        }

        [Test]
        public void Test_NextInWhile_Basic_Objects()
        {
            var people = new List<Person>
            {
                new Person { PersonId = 1, FirstName = "Will", LastName= "Smith" },
                new Person { PersonId = 2, FirstName = "Joe",  LastName= "Smith" },
                new Person { PersonId = 3, FirstName = "Patty",LastName= "Smith" },
                new Person { PersonId = 4, FirstName = "John", LastName= "Jones" },
                new Person { PersonId = 5, FirstName = "Frank",LastName= "Smith" }
            };

            var will = people[0];
            var kin = will.NextInWhile(people, p => p.LastName == "Smith");

            ClassicAssert.AreEqual(2, kin.Count); // stops when it gets to Jone Jones

            var john = people[3];
            var smiths = john.NextInWhile(people, p => p.LastName == "Smith", -1);

            ClassicAssert.AreEqual(3, smiths.Count); // stops when it gets to Will Smith
        }

        [Test]
        public void Test_IsFirstIn_Objects()
        {
            var people = new List<Person>
            {
                new Person { PersonId = 1, FirstName = "Will", LastName= "Smith" },
                new Person { PersonId = 2, FirstName = "Joe", LastName= "Jones" },
                new Person { PersonId = 3, FirstName = "Patty", LastName= "Smith" }
            };

            ClassicAssert.AreEqual(true, people[0].IsFirstIn(people));
            ClassicAssert.AreEqual(false, people[1].IsFirstIn(people));
            ClassicAssert.AreEqual(false, people[2].IsFirstIn(people));
        }

        [Test]
        public void Test_IsLastIn_Objects()
        {
            var people = new List<Person>
            {
                new Person { PersonId = 1, FirstName = "Will", LastName= "Smith" },
                new Person { PersonId = 2, FirstName = "Joe", LastName= "Jones" },
                new Person { PersonId = 3, FirstName = "Patty", LastName= "Smith" }
            };

            ClassicAssert.AreEqual(false, people[0].IsLastIn(people));
            ClassicAssert.AreEqual(false, people[1].IsLastIn(people));
            ClassicAssert.AreEqual(true,  people[2].IsLastIn(people));
        }

        [Test]
        public void Test_IList_Replace_Basic()
        {
            var words = "This is the old item.".Split(' ').ToList();

            words.ReplaceItem(3, "new");

            ClassicAssert.AreEqual(5, words.Count);
            ClassicAssert.AreEqual("This is the new item.", string.Join(' ', words));
        }

        [Test]
        public void Test_IList_Replace_Objects()
        {
            var people = new List<Person>
            {
                new Person { PersonId = 1, FirstName = "Will", LastName= "Smith" },
                new Person { PersonId = 2, FirstName = "Joe", LastName= "Jones" },
                new Person { PersonId = 3, FirstName = "Patty", LastName= "Smith" },
            };

            var person = new Person { PersonId = 4, FirstName = "Joe", LastName = "Blogs" };

            people.ReplaceItem(2, person);

            ClassicAssert.AreEqual(3, people.Count);
            ClassicAssert.AreEqual($"Joe Blogs",  $"{people[2].FirstName} {people[2].LastName}");
        }


        [Test]
        public void Test_IList_CombineItems_Objects()
        {
            var tokens = new List<Token>
            {
                new Word { Text = "the president",      WordId = 2 },
                new Word { Text = "of",                 WordId = 3 },
                new Word { Text = "the united states",  WordId = 4 }
            };

            var newTokens = new List<Token>
            {
                new Word { Text = "a",          WordId = 14 },
                new Word { Text = "foreign",    WordId = 15 },
                new Word { Text = "country",    WordId = 16 }
            };


            tokens.ReplaceItems(2, newTokens);  // replace token "the united states" with "a foreign country"

            ClassicAssert.AreEqual(5, tokens.Count);
            ClassicAssert.AreEqual("the president of a foreign country", string.Join(" ", tokens.Select(s => s.Text)));
        }

        [Test]
        public void Test_IList_Left_Outer_Join_With_Selected_Ints()
        {
            var words = new List<Word>
            {
                new Word { Text = "a",  WordId = 1, Info = "First Letter"},
                new Word { Text = "b",  WordId = 2, Info = "Second Letter"},
                new Word { Text = "c",  WordId = 3, Info = "Third Letter"},
                new Word { Text = "d",  WordId = 4, Info = "Fourth Letter"},
                new Word { Text = "e",  WordId = 5, Info = "Fifth Letter"}
            };

            int[] ids = { 2, 3, 4 }; // 1 and 5 NOT selected

            var results =   from w in words
                            join i in ids
                                on w.WordId equals i into idList
                            from num in idList.DefaultIfEmpty()
                            select new
                            {
                                w.Text,
                                num,
                                selected = num != 0
                            };

            ClassicAssert.AreEqual(5, results.Count());
            ClassicAssert.AreEqual(3, results.Count(c => c.selected == true));
            ClassicAssert.AreEqual(2, results.Count(c => c.selected == false));

        }

        [Test]
        public void Test_Hierarchical_Sort_Version_1()
        {
            var data   = GetTempHierarchicalData();
            var result = data.OrderBy(d => d.SortOrder).ThenBy(d => d.ParentID);

			ClassicAssert.IsNotNull(result);
        }

        private List<TempTable> GetTempHierarchicalData()
        {
            return new List<TempTable>
            {
                new TempTable { ID = 1, ParentID = null, Name = "Test 1", SortOrder = 1 },
                new TempTable { ID = 2, ParentID = 1,    Name = "Test 2", SortOrder = 1 },
                new TempTable { ID = 3, ParentID = 1,    Name = "Test 3", SortOrder = 3 },
                new TempTable { ID = 4, ParentID = 2,    Name = "Test 4", SortOrder = 1 },
                new TempTable { ID = 5, ParentID = 1,    Name = "Test 5", SortOrder = 2 }
            };
        }
    }

    public class TempTable
    {
        public int ID { get; set; }

        public int? ParentID { get; set; }

        public String Name { get; set; }

        public int SortOrder { get; set; }

        public override string ToString()
        {
            return $"{Name} Sort: {SortOrder} ParentID: {ParentID}";
        }
    }
}
