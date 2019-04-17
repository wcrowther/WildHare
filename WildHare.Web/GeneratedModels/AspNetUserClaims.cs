
using System;

namespace WildHare.Web.Models
{
	public class AspNetUserClaims
	{
		public int Id { get; set; }

		public nvarchar UserId { get; set; }

		public nvarchar ClaimType { get; set; }

		public nvarchar ClaimValue { get; set; }
	}
}