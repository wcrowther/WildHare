using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WildHare.Tests.Models;
using WildHare.Tests.Models.Generics;

namespace WildHare.Tests;

[TestFixture]
public class MiscTests
{
	[Test]
	public void Test_Result_Basic()
	{
		var result = new Returns();

		Assert.AreEqual(null,	result.Data);
		Assert.AreEqual(false,	result.HasData);
		Assert.AreEqual(true,	result.Ok);
	}

	[Test]
	public void Test_Result_Ok_Null()
	{
		var result = Returns.IsSuccess(null);

		Assert.AreEqual(true,	result.Ok);
		Assert.AreEqual(null,	result.Data);
		Assert.AreEqual(false,	result.HasData);
	}

	[Test]
	public void Test_Result_Ok_With_Value()
	{
		var result = Returns.IsSuccess("String value");

		Assert.AreEqual(true,			result.Ok);
		Assert.AreEqual("String value", result.Data);
		Assert.AreEqual(true,			result.HasData);
	}

	[Test]
	public void Test_Result_IfData_With_Value()
	{
		Returns result = Returns.IfData("String value", "Error");

		Assert.AreEqual(true,			result.Ok);
		Assert.AreEqual("String value", result.Data);
		Assert.AreEqual("String value", result.ToString());
		Assert.AreEqual(true,			result.HasData);

		Returns result2 = Returns.IfData(null, "Error");

		Assert.AreEqual(false,		result2.Ok);
		Assert.AreEqual(null,			result2.Data);
		Assert.AreEqual(false,		result2.HasData);
		Assert.AreEqual("Error",	result2.Error.Message);
		Assert.AreEqual("Error",	result2.ToString());

	}

	[Test]
	public void Test_Result_Ok_With_Null_Guard()
	{
		string val = null;
		var result = Returns.IsSuccess(val ?? "String if null");

		Assert.AreEqual(true, result.Ok);
		Assert.AreEqual("String if null", result.Data);
	}

	[Test]
	public void Test_Result_IsFailure_String_ErrorM()
	{
		var result = Returns.IsFailure("Error message");

		Assert.AreEqual(false, result.Ok);
		Assert.AreEqual("Error message", result.Error.Message);
	}

	[Test]
	public void Test_Result_IsFailure_Error()
	{
		var result = Returns.IsFailure(new Error("Error message"));

		Assert.AreEqual(false, result.Ok);
		Assert.AreEqual("Error message", result.Error.Message);
	}

	[Test]
	 public void Test_Result_String_Basic()
	 {
		  var result = new Returns<string>();

		  Assert.AreEqual(null,	   result.Data);
		  Assert.AreEqual(false,   result.HasData);
		  Assert.AreEqual(true,	   result.Ok);
	 }

	 [Test]
	 public void Test_Result_String_Ok_Null()
	 {
		  var result = Returns<string>.IsSuccess(null);

		  Assert.AreEqual(true,	   result.Ok);
		  Assert.AreEqual(null,	   result.Data);
		  Assert.AreEqual(false,   result.HasData);
	 }

	 [Test]
	 public void Test_Result_String_Ok_With_Value()
	 {
		  var result = Returns<string>.IsSuccess("String value");

		  Assert.AreEqual(true,				result.Ok);
		  Assert.AreEqual("String value",	result.Data);
		  Assert.AreEqual(true,				result.HasData);
	 }

	 [Test]
	 public void Test_Result_String_Ok_With_Null_Guard()
	 {
		  string val	 = null;
		  var result	 = Returns<string>.IsSuccess(val ?? "String if null");

		  Assert.AreEqual(true,				 result.Ok);
		  Assert.AreEqual("String if null",	 result.Data);
	 }

	 [Test]
	 public void Test_Result_Array_Ok_With_Null_Guard()
	 {
		  string[] values = null;
		  var result = Returns<string[]>.IsSuccess(values ?? []);

		  Assert.AreEqual(true,	   result.Ok); // no exception
		  Assert.AreEqual(true,	   result.HasData); 
		  Assert.AreEqual(0,	   result.Data.Length);
	 }

