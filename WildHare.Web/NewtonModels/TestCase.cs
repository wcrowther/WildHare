using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: TestCase

namespace Me.Logic.Models
{
    public class TestCase
    {
        [Key]
		public int TestId { get; set; }

		[StringLength(2000)]
		public string RawText { get; set; }

		[StringLength(1000)]
		public string Comments { get; set; }

		public decimal SortOrder { get; set; }

		public bool Active { get; set; }

		[StringLength(300)]
		public string ExpectedAnswer { get; set; }

		public int? TestCaseTypeId { get; set; }

		public DateTime Created { get; set; }

		public override string ToString()
		{
			return $"TestId: {TestId}";
		}
    }
}