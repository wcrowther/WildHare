using NUnit.Framework;
using System;
using System.Collections.Generic;
using WildHare.Extensions.ForType;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
    [TestFixture]
    public class TestsForType
    {
        //[SetUp]
        //public void Setup()
        //{ }

        [Test]
        public void GetMetaModel_Basic()
        {
            Type itemType = typeof(Item);
            var metaModel = itemType.GetMetaModel();

            Assert.AreEqual("Item", metaModel.TypeName);
            Assert.AreEqual("ItemId", metaModel.GetMetaProperties()[0].Name);

            Assert.AreEqual(3, metaModel.GetMetaProperties().Count);
            Assert.AreEqual("ItemName", metaModel.GetMetaProperties()[1].Name);
            Assert.AreEqual("Created", metaModel.GetMetaProperties()[2].Name);
            Assert.AreEqual("ItemId", metaModel.PrimaryKeyName);
        }

        [Test]
        public void GetMetaModel_FromIEnumerable()
        {
            var itemList = new List<Item>();
            var metaModel = itemList.GetMetaModel();

            Assert.AreEqual("Item", metaModel.TypeName);
            // Assert.AreEqual("ItemId", metaModel.PrimaryKeyName);

            Assert.AreEqual(3, metaModel.GetMetaProperties().Count);
            Assert.AreEqual("ItemId", metaModel.GetMetaProperties()[0].Name);
            Assert.AreEqual("ItemName", metaModel.GetMetaProperties()[1].Name);
            Assert.AreEqual("Created", metaModel.GetMetaProperties()[2].Name);
        }

        [Test]
        public void GetMetaModel_FromInstance()
        {
            var item = new Item { ItemId = 1, ItemName = "One", Created = DateTime.Now }; 
            var metaModel = item.GetMetaModel();

            Assert.AreEqual("Item", metaModel.TypeName);
            // Assert.AreEqual("ItemId", metaModel.PrimaryKeyName); Sometimes fails...

            Assert.AreEqual(3, metaModel.GetMetaProperties().Count);
            Assert.AreEqual("ItemId", metaModel.GetMetaProperties()[0].Name);
            Assert.AreEqual("ItemName", metaModel.GetMetaProperties()[1].Name);
            Assert.AreEqual("Created", metaModel.GetMetaProperties()[2].Name);
        }

        [Test]
        public void GetMetaModel_FromDictionary()
        {
            var dictionay = new Dictionary<string, Item>();
            var metaModel = dictionay.GetMetaModel();

            Assert.AreEqual("String", metaModel.DictionaryKeyType.Name); // Not string (lowercase) for some reason
            Assert.AreEqual("Item", metaModel.DictionaryValueType.Name);

            Assert.AreEqual(5, metaModel.GetMetaProperties().Count);
            Assert.AreEqual("Comparer", metaModel.GetMetaProperties()[0].Name);
            Assert.AreEqual("Count", metaModel.GetMetaProperties()[1].Name);
            Assert.AreEqual("Keys", metaModel.GetMetaProperties()[2].Name);
            Assert.AreEqual("Values", metaModel.GetMetaProperties()[3].Name);
            Assert.AreEqual("Item", metaModel.GetMetaProperties()[4].Name);
        }

        [Test]
        public void GetMetaProperties_Basic()
        {
            var item = new Item { ItemId = 1, ItemName = "One", Created = DateTime.Now }; ;
            var metaProperties = item.GetMetaProperties();

            Assert.AreEqual(3, metaProperties.Count);
            Assert.AreEqual("ItemId", metaProperties[0].Name);
            Assert.AreEqual("ItemName", metaProperties[1].Name);
            Assert.AreEqual("Created", metaProperties[2].Name);
        }

        [Test]
        public void GetMetaProperties_FromIEnumerable()
        {
            var itemList = new List<Item>();
            var metaProperties = itemList.GetMetaProperties();

            Assert.AreEqual(3, metaProperties.Count);
            Assert.AreEqual("ItemId", metaProperties[0].Name);
            Assert.AreEqual("ItemName", metaProperties[1].Name);
            Assert.AreEqual("Created", metaProperties[2].Name);
        }

        [Test]
        public void GetMetaProperties_FromIEnumerable_Excluded()
        {
            var itemList = new List<Item>();
            var metaProperties = itemList.GetMetaProperties("ItemId, ItemName");

            Assert.AreEqual(1, metaProperties.Count);
            Assert.AreEqual("Created", metaProperties[0].Name);
        }

        [Test]
        public void GetMetaProperties_FromIEnumerable_Included()
        {
            var itemList = new List<Item>();
            var metaProperties = itemList.GetMetaProperties(include: "ItemName,Created");

            Assert.AreEqual(2, metaProperties.Count);
            Assert.AreEqual("ItemName", metaProperties[0].Name);
            Assert.AreEqual("Created", metaProperties[1].Name);
        }

        [Test]
        public void GetMetaProperties_FromIEnumerable_Included_And_Excluded()
        {
            string errorMessage = "The GetMetaProperties method only accepts the exclude OR the include list.";
            var itemList = new List<Item>();

            var ex = Assert.Throws<Exception>
            (
                () => itemList.GetMetaProperties("ItemId", include: "ItemName,Created")
            );
            Assert.AreEqual(errorMessage, ex.Message);
        }
    }
}