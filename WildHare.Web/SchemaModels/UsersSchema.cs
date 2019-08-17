
using System;

namespace WildHare.Web.SchemaModels
{
	public class UsersSchema
	{
		public short Uid { get; set; }

		public string User_Name { get; set; }

		public DateTime Createdate { get; set; }

		public DateTime Updatedate { get; set; }
	}
}