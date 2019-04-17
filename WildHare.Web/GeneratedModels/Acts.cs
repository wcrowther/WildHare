
using System;

namespace WildHare.Web.Models
{
	public class Acts
	{
		public int ActId { get; set; }

		public int TimelineId { get; set; }

		public int ActNumber { get; set; }

		public nvarchar ActText { get; set; }

		public bit Hidden { get; set; }

		public datetime2 DateCreated { get; set; }
	}
}