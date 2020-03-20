using System;
using System.ComponentModel.DataAnnotations;

// Generated from table: Locations

namespace WildHare.Web.Models
{
    public class Location
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

		public override string ToString()
		{
			return $"LocationId: {LocationId}";
		}
    }
}