
using System;

namespace WildHare.Web.Models
{
	public class AspNetUsers
	{
		public nvarchar Id { get; set; }

		public nvarchar Hometown { get; set; }

		public nvarchar Email { get; set; }

		public bit EmailConfirmed { get; set; }

		public nvarchar PasswordHash { get; set; }

		public nvarchar SecurityStamp { get; set; }

		public nvarchar PhoneNumber { get; set; }

		public bit PhoneNumberConfirmed { get; set; }

		public bit TwoFactorEnabled { get; set; }

		public datetime LockoutEndDateUtc { get; set; }

		public bit LockoutEnabled { get; set; }

		public int AccessFailedCount { get; set; }

		public nvarchar UserName { get; set; }
	}
}