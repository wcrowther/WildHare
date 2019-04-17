
using System;

namespace WildHare.Web.Models
{
	public class sysdiagrams
	{
		public nvarchar name { get; set; }

		public int principal_id { get; set; }

		public int diagram_id { get; set; }

		public int version { get; set; }

		public varbinary definition { get; set; }
	}
}