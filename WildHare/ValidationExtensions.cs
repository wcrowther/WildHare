using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WildHare.Extensions.DataAnnotations
{
    public static class Validator
    {
        public static ValidationResponse DataAnnotationsValidate(this object model, bool strict = true)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, context, results, true);

            return new ValidationResponse()
            {
                IsValid = isValid,
                Results = results
            };
        }

        public class ValidationResponse
        {
            public ValidationResponse()
            {
                Results = new List<ValidationResult>();
                IsValid = false;
            }

            public List<ValidationResult> Results { get; set; }

            public bool IsValid { get; set; }
        }
    }
}
