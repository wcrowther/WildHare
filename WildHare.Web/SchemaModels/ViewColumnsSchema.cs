
using System;

namespace WildHare.Web.SchemaModels
{
	public class ViewColumnsSchema
	{
		public string View_Catalog { get; set; }

		public string View_Schema { get; set; }

		public string View_Name { get; set; }

		public string Table_Catalog { get; set; }

		public string Table_Schema { get; set; }

		public string Table_Name { get; set; }

		public string Column_Name { get; set; }
	}
}