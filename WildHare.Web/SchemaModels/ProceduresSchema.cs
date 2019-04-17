
using System;

namespace WildHare.Web.SchemaModels
{
	public class ProceduresSchema
	{
		public string SPECIFIC_CATALOG { get; set; }

		public string SPECIFIC_SCHEMA { get; set; }

		public string SPECIFIC_NAME { get; set; }

		public string ROUTINE_CATALOG { get; set; }

		public string ROUTINE_SCHEMA { get; set; }

		public string ROUTINE_NAME { get; set; }

		public string ROUTINE_TYPE { get; set; }

		public DateTime CREATED { get; set; }

		public DateTime LAST_ALTERED { get; set; }
	}
}