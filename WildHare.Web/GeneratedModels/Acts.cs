
using System;
using System.ComponentModel.DataAnnotations;

                // For table: Acts

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

		[StringLength(50)]
		public string ActSet { get; set; }

		[StringLength(50)]
		public string ActSubSet { get; set; }

		public bool Hidden { get; set; }

		public DateTime DateCreated { get; set; }

		public override string ToString()
		{
			return $"ActId: {ActId}";
		}
	}
}