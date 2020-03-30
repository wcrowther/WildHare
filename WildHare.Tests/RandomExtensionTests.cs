using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Extensions.Xtra;

namespace WildHare.Tests
{
    [TestFixture]
    public class RandomExtensionTests
    {
        [Test]
        public void Test_Random_Skip_For_Determinate_Random_Tree()
        {
            var random = new Random(1234);
            var solution = new List<int>();

            for (int i = 0; i < 5; i++)
            {
                solution.Add(random.Next(1, 101));
            }

            Assert.AreEqual(solution, new List<int>{ 40, 90, 32, 95, 34 });

            var random2 = new Random(1234);
            int number = random2.Skip(4).Next(1, 101);

            Assert.AreEqual(solution[4], number);
        }
    }
}
