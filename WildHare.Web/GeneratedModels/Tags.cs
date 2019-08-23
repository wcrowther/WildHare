
using System;
using System.ComponentModel.DataAnnotations;

                // For table: Tags

namespace WildHare.Web.Models
{
	public class Tags
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