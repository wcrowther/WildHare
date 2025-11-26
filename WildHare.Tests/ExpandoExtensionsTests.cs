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

    // https ://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject.trysetmember?view=net-5.0

    [TestFixture]
    public class TestsForExpando
    {
        // ==============================================================================
        // Example using ExpandoObject as a cache
        // SEE DynamicExpando as a better solution now that it has an indexer 3/2021
        // ==============================================================================

        public class Dummy
        {
            private ExpandoObject cache = new ExpandoObject();

            public dynamic Cache
            {
                get => cache ?? null;
                set => cache = value;
            }
        }

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

            ClassicAssert.AreEqual(1, dummy.Cache.Invoice.InvoiceId);
            ClassicAssert.AreEqual(7890, dummy.Cache.Invoice.AccountId);
            ClassicAssert.AreEqual(99.99M, dummy.Cache.Invoice.InvoiceItems[0].Fee);
            ClassicAssert.AreEqual(5678, dummy.Cache.Invoice.InvoiceItems[1].InvoiceItemId);
            ClassicAssert.AreEqual(3, dummy.Cache.Invoice.InvoiceItems.Count);

            // Cast to ExpandoObject
            ExpandoObject cache = dummy.Cache;

            ClassicAssert.AreEqual(1, cache.Get<Invoice>("Invoice").InvoiceId);
            ClassicAssert.AreEqual(7890, cache.Get<Invoice>("Invoice").AccountId);
            ClassicAssert.AreEqual(99.99M, cache.Get<Invoice>("Invoice").InvoiceItems[0].Fee);
            ClassicAssert.AreEqual(5678, cache.Get<Invoice>("Invoice").InvoiceItems[1].InvoiceItemId);

            // Inline cast to ExpandoObject alternative
            ClassicAssert.AreEqual(1, ((ExpandoObject)dummy.Cache).Get<Invoice>("Invoice").InvoiceId);
            ClassicAssert.AreEqual(7890, ((ExpandoObject)dummy.Cache).Get<Invoice>("Invoice").AccountId);
            ClassicAssert.AreEqual(99.99M, ((ExpandoObject)dummy.Cache).Get<Invoice>("Invoice").InvoiceItems[0].Fee);
            ClassicAssert.AreEqual(5678, ((ExpandoObject)dummy.Cache).Get<Invoice>("Invoice").InvoiceItems[1].InvoiceItemId);

            // Invoke with non-extension method syntax alternative (with ex using statement above)
            ClassicAssert.AreEqual(1, Ex.Get<Invoice>(dummy.Cache, "Invoice").InvoiceId);
            ClassicAssert.AreEqual(7890, Ex.Get<Invoice>(dummy.Cache, "Invoice").AccountId);
            ClassicAssert.AreEqual(99.99M, Ex.Get<Invoice>(dummy.Cache, "Invoice").InvoiceItems[0].Fee);
            ClassicAssert.AreEqual(5678, Ex.Get<Invoice>(dummy.Cache, "Invoice").InvoiceItems[1].InvoiceItemId);
        }

        [Test]
        public void GetByName_When_Item_Is_Null()
        {
            var dummy = new Dummy();
            dummy.Cache.Invoice = null;

            // Cast to ExpandoObject
            ExpandoObject cache = dummy.Cache;

            ClassicAssert.IsNull(cache.Get<Invoice>("Invoice")?.InvoiceId);
            ClassicAssert.IsNull(cache.Get<Invoice>("Invoice")?.InvoiceItems[0]?.Fee);
			ClassicAssert.IsNull(cache.Get("Invoice"));
        }

        [Test]
        public void GetByName_When_Item_Is_Never_Initialized()
        {
            var dummy = new Dummy();

            // Cast to ExpandoObject
            ExpandoObject cache = dummy.Cache;

            ClassicAssert.IsNull(cache.Get<Invoice>("Invoice")?.InvoiceId);
            ClassicAssert.IsNull(cache.Get<Invoice>("Invoice")?.InvoiceItems[0]?.Fee);
			ClassicAssert.IsNull(cache.Get("Invoice"));
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

            ClassicAssert.AreEqual(null, cache.Get<Invoice>(name)); // Does not throw
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

            ClassicAssert.AreEqual(1, cache.Get<Invoice>(name).InvoiceId);
        }

        [Test]
        public void Test_Add_And_Get_AsString()
        {
            string name = "InvoiceItem";
            string value = "Doodad";
            var dummy = new Dummy();

            ExpandoObject cache = dummy.Cache;
            cache.Add(name, value);

            ClassicAssert.AreEqual(value, cache.Get(name));
        }

        [Test]
        public void Test__Get_List_From_Expando()
        {
            var dummy = new Dummy();
            dummy.Cache.Invoices = new List<Invoice>
            {
                new Invoice{ InvoiceId = 1, AccountId = 1},
                new Invoice{ InvoiceId = 2, AccountId = 1}
            };

            ClassicAssert.AreEqual(2, dummy.Cache.Invoices.Count);
        }

        [Test]
        public void GetByItemName_List_Select()
        {
            var dummy = new Dummy();
            dummy.Cache.InvoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem{ InvoiceItemId = 1234 , Fee = 99.99M, Product = "Doodad" },
                new InvoiceItem{ InvoiceItemId = 5678 , Fee = 2.22M,  Product = "Thingamajig" },
                new InvoiceItem{ InvoiceItemId = 9000 , Fee = 100.00M,Product = "Thing" }
            };

            // Cast to ExpandoObject
            ExpandoObject cache = dummy.Cache;

            List<InvoiceItem> itemsList = cache.Get<List<InvoiceItem>>("InvoiceItems");

            ClassicAssert.AreEqual(3, cache.Get<List<InvoiceItem>>("InvoiceItems").Select(a => a.InvoiceId).Count());
            ClassicAssert.AreEqual(1234, cache.Get<List<InvoiceItem>>("InvoiceItems").Select(a => a.InvoiceItemId).First());
            ClassicAssert.AreEqual(3, itemsList.Count);
        }

        [Test]
        public void Dynamic_List_Add_Items()
        {
            var dummy = new Dummy();
            dummy.Cache.InvoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem{ InvoiceItemId = 1234 , Fee = 99.99M, Product = "Doodad" },
                new InvoiceItem{ InvoiceItemId = 5678 , Fee = 2.22M,  Product = "Thingamajig" },
                new InvoiceItem{ InvoiceItemId = 9000 , Fee = 100.00M,Product = "Thing" }
            };

            var invoiceItem = new InvoiceItem { InvoiceItemId = 1000, Fee = 10.00M, Product = "Extra" };

            dummy.Cache.InvoiceItems.Add(invoiceItem);

            ClassicAssert.AreEqual(4, dummy.Cache.InvoiceItems.Count);
        }


        [Test]
        public void Dynamic_List_()
        {
            var dummy = new Dummy();
            dummy.Cache.InvoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem{ InvoiceItemId = 1234 , Fee = 99.99M, Product = "Doodad" },
                new InvoiceItem{ InvoiceItemId = 5678 , Fee = 2.22M,  Product = "Thingamajig" },
                new InvoiceItem{ InvoiceItemId = 9000 , Fee = 100.00M,Product = "Thing" }
            };

            var invoiceItem = new InvoiceItem { InvoiceItemId = 1000, Fee = 10.00M, Product = "Extra" };

            dummy.Cache.InvoiceItems.Add(invoiceItem);

            ClassicAssert.AreEqual(4, dummy.Cache.InvoiceItems.Count);
        }
    }
}
