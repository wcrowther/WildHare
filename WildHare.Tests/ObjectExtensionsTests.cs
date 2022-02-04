using NUnit.Framework;
using System;
using WildHare.Extensions;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
    [TestFixture]
    public class ObjectExtensionsTests
    {

        [Test]
        public void GetMetaModel_Basic()
        {
            Type itemType = typeof(Item);
            var metaModel = itemType.GetMetaModel();

            Assert.AreEqual("Item", metaModel.TypeName);
            Assert.AreEqual(5, metaModel.GetMetaProperties().Count);
            Assert.AreEqual("ItemId", metaModel.PrimaryKeyName);
            Assert.AreEqual(typeof(int), metaModel.PrimaryKeyMeta.PropertyType);
            Assert.AreEqual("Int32", metaModel.PrimaryKeyMeta.PropertyType.Name);  
            // note: Reflection returns .net name not c# alias name ie: Int32 instead of int
        }
    }
}
