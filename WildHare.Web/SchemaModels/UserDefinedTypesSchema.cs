
using System;

namespace WildHare.Web.SchemaModels
{
	public class UserDefinedTypesSchema
	{
		public string assembly_name { get; set; }

		public string udt_name { get; set; }

		public object version_major { get; set; }

		public object version_minor { get; set; }

		public object version_build { get; set; }

		public object version_revision { get; set; }

		public object culture_info { get; set; }

		public object public_key { get; set; }

		public bool is_fixed_length { get; set; }

		public short max_length { get; set; }

		public DateTime Create_Date { get; set; }

		public string Permission_set_desc { get; set; }
	}
}