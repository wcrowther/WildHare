using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using WildHare.Extensions;
using WildHare.Tests.Models;
using ex = WildHare.Extensions.ExpandoExtensions;

namespace WildHare.Tests
{

    // Example using ExpandoObject as a cache
    public class Dummy  
    {
        private ExpandoObject cache = new ExpandoObject();

        public dynamic Cache
        {
            get { return cache ?? null; }
            set { cache = value; }
        }
    }

    [TestFixture]
    public class TestsForExpando
    {
        [Test]
        public void GetByItemName_Basic()
        {
            var dummy = new Dummy();
            dummy.Cache.Invoice = new Invoice
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

            Assert.AreEqual(1,      dummy.Cache.Invoice.InvoiceId);
            Assert.AreEqual(7890,   dummy.Cache.Invoice.AccountId);
            Assert.AreEqual(99.99M, dummy.Cache.Invoice.InvoiceItems[0].Fee);
            Assert.AreEqual(5678,   dummy.Cache.Invoice.InvoiceItems[1].InvoiceItemId);

            // Cast to ExpandoObject
            ExpandoObject cache = dummy.Cache;

            Assert.AreEqual(1,      cache.Get<Invoice>("Invoice").InvoiceId);
            Assert.AreEqual(7890,   cache.Get<Invoice>("Invoice").AccountId);
            Assert.AreEqual(99.99M, cache.Get<Invoice>("Invoice").InvoiceItems[0].Fee);
            Assert.AreEqual(5678,   cache.Get<Invoice>("Invoice").InvoiceItems[1].InvoiceItemId);

            // Inline cast to ExpandoObject alternative
            Assert.AreEqual(1,      ((ExpandoObject)dummy.Cache).Get<Invoice>("Invoice").InvoiceId);
            Assert.AreEqual(7890,   ((ExpandoObject)dummy.Cache).Get<Invoice>("Invoice").AccountId);
            Assert.AreEqual(99.99M, ((ExpandoObject)dummy.Cache).Get<Invoice>("Invoice").InvoiceItems[0].Fee);
            Assert.AreEqual(5678,   ((ExpandoObject)dummy.Cache).Get<Invoice>("Invoice").InvoiceItems[1].InvoiceItemId);

            // Invoke with non-extension method syntax alternative (with ex using statement above)
            Assert.AreEqual(1,      ex.Get<Invoice>(dummy.Cache, "Invoice").InvoiceId);
            Assert.AreEqual(7890,   ex.Get<Invoice>(dummy.Cache, "Invoice").AccountId);
            Assert.AreEqual(99.99M, ex.Get<Invoice>(dummy.Cache, "Invoice").InvoiceItems[0].Fee);
            Assert.AreEqual(5678,   ex.Get<Invoice>(dummy.Cache, "Invoice").InvoiceItems[1].InvoiceItemId);
        }

        [Test]
        public void GetByName_When_Item_Is_Null()
        {
            var dummy = new Dummy();
            dummy.Cache.Invoice = null;

            // Cast to ExpandoObject
            ExpandoObject cache = dummy.Cache;

            Assert.IsNull(cache.Get<Invoice>("Invoice")?.InvoiceId);
            Assert.IsNull(cache.Get<Invoice>("Invoice")?.InvoiceItems[0]?.Fee);
            Assert.IsNull(cache.Get("Invoice"));
        }

        [Test]
        public void GetByName_When_Item_Is_Never_Initialized()
        {
            var dummy = new Dummy();

            // Cast to ExpandoObject
            ExpandoObject cache = dummy.Cache;

            Assert.IsNull(cache.Get<Invoice>("Invoice")?.InvoiceId);
            Assert.IsNull(cache.Get<Invoice>("Invoice")?.InvoiceItems[0]?.Fee);
            Assert.IsNull(cache.Get("Invoice"));
        }

        [Test]
        public void TestRemoveByName()
        {
            var name = "Invoice";
            var dummy = new Dummy();
            dummy.Cache.Invoice = new Invoice
            {
                InvoiceId = 1
            };
            ExpandoObject cache = dummy.Cache;
            cache.Remove(name);

            Assert.AreEqual(null, cache.Get<Invoice>(name)); // Does not throw
        }

        [Test]
        public void TestAddByName()
        {
            string name = "Invoice";
            var dummy = new Dummy();
            var invoice = new Invoice
            {
                InvoiceId = 1
            };

            ExpandoObject cache = dummy.Cache;
            cache.Add(name, invoice);

            Assert.AreEqual(1, cache.Get<Invoice>(name).InvoiceId);
        }

        [Test]
        public void Test_Add_And_Get_AsString()
        {
            string name = "InvoiceItem";
            string value = "Doodad";
            var dummy = new Dummy();

            ExpandoObject cache = dummy.Cache;
            cache.Add(name, value);

            Assert.AreEqual(value, cache.Get(name));
        }



    }
}
