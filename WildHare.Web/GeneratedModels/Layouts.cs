
using System;

namespace WildHare.Web.Models
{
	public class Layouts
	{
		public int LayoutId { get; set; }

		public nvarchar LayoutName { get; set; }

		public nvarchar LayoutDescription { get; set; }

		public datetime2 DateCreated { get; set; }

		public nvarchar Template { get; set; }
	}
}