	 [Test]
	 public void Test_Result_String_Error_With_String()
	 {
		  string message	  = "The returns is an error.";
		  var result		  = Returns<string>.IsFailure(new Exception(message));

		  Assert.AreEqual(false,	result.Ok);
		  Assert.AreEqual(null,		result.Data);
		  Assert.AreEqual(message,	result.Error.Message);
	 }

	 [Test]
	 public void Test_Result_String_Error_With_Inner_Exception()
	 {
		  var ex	 = new Exception("InnerException");
		  var result = Returns.IsFailure(new Exception("Error", ex));

		  Assert.AreEqual(false,			result.Ok);
		  Assert.AreEqual(null,				result.Data);
		  Assert.AreEqual("Error",			result.Error.Message);
		  Assert.AreEqual("InnerException", result.Error.InnerError.Message);
	 }

	 [Test]
	 public void Test_Result_String_Error_With_Exception_With_Fallback_Data()
	 {
		  string message = "The returns is an error.";
		  var exception = new Exception(message);
		  var result = Returns.IsFailure(exception);

		  Assert.AreEqual(false,	result.Ok);
		  Assert.AreEqual(null,		result.Data);
		  Assert.AreEqual(message,	result.Error.Message);
	 }

	 [Test]
	 public void Test_Result_List()
	 {
		  var list		 = new List<string> { "one", "two", "three" };
		  var result	 = Returns<List<string>>.IsSuccess(list);

		  Assert.AreEqual(true,	   result.Ok);
		  Assert.AreEqual("two",   result.Data[1]);
		  Assert.AreEqual(null,	   result.Error);
	 }


	 [Test]
	 public void Test_Result_List_Empty()
	 {
		  List<string> nullList = null;
		  var result = Returns<List<string>>.IsSuccess(nullList);

		  Assert.AreEqual(true,	   result.Ok);
		  Assert.AreEqual(false,   result.HasData);
		  Assert.AreEqual(null,	   result.Error);
	 }

	 [Test]
	 public void Test_Result_Array_Empty()
	 {
		  string[] array = null;
		  var result = Returns<string[]>.IsSuccess(array);

		  Assert.AreEqual(true,		result.Ok);
		  Assert.AreEqual(false,	result.HasData);
		  Assert.AreEqual(null,		result.Error);
	 }

	 // ===================================================================

	 [Test]
	 public void Test_Result_String_ToString()
	 {
		  var result = new Returns("String value");

		  Assert.AreEqual(true, result.Ok);
		  Assert.AreEqual("String value", $"{result}");

		  Returns resultEx = new Exception("IsFailure");

		  Assert.AreEqual(false, resultEx.Ok);
		  Assert.AreEqual("IsFailure", $"{resultEx}");

		  Returns resultEx2 = new Exception("IsFailure");
		  string message   = resultEx2.ToString();

		  Assert.AreEqual(false, resultEx2.Ok);
		  Assert.AreEqual("IsFailure", message);
	 }


	 [Test]
	 public void Test_Ternary_Boolean()
	 {
		  Returns<bool?> result = Returns<bool?>.IsSuccess(null);

		  Assert.AreEqual(true,    result.Ok); // null values are Ok if no exception
		  Assert.AreEqual(null,	   result.Data);
		  Assert.AreEqual(false,   result.HasData);

		  Returns<bool?> result2 = Returns<bool?>.IsSuccess(true);

		  Assert.AreEqual(true, result2.Ok); 
		  Assert.AreEqual(true, result2.HasData ? result2.Data : null);
	 }


	[Test]
	public void Test_Result_Array_With_Null_Guard()
	{
		// You need a null guard as null will NOT
		// implicitly convert to Returns<string>

		string[] values = null;
		var result = Returns<string[]>.IsSuccess(values ?? []);

		Assert.AreEqual(true,	result.Ok); // no exception
		Assert.AreEqual(true,	result.HasData);
		Assert.AreEqual(0,		result.Data.Length);
	}

	[Test]
	 public void Test_Object()
	 {
		  var result = Returns<Person>.IsSuccess(new Person { FirstName = "Will" });

		  Assert.AreEqual(true,   result.Ok); 
		  Assert.AreEqual("Will", result.Data.FirstName);
	 }

