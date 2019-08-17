
using System;

namespace WildHare.Web.SchemaModels
{
	public class ForeignKeysSchema
	{
		public string Constraint_Catalog { get; set; }

		public string Constraint_Schema { get; set; }

		public string Constraint_Name { get; set; }

		public string Table_Catalog { get; set; }

		public string Table_Schema { get; set; }

		public string Table_Name { get; set; }

		public string Constraint_Type { get; set; }

		public string Is_Deferrable { get; set; }

		public string Initially_Deferred { get; set; }
	}
}