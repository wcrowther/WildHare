using NUnit.Framework;
using System;
using System.Collections.Generic;
using WildHare.Extensions;
using WildHare.Tests.Models;

namespace WildHare.Tests;

[TestFixture]
public class MiscTests
{
	 [Test]
	 public void Test_Result_Basic()
	 {
		  var result = new Result();

		  Assert.AreEqual(false, result.Success);
		  Assert.AreEqual(0, result.Data);
		  Assert.AreEqual(null, result.Message);
	 }

	 [Test]
	 public void Test_Result_Success()
	 {
		  var result = Result.Ok();

		  Assert.AreEqual(true, result.Success);
		  Assert.AreEqual(0, result.Data);
		  Assert.AreEqual(null, result.Message);
	 }

	 [Test]
	 public void Test_Result_Error()
	 {
		  string message = "The result is an error.";
		  var exception  = new Exception();
		  var result     = Result.Error(message, exception);

		  Assert.AreEqual(false, result.Success);
		  Assert.AreEqual(0, result.Data);
		  Assert.AreEqual(exception, result.Exception);
		  Assert.AreEqual(message, result.Message);
	 }

	 [Test]
	 public void Test_Result_HasData_Three()
	 {
		  var list1   = new List<string> { "one", "two", "three" };		  
		  var result1 = Result<List<string>>.HasData(list1, "Has records", "Has no records");

		  Assert.AreEqual(true, result1.Success);
		  Assert.AreEqual(3, result1.Data.Count);
		  Assert.AreEqual("Has records", result1.Message);
	 }

	 [Test]
	 public void Test_Result_HasData_None()
	 {
		  var list = new List<string>();
		  var result = Result<List<string>>.HasData(list, "Has records", "Has no records");

		  Assert.AreEqual(true, result.Success);
		  Assert.AreEqual(0, result.Data.Count);
		  Assert.AreEqual("Has records", result.Message);
	 }

	 [Test]
	 public void Test_Result_HasData_Int()
	 {
		  var result = Result<int>.HasData(0, "Has data", "Has no data");

		  Assert.AreEqual(true, result.Success);
		  Assert.AreEqual("Has data", result.Message);
	 }

	 [Test]
	 public void Test_Result_HasCount_IEnumerableOfObject()
	 {
		  var list   = new List<string>{ "frog", "cat", "dog" };
		  var result = Result<List<string>>.HasCount(list, "Has count", "Has no count");

		  Assert.AreEqual(true, result.Success);
		  Assert.AreEqual(3, result.Data.Count);
		  Assert.AreEqual("Has count", result.Message);
	 }

	 [Test]
	 public void Test_Result_HasCount_IEnumerableOfObject_Null()
	 {
		  var result = Result<List<string>>.HasCount(null, "Has count", "Has no count");

		  Assert.AreEqual(false, result.Success);
		  Assert.AreEqual(null, result.Data);
		  Assert.AreEqual("Has no count", result.Message);
	 }

	 [Test]
	 public void Test_Result_HasCount_IEnumerableOfObject_Zero_Count()
	 {
		  var list = new List<string> {};
		  var result = Result<List<string>>.HasCount(list, "Has count", "Has no count");

		  Assert.AreEqual(false, result.Success);
		  Assert.AreEqual( 0, result.Data.Count);
		  Assert.AreEqual("Has no count", result.Message);
	 }

	 [Test]
	 public void Test_Result_HasCount_IEnumerableOfObject_Exceptionz()
	 {
		  var result = Result<int>.HasCount(5, "Has count", "Has no count");

		  Assert.AreEqual(false, result.Success);
		  Assert.AreEqual(0, result.Data);
		  Assert.IsTrue(result.Message.StartsWith("HasCount() data type"));
	 }

	 [Test]
	 public void Test_Result_Test_Exception()
	 {
		  var result = new Result<int> { Exception = new NotImplementedException()};

		  Assert.IsInstanceOf<NotImplementedException>(result.Exception);
	 }
}