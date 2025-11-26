using System;

namespace WildHare.Tests.Models.Generics;

public class Result(bool ok, string message = "") 
{
	public bool Ok { get; } = ok;

	public string Message { get; } = message;

    public static Result Success => new(true);

    public static Result Failure(string message) => new(false, message);

    public override string ToString() => Ok ? "Success" : Message;
};


public static class ResultExtensions
{
	public static (T Data, Result Result) ToResult<T>(this T data, Result errorResult = null)
	{
		errorResult ??= Result.Failure("ToResult.data is null.");

		return data is not null ? (data, Result.Success) : (default, errorResult);
	}

	public static (T Data, Result Result) ToResult<T>(this T data, Func<T,bool> func, Result errorResult = null)
	{
		errorResult ??= Result.Failure("ToResult.data is does not meet func conditions.");

		return func(data) ? (data, Result.Success) : (default, errorResult);
	}

	public static (T Data, Result Result) ToSuccess<T>(this T data) where T : notnull  => (data ?? default, Result.Success);

	public static (T Data, Result Result) ToSuccess<T>(this T data, T defaultValue) => (data ?? defaultValue, Result.Success);

	public static (T Data, Result Result) ToFailure<T>(this T data, string message) => (data, Result.Failure(message));
}

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