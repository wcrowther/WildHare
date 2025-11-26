using AngleSharp.Io;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using WildHare.Tests.Models;
using WildHare.Tests.Models.Generics;

namespace WildHare.Tests;

[TestFixture]
public class ResultTests
{
	[Test]
	public void Test_Result_String_Basic()
	{
		var response = "Basic String".ToResult();

		ClassicAssert.AreEqual("Basic String",	response.Data);
		ClassicAssert.AreEqual("",				response.Result.Message);
		ClassicAssert.AreEqual(true,			response.Result.Ok);
	}

	[Test]
	public void Test_Result_String_Basic2()
	{
		var response = "Basic String".ToSuccess();

		ClassicAssert.AreEqual("Basic String", response.Data);
		ClassicAssert.AreEqual("",				response.Result.Message);
		ClassicAssert.AreEqual(true,			response.Result.Ok);
	}

	[Test]
	public void Test_Result_String_Null()
	{
		string nullStr		= null;
		var response		= nullStr.ToSuccess("");

		ClassicAssert.AreEqual("",		response.Data);
		ClassicAssert.AreEqual("",		response.Result.Message);
		ClassicAssert.AreEqual(true,	response.Result.Ok);
	}

	[Test]
	public void Test_Result_String_Null2()
	{
		string nullStr	= null;
		var response	= nullStr.ToSuccess();

		ClassicAssert.AreEqual(null,	response.Data);
		ClassicAssert.AreEqual("",		response.Result.Message);
		ClassicAssert.AreEqual(true,	response.Result.Ok);
	}

	[Test]
	public void Test_List_ToSuccess_Is_Null()
	{
		List<Item> list = null;
		var response = list.ToSuccess([]); // must assign a default

		ClassicAssert.AreEqual(0,		response.Data?.Count );
		ClassicAssert.AreEqual("",		response.Result.Message);
		ClassicAssert.AreEqual(true,	response.Result.Ok);
	}


	[Test]
	public void Test_Result_String_Failure()
	{
		var response = "InvalidString".ToFailure("String is not valid.");

		ClassicAssert.AreEqual("InvalidString",		response.Data);
		ClassicAssert.AreEqual("String is not valid.",	response.Result.Message);
		ClassicAssert.AreEqual(false,					response.Result.Ok);
	}

	[Test]
	public void Test_Result_List_Basic()
	{
		var response = new List<string>{ "one", "two", "three" }
						.ToResult();	

		ClassicAssert.AreEqual(3,		response.Data.Count);
		ClassicAssert.AreEqual("",		response.Result.Message);
		ClassicAssert.AreEqual(true,	response.Result.Ok);
	}

	[Test]
	public void Test_Result_String_ToSuccess_With_Null_Guard()
	{
		List<string> val	= null;
		var response			= val.ToSuccess([]);

		ClassicAssert.AreEqual(0,		response.Data?.Count);
		ClassicAssert.AreEqual("",		response.Result.Message);
		ClassicAssert.AreEqual(true,	response.Result.Ok);
	}

	[Test]
	public void Test_Result_String_ToResult()
	{
		List<string> val = null;
		var response = val.ToResult();

		ClassicAssert.AreEqual(null, response.Data);
		ClassicAssert.AreEqual("ToResult.data is null.", response.Result.Message);
		ClassicAssert.AreEqual(false, response.Result.Ok);
	}

	[Test]
	public void Test_Result_With_func_ToResult()
	{
		List<string> val = ["One", "Two"];
		var response = val.ToResult(v => v.Count > 1);

		ClassicAssert.AreEqual(2,		response.Data.Count);
		ClassicAssert.AreEqual("",		response.Result.Message);
		ClassicAssert.AreEqual(true,	response.Result.Ok);

		var secondResponse = response.Result.Ok ? response.Data[1] : response.Data[0];

		ClassicAssert.AreEqual("Two", secondResponse);
	}

	[Test]
	public void Test_Result_With_Steps_Success()
	{
		var response =  new Processor()
							.ToSuccess()
							.SuccessStep(2)
							.SuccessStep(3)
							.SuccessStep(4);

		ClassicAssert.AreEqual(3,		response.Data.Step);
		ClassicAssert.AreEqual(9,		response.Data.Amount);
		ClassicAssert.AreEqual(true,	response.Result.Ok);
		ClassicAssert.AreEqual("",		response.Result.Message);
	}

	// [Test]
	// public void Test_Result_With_Steps_Failures()
	// {
	// 	var response = new Processor()
	// 						.ToSuccess()
	// 						.FailureStep()
	// 						.SuccessStep(3)
	// 						.SuccessStep(2);
	// 
	// 	ClassicAssert.AreEqual(3, response.Data.Step);
	// 	ClassicAssert.AreEqual(5, response.Data.Amount);
	// 	ClassicAssert.AreEqual(false, response.Result.Ok);
	// 	ClassicAssert.AreEqual("Step 1 failed.", response.Result.Message);
	// }
}


// ==================================================================================
// TestSteps class for tests above ^^^
// ==================================================================================

public static class TestSteps
{
	public static (Processor Data, Result Result) SuccessStep(this (Processor data, Result result) step, decimal addToAmount = 0)
	{
		step.data.Step++;
		step.data.Amount += addToAmount;

		return (step.data, step.result);
	}

	public static (Processor Data, Result Result) FailureStep(this (Processor data, Result result) step)
	{
		step.data.Step++;
		string failureMessage = $"Step {step.data.Step} failed.";
		step.data.Errors.Add(failureMessage);
		step.ToFailure(failureMessage);

		return (step.data, step.result);
	}
}



