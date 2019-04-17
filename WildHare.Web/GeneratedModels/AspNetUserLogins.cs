
using System;

namespace WildHare.Web.Models
{
	public class AspNetUserLogins
	{
		public nvarchar LoginProvider { get; set; }

		public nvarchar ProviderKey { get; set; }

		public nvarchar UserId { get; set; }
	}
}