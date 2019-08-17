
using System;
using System.ComponentModel.DataAnnotations;

namespace WildHare.Web.Models
{
	public class Locations
	{
		public int LocationId { get; set; }

		public int TeventId { get; set; }

		[StringLength(100)]
		public string LocationName { get; set; }

		[StringLength(100)]
		public string Address { get; set; }

		[StringLength(100)]
		public string State { get; set; }

		[StringLength(50)]
		public string PostalCode { get; set; }

		[StringLength(50)]
		public string Country { get; set; }

		public DateTime DateCreated { get; set; }

	}
}