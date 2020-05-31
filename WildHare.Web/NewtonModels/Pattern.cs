using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: Pattern

namespace Me.Logic.Models
{
    public class Pattern
    {
        [Key]
		public int PatternId { get; set; }

		[StringLength(50)]
		public string PatternName { get; set; }

		[StringLength(50)]
		public string Match { get; set; }

		[StringLength(500)]
		public string Note { get; set; }

		public int MemberCount { get; set; }

		public int SortOrder { get; set; }

		public bool Archived { get; set; }

		public override string ToString()
		{
			return $"PatternId: {PatternId}";
		}
    }
}