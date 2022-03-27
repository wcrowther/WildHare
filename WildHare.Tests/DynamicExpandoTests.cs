using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using WildHare.Extensions;
using WildHare.Models;
using WildHare.Tests.Models;
using ex = WildHare.Extensions.ExpandoExtensions;

namespace WildHare.Tests
{
    // REFERENCE: https ://stackoverflow.com/questions/1653046/what-are-the-true-benefits-of-expandoobject

    [TestFixture]
    public class DynamicExpandoTests
    {
        // ===============================================================================================================
        // DynamicExpando
        // ===============================================================================================================

        public class Item
        {
            public string Name { get; set; } = "Fred";

            public dynamic Tags { get; set; } = new DynamicExpando();
        }

        [Test]
        public void DynamicExpando_Test_Basic()
        {
            var item = new Item();

            item.Tags.IntegerValue      = 3;
            item.Tags.Word              = "united";
            item.Tags.List              = new List<string>{ "tom", "dick", "harry" };
            item.Tags.List2             = new List<string>();
            item.Tags["Country"]        = "England";

            Assert.AreEqual("Fred",     item.Name);
            Assert.AreEqual(3,          item.Tags.IntegerValue);
            Assert.AreEqual("united",   item.Tags.Word);
            Assert.AreEqual(null,       item.Tags.Sport);
            Assert.AreEqual(3,          item.Tags.List.Count);
            Assert.AreEqual(0,          item.Tags.List2.Count);
            Assert.AreEqual("England",  item.Tags.Country);

            item.Tags.List.Add("will");
            item.Tags["Word"]           = "manchester";
            item.Tags.Country           = "Italy";

            Assert.AreEqual(4,            item.Tags.List.Count);
            Assert.AreEqual("manchester", item.Tags.Word);
            Assert.AreEqual("Italy",      item.Tags.Country);

            item.Tags.Remove("IntegerValue");

            Assert.IsNull(item.Tags.IntegerValue);
        }

        public void DynamicExpando_Test_Basic_2()
        {
            var item = new Item();


            item.Tags["Fruit"] = new List<string> { "apple", "orange", "pear" };

            Assert.AreEqual(3, item.Tags.Fruit.Count);

            item.Tags.Fruit.Add("kiwi");

            Assert.AreEqual(4, item.Tags.Fruit.Count);

            // THIS DOES NOT WORK: 
            // Assert.AreEqual("kiwi",       item.Tags.Fruit.ElementAt(3));

            var ex = Assert.Throws<Exception>
            (
                () => item.Tags.Fruit.ElementAt(3)
            );

            // ... as extension methods cannot be called on Dynamic, but can be 
            // called using (klunky) non-extension method syntax as:

            Assert.AreEqual("kiwi", Enumerable.ElementAt(item.Tags.Fruit, 3));
        }

        [Test]
        public void DynamicExpando_Test_Functions()
        {
            var item = new Item();

            item.Tags.IntegerValue = 3;
            item.Tags.Word = "united";
            item.Tags.List = new List<string> { "tom", "dick", "harry" };
            item.Tags.List2 = new List<string>();
            item.Tags["Country"] = "England";

            Assert.AreEqual(5, item.Tags.Count);
            Assert.AreEqual(3, item.Tags.First().Value);
            Assert.AreEqual("IntegerValue", item.Tags.First().Key);
            // Assert.AreEqual(true, item.Tags.Any(a => a.Key == "Country" ));  // FAILS


            item.Tags.Remove("IntegerValue");

            Assert.AreEqual(4, item.Tags.Count);
            Assert.AreEqual("united", item.Tags.FirstOrDefault().Value);

            item.Tags.Clear();

            var exception = Assert.Throws<InvalidOperationException>
            (
                () => item.Tags.First()
            );

            Assert.IsNotNull(exception);
            Assert.IsNull(item.Tags.FirstOrDefault().Key);
            Assert.IsNull(item.Tags.FirstOrDefault().Value);
        }

        [Test]
        public void DynamicExpando_Test_Advanced()
        {
            var item = new Item();

            item.Tags.Invoice = new Invoice
            {
                InvoiceId = 1,
                AccountId = 7890,
                InvoiceItems = new List<InvoiceItem>
                {
                    new InvoiceItem{ InvoiceItemId = 1234 , Fee = 99.99M, Product = "Doodad" },
                    new InvoiceItem{ InvoiceItemId = 5678 , Fee = 2.22M,  Product = "Thingamajig" },
                    new InvoiceItem{ InvoiceItemId = 9000 , Fee = 100.00M,Product = "Thing" }
                }
            };

            Assert.AreEqual(1,          item.Tags.Invoice.InvoiceId);
            Assert.AreEqual(7890,       item.Tags.Invoice.AccountId);
            Assert.AreEqual(99.99M,     item.Tags.Invoice.InvoiceItems[0].Fee);
            Assert.AreEqual(5678,       item.Tags.Invoice.InvoiceItems[1].InvoiceItemId);

            //List<InvoiceItem> items = item.Tags.Invoice.InvoiceItems.ToList();
            //Assert.AreEqual(202.21M,  items.Sum(s => s.Fee));

        }
    }
}
