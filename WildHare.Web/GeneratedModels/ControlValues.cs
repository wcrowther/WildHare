
using System;
using System.ComponentModel.DataAnnotations;

namespace WildHare.Web.Models
{
	public class ControlValues
	{
		[Key]
		public int ControlValueId { get; set; }

		public int TimelineId { get; set; }

		public int ControlId { get; set; }

		public int ActNumber { get; set; }

		public string Value { get; set; }

		public DateTime DateCreated { get; set; }

	}
}