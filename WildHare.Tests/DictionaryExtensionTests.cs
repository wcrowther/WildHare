using NUnit.Framework;
using NUnit.Framework.Legacy;
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
			Dictionary<int, string> dictionary = new(){{1, "platypus"}, {2, "cheetah"}, {3, "cat"}};

			// Future version (c# 13+) will support this??
			// Dictionary<int, string> dictionary = [1: "platypus", 2: "cheetah", 3: "cat"];


			// GetValueOrDefault is a method on Dictionary - no extension method needed for this

			ClassicAssert.AreEqual("cheetah", dictionary[2]);
            ClassicAssert.AreEqual("Not Found", dictionary.GetValueOrDefault(5, "Not Found"));
        }

        [Test]
        public void Test_Dictionary_Safe_If_Key_Does_Not_Exist_With_string_string()
        {
            var dictionary = new Dictionary<string, string> { {"Participle", "past"},{"IsPlural", "true"},{"Numeric", "123"} };

            // GetValueOrDefault is a method on Dictionary - no extension method needed for this

            ClassicAssert.AreEqual("past", dictionary.GetValueOrDefault("Participle", "present"));
            ClassicAssert.AreEqual("present", dictionary.GetValueOrDefault("Participle_X", "present"));

            ClassicAssert.AreEqual("true", dictionary.GetValueOrDefault("IsPlural", "true"));
            ClassicAssert.AreEqual("false", dictionary.GetValueOrDefault("IsPlural_X", "false"));

            ClassicAssert.AreEqual("123", dictionary.GetValueOrDefault("Numeric", "0"));
            ClassicAssert.AreEqual("0", dictionary.GetValueOrDefault("Numeric_X", "0"));
        }



        [Test]
        public void Test_ToQueryDictionary()
        {
            string queryString = "?name=fred&email=fred@fred.com&customer=true";
            var qDictionary = queryString.RemoveStart("?").ToQueryDictionary();

            ClassicAssert.AreEqual(3, qDictionary.Count);
            ClassicAssert.AreEqual("fred", qDictionary["name"]);
            ClassicAssert.AreEqual("fred@fred.com", qDictionary["email"]);
            ClassicAssert.AreEqual("true", qDictionary["customer"]);
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

            ClassicAssert.AreEqual(expected, qDictionary);
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
                {"isAdult", true},
                {"CurrencyConversion", 1.5m },
                {"CurrencyConversion2", "1.5" }
            };

            ClassicAssert.AreEqual("Braves", dictionary.Get("team"));
            ClassicAssert.AreEqual(50, dictionary.Get<int>("age"));
            ClassicAssert.AreEqual(fiftyYearsAgo, dictionary.Get<DateTime>("birthdate"));
            ClassicAssert.AreEqual(true, dictionary.Get<bool>("isAdult"));

            ClassicAssert.AreEqual(1.5, dictionary.Get<decimal>("CurrencyConversion"));
            ClassicAssert.AreEqual(1.5m, dictionary.TryGet<decimal>("CurrencyConversion", out decimal val) ? val : 3);
            ClassicAssert.AreEqual(22.2, dictionary.TryGet<decimal>("missing", out decimal dec2) ? dec2 : 22.2m);

            ClassicAssert.AreEqual(null, dictionary.Get("missing"));
            ClassicAssert.AreEqual(null, dictionary.Get<string>("missing"));

            ClassicAssert.AreEqual(true, dictionary.Get("team") is string);
            ClassicAssert.AreEqual(false, dictionary.Get("missing") is string);

            ClassicAssert.AreEqual(1.5, dictionary.Get<decimal>("CurrencyConversion2"));

        }

        [Test]
        public void Test_String_Object_Dictionary_TryGet_Extension()
        {
            var dictionary = new Dictionary<string, object>
            {
                {"team", "Braves"},
                {"age", 50}
            };

            ClassicAssert.AreEqual(50, dictionary.TryGet("age", out int age) ? age : 0);
            ClassicAssert.AreEqual(0, dictionary.TryGet("not_age", out int age2) ? age2 : 0);
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

            ClassicAssert.AreEqual(warning, dictionary.Get("warning"));
            ClassicAssert.AreEqual(50, dictionary.Get<int>("age"));
            ClassicAssert.AreEqual(true, dictionary.Get<bool>("isAdult"));
            ClassicAssert.AreEqual(fiftyYearsAgo, dictionary.Get<DateTime>("birthdate"));
        }

        [Test]
        public void Test_String_String_Dictionary_Get_With_Missing_Values()
        {
            DateTime fiftyYearsAgo = DateTime.Now.AddYears(-50);
            string fiftyYearsStr = fiftyYearsAgo.ToString();
            string warning = "test warning";

            var dictionary = new Dictionary<string, string>
            {
                {"warning", warning},
                {"age", "50"},
                {"isAdult", "true"},
                {"birthdate", fiftyYearsStr}
            };

            ClassicAssert.AreEqual(null, dictionary.Get("missing"));
            ClassicAssert.AreEqual("no value", dictionary.Get("missing", "no value"));

            ClassicAssert.AreEqual(0, dictionary.Get<int>("missing"));
            ClassicAssert.AreEqual(5, dictionary.Get<int>("missing", 5));

            ClassicAssert.AreEqual(false, dictionary.Get<bool>("missing"));
            ClassicAssert.AreEqual(true, dictionary.Get<bool>("missing", true));

            ClassicAssert.AreEqual(null, dictionary.Get<bool?>("missing"));
            ClassicAssert.AreEqual(true, dictionary.Get<bool?>("missing", true));

            dictionary.Set("Group", "Test");
            ClassicAssert.AreEqual("Test", dictionary.Get("Group"));

            dictionary.Set("IsTrue", true);
            ClassicAssert.AreEqual(true, dictionary.Get<bool>("IsTrue"));

            dictionary.Set("Numeric", 1234);
            ClassicAssert.AreEqual(1234, dictionary.Get<int>("Numeric"));

            // Uses DateTime Set overload
            dictionary.Set("Date", fiftyYearsAgo);
            ClassicAssert.AreEqual(fiftyYearsAgo, fiftyYearsAgo);
        }
    }
}
