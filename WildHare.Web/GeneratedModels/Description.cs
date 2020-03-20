using System;
using System.ComponentModel.DataAnnotations;

// Generated from table: Description

namespace WildHare.Web.Models
{
    public class Description
    {
        [Key]
		public int DescriptionId { get; set; }

		public int TimelineId { get; set; }

		[StringLength(100)]
		public string Headline { get; set; }

		public DateTime DateCreated { get; set; }

		public override string ToString()
		{
			return $"DescriptionId: {DescriptionId}";
		}
    }
}