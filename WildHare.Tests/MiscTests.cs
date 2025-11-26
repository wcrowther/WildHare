using NUnit.Framework;
using NUnit.Framework.Legacy;
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

		  ClassicAssert.AreEqual(null,	   result.Data);
		  ClassicAssert.AreEqual(false,   result.HasData);
		  ClassicAssert.AreEqual(true,	   result.Ok);
	 }

	 [Test]
	 public void Test_Result_String_Ok_Null()
	 {
		  var result = Returns<string>.IsSuccess(null);

		  ClassicAssert.AreEqual(true,	   result.Ok);
		  ClassicAssert.AreEqual(null,	   result.Data);
		  ClassicAssert.AreEqual(false,   result.HasData);
	 }

	 [Test]
	 public void Test_Result_String_Ok_With_Value()
	 {
		  var result = Returns<string>.IsSuccess("String value");

		  ClassicAssert.AreEqual(true,				 result.Ok);
		  ClassicAssert.AreEqual("String value",	 result.Data);
		  ClassicAssert.AreEqual(true,			 result.HasData);
	 }

	 [Test]
	 public void Test_Result_String_Ok_With_Null_Guard()
	 {
		  string val	 = null;
		  var result	 = Returns<string>.IsSuccess(val ?? "String if null");

		  ClassicAssert.AreEqual(true,				 result.Ok);
		  ClassicAssert.AreEqual("String if null",	 result.Data);
	 }

	 [Test]
	 public void Test_Result_Array_Ok_With_Null_Guard()
	 {
		  string[] values = null;
		  var result = Returns<string[]>.IsSuccess(values ?? []);

		  ClassicAssert.AreEqual(true,	   result.Ok); // no exception
		  ClassicAssert.AreEqual(true,	   result.HasData); 
		  ClassicAssert.AreEqual(0,	   result.Data.Length);
	 }

	 [Test]
	 public void Test_Result_String_Error_With_String()
	 {
		  string message	  = "The returns is an error.";
		  var result		  = Returns<string>.IsFailure(new Exception(message));

		  ClassicAssert.AreEqual(false,	result.Ok);
		  ClassicAssert.AreEqual(null,		result.Data);
		  ClassicAssert.AreEqual(message,	result.Error.Message);
	 }

	 [Test]
	 public void Test_Result_String_Error_With_Inner_Exception()
	 {
		  var ex	 = new Exception("InnerException");
		  var result = Returns<string>.IsFailure(new Exception("Error", ex));

		  ClassicAssert.AreEqual(false, result.Ok);
		  ClassicAssert.AreEqual(null, result.Data);
		  ClassicAssert.AreEqual("Error", result.Error.Message);
		  ClassicAssert.AreEqual("InnerException", result.Error.InnerError.Message);
	 }

	 [Test]
	 public void Test_Result_String_Error_With_Exception_With_Fallback_Data()
	 {
		  string message = "The returns is an error.";
		  var exception = new Exception(message);
		  var result = Returns<string>.IsFailure(exception);

		  ClassicAssert.AreEqual(false, result.Ok);
		  ClassicAssert.AreEqual(null, result.Data);
		  ClassicAssert.AreEqual(message, result.Error.Message);
	 }

	 [Test]
	 public void Test_Result_List()
	 {
		  var list		 = new List<string> { "one", "two", "three" };
		  var result	 = Returns<List<string>>.IsSuccess(list);

		  ClassicAssert.AreEqual(true,	   result.Ok);
		  ClassicAssert.AreEqual("two",   result.Data[1]);
		  ClassicAssert.AreEqual(null,	   result.Error);
	 }


	 [Test]
	 public void Test_Result_List_Empty()
	 {
		  List<string> nullList = null;
		  var result = Returns<List<string>>.IsSuccess(nullList);

		  ClassicAssert.AreEqual(true,	   result.Ok);
		  ClassicAssert.AreEqual(false,   result.HasData);
		  ClassicAssert.AreEqual(null,	   result.Error);
	 }

	 [Test]
	 public void Test_Result_Array_Empty()
	 {
		  string[] array = null;
		  var result = Returns<string[]>.IsSuccess(array);

		  ClassicAssert.AreEqual(true, result.Ok);
		  ClassicAssert.AreEqual(false, result.HasData);
		  ClassicAssert.AreEqual(null, result.Error);
	 }

	 // ===================================================================

	 [Test]
	 public void Test_Result_String_ToString()
	 {
		  var result = new Returns() {Data = "String value"};

		  ClassicAssert.AreEqual(true, result.Ok);
		  ClassicAssert.AreEqual("String value", $"{result}");

		  Returns resultEx = new Exception("IsFailure");

		  ClassicAssert.AreEqual(false, resultEx.Ok);
		  ClassicAssert.AreEqual("IsFailure", $"{resultEx}");

		  Returns resultEx2 = new Exception("IsFailure");
		  string message   = resultEx2.ToString();

		  ClassicAssert.AreEqual(false, resultEx2.Ok);
		  ClassicAssert.AreEqual("IsFailure", message);
	 }


	 [Test]
	 public void Test_Ternary_Boolean()
	 {
		  Returns<bool?> result = Returns<bool?>.IsSuccess(null);

		  ClassicAssert.AreEqual(true,    result.Ok); // null values are Ok if no exception
		  ClassicAssert.AreEqual(null,	   result.Data);
		  ClassicAssert.AreEqual(false,   result.HasData);

		  Returns<bool?> result2 = Returns<bool?>.IsSuccess(true);

		  ClassicAssert.AreEqual(true, result2.Ok); 
		  ClassicAssert.AreEqual(true, result2.HasData ? result2.Data : null);
	 }


	[Test]
	public void Test_Result_Array_With_Null_Guard()
	{
		// You need a null guard as null will NOT
		// implicitly convert to Returns<string>

		string[] values = null;
		var result = Returns<string[]>.IsSuccess(values ?? []);

		ClassicAssert.AreEqual(true, result.Ok); // no exception
		ClassicAssert.AreEqual(true, result.HasData);
		ClassicAssert.AreEqual(0, result.Data.Length);
	}

	[Test]
	 public void Test_Object()
	 {
		  var result = Returns<Person>.IsSuccess(new Person { FirstName = "Will" });

		  ClassicAssert.AreEqual(true,   result.Ok); 
		  ClassicAssert.AreEqual("Will", result.Data.FirstName);
	 }

	 [Test]
	 public void Test_Result_HasData_None()
	 {
	 	 List<string> list = null;
	 	 var result = Returns<List<string>>.IsSuccess(list ?? []);

		 ClassicAssert.AreEqual(true,	   result.Ok);  // no exception
		 ClassicAssert.AreEqual(true,	   result.HasData);
	 	 ClassicAssert.AreEqual(0,		   result.Data.Count);
	 }

	 [Test]
	 public void Test_Result_Has_Data_Three()
	 {
	 	 var list = new List<string> { "one", "two", "three" };
	 	 var result = Returns<List<string>>.IsSuccess(list ?? []);
	 
	 	 ClassicAssert.AreEqual(true,	   result.Ok);
	 	 ClassicAssert.AreEqual(true,	   result.HasData);
	 	 ClassicAssert.AreEqual(3,		   result.Data.Count);
	 	 ClassicAssert.AreEqual("two",	   result.Data.ElementAt(1));
	 }

	[Test]
	public void Test_Result_Complex_With_Steps()
	{
		Returns result =  "Test".IsNotNullString()
								.CanBeConvertedToUpper()
								.IsEqualToString("TEST is valid");

		ClassicAssert.AreEqual(true, result.Ok);
		ClassicAssert.AreEqual("TEST is valid", result.Data);
		ClassicAssert.AreEqual(null, result.Error?.ErrorList());
	}


	[Test]
	public void Test_Result_Returns_Null_Complex_With_Steps()
	{
		string[] expectedErrors = [""];
		string str = null;
		Returns result = str.IsNotNullString()
							.CanBeConvertedToUpper()
							.IsEqualToString("");
	
		ClassicAssert.AreEqual(false, result.Ok);
		ClassicAssert.AreEqual(null, result.Data);
		ClassicAssert.AreEqual("IsEqualToString IsFailure", result.Error.Message);
	}
}


