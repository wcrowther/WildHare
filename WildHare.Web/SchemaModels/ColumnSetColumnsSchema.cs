
using System;

namespace WildHare.Web.SchemaModels
{
	public class ColumnSetColumnsSchema
	{
		public string Table_Catalog { get; set; }

		public string Table_Schema { get; set; }

		public string Table_Name { get; set; }

		public string Column_Name { get; set; }

		public int Ordinal_Position { get; set; }

		public string Column_Default { get; set; }

		public string Is_Nullable { get; set; }

		public string Data_Type { get; set; }

		public int Character_Maximum_Length { get; set; }

		public int Character_Octet_Length { get; set; }

		public byte Numeric_Precision { get; set; }

		public short Numeric_Precision_Radix { get; set; }

		public int Numeric_Scale { get; set; }

		public short Datetime_Precision { get; set; }

		public string Character_Set_Catalog { get; set; }

		public string Character_Set_Schema { get; set; }

		public string Character_Set_Name { get; set; }

		public string Collation_Catalog { get; set; }

		public bool Is_Sparse { get; set; }

		public bool Is_Column_Set { get; set; }

		public bool Is_Filestream { get; set; }
	}
}