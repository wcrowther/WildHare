
using System;

namespace WildHare.Web.SchemaModels
{
	public class IndexColumnsSchema
	{
		public string constraint_catalog { get; set; }

		public string constraint_schema { get; set; }

		public string constraint_name { get; set; }

		public string table_catalog { get; set; }

		public string table_schema { get; set; }

		public string table_name { get; set; }

		public string column_name { get; set; }

		public int ordinal_position { get; set; }

		public byte KeyType { get; set; }

		public string index_name { get; set; }
	}
}