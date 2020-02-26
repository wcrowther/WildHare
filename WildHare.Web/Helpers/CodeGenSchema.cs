using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;

namespace WildHare.Web
{
	public static class CodeGenSchema
    {
		/* ==========================================================================
         * DIRECTIONS
         * 
         * PLACE FOLLOWING LINE OF CODE SOMEWHERE IT WILL BE RUN ON COMPILE
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
			// List of Various Schemas
			CreateSqlServerSchemaModel("MetaDataCollections", false);

			CreateSqlServerSchemaModel("Columns", false);
			CreateSqlServerSchemaModel("AllColumns", false);
			CreateSqlServerSchemaModel("ColumnSetColumns", false);
			CreateSqlServerSchemaModel("StructuredTypeMembers", false);
			CreateSqlServerSchemaModel("DataTypes", false);
			CreateSqlServerSchemaModel("Restrictions", false);
			CreateSqlServerSchemaModel("ReservedWords", false);
			CreateSqlServerSchemaModel("Databases", false);
			CreateSqlServerSchemaModel("ForeignKeys", false);
			CreateSqlServerSchemaModel("Indexes", false);
			CreateSqlServerSchemaModel("IndexColumns", false);
			CreateSqlServerSchemaModel("Procedures", false);
			CreateSqlServerSchemaModel("ProcedureParameters", false);
			CreateSqlServerSchemaModel("Tables", false);
			CreateSqlServerSchemaModel("Users", false);
			CreateSqlServerSchemaModel("Views", false);
			CreateSqlServerSchemaModel("ViewColumns", false);
			CreateSqlServerSchemaModel("UserDefinedTypes", false);

			return "CodeGenSchema.Init() complete....";
		}

		private static bool CreateSqlServerSchemaModel(string schemaName, bool overwrite = true)
		{
			string output = "";

			using (var conn = GetConnection())
			{
				conn.Open();
				DataTable table = conn.GetSchema(schemaName);

				
				var schemaString = DisplayDebugData(table);
				Debug.WriteLine(schemaString); 

				output =
				$@"
				using System;

				namespace {namespaceRoot}.SchemaModels
				{{
					public class {schemaName}Schema
					{{
						{ CreateTableClassProperties(table) }
					}}
				}}";

				conn.Close();
			};

			bool isSuccess = output.RemoveLineIndents(4, "\t")
							.WriteToFile(($"{outputDir}{schemaName}Schema.cs"), overwrite);


			return isSuccess;
		}

		private static string CreateTableClassProperties(DataTable table)
		{
			string output = "";
			string start = "\t\t";
			string end = "\r\n\r\n";

			foreach (DataColumn col in table.Columns)
			{
                bool isNullable = col.AllowDBNull;
                output += $"{start}public {col.DataType.Name.FromDotNetTypeToCSharpType(isNullable)} {col.ColumnName.ProperCase(true)} {{ get; set; }}{end}";
			}

			return output.RemoveStartEnd(start, end);
		}

		// Nest this string in connection using: var schemaString = DisplayDebugData(table);
		private static string DisplayDebugData(DataTable table)
		{
			var sb = new StringBuilder();

			sb.AppendLine("============================");

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
