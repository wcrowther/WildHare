using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WildHare.Extensions;

namespace WildHare.Tests
{
	[TestFixture]
	public class LeetCodingTests
	{
		[TestCase("the sky is blue", "blue is sky the")]
		[TestCase("  hello world  ", "world hello")]
		public void String_ReverseWords(string s, string output)
		{
			var array = s.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

			Array.Reverse(array);

			string str = string.Join(" ", array);

			Debug.WriteLine($"Original: {s} Reversed: {str}");

			Assert.AreEqual(output, str);
		}

		[TestCase("abciiidef", 3, 3)]
		//[TestCase("aeiou", 2, 2)]
		//[TestCase("leetcode", 3, 2)]
		public void Maximum_Number_Of_Vowels(string s, int k, int output)
		{
			var span = s.AsSpan();
			int max = 0;

			for (int i = 0; i <  span.Length; i++)
			{
				int vowelCount = 0;
				for (int x = 0; x <  k; x++)
				{
					if (i + x > span.Length)
						continue;

					char ch = span[x+i];
					if ("aeiou".IndexOf(ch) >= 0)
						vowelCount++;
					x++;
				}

				Console.WriteLine($"{span[i]} Vowel Count: {vowelCount} Max: {max}");

				if (vowelCount > max)
					max = vowelCount;
				i++;
			}

			Assert.AreEqual(output, max);
		}

		[Test]
		public void countPossibleWinners()
		{
			int[] initialRewards = { 3, 8, 10, 9};
			int output = 2;


			int initialMax = initialRewards.Max();
			int points = initialRewards.Count();


			var numberOfWinners = initialRewards
								.Select(s => s - points)
								.Count(w => w > initialMax +1);

			Assert.AreEqual(output, numberOfWinners);
		}

		[TestCase(new[] { 1, 12, -5, -6, 50, 3 }, 4, 12.75000)]
		[TestCase(new[] { 5 }, 1, 5.00000)]
		public void Test_Maximum_Average_Subarray(int[] nums, int k, double answer)
		{
			var maxAvg = FindMaxAverage(nums, k);

			Assert.AreEqual(answer, maxAvg);  // FAILED LEETCODE TEST CASE COMPLEXITY
		}



		// ================================================================================================
		// PRIVATE FUNCTIONS
		// ================================================================================================

		static double FindMaxAverage(int[] nums, int k)
		{
			var numSpan = nums.AsSpan();
			var avgList = new List<double>();

			for (int i = 0; i < numSpan.Length; i++)
			{
				int numsRemaining = numSpan.Length - i;

				if (numsRemaining >= k)
				{
					ReadOnlySpan<int> slice = numSpan.Slice(i, k);
					var avg = slice.ToArray().Sum() / (double)k;
					avgList.Add(avg);
				}
			}
			return avgList.Max();
		}
	}
}
