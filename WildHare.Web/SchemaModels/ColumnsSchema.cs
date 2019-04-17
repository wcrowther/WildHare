
using System;

namespace WildHare.Web.SchemaModels
{
	public class ColumnsSchema
	{
		public string TABLE_CATALOG { get; set; }

		public string TABLE_SCHEMA { get; set; }

		public string TABLE_NAME { get; set; }

		public string COLUMN_NAME { get; set; }

		public int ORDINAL_POSITION { get; set; }

		public string COLUMN_DEFAULT { get; set; }

		public string IS_NULLABLE { get; set; }

		public string DATA_TYPE { get; set; }

		public int CHARACTER_MAXIMUM_LENGTH { get; set; }

		public int CHARACTER_OCTET_LENGTH { get; set; }

		public byte NUMERIC_PRECISION { get; set; }

		public short NUMERIC_PRECISION_RADIX { get; set; }

		public int NUMERIC_SCALE { get; set; }

		public short DATETIME_PRECISION { get; set; }

		public string CHARACTER_SET_CATALOG { get; set; }

		public string CHARACTER_SET_SCHEMA { get; set; }

		public string CHARACTER_SET_NAME { get; set; }

		public string COLLATION_CATALOG { get; set; }

		public bool IS_SPARSE { get; set; }

		public bool IS_COLUMN_SET { get; set; }

		public bool IS_FILESTREAM { get; set; }

		public override string ToString()
		{
			return $"{TABLE_SCHEMA}.{TABLE_NAME} : {COLUMN_NAME} ({DATA_TYPE} {(IS_NULLABLE == "YES" ? "Nullable" : "")})";	
		}
	}
}