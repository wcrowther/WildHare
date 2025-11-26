using System;
using System.Collections.Generic;

namespace WildHare.Tests.Extensions;

public static class TupleExtensions
{
	extension<T1, T2>((T1, Result) tuple)
    {
        public bool IsFirstDefault => EqualityComparer<T1>.Default.Equals(tuple.Item1, default(T1));
		public string Describe() => $"First = {tuple.Item1}, Second = {tuple.Item2}";
	}

	extension<T1, T2>((T1, T2))
	{

		public static (T1, T2) Create(T1 a, T2 b) => (a, b);
    }
}