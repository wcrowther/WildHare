using System;

namespace WildHare.Tests.Models.Generics;

public class Result<T>
{
	 public T Data { get; init; }

	 public Exception Exception { get; init; }

	 public bool Success => Exception is null;

	 public bool HasData => Data is not null;

	 public static implicit operator Result<T>(T data) => new() { Data = data };

	 public static implicit operator Result<T>(Exception ex) => new() { Exception = ex };

	 public static Result<T> Ok(T data) => new() { Data = data };

	 public static Result<T> Error(Exception exception, T data = default) => new() { Exception = exception, Data = data };
}

public class Result : Result<string>
{
	 public static implicit operator Result(string data) => new() { Data = data };

	 public static implicit operator Result(Exception ex) => new() { Exception = ex };

	 public static new Result Ok(string data = "") => new() { Data = data };

	 public static new Result Error(Exception exception, string data = "") => new() { Exception = exception, Data = data };

	 public override string ToString() => Success ? Data : Exception.Message;
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


