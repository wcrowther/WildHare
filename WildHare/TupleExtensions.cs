
using System;

namespace WildHare.Extensions
{
    public static class TupleExtensions
    {



		public static Res Map<Data,Res>(
			this (Data, MetaError)	source,
			Func<Data, Res>			mapData,
			Func<MetaError, Res>	mapError)
		{
			return source.Item2?.Message is null ? mapData(source.Item1) : mapError(source.Item2);
		}

		// PLACEHOLDER
		// public static Res MapToResults<Data, Res>(
		//		this (Data, MetaError) source,
		//		Func<Data, Res> mapData,
		//		Func<MetaError, Res> mapError)
		// {
		//		return source.Item2?.Message is null ? mapData(source.Item1) : mapError(source.Item2);
		// }


		public static bool IsSuccess<Data>(this (Data, MetaError) source) => source.Item2?.Message.IsNullOrEmpty() ?? true;

		public static bool HasError<Data>(this (Data, MetaError) source) => !source.Item2?.Message.IsNullOrEmpty() ?? false;
	}
}
