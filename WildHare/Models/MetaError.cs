using System;
using System.Collections.Generic;

namespace WildHare;

public class MetaError(string message = null, MetaError innerError = default)
{
	public string Message { get => message ?? ""; }

	public MetaError InnerError { get => innerError; }

	public static implicit operator MetaError(Exception ex) => ex.InnerException is null
		? new MetaError(ex?.Message)
		: new MetaError(ex?.Message, ex?.InnerException);

	public List<string> ErrorList()
	{
		var errorList = new List<string> { Message };

		if (InnerError != null)
			errorList.AddRange(InnerError.ErrorList());

		return errorList;
	}
}

