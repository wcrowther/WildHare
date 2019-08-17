
using System;

namespace WildHare.Web.SchemaModels
{
	public class UserDefinedTypesSchema
	{
		public string Assembly_Name { get; set; }

		public string Udt_Name { get; set; }

		public object Version_Major { get; set; }

		public object Version_Minor { get; set; }

		public object Version_Build { get; set; }

		public object Version_Revision { get; set; }

		public object Culture_Info { get; set; }

		public object Public_Key { get; set; }

		public bool Is_Fixed_Length { get; set; }

		public short Max_Length { get; set; }

		public DateTime Create_Date { get; set; }

		public string Permission_Set_Desc { get; set; }
	}
}