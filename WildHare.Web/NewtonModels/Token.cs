using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: Token

namespace Me.Logic.Models
{
    public class Token
    {
        [Key]
		public int TokenId { get; set; }

		[StringLength(128)]
		public string Discriminator { get; set; }

		public int? AbstractId { get; set; }

		public override string ToString()
		{
			return $"TokenId: {TokenId}";
		}
    }
}