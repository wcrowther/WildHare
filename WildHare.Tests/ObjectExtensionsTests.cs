using NUnit.Framework;
using NUnit.Framework.Legacy;
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

            ClassicAssert.AreEqual("Item", metaModel.TypeName);
            ClassicAssert.AreEqual(5, metaModel.GetMetaProperties().Count);
            ClassicAssert.AreEqual("ItemId", metaModel.PrimaryKeyName);
            ClassicAssert.AreEqual(typeof(int), metaModel.PrimaryKeyMeta.PropertyType);
            ClassicAssert.AreEqual("Int32", metaModel.PrimaryKeyMeta.PropertyType.Name); 
            
            // note: Reflection returns .net name not c# alias name ie: Int32 instead of int
        }
    }
}
