using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: Tags

namespace WildHare.Web.Models
{
    public class Tag
    {
        public int TagId { get; set; }

		[StringLength(50)]
		public string TagName { get; set; }

		public DateTime DateCreated { get; set; }

		public override string ToString()
		{
			return $"TagId: {TagId}";
		}
    }
}