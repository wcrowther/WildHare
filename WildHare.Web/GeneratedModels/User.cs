using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: Users

namespace WildHare.Web.Models
{
    public class User
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

		public override string ToString()
		{
			return $"UserId: {UserId}";
		}
    }
}