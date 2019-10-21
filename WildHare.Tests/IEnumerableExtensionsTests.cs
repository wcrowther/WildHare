using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WildHare.Extensions;
using WildHare.Tests.Models;

namespace WildHare.Tests
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        [SetUp]
        public void Setup()
        { }

        [Test]
        public void Test_ElementIn_IList_Basic()
        {
            var numbers = new List<string> { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

            Assert.AreEqual("zero",  numbers.ElementIn(0));
            Assert.AreEqual("two",  numbers.ElementIn(2));
            Assert.AreEqual("zero", numbers.ElementIn(10));
            Assert.AreEqual("five", numbers.ElementIn(15));
            Assert.AreEqual("six",  numbers.ElementIn(106));
            Assert.AreEqual("six",  numbers.ElementIn(1606));
        }

        [Test]
        public void Test_ElementIn_IList_Basic2()
        {
            var numbers = new List<string> { "zero", "one", "two" };

            Assert.AreEqual("one", numbers.ElementIn(1));
            Assert.AreEqual("one", numbers.ElementIn(4));
            Assert.AreEqual("zero", numbers.ElementIn(15));
        }

        [Test]
        public void Test_ElementIn_IList_Single_Element()
        {
            var numbers = new List<string> { "zero" };

            Assert.AreEqual("zero", numbers.ElementIn(11));
            Assert.AreEqual("zero", numbers.ElementIn(4456));
            Assert.AreEqual("zero", numbers.ElementIn(155577555));
        }

        [Test]
        public void Test_ElementIn_IList_Empty_List()
        {
            void ExceptionIfEmptyList()
            {
                var numbers = new List<string>();
                var x = numbers.ElementIn(10);
            }

            Assert.Throws<Exception>(() => ExceptionIfEmptyList());
        }

        [Test]
        public void Test_ElementIn_IEnumerable_Basic()
        {
            var numbers = new string[]{ "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            Assert.AreEqual("zero", numbers.ElementIn(0));
            Assert.AreEqual("two", numbers.ElementIn(2));
            Assert.AreEqual("zero", numbers.ElementIn(10));
            Assert.AreEqual("five", numbers.ElementIn(15));
            Assert.AreEqual("six", numbers.ElementIn(106));
            Assert.AreEqual("six", numbers.ElementIn(1606));
        }

        [Test]
        public void Test_ElementIn_IEnumerable_Basic2()
        {
            var numbers = new string[]{ "zero", "one", "two" };

            Assert.AreEqual("one", numbers.ElementIn(1));
            Assert.AreEqual("one", numbers.ElementIn(4));
            Assert.AreEqual("zero", numbers.ElementIn(15));
        }

        [Test]
        public void Test_ElementIn_IEnumerable_Single_Element()
        {
            var numbers = new string[]{ "zero" };

            Assert.AreEqual("zero", numbers.ElementIn(11));
            Assert.AreEqual("zero", numbers.ElementIn(4456));
            Assert.AreEqual("zero", numbers.ElementIn(155577555));
        }

        [Test]
        public void Test_ElementIn_IEnumerable_Empty_List()
        {
            void ExceptionIfEmptyList()
            {
                var numbers = new string[] { };
                var x = numbers.ElementIn(10);
            }

            Assert.Throws<Exception>(() => ExceptionIfEmptyList());
        }

        [Test]
        public void Test_ElementInOrDefault_IEnumerable_Basic()
        {
            var numbers = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            Assert.AreEqual("zero", numbers.ElementInOrDefault(0));
            Assert.AreEqual("two",  numbers.ElementInOrDefault(2));
            Assert.AreEqual("zero", numbers.ElementInOrDefault(10));
            Assert.AreEqual("five", numbers.ElementInOrDefault(15));
            Assert.AreEqual("six",  numbers.ElementInOrDefault(106));
            Assert.AreEqual("six",  numbers.ElementInOrDefault(1606));
        }

        [Test]
        public void Test_ElementInOrDefault_IEnumerable_Empty_List()
        {
            string result = null;
            void ExceptionIfEmptyList()
            {
                var numbers = new string[] { };
                result = numbers.ElementInOrDefault(10);
            }

            Assert.DoesNotThrow(() => ExceptionIfEmptyList());
            Assert.IsNull(result);
        }

        [Test]
        public void Test_ElementInOrDefault_IList_Empty_List()
        {
            string result = null;
            void ExceptionIfEmptyList()
            {
                var numbers = new List<string>();
                result = numbers.ElementInOrDefault(10);
            }

            Assert.DoesNotThrow(() => ExceptionIfEmptyList());
            Assert.IsNull(result);
        }

        [Test]
        public void Test_MatchList_Basic()
        {
            string[] phraseArray = {"the", "president", "of", "the", "united", "states", "is", "a", "politician" };
            var splitSentence    = "the president of the united states".Split(' ');

            var matches = phraseArray.MatchList(splitSentence, (a, b) => a == b).ToList();
            
            Assert.AreEqual(6, matches.Count);
            Assert.AreEqual("the president of the united states", string.Join(' ', matches));
        }



    }
}