// ==================================================================================
// TestSteps class for tests above ^^^
// ==================================================================================

public static class ReturnsTestSteps
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
// 	ClassicAssert.AreEqual(true, result.Ok);
// 	ClassicAssert.AreEqual("String value", result.Data);
// }
// 
// [Test]
// public void Test_Implicit_No_Type_Result_String_With_Null_Guard()
// {
// 	string val = null;
// 	Returns result = val ?? "String value";
// 
// 	ClassicAssert.AreEqual(true, result.Ok);
// 	ClassicAssert.AreEqual("String value", result.Data);
// }
// 
// [Test]
// public void Test_Implicit_IEnumerable_With_Null_Guard()
// {
// 	List<Item> items = null;
// 	Returns<List<Item>> returns = items ?? [];
// 
// 	ClassicAssert.AreEqual(true, returns.Ok);
// 	ClassicAssert.AreEqual(0, returns.Data.Count);
// }
// 
// [Test]
// public void Test_Implicit_Result_Exception()
// {
// 	Exception ex = new("IsFailure");
// 	Returns result = ex;
// 
// 	ClassicAssert.AreEqual(false, result.Ok);
// 	ClassicAssert.AreEqual(null, result.Data);
// 	ClassicAssert.AreEqual("IsFailure", result.Error.Message);
// }
// 
// [Test]
// public void Test_Implicit_Result_NotImplementedException()
// {
// 	Returns result = new NotImplementedException();
// 
// 	ClassicAssert.AreEqual(false, result.Ok);
// 	ClassicAssert.AreEqual(null, result.Data);
// 	ClassicAssert.AreEqual("The method or operation is not implemented.", result.Error.Message);
// }
// 
// [Test]
// public void Test_Implicit_Typed_Result_NotImplementedException()
// {
// 	Returns<int[]> result = new NotImplementedException();
// 
// 	ClassicAssert.AreEqual(false, result.Ok);
// 	ClassicAssert.AreEqual(null, result.Data);
// 	ClassicAssert.AreEqual("The method or operation is not implemented.", result.Error.Message);
// }

