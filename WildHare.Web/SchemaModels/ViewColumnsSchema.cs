
using System;

namespace WildHare.Web.SchemaModels
{
	public class ViewColumnsSchema
	{
		public string VIEW_CATALOG { get; set; }

		public string VIEW_SCHEMA { get; set; }

		public string VIEW_NAME { get; set; }

		public string TABLE_CATALOG { get; set; }

		public string TABLE_SCHEMA { get; set; }

		public string TABLE_NAME { get; set; }

		public string COLUMN_NAME { get; set; }
	}
}