
using System;

namespace WildHare.Web.SchemaModels
{
	public class DatabasesSchema
	{
		public string database_name { get; set; }

		public short dbid { get; set; }

		public DateTime create_date { get; set; }
	}
}