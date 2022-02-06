using NUnit.Framework;
using SeedPacket.DataSources;
using SeedPacket.Extensions;
using SeedPacket.Functions;
using SeedPacket.Generators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WildHare.Extensions;
using WildHare.Extensions.List;
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
        public void Test_ElementInOrDefault_IList_Ints_DefaultItem()
        {
            int[] numbers = { };

            Assert.AreEqual(25, numbers.ElementInOrDefault(0, 25));
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
                new Word{ WordId = 1, Text="the" },
                new Word{ WordId = 2, Text="president" },
                new Word{ WordId = 3, Text="of" },
                new Word{ WordId = 1, Text="the" },
                new Word{ WordId = 4, Text="united" },
                new Word{ WordId = 5, Text="states" },
                new Word{ WordId = 6, Text="is" },
                new Word{ WordId = 7, Text="a" },
                new Word{ WordId = 8, Text="politician" },

            };
            var wordList_B = new List<Word>
            {
                new Word{ WordId = 1, Text="the" },
                new Word{ WordId = 2, Text="president" },
                new Word{ WordId = 3, Text="of" },
                new Word{ WordId = 1, Text="the" },
                new Word{ WordId = 4, Text="united" },
                new Word{ WordId = 5, Text="states" }

            };
            var wordList_C = new List<Word>
            {
                new Word{ WordId = 4, Text="united" },
                new Word{ WordId = 5, Text="states" }

            };

            var matches = wordList_A.MatchList(wordList_B, (a, b) => a.Text == b.Text).ToArray();

            Assert.AreEqual(6, matches.Count());
            Assert.AreEqual("the president of the united states", string.Join(' ', matches.Select(s => s.Text)));

            var matches2 = wordList_A.MatchList(wordList_C, (a, c) => a.Text == c.Text).ToArray();

            Assert.AreEqual(0, matches2.Count());

            var matches3 = wordList_B.MatchList(wordList_C, (b, c) => b.Text == c.Text).ToArray();

            Assert.AreEqual(0, matches3.Count());
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
        public void Test_InList_Not_Found()
        {
            string[] phraseArray = { "the", "president", "of", "the", "united", "states", "is", "a", "politician" };
            var splitSentence = "nothing to find".Split(' ');

            int[] indexes = phraseArray.InList(splitSentence, (a, b) => a == b);

            Assert.AreEqual(-1, indexes.First());
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
            // the president of the united states lives in the United States (of America)

            Word[] wordArray = {
                new Word{ WordId=1, Text="the" },
                new Word{ WordId=2, Text="president" },
                new Word{ WordId=3, Text="of" },
                new Word{ WordId=1, Text="the" },
                new Word{ WordId=4, Text="United" },
                new Word{ WordId=5, Text="States" },
                new Word{ WordId=6, Text="lives" },
                new Word{ WordId=7, Text="in" },
                new Word{ WordId=1, Text="the" },
                new Word{ WordId=4, Text="united" },
                new Word{ WordId=5, Text="states" }
            };

            var wordList = new List<List<Word>>
            {
                new List<Word>
                {
                    new Word{ WordId=1, Text="the"   }, new Word{ WordId=2, Text="president" },
                    new Word{ WordId=3, Text="of"    }, new Word{ WordId=1, Text="the" },
                    new Word{ WordId=4, Text="united"}, new Word{ WordId=5, Text="states"}
                },
                new List<Word>{ new Word{ WordId=1, Text="the" },  new Word{ WordId=2, Text="president"} },
                new List<Word>{ new Word{ WordId=1, Text="the" },  new Word{ WordId=4, Text="united"}, new Word{ WordId=5, Text="states"} },
                new List<Word>{ new Word{ WordId=1, Text="the" } },
                new List<Word>{ new Word{ WordId=8, Text="not" }, new Word{ WordId=9, Text="found" } },
                new List<Word>{ new Word{ WordId=10, Text="other" }, new Word{ WordId=11, Text= "strings" }  },
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
            Assert.AreEqual("the president", string.Join(" ", lookupList[1].List.Select(s => s.Text)));

            Assert.AreEqual(3, lookupList[2].Index);
            Assert.AreEqual("the united states", string.Join(" ", lookupList[2].List.Select(s => s.Text)));

            Assert.AreEqual(8, lookupList[3].Index);
            Assert.AreEqual("the united states", string.Join(" ", lookupList[3].List.Select(s => s.Text)));

            Assert.AreEqual(0, lookupList[4].Index);
            Assert.AreEqual(3, lookupList[5].Index);
            Assert.AreEqual(8, lookupList[6].Index);
        }

        [Test]
        public void Test_Dictionary_of_Funcs_Basic()
        {
            var funcs = new Dictionary<string, Func<int, int, int>>
            {
                { "add", Add },
                { "subtract", Subtract },
                { "multiply", Multiply },
                { "divide", null }
            };

            funcs["divide"] = Divide;

            Assert.AreEqual(10, funcs["add"].DynamicInvoke(5, 5));
            Assert.AreEqual(0, funcs["subtract"].DynamicInvoke(5, 5));
            Assert.AreEqual(25, funcs["multiply"].DynamicInvoke(5, 5));
            Assert.AreEqual(1, funcs["divide"].DynamicInvoke(5, 5));
        }

        [Test]
        public void Test_MatchList_Int_SpeedTest()
        {
            List<int[]> randomPatterns = GetRandomIntList(10000, 21, 1234);

            var stopwatch = Stopwatch.StartNew();

            int[] pattern = { 3, 4, 5 };
            var matches = new List<IEnumerable<int>>();

            foreach (var randomPattern in randomPatterns)
            {
                IEnumerable<int> patternMatch = randomPattern.MatchList(pattern, (a, b) => a == b, false);

                // 8ms for 10,000 records  41ms for 100,000 records
                if (patternMatch.Count() >= 3)
                {
                    // Debug.WriteLine($"Pattern: {string.Join(", ", patternMatch)} RandomPattern: {string.Join(", ", randomPattern)}");
                    matches.Add(randomPattern);
                }

                // EXACT VERSION -- QUICKER 1ms for 10,000 records - 16ms for 100,000 records
                // Only returns boolean
                // if (randomPattern.SequenceEqual(pattern))
                // {
                //    matches.Add(randomPattern);
                // }
            }

            stopwatch.Stop();

            Debug.WriteLine("Elapsed time: " + stopwatch.ElapsedMilliseconds + " ms");

            Assert.AreEqual(13, matches.Count);

            Assert.IsTrue(matches[0].SequenceEqual(new[] { 3, 4, 0, 3, 5 }));
            Assert.IsTrue(matches[1].SequenceEqual(new[] { 3, 4, 5, 18, 4 }));
            Assert.IsTrue(matches[2].SequenceEqual(new[] { 3, 4, 20, 6, 5 }));
            Assert.IsTrue(matches[3].SequenceEqual(new[] { 3, 10, 10, 4, 5 }));
            Assert.IsTrue(matches[4].SequenceEqual(new[] { 14, 3, 3, 4, 5 }));
            Assert.IsTrue(matches[5].SequenceEqual(new[] { 18, 3, 4, 5, 15 }));

        }

        [Test]
        public void Test_MatchList_String_SpeedTest()
        {
            var stopwatch = Stopwatch.StartNew();

            // Get 100,000 sequences of 5 random names in .38 to .64 seconds.
            IEnumerable<string[]> randomNames = GetRandomNameList(10000, 34567);

            stopwatch.Stop();
            Debug.WriteLine("GetRandomNameList Elapsed time: " + stopwatch.ElapsedMilliseconds + " ms");

            stopwatch.Restart();

            string[] pattern = { "Janice", "Jesse", "John" };
            var matches = new List<IEnumerable<string>>();

            foreach (var randomPattern in randomNames)
            {
                IEnumerable<string> match = randomPattern.MatchList(pattern, (a, b) => a == b, false);

                // 13ms,16ms,23ms,24ms for 10,000 records | 188ms for 100,000 records (Debug)
                if (match.Count() >= 3)
                {
                    matches.Add(randomPattern);
                }
            }

            stopwatch.Stop();
            Debug.WriteLine("MatchList Elapsed time: " + stopwatch.ElapsedMilliseconds + " ms");

            Assert.AreEqual(5, matches.Count);

            Assert.IsTrue(matches[0].SequenceEqual( new[] { "Janice", "Jesse", "John", "Denise", "Robert" }));
            Assert.IsTrue(matches[2].SequenceEqual( new[] { "Janice", "Christian", "Jesse", "Peter", "John"    }));
            Assert.IsTrue(matches[4].SequenceEqual( new[] { "Janice", "Juan", "Zoey", "Jesse", "John" }));

        }

        [Test]
        public void Test_PatternMatch_Basic()
        {
            string[] phraseList = { "is", "the", "president", "of", "the", "united", "states", "a", "politician" };
            var pattern = "president of the united states".Split(' ');

            var matches = phraseList.PatternMatch(pattern, (a, b) => a == b).ToList();

            Assert.AreEqual(5, matches.Count);
            Assert.AreEqual("president of the united states", string.Join(' ', matches));
        }

        [Test]
        public void Test_PatternMatch_Basic2()
        {
            string[] phraseList = { "is", "the", "president", "of", "the", "united", "states", "a", "politician" };
            var pattern = "a politician".Split(' ');

            var matches = phraseList.PatternMatch(pattern, (a, b) => a == b).ToList();

            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual("a politician", string.Join(' ', matches));
        }

        [Test]
        public void Test_PatternMatch_Basic3()
        {
            string[] phraseList = { "the", "president", "of", "the", "united", "states" };
            var pattern = "prime minister".Split(' ');

            var matches = phraseList.PatternMatch(pattern, (a, b) => a == b).ToList();

            Assert.AreEqual(0, matches.Count);
            Assert.AreEqual("", string.Join(' ', matches));
        }

        [Test]
        public void Test_PatternMatch_List_Is_Empty()
        {
            string[] phraseList = { };
            var pattern = "the president".Split(' ');

            var matches = phraseList.PatternMatch(pattern, (a, b) => a == b).ToList();

            Assert.AreEqual(0, matches.Count);
            Assert.AreEqual("", string.Join(' ', matches));
        }

        [TestCase("1 clown in the list.",   new[]{"Bozo"}, "clown")]
        [TestCase("3 clowns in the list.",  new[]{"Bozo","Bonzo","Clapo"}, "clown", null)]
        [TestCase("1 fox in the list.",     new[]{"Wily"}, "fox")]
        [TestCase("3 foxes in the list.",   new[]{"Wily","Willy","Wooly" }, "fox", "")]
        [TestCase("1 octopus in the list.", new[]{"Squishy"}, "octopus", "octopi")]
        [TestCase("3 octopi in the list.",  new[]{"Squishy","Squashy","Slimy" }, "octopus", "octopi")]
        public void Test_Pluralize_With_IEnumerable_Basic(string result, IEnumerable<string> list, string singular, string plural = null)
        {
            string message = $"{list.Count()} {list.Pluralize(singular, plural)} in the list.";

            Assert.AreEqual(result, message);
        }

        // ================================================================================================
        // PRIVATE FUNCTIONS
        // ================================================================================================

        private int Add(int arg1, int arg2) => arg1 + arg2;

        private int Subtract(int arg1, int arg2) => arg1 - arg2;

        private int Multiply(int arg1, int arg2) => arg1 * arg2;

        private int Divide(int arg1, int arg2) => arg1 / arg2;

        private List<int[]> GetRandomIntList(int count, int maxInt, int seed)
        {
            var list = new List<int[]>();
            var random = new Random(seed);

            for (int i = 0; i < count; i++)
            {
                int a = random.Next(maxInt);
                int b = random.Next(maxInt);
                int c = random.Next(maxInt);
                int d = random.Next(maxInt);
                int e = random.Next(maxInt);

                int[] ints = new int[5];
                ints[0] = a;
                ints[1] = b;
                ints[2] = c;
                ints[3] = d;
                ints[4] = e;

                list.Add(ints);
            }

            return list;
        }

        private IEnumerable<string[]> GetRandomNameList(int count, int seed)
        {
            var randomNameList = new List<string[]>();

            var generator               = new MultiGenerator();
            generator.Cache.NameList    = new List<string>().Seed(1, 100, generator, "FirstName");

            for (int i = 0; i < count; i++)
            {
                var arrayOfNames = Funcs.GetListFromCacheRandom<string>(generator, "NameList", 5, 5, false).ToArray();
                randomNameList.Add(arrayOfNames);
            }

            return randomNameList;
        }
    }
}
