using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using WildHare.Extensions;
using WildHare.Tests.Models;

namespace WildHare.Tests
{

    // Example using ExpandoObject as a cache
    public class Dummy  
    {
        private ExpandoObject cache = new ExpandoObject();

        public dynamic Cache
        {
            get { return cache; }
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
                    new InvoiceItem{ InvoiceItemId = 1234 , Fee = 99.99M },
                    new InvoiceItem{ InvoiceItemId = 5678 , Fee = 2.22M }
                }
            };

            // Cast to ExpandoObject
            ExpandoObject cache = dummy.Cache;

            Assert.AreEqual(1,      cache.GetByItemName<Invoice>("Invoice").InvoiceId);
            Assert.AreEqual(7890,   cache.GetByItemName<Invoice>("Invoice").AccountId);
            Assert.AreEqual(99.99M, cache.GetByItemName<Invoice>("Invoice").InvoiceItems[0].Fee);
            Assert.AreEqual(5678,   cache.GetByItemName<Invoice>("Invoice").InvoiceItems[1].InvoiceItemId);

            // Inline cast to ExpandoObject alternative
            Assert.AreEqual(1,      ((ExpandoObject)dummy.Cache).GetByItemName<Invoice>("Invoice").InvoiceId);
            Assert.AreEqual(7890,   ((ExpandoObject)dummy.Cache).GetByItemName<Invoice>("Invoice").AccountId);
            Assert.AreEqual(99.99M, ((ExpandoObject)dummy.Cache).GetByItemName<Invoice>("Invoice").InvoiceItems[0].Fee);
            Assert.AreEqual(5678,   ((ExpandoObject)dummy.Cache).GetByItemName<Invoice>("Invoice").InvoiceItems[1].InvoiceItemId);

            // Invoke with non-extension method syntax alternative
            Assert.AreEqual(1,      ExpandoExtensions.GetByItemName<Invoice>(dummy.Cache, "Invoice").InvoiceId);
            Assert.AreEqual(7890,   ExpandoExtensions.GetByItemName<Invoice>(dummy.Cache, "Invoice").AccountId);
            Assert.AreEqual(99.99M, ExpandoExtensions.GetByItemName<Invoice>(dummy.Cache, "Invoice").InvoiceItems[0].Fee);
            Assert.AreEqual(5678,   ExpandoExtensions.GetByItemName<Invoice>(dummy.Cache, "Invoice").InvoiceItems[1].InvoiceItemId);
        }

        [Test]
        public void TestRemoveByItemName()
        {
            var name = "Invoice";
            var dummy = new Dummy();
            dummy.Cache.Invoice = new Invoice
            {
                InvoiceId = 1
            };
            ExpandoObject cache = dummy.Cache;
            cache.RemoveItemByName(name);

            Assert.AreEqual(null, cache.GetByItemName<Invoice>(name)); // Does not throw
        }

        [Test]
        public void TestAddByItemName()
        {
            string name = "Invoice";
            var dummy = new Dummy();
            var invoice = new Invoice
            {
                InvoiceId = 1
            };

            ExpandoObject cache = dummy.Cache;
            cache.AddItemByName(name, invoice);

            Assert.AreEqual(1, cache.GetByItemName<Invoice>(name).InvoiceId);
        }
    }
}
