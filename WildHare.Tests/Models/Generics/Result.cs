namespace WildHare.Tests.Models.Generics;

public class Result(bool ok, string message = "") 
{
	public bool Ok { get; } = ok;

	public string Message { get; } = message;

	public override string ToString() => Ok ? "Success" : Message;
};


public static class ResultExtensions
{
	public static (T Data, Result Result) ToResult<T>(this T data, Result errorResult = null) 
		=> data is null ? (default, errorResult ?? new Result(false, "ToResult.data is null.")) 
							: (data, new Result(true, ""));

	public static (T Data, Result Result) ToSuccess<T>(this T data) where T : notnull 
		=> (data, new Result(true));

	public static (T Data, Result Result) ToSuccess<T>(this T data, T defaultValue) 
		=> (data ?? defaultValue, new Result(true));


	public static (T Data, Result Result) ToFailure<T>(this T data, string message) 
		=> (data, new Result(false, message));
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