using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: Features

namespace Me.Logic.Models
{
    public class Feature
    {
        [Key]
		public int FeatureId { get; set; }

		[StringLength(50)]
		public string FeatureName { get; set; }

		[StringLength(200)]
		public string Title { get; set; }

		public string Description { get; set; }

		[StringLength(2000)]
		public string Resolution { get; set; }

		public decimal SortOrder { get; set; }

		public bool Archived { get; set; }

		public DateTime? Created { get; set; }

		public override string ToString()
		{
			return $"FeatureId: {FeatureId}";
		}
    }
}