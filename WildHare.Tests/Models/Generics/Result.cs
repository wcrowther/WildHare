using System;
using System.Collections.Generic;
using WildHare.Extensions;

namespace WildHare.Tests.Models.Generics;

public class Result(bool ok, string message = "", Result innerResult = null) 
{
	public bool Ok { get; } = ok;

	public string Message { get; } = message;

	public Result InnerResult { get; } = innerResult;

	public override string ToString() => Ok ? "Success" : Message;
};

// public class Results<T>(T data, Result result = null)
// {
// 	public T Data { get; } = data;
// 
// 	public Result Result { get; } = result ?? new Result(true);
// 
// 	public void Deconstruct(out T Data, out Result Result)
// 	{
// 		Data = this.Data;
// 		Result = this.Result;
// 	}
// };

public static class ResultExtensions
{
	public static (T Data, Result Result) ToResult<T>(this	T data, 
															T defaultValue	= default,
															Result errorResult = null,
															Result innerResult = null
													 )
	{
		var eResult = errorResult ?? new Result(false, "ToResult - Data is null", innerResult);
		var dataOrDefault = (data ?? defaultValue);

		return dataOrDefault is null
			? (default, eResult)
			: (dataOrDefault, new Result(true, innerResult:innerResult));
	}

	public static (T Data, Result Result) Success<T>(this T data) where T : notnull
	{
		return (data, new Result(true));
	}

	public static (T Data, Result Result) Success<T>(this T data, T defaultValue) 
	{
		return (data ?? defaultValue, new Result(true));
	}


	public static (T Data, Result Result) Failure<T>(this T data, string message)
	{
		return (data, new Result(false, message));
	}

	public static List<string> GetAllMessages(this Result result, int maxDepth = 10)
	{
		var messages = new List<string>();
		CollectMessages(result, messages, 0, maxDepth);

		return messages;
	}

	// ======================================================================================

	private static void CollectMessages(Result result, List<string> messages, int currentDepth, int maxDepth)
	{
		if (result == null || currentDepth >= maxDepth)
			return;

		if (!string.IsNullOrEmpty(result.Message))
			messages.Add(result.Message);

		CollectMessages(result.InnerResult, messages, currentDepth + 1, maxDepth);
	}
}




/* 
===============================================================================
NOT REALLY NEEDED ANYMORE
===============================================================================
	
public static Res Map<Data, Res>(this (Data Data, Result Result) source,
										 Func<Data, Res> mapData,
										 Func<Result, Res> mapError)
{
	return source.Result.Ok ? mapData(source.Data) : mapError(source.Result);
}

public static void Act<Data>(this (Data Data, Result Result) source,
									Action<Data> actOnData,
									Action<Result> actOnError)
{
	if (source.Result.Ok)
		actOnData(source.Data);
	else
		actOnError(source.Result);
}
===============================================================================
INSTEAD USE THE RESULT.ISSUCCESS	
===============================================================================

*/