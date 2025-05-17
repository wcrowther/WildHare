using System;
using System.Collections.Generic;

namespace WildHare.Tests.Models.Generics;

public class Error(string message = null, Error innerError = default)
{
	public string Message { get => message ?? ""; } 

	public Error InnerError { get => innerError; }

	public static implicit operator Error(Exception ex) => ex.InnerException is null 
		? new Error(ex?.Message) 
		: new Error(ex?.Message, ex?.InnerException);

	public List<string> ErrorList()
	{
		var errorList = new List<string> { Message };

		if (InnerError != null)
			errorList.AddRange(InnerError.ErrorList());

		return errorList;
	}
}


