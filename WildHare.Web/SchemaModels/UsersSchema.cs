
using System;

namespace WildHare.Web.SchemaModels
{
	public class UsersSchema
	{
		public short uid { get; set; }

		public string user_name { get; set; }

		public DateTime createdate { get; set; }

		public DateTime updatedate { get; set; }
	}
}