
using System;

namespace WildHare.Web.Models
{
	public class Tags
	{
		public int TagId { get; set; }

		public nvarchar TagName { get; set; }

		public datetime2 DateCreated { get; set; }
	}
}