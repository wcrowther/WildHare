using System.Data;
using System.Data.SqlClient;
using System.Text;
using WildHare.Extensions;

namespace WildHare.Web
{
	public static class CodeGenSchema
    {
		/* ==========================================================================
         * DIRECTIONS
         * 
         * PLACE FOLLOWING LINE OF CODE SOMEWHERE IT WILL BE RUN ON COMPLIE
         * OR ALTERNATIVELY RUN IN THE IMMEDIATE WINDOW:
         * 
           WildHare.Web.CodeGenSchema.Init();
        ========================================================================== */

		// FOR SCHEMA DOCS SEE: https: //docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-schema-collections

		private static readonly string namespaceRoot = "WildHare.Web";
		private static readonly string outputDir = @"C:\Code\Trunk\WildHare\WildHare.Web\SchemaModels\";


		public static SqlConnection GetConnection()
		{
			string sqlConnString = "Data Source=Behemoth;Initial Catalog=Tevents;Connect Timeout=30;Persist Security Info=True;MultipleActiveResultSets=True;User ID=Tevents_User;Password=!london!";
			return new SqlConnection(sqlConnString);
		}

		public static string Init()
        {
			CreateSqlServerSchemaModel("MetaDataCollections", true);
			CreateSqlServerSchemaModel("Columns", false);
			CreateSqlServerSchemaModel("AllColumns", true);
			CreateSqlServerSchemaModel("ColumnSetColumns", true);
			CreateSqlServerSchemaModel("StructuredTypeMembers", true);
			CreateSqlServerSchemaModel("DataTypes", true);
			CreateSqlServerSchemaModel("Restrictions", true);
			CreateSqlServerSchemaModel("ReservedWords", true);
			CreateSqlServerSchemaModel("Databases", true);
			CreateSqlServerSchemaModel("ForeignKeys", true);
			CreateSqlServerSchemaModel("Indexes", true);
			CreateSqlServerSchemaModel("IndexColumns", true);
			CreateSqlServerSchemaModel("Procedures", true);
			CreateSqlServerSchemaModel("ProcedureParameters", true);
			CreateSqlServerSchemaModel("Tables", true);
			CreateSqlServerSchemaModel("Users", true);
			CreateSqlServerSchemaModel("Views", true);
			CreateSqlServerSchemaModel("ViewColumns", true);
			CreateSqlServerSchemaModel("UserDefinedTypes", true);

			return "CodeGenSchema.Init() complete....";
		}

		private static bool CreateSqlServerSchemaModel(string schemaName, bool overwrite = true)
		{
			string output = "";

			using (var conn = GetConnection())
			{
				conn.Open();
				DataTable table = conn.GetSchema(schemaName);

				// Debugging : 
				var schemaString = DisplayData(table);

				output =
				$@"
				using System;

				namespace {namespaceRoot}.SchemaModels
				{{
					public class {schemaName}Schema
					{{
						{ CreateProperty(table) }
					}}
				}}";

				conn.Close();
			};

			bool isSuccess = output.RemoveLineIndents(4, "\t")
					.WriteToFile(($"{outputDir}{schemaName}Schema.cs"), true);


			return isSuccess;
		}

		private static string CreateProperty(DataTable table)
		{
			string output = "";
			string start = "\t\t";
			string end = "\r\n\r\n";

			foreach (DataColumn col in table.Columns)
			{
				output += $"{start}public {col.DataType.Name.RemoveStart("System.")} {col.ColumnName} {{ get; set; }}{end}";
			}

			return output.RemoveStartEnd(start, end);
		}

		// Nest this string in connection using: var schemaString = DisplayData(table);
		private static string DisplayData(DataTable table)
		{
			var sb = new StringBuilder();

			foreach (DataRow row in table.Rows)
			{
				foreach (DataColumn col in table.Columns)
				{
					sb.AppendLine($"{col.ColumnName} = {row[col]}");
				}
				sb.AppendLine("============================");
			}
			return sb.ToString();
		}
	}
}