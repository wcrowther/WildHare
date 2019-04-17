
using System;

namespace WildHare.Web.Models
{
	public class ControlValues
	{
		public int ControlValueId { get; set; }

		public int TimelineId { get; set; }

		public int ControlId { get; set; }

		public int ActNumber { get; set; }

		public nvarchar Value { get; set; }

		public datetime2 DateCreated { get; set; }
	}
}