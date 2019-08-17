
using System;
using System.ComponentModel.DataAnnotations;

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

	}
}