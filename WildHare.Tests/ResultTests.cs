using NUnit.Framework;
using System.Collections.Generic;
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
		var result = "Basic String".Success();

		Assert.AreEqual("Basic String", result.Data);
		Assert.AreEqual("",				result.Result.Message);
		Assert.AreEqual(true,			result.Result.Ok);
	}

	[Test]
	public void Test_Result_String_Failure()
	{
		var result = "InvalidString".Failure("String is not valid.");

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
		var result			= val.ToResult([]);

		Assert.AreEqual(0,		result.Data.Count);
		Assert.AreEqual("",		result.Result.Message);
		Assert.AreEqual(true,	result.Result.Ok);
	}

	// [Test]
	// public void Test_Result__With_Steps_Success()
	// {
	// 	string[] expectedErrors = [""];
	// 	string str = null;
	// 	Result result =  str.ConvertStringToInt()
	// 						.ConvertIntToString()
	// 						.UpperCaseString()
	// 						.IsEqualToString("");
	// 
	// 	Assert.AreEqual("",		result.Message);
	// 	Assert.AreEqual(false,	result.Ok);
	// 	Assert.AreEqual("IsEqualToString IsFailure", result.Error.Message);
	// }
}


// ==================================================================================
// TestSteps class for tests above ^^^
// ==================================================================================

// public static class TestSteps
// {
// 	public static (int, Result) ConvertStringToInt(this string s)
// 	{
// 		var x = int.TryParse(s, out int resp) ? resp : 0;	
// 
// 		return x.ToResult(); 
// 	}
// 	
// 	public static (string, Result) ConvertIntToString(this (int data, Result result) result)
// 	{
// 		var data = result.data.ToString();
// 
// 		return data.ToResult();
// 	}
// 
// 	public static (string, Result) UpperCaseString(this (string, Result) result)
// 	{
// 		return Returns.IfData(result?.Data?.ToUpper(), "ToUpper failed.");
// 	}
// 
// 	public static Result IsEqualToString(this (string, Result) result, string str)
// 	{
// 		return result.Ok
// 			? Returns.Ok($"{result.Data} is valid")  
// 			: new Exception("IsEqualToString IsFailure");
// 	}
// }


