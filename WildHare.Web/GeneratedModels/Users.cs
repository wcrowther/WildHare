
using System;
using System.ComponentModel.DataAnnotations;

namespace WildHare.Web.Models
{
	public class Users
	{
		[Key]
		public int UserId { get; set; }

		[StringLength(100)]
		public string UserName { get; set; }

		[StringLength(50)]
		public string FirstName { get; set; }

		[StringLength(50)]
		public string LastName { get; set; }

		public DateTime DateCreated { get; set; }

	}
}