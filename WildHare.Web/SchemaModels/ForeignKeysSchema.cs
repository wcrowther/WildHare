
using System;

namespace WildHare.Web.SchemaModels
{
	public class ForeignKeysSchema
	{
		public string CONSTRAINT_CATALOG { get; set; }

		public string CONSTRAINT_SCHEMA { get; set; }

		public string CONSTRAINT_NAME { get; set; }

		public string TABLE_CATALOG { get; set; }

		public string TABLE_SCHEMA { get; set; }

		public string TABLE_NAME { get; set; }

		public string CONSTRAINT_TYPE { get; set; }

		public string IS_DEFERRABLE { get; set; }

		public string INITIALLY_DEFERRED { get; set; }
	}
}