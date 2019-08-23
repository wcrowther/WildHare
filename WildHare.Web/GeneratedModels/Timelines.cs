
using System;
using System.ComponentModel.DataAnnotations;

                // For table: Timelines

namespace WildHare.Web.Models
{
	public class Timelines
	{
		[Key]
		public int TimelineId { get; set; }

		public int TeventId { get; set; }

		[StringLength(100)]
		public string TimelineDescription { get; set; }

		public int LayoutId { get; set; }

		public DateTime DateCreated { get; set; }

		public override string ToString()
		{
			return $"TimelineId: {TimelineId}";
		}
	}
}