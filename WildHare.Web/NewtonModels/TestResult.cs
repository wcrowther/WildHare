using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: TestResult

namespace Me.Logic.Models
{
    public class TestResult
    {
        [Key]
		public int TestResultId { get; set; }

		[StringLength(300)]
		public string Result { get; set; }

		public int TestRunId { get; set; }

		public int? TestId { get; set; }

		public bool Passed { get; set; }

		public DateTime BeginTest { get; set; }

		public DateTime EndTest { get; set; }

		public int SortOrder { get; set; }

		public int? ExceptionId { get; set; }

		public override string ToString()
		{
			return $"TestResultId: {TestResultId}";
		}
    }
}