using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using WildHare.Extensions;
using WildHare.Tests.Models;
using Ex = WildHare.Extensions.ExpandoExtensions;

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

            ClassicAssert.AreEqual("Fred",     item.Name);
            ClassicAssert.AreEqual(3,          item.Tags.IntegerValue);
            ClassicAssert.AreEqual("united",   item.Tags.Word);
            ClassicAssert.AreEqual(null,       item.Tags.Sport);
            ClassicAssert.AreEqual(3,          item.Tags.List.Count);
            ClassicAssert.AreEqual(0,          item.Tags.List2.Count);
            ClassicAssert.AreEqual("England",  item.Tags.Country);

            item.Tags.List.Add("will");
            item.Tags["Word"]           = "manchester";
            item.Tags.Country           = "Italy";

            ClassicAssert.AreEqual(4,            item.Tags.List.Count);
            ClassicAssert.AreEqual("manchester", item.Tags.Word);
            ClassicAssert.AreEqual("Italy",      item.Tags.Country);

            item.Tags.Remove("IntegerValue");

			ClassicAssert.IsNull(item.Tags.IntegerValue);
        }

        public void DynamicExpando_Test_Basic_2()
        {
            var item = new Item();


            item.Tags["Fruit"] = new List<string> { "apple", "orange", "pear" };

            ClassicAssert.AreEqual(3, item.Tags.Fruit.Count);

            item.Tags.Fruit.Add("kiwi");

            ClassicAssert.AreEqual(4, item.Tags.Fruit.Count);

            // THIS DOES NOT WORK: 
            // ClassicAssert.AreEqual("kiwi",       item.Tags.Fruit.ElementAt(3));

            var ex = Assert.Throws<Exception>
            (
                () => item.Tags.Fruit.ElementAt(3)
            );

            // ... as extension methods cannot be called on Dynamic, but can be 
            // called using (klunky) non-extension method syntax as:

            ClassicAssert.AreEqual("kiwi", Enumerable.ElementAt(item.Tags.Fruit, 3));
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

            ClassicAssert.AreEqual(5, item.Tags.Count);
            ClassicAssert.AreEqual(3, item.Tags.First().Value);
            ClassicAssert.AreEqual("IntegerValue", item.Tags.First().Key);
            // ClassicAssert.AreEqual(true, item.Tags.Any(a => a.Key == "Country" ));  // FAILS


            item.Tags.Remove("IntegerValue");

            ClassicAssert.AreEqual(4, item.Tags.Count);
            ClassicAssert.AreEqual("united", item.Tags.FirstOrDefault().Value);

            item.Tags.Clear();

            var exception = ClassicAssert.Throws<InvalidOperationException>
            (
                () => item.Tags.First()
            );

            ClassicAssert.IsNotNull(exception);
            ClassicAssert.IsNull(item.Tags.FirstOrDefault().Key);
			ClassicAssert.IsNull(item.Tags.FirstOrDefault().Value);
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

            ClassicAssert.AreEqual(1,          item.Tags.Invoice.InvoiceId);
            ClassicAssert.AreEqual(7890,       item.Tags.Invoice.AccountId);
            ClassicAssert.AreEqual(99.99M,     item.Tags.Invoice.InvoiceItems[0].Fee);
            ClassicAssert.AreEqual(5678,       item.Tags.Invoice.InvoiceItems[1].InvoiceItemId);

            //List<InvoiceItem> items = item.Tags.Invoice.InvoiceItems.ToList();
            //ClassicAssert.AreEqual(202.21M,  items.Sum(s => s.Fee));

        }
    }
}
