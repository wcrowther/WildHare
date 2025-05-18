using System;
using System.Collections.Generic;
using WildHare.Extensions;

namespace WildHare.Tests.Models.Generics;

public class Result(bool ok, string message = "") 
{
	public bool Ok { get; } = ok;

	public string Message { get; } = message;

	public List<Result> InnerResults { get; } = [];

	public override string ToString() => Ok ? "Success" : Message;
};

public static class ResultExtensions
{
	public static (T Data, Result Result) ToResult<T>(this	T data, 
															T defaultValue	= default,
															Result errorResult	= null
													 )
	{
		var eResult = errorResult ?? new Result(false, "ToResult - Data is null");
		var dataOrDefault = (data ?? defaultValue);

		return dataOrDefault is null
			? (default, eResult)
			: (dataOrDefault, new Result(true));
	}

	public static (T Data, Result Result) Success<T>(this T data)
	{
		return (data, new Result(true));
	}

	public static (T Data, Result Result) Failure<T>(this T data, string message)
	{
		return (data, new Result(false, message));
	}

	public static List<string> GetAllMessages(Result result, int maxDepth = 10)
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

		if (!result.Message.IsNullOrEmpty()) 
			messages.Add(result.Message);

		foreach (var inner in result.InnerResults)
		{
			CollectMessages(inner, messages, currentDepth + 1, maxDepth);
		}
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