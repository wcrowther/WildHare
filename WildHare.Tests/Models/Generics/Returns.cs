using System;

namespace WildHare.Tests.Models.Generics;

public class Returns<T>
{
	public T Data { get; init; } = default;

	public Error Error { get; init; }

	public bool Ok => Error is null;

	public bool HasData => Data is not null;

	public static implicit operator Returns<T>(T data) => new() { Data = data };

	public static implicit operator Returns<T>(Error error) => IsFailure(error);

	public static implicit operator Returns<T>(Exception exception) => IsFailure(exception);

	public static Returns<T> IsSuccess(T data) => new() { Data = data };

	public static Returns<T> IfData(T data, string errorMessage) => data is not null ? new() { Data = data } : IsFailure(errorMessage);

	public static Returns<T> IsFailure(string errorMessage) => new() { Error = new Error(errorMessage) };

	public static Returns<T> IsFailure(Error error) => new() { Error = error };
}

public class Returns
{
	public string Data { get; init; } 

	public Error Error { get; init; }

	public bool Ok => Error is null;

	public bool HasData => Data is not null;


	public static implicit operator Returns(string data) => new() { Data = data };

	public static implicit operator Returns(Error error) => IsFailure(error);

	public static implicit operator Returns(Exception exception) => IsFailure(exception);


	public static Returns IsSuccess(string data) => new() { Data = data };

	public static Returns IfData(string data, string errorMessage) => data is not null ? new() { Data = data } : IsFailure(errorMessage);

	public static Returns IsFailure(string errorMessage) => new() { Error = new Error(errorMessage) };

	public static Returns IsFailure(Error error) => new() { Error = error };

	public override string ToString() => Ok ? Data : Error?.Message;
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
