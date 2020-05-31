using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: SymbolRule

namespace Me.Logic.Models
{
    public class SymbolRule
    {
        [Key]
		public int SymbolRuleId { get; set; }

		public int SortOrder { get; set; }

		[StringLength(50)]
		public string RuleGroup { get; set; }

		public bool Archived { get; set; }

		[StringLength(128)]
		public string Discriminator { get; set; }

		public override string ToString()
		{
			return $"SymbolRuleId: {SymbolRuleId}";
		}
    }
}