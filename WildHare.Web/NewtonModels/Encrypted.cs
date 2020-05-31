using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: Encrypted

namespace Me.Logic.Models
{
    public class Encrypted
    {
        [Key]
		public int Id { get; set; }

		[StringLength(100)]
		public string Name { get; set; }

		[StringLength(100)]
		public string Encoded { get; set; }

		public override string ToString()
		{
			return $"Id: {Id}";
		}
    }
}