using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using static System.Environment;

namespace WildHare.Tests
{
    [TestFixture]
    public class DictionaryExtensionTests
    {
		[Test]
		public void Test_Dictionary_Safe_If_Key_Does_Not_Exist()
		{
            var dictionary =  new Dictionary<int, string>
                {
                    {1, "platypus"},
                    {2, "cheetah"},
                    {3, "cat"}
                };

            Assert.AreEqual("cheetah", dictionary[2]);
            Assert.AreEqual("Not Found", dictionary.GetValueOrDefault(5,"Not Found"));
		}

    }
}
