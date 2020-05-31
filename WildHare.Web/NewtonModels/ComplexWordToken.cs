using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: ComplexWordToken

namespace Me.Logic.Models
{
    public class ComplexWordToken
    {
        [Key]
		public int TokenId { get; set; }

		public int ComplexId { get; set; }

		public override string ToString()
		{
			return $"TokenId: {TokenId}";
		}
    }
}