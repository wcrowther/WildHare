
using System;

namespace WildHare.Web.SchemaModels
{
	public class IndexesSchema
	{
		public string constraint_catalog { get; set; }

		public string constraint_schema { get; set; }

		public string constraint_name { get; set; }

		public string table_catalog { get; set; }

		public string table_schema { get; set; }

		public string table_name { get; set; }

		public string index_name { get; set; }

		public string type_desc { get; set; }
	}
}