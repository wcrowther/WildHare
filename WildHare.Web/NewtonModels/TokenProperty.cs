using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: TokenProperty

namespace Me.Logic.Models
{
    public class TokenProperty
    {
        [Key]
		public int TokenPropertyId { get; set; }

		public int TokenId { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

		public override string ToString()
		{
			return $"TokenPropertyId: {TokenPropertyId}";
		}
    }
}