using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: ControlValues

namespace WildHare.Web.Models
{
    public class ControlValue
    {
        [Key]
		public int ControlValueId { get; set; }

		public int TimelineId { get; set; }

		public int ControlId { get; set; }

		public int ActNumber { get; set; }

		public string Value { get; set; }

		public DateTime DateCreated { get; set; }

		public override string ToString()
		{
			return $"ControlValueId: {ControlValueId}";
		}
    }
}