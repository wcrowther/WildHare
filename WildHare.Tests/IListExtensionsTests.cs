using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WildHare.Extensions;
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
            Assert.AreEqual(10,itemList[4].ItemId);
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
            Assert.AreEqual(9,  destinationList[0].ItemId);
            Assert.AreEqual(10, destinationList[1].ItemId);
            Assert.AreEqual(1,  destinationList[2].ItemId);
            Assert.AreEqual(2,  destinationList[3].ItemId);
            Assert.AreEqual(3,  destinationList[4].ItemId);
        }
    }
}
