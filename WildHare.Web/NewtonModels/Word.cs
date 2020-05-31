using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: Word

namespace Me.Logic.Models
{
    public class Word
    {
        [Key]
		public int WordId { get; set; }

		[StringLength(100)]
		public string WordName { get; set; }

		public decimal? Commonality { get; set; }

		[StringLength(50)]
		public string PartOfSpeech { get; set; }

		public bool Archived { get; set; }

		public override string ToString()
		{
			return $"WordId: {WordId}";
		}
    }
}
