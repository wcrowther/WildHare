using System;

namespace WildHare.Tests.Models;

public class Result<T> 
{
	 public bool Success => Exception is null && Data is not null;

	 public Exception Exception { get; init; }

	 public T Data { get; init; }

	 public static implicit operator Result<T>(T data)			 => new(){ Data = data };

	 public static implicit operator Result<T>(Exception ex)	 => new() { Exception = ex };

	 public static Result<T> Ok(T data)
	 {
		  return new() { Data = data };
	 }

	 public static Result<T> Error(string message, T data = default)
	 {
		  return new() { Exception = new Exception(message), Data = data };
	 }

	 public static Result<T> Error(Exception exception, T data = default)
	 {
		  return new() { Exception = exception, Data = data };
	 }
}

public class Result : Result<string>
{
	 public static implicit operator Result(string data)	=> new(){ Data = data };

	 public static implicit operator Result(Exception ex)	=> new(){ Exception = ex };

	 public override string ToString() => Success ? Data : Exception.Message;
}


