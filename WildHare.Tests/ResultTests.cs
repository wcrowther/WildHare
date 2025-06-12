using NUnit.Framework;
using System;
using System.Collections.Generic;
using WildHare.Tests.Models;
using WildHare.Tests.Models.Generics;

namespace WildHare.Tests;

[TestFixture]
public class ResultTests
{
	[Test]
	public void Test_Result_String_Basic()
	{
		var result = "Basic String".ToResult();

		Assert.AreEqual("Basic String",	result.Data);
		Assert.AreEqual("",				result.Result.Message);
		Assert.AreEqual(true,			result.Result.Ok);
	}

	[Test]
	public void Test_Result_String_Basic2()
	{
		var result = "Basic String".ToSuccess();

		Assert.AreEqual("Basic String", result.Data);
		Assert.AreEqual("",				result.Result.Message);
		Assert.AreEqual(true,			result.Result.Ok);
	}

	[Test]
	public void Test_Result_String_Null()
	{
		string nullStr = null;
		var result  = nullStr.ToSuccess("");

		Assert.AreEqual("", result.Data);
		Assert.AreEqual("", result.Result.Message);
		Assert.AreEqual(true, result.Result.Ok);
	}

	[Test]
	public void Test_Result_List_Is_Null()
	{
		var list = new List<Item>();
		list = null;
		var result = list.ToSuccess([]); // must assign a default

		Assert.AreEqual(0, result.Data.Count );
		Assert.AreEqual("", result.Result.Message);
		Assert.AreEqual(true, result.Result.Ok);
	}


	[Test]
	public void Test_Result_String_Failure()
	{
		var result = "InvalidString".ToFailure("String is not valid.");

		Assert.AreEqual("InvalidString",		result.Data);
		Assert.AreEqual("String is not valid.",	result.Result.Message);
		Assert.AreEqual(false,					result.Result.Ok);
	}

	[Test]
	public void Test_Result_List_Basic()
	{
		var result = new List<string>{ "one", "two", "three" }
						.ToResult();	

		Assert.AreEqual(3,		result.Data.Count);
		Assert.AreEqual("",		result.Result.Message);
		Assert.AreEqual(true,	result.Result.Ok);
	}

	[Test]
	public void Test_Result_String_Ok_With_Null_Guard()
	{
		List<string> val	= null;
		var result			= val.ToSuccess([]);

		Assert.AreEqual(0,		result.Data.Count);
		Assert.AreEqual("",		result.Result.Message);
		Assert.AreEqual(true,	result.Result.Ok);
	}

	// [Test]
	// public void Test_Result_With_Recurive_Messages()
	// {
	// 	var result  = false.ToFailure("Should not be false.");
	// 	var result2 = result.ToFailure("Should not be false again.");
	// 	var result3 = result2.ToFailure("Should not be false again.");
	// 
	// 
	// 
	// 
	// 
	// 	Assert.AreEqual(0, result3.GetAllMessages();
	// 	// Assert.AreEqual("", result.Result.Message);
	// 	// Assert.AreEqual(true, result.Result.Ok);
	// }

	[Test]
	public void Test_Result__With_Steps_Success()
	{
		string[] expectedErrors = [""];
		string str = null;
		var result = str.IsNotNullString2()
						.CanBeConvertedToUpper2()
						.IsEqualToString2("");

		Assert.AreEqual(false, result.Result.Ok);
		Assert.AreEqual(null, result.Data);
		Assert.AreEqual("Bad data.", result.Result.Message);
	}
}


// ==================================================================================
// TestSteps class for tests above ^^^
// ==================================================================================

public static class TestSteps
{
	public static (string Data, Result Result) IsNotNullString2(this string s)
	{
		// Add failure message if s is null
		return s.ToResult();
	}

	public static (string Data, Result Result) CanBeConvertedToUpper2(this (string Data, Result Result) result)
	{
		// Add failure message if result.Data is not a string
		return (result.Data, result.Result);
	}

	public static (string Data, Result Result) IsEqualToString2(this (string Data, Result Result) result, string str)
	{
		// Check equivalence
		return result.Result.Ok
			? result.Data.ToSuccess()
			: result.Data.ToFailure("Bad data.");
	}
}



