
using System;

namespace WildHare.Web.SchemaModels
{
	public class ProceduresSchema
	{
		public string Specific_Catalog { get; set; }

		public string Specific_Schema { get; set; }

		public string Specific_Name { get; set; }

		public string Routine_Catalog { get; set; }

		public string Routine_Schema { get; set; }

		public string Routine_Name { get; set; }

		public string Routine_Type { get; set; }

		public DateTime Created { get; set; }

		public DateTime Last_Altered { get; set; }
	}
}