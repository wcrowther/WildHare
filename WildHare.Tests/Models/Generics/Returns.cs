using System;

namespace WildHare.Tests.Models.Generics;

public class Returns<T>
{
	 public T Data { get; init; }

	 public Exception Exception { get; init; }

	 public bool Ok => Exception is null;

	 public bool HasData => Data is not null;

	 public static implicit operator Returns<T>(T data) => new() { Data = data };

	 public static implicit operator Returns<T>(Exception ex) => new() { Exception = ex };

	 public static Returns<T> Success(T data) => new() { Data = data };

	public static Returns<T> Failure(string errorMessage) => new() { Exception = new Exception(errorMessage) };

	public static Returns<T> Failure(Exception exception) => new() { Exception = exception };
}

public class Returns : Returns<string>
{
	 public static implicit operator Returns(string data) => new() { Data = data };

	 public static implicit operator Returns(Exception ex) => new() { Exception = ex };

	 public override string ToString() => Ok ? Data : Exception.Message;
}

public class Error(string message, Error innerError = null)
{
	public string Message { get => message; }

	public Error InnerError { get => innerError; }

	public static implicit operator Error(Exception ex)
	{
		return new Error(ex.Message, ex.InnerException);
	}
}











// private static D GetDefault<D>()
// {
// 	 return typeof(D) switch
// 	 {
// 		  Type t when t == typeof(string) => (D)(object)"",
// 		  Type t when t.IsArray => (D)(object)Array.CreateInstance(t.GetElementType()!, 0),
// 		  Type t when typeof(IEnumerable).IsAssignableFrom(t)  && t.IsGenericType =>
// 			  (D)(object)Activator.CreateInstance(typeof(List<>).MakeGenericType(t.GetGenericArguments()))!,
// 		  _ => default
// 	 };
// }


