using System;
using WildHare.Tests.Models.Generics;

namespace WildHare.Tests.Extensions;

 public static class TestSteps
 {
	  public static Result Start(this string s)
	  {
		   return s;
	  }

	  public static Result Step1(this Result result)
	  {
		   return result.Data.ToUpper();
	  }

	  public static Result Step2(this Result result)
	  {
		   return new Exception("Step2 Error");
	  }

	  public static Result Step3(this Result result)
	  {
		   return result.Success ? "Success" : new Exception("Step3 Error", result.Exception);
	  }
}
