
using System;

namespace WildHare.Web.Models
{
	public class Controls
	{
		public int ControlId { get; set; }

		public int LayoutId { get; set; }

		public nvarchar ControlName { get; set; }

		public varchar DataType { get; set; }

		public nchar MinValue { get; set; }

		public nchar MaxValue { get; set; }

		public nvarchar DefaultValue { get; set; }

		public datetime2 DateCreated { get; set; }
	}
}