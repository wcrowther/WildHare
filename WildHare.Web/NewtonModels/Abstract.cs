using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: Abstract

namespace Me.Logic.Models
{
    public class Abstract
    {
        [Key]
		public int AbstractId { get; set; }

		[StringLength(50)]
		public string PartOfSpeech { get; set; }

		[StringLength(100)]
		public string Concept { get; set; }

		[StringLength(100)]
		public string ConceptType { get; set; }

		[StringLength(100)]
		public string Value { get; set; }

		public int? ValueType { get; set; }

		public override string ToString()
		{
			return $"AbstractId: {AbstractId}";
		}
    }
}