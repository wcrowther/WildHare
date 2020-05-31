using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: TestRun

namespace Me.Logic.Models
{
    public class TestRun
    {
        [Key]
		public int TestRunId { get; set; }

		public DateTime Created { get; set; }

		public int TestsPassed { get; set; }

		public double TestsTotalMilliseconds { get; set; }

		public override string ToString()
		{
			return $"TestRunId: {TestRunId}";
		}
    }
}