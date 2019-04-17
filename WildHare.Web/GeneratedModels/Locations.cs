
using System;

namespace WildHare.Web.Models
{
	public class Locations
	{
		public int LocationId { get; set; }

		public int TeventId { get; set; }

		public nvarchar LocationName { get; set; }

		public nvarchar Address { get; set; }

		public nvarchar State { get; set; }

		public nvarchar PostalCode { get; set; }

		public nvarchar Country { get; set; }

		public datetime2 DateCreated { get; set; }
	}
}