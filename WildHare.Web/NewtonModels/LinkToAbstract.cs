using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: LinkToAbstract

namespace Me.Logic.Models
{
    public class LinkToAbstract
    {
        [Key]
		public int LinkId { get; set; }

		public int? WordId { get; set; }

		public int? ComplexId { get; set; }

		public int AbstractId { get; set; }

		public string XmlData { get; set; }

		public override string ToString()
		{
			return $"LinkId: {LinkId}";
		}
    }
}