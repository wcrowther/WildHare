using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: Tevents

namespace WildHare.Web.Models
{
    public class Tevent
    {
        [Key]
		public int TeventId { get; set; }

		public Guid TeventGuid { get; set; }

		[StringLength(100)]
		public string TeventName { get; set; }

		[StringLength(500)]
		public string TeventSummary { get; set; }

		public string TeventInfo { get; set; }

		public DateTime? StartDate { get; set; }

		public int? OwnerUserId { get; set; }

		[StringLength(200)]
		public string TeaserSrc { get; set; }

		public DateTime DateCreated { get; set; }

		public override string ToString()
		{
			return $"TeventId: {TeventId}";
		}
    }
}