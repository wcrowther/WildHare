using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: TestCaseType

namespace Me.Logic.Models
{
    public class TestCaseType
    {
        [Key]
		public int TestCaseTypeId { get; set; }

		[StringLength(50)]
		public string Name { get; set; }

		public int SortOrder { get; set; }

		public DateTime Created { get; set; }

		public override string ToString()
		{
			return $"TestCaseTypeId: {TestCaseTypeId}";
		}
    }
}