using System;
using System.ComponentModel.DataAnnotations;

// Generated from table: Controls

namespace WildHare.Web.Models
{
    public class Control
    {
        [Key]
		public int ControlId { get; set; }

		public int LayoutId { get; set; }

		[StringLength(50)]
		public string ControlName { get; set; }

		[StringLength(20)]
		public string DataType { get; set; }

		[StringLength(10)]
		public string MinValue { get; set; }

		[StringLength(10)]
		public string MaxValue { get; set; }

		[StringLength(1000)]
		public string DefaultValue { get; set; }

		public int TabIndex { get; set; }

		public DateTime DateCreated { get; set; }

		public override string ToString()
		{
			return $"ControlId: {ControlId}";
		}
    }
}