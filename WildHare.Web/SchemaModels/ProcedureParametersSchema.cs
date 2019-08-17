
using System;

namespace WildHare.Web.SchemaModels
{
	public class ProcedureParametersSchema
	{
		public string Specific_Catalog { get; set; }

		public string Specific_Schema { get; set; }

		public string Specific_Name { get; set; }

		public int Ordinal_Position { get; set; }

		public string Parameter_Mode { get; set; }

		public string Is_Result { get; set; }

		public string As_Locator { get; set; }

		public string Parameter_Name { get; set; }

		public string Data_Type { get; set; }

		public int Character_Maximum_Length { get; set; }

		public int Character_Octet_Length { get; set; }

		public string Collation_Catalog { get; set; }

		public string Collation_Schema { get; set; }

		public string Collation_Name { get; set; }

		public string Character_Set_Catalog { get; set; }

		public string Character_Set_Schema { get; set; }

		public string Character_Set_Name { get; set; }

		public byte Numeric_Precision { get; set; }

		public short Numeric_Precision_Radix { get; set; }

		public int Numeric_Scale { get; set; }

		public short Datetime_Precision { get; set; }

		public string Interval_Type { get; set; }

		public short Interval_Precision { get; set; }
	}
}