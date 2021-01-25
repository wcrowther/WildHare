using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public void Test_ElementIn_IEnumerable_Single_Element()
        {
            var numbers = new string[] { "zero" };

            Assert.AreEqual("zero", numbers.ElementIn(11));
            Assert.AreEqual("zero", numbers.ElementIn(4456));
            Assert.AreEqual("zero", numbers.ElementIn(155577555));
        }

        [Test]
        public void Test_ElementInOrDefault_IEnumerable_Basic()
        {
            var numbers = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            Assert.AreEqual("zero", numbers.ElementInOrDefault(0));
            Assert.AreEqual("two", numbers.ElementInOrDefault(2));
            Assert.AreEqual("zero", numbers.ElementInOrDefault(10));
            Assert.AreEqual("five", numbers.ElementInOrDefault(15));
            Assert.AreEqual("six", numbers.ElementInOrDefault(106));
            Assert.AreEqual("six", numbers.ElementInOrDefault(1606));
            Assert.AreEqual("two", numbers.ElementInOrDefault(-2));
            Assert.AreEqual("five", numbers.ElementInOrDefault(-15));
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
        public void Test_ElementInOrDefault_IList_Ints()
        {
            int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Assert.AreEqual(0, numbers.ElementInOrDefault(0));
            Assert.AreEqual(2, numbers.ElementInOrDefault(2));
            Assert.AreEqual(0, numbers.ElementInOrDefault(10));
            Assert.AreEqual(5, numbers.ElementInOrDefault(15));
            Assert.AreEqual(6, numbers.ElementInOrDefault(106));
            Assert.AreEqual(6, numbers.ElementInOrDefault(1606));
            Assert.AreEqual(0, numbers.ElementInOrDefault(DateTime.Parse("1/1/2020").Year));
        }

        [Test]
        public void Test_ElementInOrDefault_IList_Date_To_Int()
        {
            int[] numbers = { 1, 2, 3, 4 };
            var dateYear2018 = DateTime.Parse("1/1/2018").Year;
            var dateYear2019 = DateTime.Parse("1/1/2019").Year;
            var dateYear2020 = DateTime.Parse("1/1/2020").Year;
            var dateYear2021 = DateTime.Parse("1/1/2021").Year;

            Assert.AreEqual(3, numbers.ElementInOrDefault(dateYear2018));
            Assert.AreEqual(4, numbers.ElementInOrDefault(dateYear2019));
            Assert.AreEqual(1, numbers.ElementInOrDefault(dateYear2020));
            Assert.AreEqual(2, numbers.ElementInOrDefault(dateYear2021));
        }

        [Test]
        public void Test_MatchList_Basic()
        {
            string[] phraseArray = { "the", "president", "of", "the", "united", "states", "is", "a", "politician" };
            var splitSentence = "the president of the united states".Split(' ');

            var matches = phraseArray.MatchList(splitSentence, (a, b) => a == b).ToList();

            Assert.AreEqual(6, matches.Count);
            Assert.AreEqual("the president of the united states", string.Join(' ', matches));
        }

        [Test]
        public void Test_MatchList_Advanced()
        {
            var wordList_A = new List<Word>
            {
                new Word{ WordId = 1, WordName="the" },
                new Word{ WordId = 2, WordName="president" },
                new Word{ WordId = 3, WordName="of" },
                new Word{ WordId = 1, WordName="the" },
                new Word{ WordId = 4, WordName="united" },
                new Word{ WordId = 5, WordName="states" },
                new Word{ WordId = 6, WordName="is" },
                new Word{ WordId = 7, WordName="a" },
                new Word{ WordId = 8, WordName="politician" },

            };
            var wordList_B = new List<Word>
            {
                new Word{ WordId = 1, WordName="the" },
                new Word{ WordId = 2, WordName="president" },
                new Word{ WordId = 3, WordName="of" },
                new Word{ WordId = 1, WordName="the" },
                new Word{ WordId = 4, WordName="united" },
                new Word{ WordId = 5, WordName="states" }

            };
            var wordList_C = new List<Word>
            {
                new Word{ WordId = 4, WordName="united" },
                new Word{ WordId = 5, WordName="states" }

            };

            var matches = wordList_A.MatchList(wordList_B, (a, b) => a.WordName == b.WordName).ToArray();

            Assert.AreEqual(6, matches.Count());
            Assert.AreEqual("the president of the united states", string.Join(' ', matches.Select(s => s.WordName)));

            var matches2 = wordList_A.MatchList(wordList_C, (a, b) => a.WordName == b.WordName).ToArray();

            Assert.AreEqual(0, matches2.Count());

            var matches3 = wordList_A.MatchList(wordList_C, (a, b) => a.WordName == b.WordName).ToArray();

            Assert.AreEqual(0, matches2.Count());
        }

        [Test]
        public void Test_InList_Basic()
        {
            string[] phraseArray = { "the", "president", "of", "the", "united", "states", "is", "a", "politician" };
            var splitSentence = "the united states".Split(' ');

            int[] indexes = phraseArray.InList(splitSentence, (a, b) => a == b);

            Assert.AreEqual(3, indexes.First());
        }

        [Test]
        public void Test_InList_Advanced()
        {
            string[] phraseArray = { "the", "president", "of", "the", "United", "States", "lives", "in", "the", "united", "states" };
            var splitSentence = "the united states".Split(' ');

            int[] indexes = phraseArray.InList(splitSentence, (a, b) => a.ToLower() == b.ToLower());

            Assert.AreEqual(3, indexes.ElementAt(0));
            Assert.AreEqual(8, indexes.ElementAt(1));
        }

        [Test]
        public void Test_InList_Advanced_ListTest_InList_Advanced_List()
        {
            string[] phraseArray = { "the", "president", "of", "the", "United", "States", "lives", "in", "the", "united", "states" };

            var wordList = new List<string[]>
            {
                new string[]{ "the", "president", "of", "the", "United", "States" },
                new string[]{ "the", "president"},
                new string[]{ "the", "united", "states" },
                new string[]{ "the"},
                new string[]{ "not", "found" },
                new string[]{ "other", "strings" },

            };

            var lookupList = new Collection<(int Index, string[] List)>();
            foreach (var list in wordList)
            {
                int[] indexes = phraseArray.InList(list, (a, b) => a.ToLower() == b.ToLower());
                foreach (var index in indexes)
                {
                    if (index > -1)
                        lookupList.Add((index, list));
                }
            }

            Assert.AreEqual(7, lookupList.Count);

            Assert.AreEqual(0, lookupList[0].Index);

            Assert.AreEqual(0, lookupList[1].Index);
            Assert.AreEqual("the president", string.Join(" ", lookupList[1].List));

            Assert.AreEqual(3, lookupList[2].Index);
            Assert.AreEqual("the united states", string.Join(" ", lookupList[2].List));

            Assert.AreEqual(8, lookupList[3].Index);
            Assert.AreEqual("the united states", string.Join(" ", lookupList[3].List));

            Assert.AreEqual(0, lookupList[4].Index);
            Assert.AreEqual(3, lookupList[5].Index);
            Assert.AreEqual(8, lookupList[6].Index);

        }

        [Test]
        public void Test_InList_Advanced_ListTest_InList_Advanced_List2()
        {
            Word[] wordArray = {
                new Word{WordId=1, WordName="the" },
                new Word{WordId=2, WordName="president" },
                new Word{WordId=3, WordName="of" },
                new Word{WordId=1, WordName="the" },
                new Word{WordId=4, WordName="United" },
                new Word{WordId=5, WordName="States" },
                new Word{WordId=6, WordName="lives" },
                new Word{WordId=7, WordName="in" },
                new Word{WordId=1, WordName="the" },
                new Word{WordId=4, WordName="united" },
                new Word{WordId=5, WordName="states" }
            };

            var wordList = new List<List<Word>>
            {
                new List<Word>(){
                    new Word{ WordId=1, WordName="the"   }, new Word{ WordId=2, WordName="president" },
                    new Word{ WordId=3, WordName="of"    }, new Word{ WordId=1, WordName="the" },
                    new Word{ WordId=4, WordName="united"}, new Word{ WordId=5, WordName="states"}
                },

                new List<Word>{ new Word{ WordId=1, WordName="the" },   new Word{ WordId=2, WordName="president"} },

                new List<Word>{ new Word{ WordId=1, WordName="the" },   new Word{ WordId=4, WordName="united"}, new Word{ WordId=5, WordName="states"} },

                new List<Word>{ new Word{ WordId=1, WordName="the" } },

                new List<Word>{ new Word{ WordId=8, WordName="not" }, new Word{ WordId=9, WordName="found" } },

                new List<Word>{ new Word{ WordId=10, WordName="other" }, new Word{ WordId=11, WordName= "strings" }  },
            };

            var lookupList = new Collection<(int Index, List<Word> List)>();

            foreach (List<Word> list in wordList)
            {
                int[] indexes = wordArray.InList(list, (a, b) => a.WordId == b.WordId);
                foreach (var index in indexes)
                {
                    if (index > -1)
                        lookupList.Add((index, list));
                }
            }

            Assert.AreEqual(7, lookupList.Count);

            Assert.AreEqual(0, lookupList[0].Index);

            Assert.AreEqual(0, lookupList[1].Index);
            Assert.AreEqual("the president", string.Join(" ", lookupList[1].List.Select(s => s.WordName)));

            Assert.AreEqual(3, lookupList[2].Index);
            Assert.AreEqual("the united states", string.Join(" ", lookupList[2].List.Select(s => s.WordName)));

            Assert.AreEqual(8, lookupList[3].Index);
            Assert.AreEqual("the united states", string.Join(" ", lookupList[3].List.Select(s => s.WordName)));

            Assert.AreEqual(0, lookupList[4].Index);
            Assert.AreEqual(3, lookupList[5].Index);
            Assert.AreEqual(8, lookupList[6].Index);

        }

    }
}
