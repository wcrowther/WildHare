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

		  Assert.AreEqual(true,				 result.Ok);
		  Assert.AreEqual("String value",	 result.Data);
		  Assert.AreEqual(true,			 result.HasData);
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
	 public void Test_Result_Array_With_Null_Guard()
	 {
		  // You need a null guard as null will NOT
		  // implicitly convert to Returns<string>
		  
		  string[] values = null;
		  Returns<string[]> result = values ?? [];

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
		  var result = Returns<string>.IsFailure(new Exception("Error", ex));

		  Assert.AreEqual(false, result.Ok);
		  Assert.AreEqual(null, result.Data);
		  Assert.AreEqual("Error", result.Error.Message);
		  Assert.AreEqual("InnerException", result.Error.InnerError.Message);
	 }

	 [Test]
	 public void Test_Result_String_Error_With_Exception_With_Fallback_Data()
	 {
		  string message = "The returns is an error.";
		  var exception = new Exception(message);
		  var result = Returns<string>.IsFailure(exception);

		  Assert.AreEqual(false, result.Ok);
		  Assert.AreEqual(null, result.Data);
		  Assert.AreEqual(message, result.Error.Message);
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

		  Assert.AreEqual(true, result.Ok);
		  Assert.AreEqual(false, result.HasData);
		  Assert.AreEqual(null, result.Error);
	 }

	 // ===================================================================

	 [Test]
	 public void Test_Implicit_Result_String_Basic()
	 {
		  Returns<string> result = "String value";

		  Assert.AreEqual(true, result.Ok);
		  Assert.AreEqual("String value", result.Data);
	 }

	 [Test]
	 public void Test_Implicit_Result_String_ToString()
	 {
		  Returns result = "String value";

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
	 public void Test_Implicit_No_Type_Result_String_()
	 {
		  Returns result = "String value";

		  Assert.AreEqual(true, result.Ok);
		  Assert.AreEqual("String value", result.Data);
	 }

	 [Test]
	 public void Test_Implicit_No_Type_Result_String_With_Null_Guard()
	 {
		  string val = null;
		  Returns result = val ?? "String value";

		  Assert.AreEqual(true, result.Ok);
		  Assert.AreEqual("String value", result.Data);
	 }

	[Test]
	public void Test_Implicit_IEnumerable_With_Null_Guard()
	{
		List<Item> items = null;
		Returns<List<Item>> returns = items ?? [];

		Assert.AreEqual(true, returns.Ok);
		Assert.AreEqual(0, returns.Data.Count);
	}

	[Test]
	public void Test_Implicit_Result_Exception()
	{
		Exception ex = new("IsFailure");
		Returns result = ex;

		Assert.AreEqual(false, result.Ok);
		Assert.AreEqual(null, result.Data);
		Assert.AreEqual("IsFailure", result.Error.Message);
	}

	[Test]
	 public void Test_Implicit_Result_NotImplementedException()
	 {
		  Returns result = new NotImplementedException();

		  Assert.AreEqual(false, result.Ok);
		  Assert.AreEqual(null, result.Data);
		  Assert.AreEqual("The method or operation is not implemented.", result.Error.Message);
	 }

	 [Test]
	 public void Test_Implicit_Typed_Result_NotImplementedException()
	 {
		  Returns<int[]> result = new NotImplementedException();

		  Assert.AreEqual(false, result.Ok);
		  Assert.AreEqual(null, result.Data);
		  Assert.AreEqual("The method or operation is not implemented.", result.Error.Message);
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
	 public void Test_Object()
	 {
		  Returns<Person> result = new Person { FirstName = "Will" };

		  Assert.AreEqual(true,   result.Ok); 
		  Assert.AreEqual("Will", result.Data.FirstName);
	 }

	 [Test]
	 public void Test_Result_HasData_None()
	 {
	 	 List<string> list = null;
	 	 Returns<List<string>> result = list ?? [];

		 Assert.AreEqual(true,	   result.Ok);  // no exception
		 Assert.AreEqual(true,	   result.HasData);
	 	 Assert.AreEqual(0,		   result.Data.Count);
	 }

	 [Test]
	 public void Test_Result_Has_Data_Three()
	 {
	 	 var list = new List<string> { "one", "two", "three" };
	 	 Returns<List<string>> result = list ?? [];
	 
	 	 Assert.AreEqual(true,	   result.Ok);
	 	 Assert.AreEqual(true,	   result.HasData);
	 	 Assert.AreEqual(3,		   result.Data.Count);
	 	 Assert.AreEqual("two",	   result.Data.ElementAt(1));
	 }

	 [Test]
	 public void Test_Result_Complex_With_Steps()
	 {
		  Returns result = "Begin".Start()
							     .Step1()
							     .Step2()
							     .Step3();
		  Assert.AreEqual(false,		result.Ok);
	 	  Assert.AreEqual(null,			result.Data);
		  Assert.AreEqual("Step2 IsFailure",		result.Error.InnerError.Message);
		  Assert.AreEqual("Step3 IsFailure",		result.Error.Message);
	 }
}

// ==================================================================================
// TestSteps class for tests above ^^^
// ==================================================================================

public static class TestSteps
{
	public static Returns Start(this string s)
	{
		return s;
	}

	public static Returns Step1(this Returns result)
	{
		return result.Data.ToUpper();
	}

	public static Returns Step2(this Returns result)
	{
		return new Exception("Step2 IsFailure");
	}

	public static Returns Step3(this Returns result)
	{
		return result.Ok ? "Ok" : new Error("Step3 IsFailure", result.Error);
	}
}
