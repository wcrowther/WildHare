using AngleSharp.Io;
using NUnit.Framework;
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

		Assert.AreEqual("Basic String",	response.Data);
		Assert.AreEqual("",				response.Result.Message);
		Assert.AreEqual(true,			response.Result.Ok);
	}

	[Test]
	public void Test_Result_String_Basic2()
	{
		var response = "Basic String".ToSuccess();

		Assert.AreEqual("Basic String", response.Data);
		Assert.AreEqual("",				response.Result.Message);
		Assert.AreEqual(true,			response.Result.Ok);
	}

	[Test]
	public void Test_Result_String_Null()
	{
		string nullStr	= null;
		var response		= nullStr.ToSuccess("");

		Assert.AreEqual("",		response.Data);
		Assert.AreEqual("",		response.Result.Message);
		Assert.AreEqual(true,	response.Result.Ok);
	}

	[Test]
	public void Test_Result_String_Null2()
	{
		string nullStr = null;
		var response = nullStr.ToSuccess();

		Assert.AreEqual(null,	response.Data);
		Assert.AreEqual("",		response.Result.Message);
		Assert.AreEqual(true,	response.Result.Ok);
	}

	[Test]
	public void Test_List_ToSuccess_Is_Null()
	{
		List<Item> list = null;
		var response = list.ToSuccess([]); // must assign a default

		Assert.AreEqual(0,		response.Data.Count );
		Assert.AreEqual("",		response.Result.Message);
		Assert.AreEqual(true,	response.Result.Ok);
	}


	[Test]
	public void Test_Result_String_Failure()
	{
		var response = "InvalidString".ToFailure("String is not valid.");

		Assert.AreEqual("InvalidString",		response.Data);
		Assert.AreEqual("String is not valid.",	response.Result.Message);
		Assert.AreEqual(false,					response.Result.Ok);
	}

	[Test]
	public void Test_Result_List_Basic()
	{
		var response = new List<string>{ "one", "two", "three" }
						.ToResult();	

		Assert.AreEqual(3,		response.Data.Count);
		Assert.AreEqual("",		response.Result.Message);
		Assert.AreEqual(true,	response.Result.Ok);
	}

	[Test]
	public void Test_Result_String_ToSuccess_With_Null_Guard()
	{
		List<string> val	= null;
		var response			= val.ToSuccess([]);

		Assert.AreEqual(0,		response.Data.Count);
		Assert.AreEqual("",		response.Result.Message);
		Assert.AreEqual(true,	response.Result.Ok);
	}

	[Test]
	public void Test_Result_String_ToResult()
	{
		List<string> val = null;
		var response = val.ToResult();

		Assert.AreEqual(null, response.Data);
		Assert.AreEqual("ToResult.data is null.", response.Result.Message);
		Assert.AreEqual(false, response.Result.Ok);
	}

	[Test]
	public void Test_Result_With_func_ToResult()
	{
		List<string> val = ["One", "Two"];
		var response = val.ToResult(v => v.Count > 1);

		Assert.AreEqual(2,		response.Data.Count);
		Assert.AreEqual("",		response.Result.Message);
		Assert.AreEqual(true,	response.Result.Ok);

		var secondResponse = response.Result.Ok ? response.Data[1] : response.Data[0];

		Assert.AreEqual("Two", secondResponse);
	}

	[Test]
	public void Test_Result_With_Steps_Success()
	{
		var response =  new Processor()
							.ToSuccess()
							.SuccessStep(2)
							.SuccessStep(3)
							.SuccessStep(4);

		Assert.AreEqual(3,		response.Data.Step);
		Assert.AreEqual(9,		response.Data.Amount);
		Assert.AreEqual(true,	response.Result.Ok);
		Assert.AreEqual("",		response.Result.Message);
	}

	[Test]
	public void Test_Result_With_Steps_Failures()
	{
		var response = new Processor()
							.ToSuccess()
							.FailureStep()
							.SuccessStep(3)
							.SuccessStep(2);

		Assert.AreEqual(3, response.Data.Step);
		Assert.AreEqual(5, response.Data.Amount);
		Assert.AreEqual(false, response.Result.Ok);
		Assert.AreEqual("Step 1 failed.", response.Result.Message);
	}
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



