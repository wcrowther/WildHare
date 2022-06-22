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
            var dictionary = new Dictionary<int, string>
            {
                {1, "platypus"},
                {2, "cheetah"},
                {3, "cat"}
            };

            // GetValueOrDefault is a method on Dictionary - no extension method needed for this

            Assert.AreEqual("cheetah", dictionary[2]);
            Assert.AreEqual("Not Found", dictionary.GetValueOrDefault(5, "Not Found"));
        }

        [Test]
        public void Test_Dictionary_Safe_If_Key_Does_Not_Exist_With_string_string()
        {
            var dictionary = new Dictionary<string, string>
            {
                {"Participle", "past"},
                {"IsPlural", "true"},
                {"Numeric", "123"}
            };

            // GetValueOrDefault is a method on Dictionary - no extension method needed for this

            Assert.AreEqual("past", dictionary.GetValueOrDefault("Participle", "present"));
            Assert.AreEqual("present", dictionary.GetValueOrDefault("Participle_X", "present"));

            Assert.AreEqual("true", dictionary.GetValueOrDefault("IsPlural", "true"));
            Assert.AreEqual("false", dictionary.GetValueOrDefault("IsPlural_X", "false"));

            Assert.AreEqual("123", dictionary.GetValueOrDefault("Numeric", "0"));
            Assert.AreEqual("0", dictionary.GetValueOrDefault("Numeric_X", "0"));
        }



        [Test]
        public void Test_ToQueryDictionary()
        {
            string queryString = "?name=fred&email=fred@fred.com&customer=true";
            var qDictionary = queryString.RemoveStart("?").ToQueryDictionary();

            Assert.AreEqual(3, qDictionary.Count);
            Assert.AreEqual("fred", qDictionary["name"]);
            Assert.AreEqual("fred@fred.com", qDictionary["email"]);
            Assert.AreEqual("true", qDictionary["customer"]);
        }

        [Test]
        public void Test_ToQueryString()
        {
            var queryDictionary = new Dictionary<string, string>
            {
                {"name", "fred"},
                {"email", "fred@fred.com"},
                {"customer", "true"}
            };
            string expected = "?name=fred&email=fred@fred.com&customer=true";

            var qDictionary = queryDictionary.ToQueryString().EnsureStart("?");

            Assert.AreEqual(expected, qDictionary);
        }


        [Test]
        public void Test_String_Object_Dictionary_Get_Extension()
        {
            var fiftyYearsAgo = DateTime.Now.AddYears(-50);

            var dictionary = new Dictionary<string, object>
            {
                {"team", "Braves"},
                {"age", 50},
                {"birthdate", fiftyYearsAgo},
                {"isAdult", true}
            };

            Assert.AreEqual("Braves", dictionary.Get("team"));
            Assert.AreEqual(50, dictionary.Get<int>("age"));
            Assert.AreEqual(fiftyYearsAgo, dictionary.Get<DateTime>("birthdate"));
            Assert.AreEqual(true, dictionary.Get<bool>("isAdult"));

            Assert.AreEqual(null, dictionary.Get("missing"));
        }

        [Test]
        public void Test_String_String_Dictionary_Get_Extension()
        {
            // Without the ToString() format, it fails with difference for milliseconds in the string

            var fiftyYearsAgo = DateTime.Now.AddYears(-50);
            string fiftyYearsStr = fiftyYearsAgo.ToString("o");
            string warning = "test warning";

            var dictionary = new Dictionary<string, string>
            {
                {"warning", warning},
                {"age", "50"},
                {"isAdult", "true"},
                {"birthdate", fiftyYearsStr}
            };

            Assert.AreEqual(warning, dictionary.Get("warning"));
            Assert.AreEqual(50, dictionary.Get<int>("age"));
            Assert.AreEqual(true, dictionary.Get<bool>("isAdult"));
            Assert.AreEqual(fiftyYearsAgo, dictionary.Get<DateTime>("birthdate"));
        }

        [Test]
        public void Test_String_String_Dictionary_Get_With_Missing_Values()
        {
            var fiftyYearsAgo = DateTime.Now.AddYears(-50);
            string fiftyYearsStr = fiftyYearsAgo.ToString();
            string warning = "test warning";

            var dictionary = new Dictionary<string, string>
            {
                {"warning", warning},
                {"age", "50"},
                {"isAdult", "true"},
                {"birthdate", fiftyYearsStr}
            };

            Assert.AreEqual(null, dictionary.Get("missing"));
            Assert.AreEqual("no value", dictionary.Get("missing", "no value"));

            Assert.AreEqual(0, dictionary.Get<int>("missing"));
            Assert.AreEqual(5, dictionary.Get<int>("missing", 5));

            Assert.AreEqual(false, dictionary.Get<bool>("missing"));
            Assert.AreEqual(true, dictionary.Get<bool>("missing", true));

            Assert.AreEqual(null, dictionary.Get<bool?>("missing"));
            Assert.AreEqual(true, dictionary.Get<bool?>("missing", true));

            dictionary.Set("Group", "Test");
            Assert.AreEqual("Test", dictionary.Get("Group"));

            dictionary.Set("IsTrue", true);
            Assert.AreEqual(true, dictionary.Get<bool>("IsTrue"));

            dictionary.Set("Numeric", 1234);
            Assert.AreEqual(1234, dictionary.Get<int>("Numeric"));
        }
    }
}
