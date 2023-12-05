using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using WildHare.Tests.Models;
using WildHare.Extensions.DataAnnotations;
using static System.Environment;

namespace WildHare.Tests
{
    [TestFixture]
    public class ValidationExtensionsTests
    {
		[Test]
		public void Test_Invalid_Validation()
		{
            var account = getInvalidAccount();

            // ----------------------------------------------------

            var response1 = account.DataAnnotationsValidate();

            Assert.IsFalse(response1.IsValid);
            Assert.AreEqual(7, response1.Results.Count);
            Assert.AreEqual("The AccountName field is required.", response1.Results.First().ErrorMessage);

            // ----------------------------------------------------

            account.AccountName = "Test Account";
            account.Email = "InvalidEmail.com";

            var response2 = account.DataAnnotationsValidate();

            Assert.IsFalse(response2.IsValid);
            Assert.AreEqual(6, response2.Results.Count);
            Assert.AreEqual("The Email field is not a valid e-mail address.", response2.Results.First().ErrorMessage);
            
            // ----------------------------------------------------

            account.Email = "ValidEmail@Email.com";
            account.PhoneNumber = "4443337777";
            account.StreetAddress = "123 Test Drive";
            account.City = "Test City";
            account.State = "Georgia";
            account.PostalCode = "GA 1234567890";

            var response3 = account.DataAnnotationsValidate();

            Assert.IsFalse(response3.IsValid);
            Assert.AreEqual(2, response3.Results.Count);
            Assert.AreEqual("The field State must be a string or array type with a maximum length of '2'.", response3.Results.First().ErrorMessage);

            // ----------------------------------------------------

            account.State = "GA";
            account.PostalCode = "30024-1234";
            var response4 = account.DataAnnotationsValidate();

            Assert.IsTrue(response4.IsValid);
            Assert.AreEqual(0, response4.Results.Count);
        }

        [Test]
        public void Test_Valid_Validation()
        {
            var account = getValidAccount();
            var response = account.DataAnnotationsValidate();

            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(0, response.Results.Count);
        }

        [Test]
        public void Test_Object_With_No_DataAnnotations()
        {
            var person = new Person();
            var response = person.DataAnnotationsValidate();

            Assert.IsTrue(response.IsValid);
            Assert.AreEqual(0, response.Results.Count);
        }

        // ============================================================

        private Account getInvalidAccount()
        {
            return new Account(); 
        }

        private Account getValidAccount()
        {
            return new Account 
            { 
                AccountId       = 1, 
                AccountName     = "Test Account",
                Email           = "Test@test.com",
                PhoneNumber     = "404-888-9999",
                StreetAddress   = "123 Test Drive",
                City            = "Test City",
                State           = "GA",
                PostalCode      = "30001",
            };
        }
    }
}
