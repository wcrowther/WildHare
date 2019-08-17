
using System;

namespace WildHare.Web.SchemaModels
{
	public class IndexColumnsSchema
	{
		public string Constraint_Catalog { get; set; }

		public string Constraint_Schema { get; set; }

		public string Constraint_Name { get; set; }

		public string Table_Catalog { get; set; }

		public string Table_Schema { get; set; }

		public string Table_Name { get; set; }

		public string Column_Name { get; set; }

		public int Ordinal_Position { get; set; }

		public byte Keytype { get; set; }

		public string Index_Name { get; set; }
	}
}