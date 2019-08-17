
using System;

namespace WildHare.Web.SchemaModels
{
	public class IndexesSchema
	{
		public string Constraint_Catalog { get; set; }

		public string Constraint_Schema { get; set; }

		public string Constraint_Name { get; set; }

		public string Table_Catalog { get; set; }

		public string Table_Schema { get; set; }

		public string Table_Name { get; set; }

		public string Index_Name { get; set; }

		public string Type_Desc { get; set; }
	}
}