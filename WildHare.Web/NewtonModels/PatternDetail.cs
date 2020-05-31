using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: PatternDetail

namespace Me.Logic.Models
{
    public class PatternDetail
    {
        [Key]
		public int PatternDetailId { get; set; }

		public int PatternId { get; set; }

		public int AbstractId { get; set; }

		public int SortOrder { get; set; }

		public override string ToString()
		{
			return $"PatternDetailId: {PatternDetailId}";
		}
    }
}