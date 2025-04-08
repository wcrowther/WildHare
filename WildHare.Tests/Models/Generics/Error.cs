using System;

namespace WildHare.Tests.Models.Generics;

public class Error(string message, Error innerError = default)
{
	public string Message { get => message ?? ""; } 

	public Error InnerError { get => innerError; }

	public static implicit operator Error(Exception ex) 
		=> ex.InnerException is null ? new Error(ex?.Message) : new Error(ex?.Message, ex?.InnerException);
}


