
using System;

namespace WildHare.Web.Models
{
	public class Timelines
	{
		public int TimelineId { get; set; }

		public int TeventId { get; set; }

		public nvarchar TimelineDescription { get; set; }

		public int LayoutId { get; set; }

		public datetime DateCreated { get; set; }
	}
}