	 [Test]
	 public void Test_Result_HasData_None()
	 {
	 	 List<string> list = null;
	 	 var result = Returns<List<string>>.IsSuccess(list ?? []);

		 Assert.AreEqual(true,	   result.Ok);  // no exception
		 Assert.AreEqual(true,	   result.HasData);
	 	 Assert.AreEqual(0,		   result.Data.Count);
	 }

	 [Test]
	 public void Test_Result_Has_Data_Three()
	 {
	 	 var list = new List<string> { "one", "two", "three" };
	 	 var result = Returns<List<string>>.IsSuccess(list ?? []);
	 
	 	 Assert.AreEqual(true,	   result.Ok);
	 	 Assert.AreEqual(true,	   result.HasData);
	 	 Assert.AreEqual(3,		   result.Data.Count);
	 	 Assert.AreEqual("two",	   result.Data.ElementAt(1));
	 }

	[Test]
	public void Test_Result_Complex_With_Steps()
	{
		Returns<string> result =  "Test".IsNotNullString()
								.CanBeConvertedToUpper()
								.IsEqualToString("TEST is valid");

		Assert.AreEqual(true,			 result.Ok);
		Assert.AreEqual("TEST is valid", result.Data);
		Assert.AreEqual(null,			 result.Error?.ErrorList());
	}


	[Test]
	public void Test_Result_Returns_Null_Complex_With_Steps()
	{
		string[] expectedErrors = [""];
		string str = null;
		Returns result = str.IsNotNullString()
							.CanBeConvertedToUpper()
							.IsEqualToString("");
	
		Assert.AreEqual(false, result.Ok);
		Assert.AreEqual(null, result.Data);
		Assert.AreEqual("IsEqualToString IsFailure", result.Error.Message);
	}
}


// ==================================================================================
// TestSteps class for tests above ^^^
// ==================================================================================

public static class TestSteps
{
	public static Returns IsNotNullString(this string s)
	{
		return Returns.IfData(s, "String is null"); 
	}

	public static Returns CanBeConvertedToUpper(this Returns result)
	{
		return Returns.IfData(result?.Data?.ToUpper(), "ToUpper failed.");
	}

	public static Returns IsEqualToString(this Returns result, string str)
	{
		return result.Ok
			? Returns.IsSuccess($"{result.Data} is valid")  
			: new Exception("IsEqualToString IsFailure");
	}
}


// ==================================================================================
// Tests for Implicit conversions (REMOVED AT LEAST FOR NOW
// ==================================================================================
//
// [Test]
// public void Test_Implicit_No_Type_Result_String_()
// {
// 	Returns result = "String value";
// 
// 	Assert.AreEqual(true, result.Ok);
// 	Assert.AreEqual("String value", result.Data);
// }
// 
// [Test]
// public void Test_Implicit_No_Type_Result_String_With_Null_Guard()
// {
// 	string val = null;
// 	Returns result = val ?? "String value";
// 
// 	Assert.AreEqual(true, result.Ok);
// 	Assert.AreEqual("String value", result.Data);
// }
// 
// [Test]
// public void Test_Implicit_IEnumerable_With_Null_Guard()
// {
// 	List<Item> items = null;
// 	Returns<List<Item>> returns = items ?? [];
// 
// 	Assert.AreEqual(true, returns.Ok);
// 	Assert.AreEqual(0, returns.Data.Count);
// }
// 
// [Test]
// public void Test_Implicit_Result_Exception()
// {
// 	Exception ex = new("IsFailure");
// 	Returns result = ex;
// 
// 	Assert.AreEqual(false, result.Ok);
// 	Assert.AreEqual(null, result.Data);
// 	Assert.AreEqual("IsFailure", result.Error.Message);
// }
// 
// [Test]
// public void Test_Implicit_Result_NotImplementedException()
// {
// 	Returns result = new NotImplementedException();
// 
// 	Assert.AreEqual(false, result.Ok);
// 	Assert.AreEqual(null, result.Data);
// 	Assert.AreEqual("The method or operation is not implemented.", result.Error.Message);
// }
// 
// [Test]
// public void Test_Implicit_Typed_Result_NotImplementedException()
// {
// 	Returns<int[]> result = new NotImplementedException();
// 
// 	Assert.AreEqual(false, result.Ok);
// 	Assert.AreEqual(null, result.Data);
// 	Assert.AreEqual("The method or operation is not implemented.", result.Error.Message);
// }

