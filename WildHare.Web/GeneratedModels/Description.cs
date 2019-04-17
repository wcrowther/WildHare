
using System;

namespace WildHare.Web.Models
{
	public class Description
	{
		public int DescriptionId { get; set; }

		public int TimelineId { get; set; }

		public varchar Headline { get; set; }

		public datetime2 DateCreated { get; set; }
	}
}