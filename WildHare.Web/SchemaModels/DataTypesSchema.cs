
using System;

namespace WildHare.Web.SchemaModels
{
	public class DataTypesSchema
	{
		public string Typename { get; set; }

		public int Providerdbtype { get; set; }

		public long Columnsize { get; set; }

		public string Createformat { get; set; }

		public string Createparameters { get; set; }

		public string Datatype { get; set; }

		public bool Isautoincrementable { get; set; }

		public bool Isbestmatch { get; set; }

		public bool Iscasesensitive { get; set; }

		public bool Isfixedlength { get; set; }

		public bool Isfixedprecisionscale { get; set; }

		public bool Islong { get; set; }

		public bool Isnullable { get; set; }

		public bool Issearchable { get; set; }

		public bool Issearchablewithlike { get; set; }

		public bool Isunsigned { get; set; }

		public short Maximumscale { get; set; }

		public short Minimumscale { get; set; }

		public bool Isconcurrencytype { get; set; }

		public bool Isliteralsupported { get; set; }

		public string Literalprefix { get; set; }

		public string Literalsuffix { get; set; }
	}
}