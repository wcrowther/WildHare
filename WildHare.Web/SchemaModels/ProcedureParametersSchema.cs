
using System;

namespace WildHare.Web.SchemaModels
{
	public class ProcedureParametersSchema
	{
		public string SPECIFIC_CATALOG { get; set; }

		public string SPECIFIC_SCHEMA { get; set; }

		public string SPECIFIC_NAME { get; set; }

		public int ORDINAL_POSITION { get; set; }

		public string PARAMETER_MODE { get; set; }

		public string IS_RESULT { get; set; }

		public string AS_LOCATOR { get; set; }

		public string PARAMETER_NAME { get; set; }

		public string DATA_TYPE { get; set; }

		public int CHARACTER_MAXIMUM_LENGTH { get; set; }

		public int CHARACTER_OCTET_LENGTH { get; set; }

		public string COLLATION_CATALOG { get; set; }

		public string COLLATION_SCHEMA { get; set; }

		public string COLLATION_NAME { get; set; }

		public string CHARACTER_SET_CATALOG { get; set; }

		public string CHARACTER_SET_SCHEMA { get; set; }

		public string CHARACTER_SET_NAME { get; set; }

		public byte NUMERIC_PRECISION { get; set; }

		public short NUMERIC_PRECISION_RADIX { get; set; }

		public int NUMERIC_SCALE { get; set; }

		public short DATETIME_PRECISION { get; set; }

		public string INTERVAL_TYPE { get; set; }

		public short INTERVAL_PRECISION { get; set; }
	}
}