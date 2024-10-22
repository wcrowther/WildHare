using NUnit.Framework;
using System.Text;
using WildHare.Extensions;

namespace WildHare.Tests;

[TestFixture]
public class StringBuilderExtensionsTests
{
	 [Test]
	 public void Test_StringBuilder_AppendIf_With_False()
	 {
		  var sb			  =	new StringBuilder("Test");
		  string textToAdd	  = "Add";

		  sb.AppendIf(false, textToAdd);

		  Assert.AreEqual("Test", sb.ToString());
	 }

	 [Test]
	 public void Test_StringBuilder_AppendIf_With_True()
	 {
		  var sb			  = new StringBuilder("Test");
		  string textToAdd	  = "Add";

		  sb.AppendIf(true, textToAdd);

		  Assert.AreEqual("TestAdd", sb.ToString());
	 }
}