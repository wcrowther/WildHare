
using System;

namespace WildHare.Web.SchemaModels
{
	public class RestrictionsSchema
	{
		public string Collectionname { get; set; }

		public string Restrictionname { get; set; }

		public string Parametername { get; set; }

		public string Restrictiondefault { get; set; }

		public int Restrictionnumber { get; set; }
	}
}