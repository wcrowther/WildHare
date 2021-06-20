using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;
using static System.Environment;

namespace WildHare.Web
{
	public static class CodeGenSchema
    {
		/* ==========================================================================
         * DIRECTIONS:
         * 
         * PLACE FOLLOWING LINE OF CODE SOMEWHERE IT WILL BE RUN ON COMPILE, RUN IN THE IMMEDIATE WINDOW, 
         * or in the .NET Core StartUp Configure() -> passing in env.ContentRootPath
         
           WildHare.Web.CodeGenSchema.Init(c:\github\WildHare);
        ========================================================================== */

		// FOR SCHEMA DOCS SEE: https: //docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-schema-collections

		private static string rootPath;
		private static readonly string namespaceRoot = "WildHare.Web";
		private static readonly string outputDir = $@"{rootPath}\Trunk\WildHare\WildHare.Web\SchemaModels\";


		public static SqlConnection GetConnection()
		{
			string sqlConnString = "Data Source=Behemoth;Initial Catalog=Tevents;Connect Timeout=30;Persist Security Info=True;MultipleActiveResultSets=True;User ID=Tevents_User;Password=!london!";
			return new SqlConnection(sqlConnString);
		}

		public static string Init(string projectRoot)
        {
			rootPath = projectRoot;
			
			// List of Various Schemas
			CreateSqlServerSchemaModel("MetaDataCollections", true);

			CreateSqlServerSchemaModel("Columns", true);
			CreateSqlServerSchemaModel("AllColumns", true);
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
			string output;
            string indent = "\t".Repeat(4);

			using (var conn = GetConnection())
			{
				conn.Open();
				DataTable table = conn.GetSchema(schemaName);

				var schemaString = DisplayDebugData(table);
				Debug.WriteLine(schemaString); 

				output =
				$@"using System;

				namespace {namespaceRoot}.SchemaModels
				{{
					public class {schemaName}Schema
					{{
						{ CreateTableClassProperties(table) }
					}}
				}}";

				conn.Close();
			};

			bool isSuccess = output
                            .RemoveStartFromAllLines(indent)
                            .WriteToFile(($"{outputDir}{schemaName}Schema.cs"), overwrite);

			return isSuccess;
		}

		private static string CreateTableClassProperties(DataTable table)
		{
			string start = "\t\t";
			string end = NewLine;

            var sb = new StringBuilder();

            foreach (DataColumn col in table.Columns)
			{
                bool isNullable = col.AllowDBNull && col.DefaultValue is null;
                string cSharpType = col.DataType.Name.DotNetTypeToCSharpType(isNullable);

                sb.AppendLine( $"{start}public {cSharpType} {col.ColumnName.ProperCase(true)} {{ get; set; }}{end}");
			}

			return sb.ToString().RemoveStartEnd(start, end);
		}

		// Nest this string in connection using: var schemaString = DisplayDebugData(table);
		private static string DisplayDebugData(DataTable table)
		{
			var sb = new StringBuilder();
            string line = "-".Repeat(25);

			sb.AppendLine(line);

			foreach (DataRow row in table.Rows)
			{
				foreach (DataColumn col in table.Columns)
				{
					sb.AppendLine($"{col.ColumnName} = {row[col]}");
				}
				sb.AppendLine(line);
			}
			return sb.ToString();
		}

	}
}
