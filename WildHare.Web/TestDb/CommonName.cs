using System;
using System.ComponentModel.DataAnnotations;

// Generated from table: CommonNames

namespace Me.Logic.Models
{
    public class CommonName
    {
        [Key]
		public int NameId { get; set; }

		[StringLength(50)]
		public string Name { get; set; }

		public bool CanBeMaleFirstName { get; set; }

		public bool CanBeFemaleFirstName { get; set; }

		public DateTime DateCreated { get; set; }

		public override string ToString()
		{
			return $"NameId: {NameId}";
		}
    }
}