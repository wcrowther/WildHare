using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: ComplexWords

namespace Me.Logic.Models
{
    public class ComplexWord
    {
        [Key]
		public int complexID { get; set; }

		[StringLength(100)]
		public string complexWord { get; set; }

		public byte? memberCount { get; set; }

		[StringLength(50)]
		public string extra { get; set; }

		public override string ToString()
		{
			return $"complexID: {complexID}";
		}
    }
}