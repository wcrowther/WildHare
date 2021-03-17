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

        //[Test]
        //public void Test_Dictionary_Get_Extension()
        //{
        //    var fiftyYearsAgo = DateTime.Now.AddYears(-50);

        //    var dictionary = new Dictionary<string, object>
        //    {
        //        {"age", 50},
        //        {"birthdate", fiftyYearsAgo},
        //        {"isAdult", true}
        //    };

        //    int age = dictionary.Get<int>("age");
        //    DateTime birthDate = dictionary.Get<DateTime>("birthdate");
        //    bool isAdult = dictionary.Get<bool>("isAdult");

        //    Assert.AreEqual(50, age);
        //    Assert.AreEqual(fiftyYearsAgo, birthDate);
        //    Assert.AreEqual(true, isAdult);

        //    Assert.AreEqual(50, dictionary.Get("age"));
        //    Assert.AreEqual(fiftyYearsAgo, dictionary.Get("birthdate"));
        //    Assert.AreEqual(true, dictionary.Get("isAdult"));

        //    //Assert.AreEqual(false, dictionary.Get("missingValue"));


        //    // int age = 20;
        //    // dictionary.Set("age", age);
        //    // age = dictionary.Get<int>("age");

        //    // or the safe way
        //    //if (dictionary.TryGet("age", out age))
        //    //{
        //    //    Console.WriteLine("The age is {0}", age);
        //    //}
        //    //else
        //    //{
        //    //    Console.WriteLine("Age not found or of wrong type");
        //    //}

        //}
    }
}
