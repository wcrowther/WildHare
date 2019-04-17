
using System;

namespace WildHare.Web.SchemaModels
{
	public class RestrictionsSchema
	{
		public string CollectionName { get; set; }

		public string RestrictionName { get; set; }

		public string ParameterName { get; set; }

		public string RestrictionDefault { get; set; }

		public int RestrictionNumber { get; set; }
	}
}