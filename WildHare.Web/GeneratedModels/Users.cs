
using System;

namespace WildHare.Web.Models
{
	public class Users
	{
		public int UserId { get; set; }

		public nvarchar UserName { get; set; }

		public nvarchar FirstName { get; set; }

		public nvarchar LastName { get; set; }

		public datetime2 DateCreated { get; set; }
	}
}