using System;

namespace WildHare.Tests.Models.Generics;

public class Returns<T>
{
	public static implicit operator Returns<T>(Error error) => IsFailure(error);

	public static implicit operator Returns<T>(Exception exception) => IsFailure(exception);


	public T Data { get; init; } = default;

	public Error Error { get; init; }

	public bool Ok => Error is null;

	public bool HasData => Data is not null;


	/// <summary>Is success path. Does not guarantee {data} is not null. If that is required,
	/// use a null guard to ensure non-null like: Ok(itemList ?? [])</summary>
	public static Returns<T> IsSuccess(T data) => new() { Data = data };

	/// <summary>If data is not null returns data, else an Error with a message of {errorMessage}.</summary>
	public static Returns<T> IfData(T data, string errorMessage) => data is not null ? new() { Data = data } : IsFailure(errorMessage);

	/// <summary>Is failure path. Returns an Error with a message of {errorMessage}.</summary>
	public static Returns<T> IsFailure(string errorMessage) => new() { Error = new Error(errorMessage) };

	/// <summary>Is failure path. Returns {error}.</summary>
	public static Returns<T> IsFailure(Error error) => new() { Error = error };
}

public class Returns
{
	public string Data { get; init; } 

	public Error Error { get; init; }

	public bool Ok => Error is null;

	public bool HasData => Data is not null;


	public static implicit operator Returns(Error error) => IsFailure(error);

	public static implicit operator Returns(Exception exception) => IsFailure(exception);


	/// <summary>Is success path. Does not guarantee {data} string is not null.</summary>
	public static Returns IsSuccess(string data) => new() { Data = data };

	/// <summary>If string is not null returns string, else an Error with a message of {errorMessage}.</summary>
	public static Returns IfData(string data, string errorMessage) => data is not null 
		? new() { Data = data } 
		: IsFailure(errorMessage);

	/// <summary>Is failure path. Returns an Error with a message of {errorMessage}.</summary>
	public static Returns IsFailure(string errorMessage) => new() { Error = new Error(errorMessage) };

	/// <summary>Is failure path. Returns {error}.</summary>
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
