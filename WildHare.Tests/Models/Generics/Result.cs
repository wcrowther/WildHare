using System;
using System.Collections.Generic;
using System.Linq;
using WildHare.Extensions;

namespace WildHare.Tests.Models;

public class Result<T>
{

    public bool Success { get; init; }

	 public string Message { get; init; }

	 public Exception Exception { get; init; }

	 public T Data { get; init; }

	 public static Result<T> Ok(T data = default, string message = null) 
	 {
		  return new() { Success = true, Data = data, Message = message };
	 }

	 public static Result<T> Return(T data = default, string message = null)
	 {
		  return new() { Success = true, Data = data, Message = message };
	 }

	 public static Result<T> Error(string message = null, Exception exception = null)
	 {
		  string msg = (message is null && exception is not null) ? exception.Message : message;
		  
		  return new() { Success = false, Message = msg, Exception = exception };
	 }

	 public static Result<T> HasData(T data = default, string message = null, string noDataMessage = null)
	 {
		  bool hasData = data is not null;
		  return new() { Success = hasData, Data = data, Message = hasData ? message : noDataMessage };
	 }
	 
	 public static Result<T> HasCount(T data = default, string message = null, string noDataMessage = null)
	 {
		  string nullError		  = noDataMessage.IsNullOrEmpty() ? "HasCount() data type cannot be null" : noDataMessage;
		  string ienumerableError = "HasCount() data type must be able to be converted to type IEnumerable<object>";

		  if (data is null)
			   return Error(nullError, new Exception(nullError));

		  if (data is IEnumerable<object> e)
			   return  new() { Success = e.Any(), Data = data, Message = e.Any() ? message : noDataMessage };

		  return Error(ienumerableError,  new Exception(ienumerableError));
	 }
}

public class Result : Result<int>
{
	 public static new Result Ok(int data = default, string message = null) => Ok(data, message);

	 public static new Result Error(string message, Exception exception = null) => Error(message, exception);
}

