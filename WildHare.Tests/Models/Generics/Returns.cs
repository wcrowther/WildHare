using System;

namespace WildHare.Tests.Models.Generics;

/// <summary>Be aware that null objects will not implicitly convert to Returns<T>. To
/// ensure Returns<T> is not null use a null-guard like: IsSuccess(itemList ?? [])</summary>
public class Returns<T>(T data = default, Error error = null)
{

	public static implicit operator Returns<T>(T data) => new(data);

	public static implicit operator Returns<T>(Error error) => IsFailure(error);

	public static implicit operator Returns<T>(Exception exception) => IsFailure(exception);


	public T Data { get => data; }

	public Error Error { get => error; }

	public bool Ok => Error is null;

	public bool HasData => Data is not null;


	/// <summary>Is success path. Does not guarantee {data} is not null.</summary>
	public static Returns<T> IsSuccess(T data) => new(data);

	/// <summary>If data is not null returns data, else an Error with a message of {errorMessage}.</summary>
	public static Returns<T> IfData(T data, string errorMessage) => data is not null ? new(data) : IsFailure(errorMessage);

	/// <summary>Is failure path. Returns an Error with a message of {errorMessage}.</summary>
	public static Returns<T> IsFailure(string errorMessage) => new(error: new Error(errorMessage));

	/// <summary>Is failure path. Returns {error}.</summary>
	public static Returns<T> IsFailure(Error error) => new(error: error);

	public override string ToString() => Ok ? Data.ToString() : Error.Message;
}

/// <summary>Version with no T type - uses string as T.</summary>
public class Returns(string data = null, Error error = null) : Returns<string>(data, error)
{
	public static implicit operator Returns(string data) => new(data);

	public static implicit operator Returns(Error error) => new(error: error);

	public static implicit operator Returns(Exception exception) => new(error: exception);

	public static new Returns IsSuccess(string data) => new(data);

	public static new Returns IfData(string data, string errorMessage)
	{
		return data is not null ? new(data) : IsFailure(errorMessage);
	}

	public static new Returns IsFailure(string errorMessage) => new(error: new Error(errorMessage));

	public static new Returns IsFailure(Error error) => new(error: error);
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
