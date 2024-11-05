using NUnit.Framework;
using System;
using System.Collections.Generic;
using WildHare.Extensions;
using System.Linq;
using WildHare.Tests.Models;
using WildHare.Tests.Extensions;
using NUnit.Framework.Constraints;

namespace WildHare.Tests;

[TestFixture]
public class MiscTests
{
	 [Test]
	 public void Test_Result_String_Basic()
	 {
		  var result = new Result<string>();

		  Assert.AreEqual(false,	  result.Success);
		  Assert.AreEqual(null,	  result.Data);
	 }

	 [Test]
	 public void Test_Result_String_Ok_Null()
	 {
		  var result = Result<string>.Ok(null);

		  Assert.AreEqual(false,   result.Success);
		  Assert.AreEqual(null,	   result.Data);
	 }

	 [Test]
	 public void Test_Result_String_Ok_With_Value()
	 {
		  var result = Result<string>.Ok("String value");

		  Assert.AreEqual(true,				 result.Success);
		  Assert.AreEqual("String value",	 result.Data);
	 }

	 [Test]
	 public void Test_Result_String_Ok_With_Null_Guard()
	 {
		  string val	 = null;
		  var result	 = Result<string>.Ok(val ?? "String if null");

		  Assert.AreEqual(true,				 result.Success);
		  Assert.AreEqual("String if null",	 result.Data);
	 }

	 [Test]
	 public void Test_Result_String_Error_With_String()
	 {
		  string message	  = "The result is an error.";
		  var result		  = Result<string>.Error(message);

		  Assert.AreEqual(false,	result.Success);
		  Assert.AreEqual(null,		result.Data);
		  Assert.AreEqual(message,	result.Exception.Message);
	 }

	 [Test]
	 public void Test_Result_String_Error_With_Exception()
	 {
		  string message	  = "The result is an error.";
		  var exception		  = new Exception(message);
		  var result		  = Result<string>.Error(exception);

		  Assert.AreEqual(false, result.Success);
		  Assert.AreEqual(null, result.Data);
		  Assert.AreEqual(message, result.Exception.Message);
	 }

	 [Test]
	 public void Test_Result_String_Error_With_Exception_With_Fallback_Data()
	 {
		  string message = "The result is an error.";
		  var exception = new Exception(message);
		  var result = Result<string>.Error(exception, "Fallback data");

		  Assert.AreEqual(false, result.Success);
		  Assert.AreEqual("Fallback data", result.Data);
		  Assert.AreEqual(message, result.Exception.Message);
	 }

	 [Test]
	 public void Test_Result_List()
	 {
		  var list		 = new List<string> { "one", "two", "three" };
		  var result	 = Result<List<string>>.Ok(list ?? new List<string>());

		  Assert.AreEqual(true, result.Success);
		  Assert.AreEqual("two", result.Data[1]);
		  Assert.AreEqual(null, result.Exception);
	 }

	 [Test]
	 public void Test_Result_string_Test_Exception()
	 {
		  var exception = new NotImplementedException();
		  var result = new Result<string> { Exception = exception };

		  Assert.IsInstanceOf<NotImplementedException>(result.Exception);
	 }

	 // ===================================================================

	 [Test]
	 public void Test_Implicit_Result_String_Basic()
	 {
		  Result<string> result = "String value";

		  Assert.AreEqual(true, result.Success);
		  Assert.AreEqual("String value", result.Data);
	 }

	 [Test]
	 public void Test_Implicit_Result_String_ToString()
	 {
		  Result result = "String value";

		  Assert.AreEqual(true, result.Success);
		  Assert.AreEqual("String value", $"{result}");

		  Result resultEx = new Exception("Error");

		  Assert.AreEqual(false, resultEx.Success);
		  Assert.AreEqual("Error", $"{resultEx}");

		  Result resultEx2 = new Exception("Error");
		  string message   = resultEx2.ToString();

		  Assert.AreEqual(false, resultEx2.Success);
		  Assert.AreEqual("Error", message);
	 }

	 [Test]
	 public void Test_Implicit_No_Type_Result_String_()
	 {
		  Result result = "String value";

		  Assert.AreEqual(true, result.Success);
		  Assert.AreEqual("String value", result.Data);
	 }

	 [Test]
	 public void Test_Implicit_No_Type_Result_String_With_Null_Guard()
	 {
		  string val = null;
		  Result result = val ?? "String value";

		  Assert.AreEqual(true, result.Success);
		  Assert.AreEqual("String value", result.Data);
	 }

	 [Test]
	 public void Test_Implicit_Result_Exception()
	 {
		  Result result = new Exception("Error");

		  Assert.AreEqual(false,   result.Success);
		  Assert.AreEqual(null,	   result.Data);
		  Assert.AreEqual("Error", result.Exception.Message);
	 }

	 [Test]
	 public void Test_Result_HasData_Three()
	 {
	 	 var list = new List<string> { "one", "two", "three" };
	 	 Result<List<string>> result = list ?? new List<string>();
	 
	 	 Assert.AreEqual(true,	   result.Success);
	 	 Assert.AreEqual(3,		   result.Data.Count);
	 	 Assert.AreEqual("two",	   result.Data.ElementAt(1));
	 }

	 [Test]
	 public void Test_Result_Complex_With_Steps()
	 {
		  Result result = "Begin".Start()
							     .Step1()
							     .Step2()
							     .Step3();
		  Assert.AreEqual(false, result.Success);
	 	  Assert.AreEqual(null, result.Data);
		  Assert.AreEqual("Error", result.Exception.InnerException.Message);
		  Assert.AreEqual("Error2", result.Exception.Message);
	 }

}
