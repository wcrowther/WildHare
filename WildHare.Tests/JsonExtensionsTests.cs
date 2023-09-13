using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using WildHare.Extensions;
using WildHare.Tests.Helpers;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
    [TestFixture]
    public class JsonExtensionsTests
    {
        // private object GetJsonObject()
        // {
        //     var obj = new JsonObject
        //     {
        //         ["Id"] = 3,
        //         ["Name"] = "Bob",
        //         ["DOB"] = new DateTime(2001, 02, 03),
        //         ["Friends"] = new JsonArray
        //         {
        //             new JsonObject
        //             {
        //                 ["Id"] = 2,
        //                 ["Name"] = "Smith"
        //             },
        //             new JsonObject
        //             {
        //                 ["Id"] = 4,
        //                 ["Name"] = "Jones"
        //             }
        //         }
        //     };
        // }

        [Test]
        public void Test_()
        {
            Assert.AreEqual(1,1);
        }
    }
}
