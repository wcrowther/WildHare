using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public void Test_TakeRandom_With_Remove_True()
        {
            var random = new Random(123456);

            var itemList = GetTestList();
            var destinationList = itemList.TakeRandom(5, random);

            Assert.AreEqual(5, destinationList.Count);
            Assert.AreEqual(5, itemList.Count);

            // 'Randomly' selected - but repeatable as 
            // random seed is passed in.

            Assert.AreEqual(3, destinationList[0].ItemId);
            Assert.AreEqual(1, destinationList[1].ItemId);
            Assert.AreEqual(2, destinationList[2].ItemId);
            Assert.AreEqual(6, destinationList[3].ItemId);
            Assert.AreEqual(9, destinationList[4].ItemId);

            Assert.AreEqual(4, itemList[0].ItemId);
            Assert.AreEqual(5, itemList[1].ItemId);
            Assert.AreEqual(7, itemList[2].ItemId);
            Assert.AreEqual(8, itemList[3].ItemId);
            Assert.AreEqual(10, itemList[4].ItemId);
        }

        [Test]
        public void Test_TakeRandomOne_With_Remove_True()
        {
            var random = new Random(123456);

            var itemList = GetTestList();
            var randomItem = itemList.TakeRandomOne(random);

            Assert.AreEqual(9, itemList.Count);
            Assert.AreEqual(3, randomItem.ItemId);
        }

        [Test]
        public void Test_TakeRandomOne_With_Remove_False()
        {
            var random = new Random(123456);

            var itemList = GetTestList();
            var randomItem = itemList.TakeRandomOne(random, false);

            itemList.Debugger();

            Assert.AreEqual(10, itemList.Count);
            Assert.AreEqual(3, randomItem.ItemId);
        }

        [Test]
        public void Test_TakeNext_With_Remove_True()
        {
            var itemList = GetTestList();
            var destinationList = itemList.TakeNext(5);

            Assert.AreEqual(5, destinationList.Count);
            Assert.AreEqual(5, itemList.Count);

            Assert.AreEqual(1, destinationList[0].ItemId);
            Assert.AreEqual(2, destinationList[1].ItemId);
            Assert.AreEqual(3, destinationList[2].ItemId);
            Assert.AreEqual(4, destinationList[3].ItemId);
            Assert.AreEqual(5, destinationList[4].ItemId);

            Assert.AreEqual(6, itemList[0].ItemId);
            Assert.AreEqual(7, itemList[1].ItemId);
            Assert.AreEqual(8, itemList[2].ItemId);
            Assert.AreEqual(9, itemList[3].ItemId);
            Assert.AreEqual(10, itemList[4].ItemId);
        }

        [Test]
        public void Test_TakeNext_With_Remove_True_And_Offset()
        {
            var itemList = GetTestList();
            var destinationList = itemList.TakeNext(5, 3);

            Assert.AreEqual(5, destinationList.Count);
            Assert.AreEqual(5, itemList.Count);

            Assert.AreEqual(4, destinationList[0].ItemId);
            Assert.AreEqual(5, destinationList[1].ItemId);
            Assert.AreEqual(6, destinationList[2].ItemId);
            Assert.AreEqual(7, destinationList[3].ItemId);
            Assert.AreEqual(8, destinationList[4].ItemId);

            Assert.AreEqual(1, itemList[0].ItemId);
            Assert.AreEqual(2, itemList[1].ItemId);
            Assert.AreEqual(3, itemList[2].ItemId);
            Assert.AreEqual(9, itemList[3].ItemId);
            Assert.AreEqual(10, itemList[4].ItemId);
        }

        [Test]
        public void Test_TakeNext_With_Remove_True_And_Large_Offset()
        {
            var itemList = GetTestList();
            var destinationList = itemList.TakeNext(5, 2022);

            Assert.AreEqual(5, destinationList.Count);
            Assert.AreEqual(5, itemList.Count);

            Assert.AreEqual(3, destinationList[0].ItemId);
            Assert.AreEqual(4, destinationList[1].ItemId);
            Assert.AreEqual(5, destinationList[2].ItemId);
            Assert.AreEqual(6, destinationList[3].ItemId);
            Assert.AreEqual(7, destinationList[4].ItemId);

            Assert.AreEqual(1, itemList[0].ItemId);
            Assert.AreEqual(2, itemList[1].ItemId);
            Assert.AreEqual(8, itemList[2].ItemId);
            Assert.AreEqual(9, itemList[3].ItemId);
            Assert.AreEqual(10, itemList[4].ItemId);
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

            Assert.AreEqual(1, first);
            Assert.AreEqual(4, second);
            Assert.AreEqual(2, third);
            Assert.AreEqual(3, fourth);
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

            Assert.AreEqual(2, first);
            Assert.AreEqual(3, second);
            Assert.AreEqual(4, third);
            Assert.AreEqual(1, fourth);
        }

        [Test]
        public void Test_TakeNext_With_Integers_Remove_True_3()
        {
            var numbers = new List<int> { 1, 2, 3, 4 };
            int offset = 61;

            var taken = numbers.TakeNext(4, offset);

            Assert.AreEqual(4, taken.Count());

            Assert.AreEqual(2, taken[0]);
            Assert.AreEqual(3, taken[1]);
            Assert.AreEqual(4, taken[2]);
            Assert.AreEqual(1, taken[3]);
        }

        [Test]
        public void Test_TakeNext_With_Remove_True_And_Offset_Is_Close_To_End_But_Wraps()
        {
            var itemList = GetTestList();
            var destinationList = itemList.TakeNext(5, 8);

            Assert.AreEqual(5, destinationList.Count);
            Assert.AreEqual(5, itemList.Count);

            Assert.AreEqual(4, itemList[0].ItemId);
            Assert.AreEqual(5, itemList[1].ItemId);
            Assert.AreEqual(6, itemList[2].ItemId);
            Assert.AreEqual(7, itemList[3].ItemId);
            Assert.AreEqual(8, itemList[4].ItemId);

            // Takes five elements as it wraps back to the beginning
            Assert.AreEqual(9, destinationList[0].ItemId);
            Assert.AreEqual(10, destinationList[1].ItemId);
            Assert.AreEqual(1, destinationList[2].ItemId);
            Assert.AreEqual(2, destinationList[3].ItemId);
            Assert.AreEqual(3, destinationList[4].ItemId);
        }

        [Test]
        public void Test_TakeNextOne_With_Remove_True()
        {
            var itemList = GetTestList();
            var nextItem = itemList.TakeNextOne();

            Assert.AreEqual(9, itemList.Count);
            Assert.AreEqual(1, nextItem.ItemId);

            var nextItem2 = itemList.TakeNextOne();

            Assert.AreEqual(8, itemList.Count);
            Assert.AreEqual(2, nextItem2.ItemId);
        }

        [Test]
        public void Test_TakeNextOne_With_Remove_False()
        {
            var itemList = GetTestList();
            var nextItem = itemList.TakeNextOne(remove: false);

            Assert.AreEqual(10, itemList.Count);
            Assert.AreEqual(1, nextItem.ItemId);

            var nextItem2 = itemList.TakeNextOne();

            Assert.AreEqual(9, itemList.Count);
            Assert.AreEqual(1, nextItem2.ItemId);
        }

        [Test]
        public void Test_TakeNextOne_With_Offset_Remove_True()
        {
            var itemList = GetTestList();
            var nextItem = itemList.TakeNextOne(3, true);

            Assert.AreEqual(9, itemList.Count);
            Assert.AreEqual(4, nextItem.ItemId);

            var nextItem2 = itemList.TakeNextOne();

            Assert.AreEqual(8, itemList.Count);
            Assert.AreEqual(1, nextItem2.ItemId);
        }

        [Test]
        public void Test_GroupBy()
        {
            var people = new List<Person>
            {
                new Person { PersonId = 1, FirstName = "Will", LastName= "Smith" },
                new Person { PersonId = 2, FirstName = "Joe", LastName= "Jones" },
                new Person { PersonId = 3, FirstName = "Patty", LastName= "Smith" },
                new Person { PersonId = 4, FirstName = "Jeff", LastName= "Jones" },
                new Person { PersonId = 5, FirstName = "Fred", LastName= "Jones" }
            };

            // familiesList Type is: System.Linq.GroupedEnumerable<Person,string> (not public?)
            // familiesList Interface is: IEnumerable<IGrouping<string, Person>>

            var familiesList = people.GroupBy(g => g.LastName);

            var famDictionary = familiesList.ToDictionary(g => g.Key, g => g.ToList());

            // simplest, best performance(?)
            var famLookUp = people.ToLookup(l => l.LastName);

            Assert.AreEqual(5, people.Count());
            Assert.AreEqual(2, familiesList.Count());

            Assert.AreEqual(2, famDictionary.Count());
            Assert.AreEqual(2, famDictionary.Values.Count());
            Assert.AreEqual(2, famDictionary.Keys.Count());
            Assert.AreEqual(2, famDictionary["Smith"].Count());
            Assert.AreEqual(3, famDictionary["Jones"].Count());

            Assert.AreEqual(2, famLookUp.Count());
            Assert.AreEqual(2, famLookUp["Smith"].Count());
            Assert.AreEqual(3, famLookUp["Jones"].Count());
            Assert.AreEqual(0, famLookUp["Crowther"].Count());
        }
    }
}
