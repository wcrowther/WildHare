
using System;
using System.ComponentModel.DataAnnotations;

namespace WildHare.Web.Models
{
	public class Acts
	{
		[Key]
		public int ActId { get; set; }

		public int TimelineId { get; set; }

		public int ActNumber { get; set; }

		[StringLength(200)]
		public string ActText { get; set; }

		public bool Hidden { get; set; }

		public DateTime DateCreated { get; set; }

	}
}