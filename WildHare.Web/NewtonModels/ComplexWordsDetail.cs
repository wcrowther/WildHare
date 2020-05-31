using System;
using System.ComponentModel.DataAnnotations;
    
// Generated from table: ComplexWordsDetail

namespace Me.Logic.Models
{
    public class ComplexWordsDetail
    {
        [Key]
		public int complexWordDetailID { get; set; }

		public int complexID { get; set; }

		public int orderNum { get; set; }

		public int wordID { get; set; }

		public override string ToString()
		{
			return $"complexWordDetailID: {complexWordDetailID}";
		}
    }
}