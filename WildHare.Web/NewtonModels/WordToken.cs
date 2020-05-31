using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: WordToken

namespace Me.Logic.Models
{
    public class WordToken
    {
        [Key]
		public int TokenId { get; set; }

		public int WordId { get; set; }

		public override string ToString()
		{
			return $"TokenId: {TokenId}";
		}
    }
}