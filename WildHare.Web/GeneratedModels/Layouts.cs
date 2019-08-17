
using System;
using System.ComponentModel.DataAnnotations;

namespace WildHare.Web.Models
{
	public class Layouts
	{
		[Key]
		public int LayoutId { get; set; }

		[StringLength(100)]
		public string LayoutName { get; set; }

		[StringLength(300)]
		public string LayoutDescription { get; set; }

		public DateTime DateCreated { get; set; }

		[StringLength(50)]
		public string Template { get; set; }

	}
}