using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: Categories

namespace Me.Logic.Models
{
    public class Category
    {
        [Key]
		public int CategoryID { get; set; }

		[StringLength(15)]
		public string CategoryName { get; set; }

		public string Description { get; set; }

		public override string ToString()
		{
			return $"CategoryID: {CategoryID}";
		}
    }
}