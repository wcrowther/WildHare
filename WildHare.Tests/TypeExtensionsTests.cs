using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        [Test]
        public void GetMetaModel_Basic()
        {
            Type itemType = typeof(Item);
            var metaModel = itemType.GetMetaModel();

            Assert.AreEqual("Item", metaModel.TypeName);
            Assert.AreEqual(4, metaModel.GetMetaProperties().Count);
            Assert.AreEqual("ItemId", metaModel.PrimaryKeyName);

            Assert.AreEqual("ItemId", metaModel.GetMetaProperties()[0].Name);
            Assert.AreEqual("ItemName", metaModel.GetMetaProperties()[1].Name);
            Assert.AreEqual("Created", metaModel.GetMetaProperties()[2].Name);
            Assert.AreEqual("Stuff", metaModel.GetMetaProperties()[3].Name);

            Assert.AreEqual(typeof(int), metaModel.GetMetaProperties()[0].PropertyType);
            Assert.AreEqual(typeof(string), metaModel.GetMetaProperties()[1].PropertyType);
            Assert.AreEqual(typeof(DateTime), metaModel.GetMetaProperties()[2].PropertyType);
            Assert.AreEqual(typeof(List<string>), metaModel.GetMetaProperties()[3].PropertyType);
        }

        [Test]
        public void GetMetaModel_FromIEnumerable()
        {
            var itemList = new List<Item>();
            var metaModel = itemList.GetMetaModel();

            Assert.AreEqual(4, metaModel.GetMetaProperties().Count);
            Assert.AreEqual("ItemId", metaModel.PrimaryKeyName);

            Assert.AreEqual("ItemId", metaModel.GetMetaProperties()[0].Name);
            Assert.AreEqual("ItemName", metaModel.GetMetaProperties()[1].Name);
            Assert.AreEqual("Created", metaModel.GetMetaProperties()[2].Name);
            Assert.AreEqual("Stuff", metaModel.GetMetaProperties()[3].Name);

            Assert.AreEqual(typeof(int), metaModel.GetMetaProperties()[0].PropertyType);
            Assert.AreEqual(typeof(string), metaModel.GetMetaProperties()[1].PropertyType);
            Assert.AreEqual(typeof(DateTime), metaModel.GetMetaProperties()[2].PropertyType);
            Assert.AreEqual(typeof(List<string>), metaModel.GetMetaProperties()[3].PropertyType);
        }

        [Test]
        public void GetMetaModel_FromInstance()
        {
            var item = new Item { ItemId = 1, ItemName = "One", Created = DateTime.Now }; 
            var metaModel = item.GetMetaModel();

            Assert.AreEqual(4, metaModel.GetMetaProperties().Count);
            Assert.AreEqual("ItemId", metaModel.PrimaryKeyName);

            Assert.AreEqual("ItemId", metaModel.GetMetaProperties()[0].Name);
            Assert.AreEqual("ItemName", metaModel.GetMetaProperties()[1].Name);
            Assert.AreEqual("Created", metaModel.GetMetaProperties()[2].Name);
            Assert.AreEqual("Stuff", metaModel.GetMetaProperties()[3].Name);

            Assert.AreEqual(typeof(int), metaModel.GetMetaProperties()[0].PropertyType);
            Assert.AreEqual(typeof(string), metaModel.GetMetaProperties()[1].PropertyType);
            Assert.AreEqual(typeof(DateTime), metaModel.GetMetaProperties()[2].PropertyType);
            Assert.AreEqual(typeof(List<string>), metaModel.GetMetaProperties()[3].PropertyType);
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
        public void GetMetaModel_From_Model_Without_Parameterless_Constructor()
        {
            var metaListModel = new List<ClassRequiringCtorParam>().GetMetaModel();

            Assert.AreEqual(1, metaListModel.GetMetaProperties().Count);
            Assert.AreEqual("ClassName", metaListModel.GetMetaProperties().First().Name);

            var list = new List<ClassRequiringCtorParam> { new ClassRequiringCtorParam("Fred") };

            Assert.AreEqual("ClassRequiringCtorParam", list.GetMetaModel().TypeName);
            Assert.AreEqual("Fred", list.First().ClassName);
        }

        [Test]
        public void GetMetaProperties_Basic()
        {
            var item = new Item { ItemId = 1, ItemName = "One", Created = DateTime.Now }; ;
            var metaProperties = item.GetMetaProperties();

            Assert.AreEqual(4, metaProperties.Count);
            Assert.AreEqual("ItemId", metaProperties[0].Name);
            Assert.AreEqual("ItemName", metaProperties[1].Name);
            Assert.AreEqual("Created", metaProperties[2].Name);
            Assert.AreEqual("Stuff", metaProperties[3].Name);
        }

        [Test]
        public void GetMetaProperties_FromIEnumerable_Excluded()
        {
            var itemList = new List<Item>();
            var metaProperties = itemList.GetMetaProperties("ItemId, ItemName");

            Assert.AreEqual(2, metaProperties.Count);
            Assert.AreEqual("Created", metaProperties[0].Name);
            Assert.AreEqual("Stuff", metaProperties[1].Name);
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

        [Test]
        public void MetaProperty_GetInstanceValue_From_Instance()
        {
            var now = DateTime.Now;
            var item = new Item
            {
                ItemId = 1,
                ItemName = "One",
                Created = now,
                Stuff = new List<string>(){ "stuff1", "stuff2"}
            };
            var metaProperties = item.GetMetaProperties();

            Assert.AreEqual(1, metaProperties[0].GetInstanceValue());           // ItemId
            Assert.AreEqual("One",  metaProperties[1].GetInstanceValue());      // ItemName
            Assert.AreEqual(now,  metaProperties[2].GetInstanceValue());        // Created
            Assert.AreEqual(2, metaProperties[3].GetInstanceValue().Count);     // Stuff
            Assert.AreEqual("stuff1", metaProperties[3].GetInstanceValue()[0]); // Stuff1
            Assert.AreEqual("stuff2", metaProperties[3].GetInstanceValue()[1]); // Stuff2

            // For some reason ElementAt extension method does not work on dynamic List<Item> ?
            // Assert.AreEqual("stuff1", metaProperties[3].GetInstanceValue().ToList().ElementAt(0));
            var list = item.Stuff.ElementAt(0); // this works for non-dynamic
        }

        [Test]
        public void MetaProperty_GetInstanceValue_With_Instance_Injected_Into_Property()
        {
            var item = new Item
            {
                ItemName = "One"
            };

            PropertyInfo propertyInfo = typeof(Item).GetProperties()[1];
            var ItemNameMetaProperty = new MetaProperty(propertyInfo);

            Assert.AreEqual("One", ItemNameMetaProperty.GetInstanceValue(item)); // ItemName
        }

        [Test]
        public void MetaProperty_SetInstanceValue_From_Instance()
        {
            // THIS IS IMPORTANT WHEN TYPES ARE DYNAMIC AS TYPED PROPERTIES 
            // CAN BE SET WITHOUT HAVING TO KNOW THE UNDERLYING COMPILE-TIME TYPE

            var item = new Item
            {
                ItemId = 1,
                ItemName = "One"
            };

            var metaProperties = item.GetMetaProperties();
            metaProperties[0].SetInstanceValue(2);
            metaProperties[1].SetInstanceValue("Two");

            Assert.AreEqual(2, item.ItemId);         
            Assert.AreEqual("Two", item.ItemName);    
        }

        [Test]
        public void MetaProperty_SetInstanceValue_With_Instance_Injected_Into_Property()
        {
            var item = new Item
            {
                ItemName = "One"
            };

            PropertyInfo propertyInfo = typeof(Item).GetProperties()[1];
            var ItemName_MetaProperty = new MetaProperty(propertyInfo);

            ItemName_MetaProperty.SetInstanceValue("Two", item);

            Assert.AreEqual("Two", item.ItemName);

            // Alternate way
            PropertyInfo propertyInfoAlt = typeof(Item).GetProperties()[1];
            var ItemName_MetaPropertyAlt = new MetaProperty(propertyInfoAlt, item);

            ItemName_MetaPropertyAlt.SetInstanceValue("Three");

            Assert.AreEqual("Three", item.ItemName);
        }

    }
}
