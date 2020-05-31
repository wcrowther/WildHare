using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: LinkProperties

namespace Me.Logic.Models
{
    public class LinkPropertie
    {
        [Key]
		public int LinkPropertyId { get; set; }

		public int LinkId { get; set; }

		[StringLength(50)]
		public string Name { get; set; }

		[StringLength(50)]
		public string Value { get; set; }

		public DateTime Created { get; set; }

		public override string ToString()
		{
			return $"LinkPropertyId: {LinkPropertyId}";
		}
    }
}