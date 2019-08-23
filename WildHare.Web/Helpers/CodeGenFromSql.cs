using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;
using WildHare.Web.SchemaModels;

namespace WildHare.Web
{
	public static class CodeGenFromSql
    {
		/* ==========================================================================
         * DIRECTIONS
         * 
         * PLACE FOLLOWING LINE OF CODE SOMEWHERE IT WILL BE RUN ON COMPLIE
         * OR ALTERNATIVELY RUN IN THE IMMEDIATE WINDOW:
         * 
           WildHare.Web.CodeGenFromSql.Init();
        ========================================================================== */

		// FOR SCHEMA DOCS SEE: https: //docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-schema-collections

		private static readonly string namespaceRoot = "WildHare.Web";
		private static readonly string outputDir = @"C:\Code\Trunk\WildHare\WildHare.Web\GeneratedModels\";
		private static readonly string sqlConnString = "Data Source=Behemoth;Initial Catalog=Tevents;Connect Timeout=30;Persist Security Info=True;MultipleActiveResultSets=True;User ID=Tevents_User;Password=!london!";

		private static readonly string start = "\t\t"; // Indentation
		private static readonly string end = Environment.NewLine;

		private static ILookup<string, ColumnsSchema> sqlTables;

		public static string Init()
        {

			// Get list of table from SQL database
			sqlTables = GetTablesFromSQL();

			// 1) Loop through the tables
			// ============================================================
			// a) Generates models for all tables in the database. 

			//foreach (var table in sqlTables)
			//{
			//	CreateModelFromSQLTable(table.Key);
			//}

			// 2) Pre-Generate a list of tables - Alternate approach
			// ============================================================
			// a) Create a string list of tables to use in this Init(). 
			// b) ie: generate a string using the modelsToCreate function below and paste it into this Init. 
			// c) This gives you the ability to remove tables that are not needed. 
			// d) Mark the 'overwrite' property as false if it has customizations that should not be overridden later.

			var modelsToCreate = string.Join("\r\n", sqlTables.Select(s => $"CreateModelFromSQLTable(\"{s.Key}\", true);"));

			CreateModelFromSQLTable("Acts", true);
			CreateModelFromSQLTable("Controls", true);
			CreateModelFromSQLTable("ControlValues", true);
			CreateModelFromSQLTable("Description", true);
			CreateModelFromSQLTable("Layouts", true);
			CreateModelFromSQLTable("Locations", true);
			CreateModelFromSQLTable("Tags", true);
			CreateModelFromSQLTable("Tevents", true);
			CreateModelFromSQLTable("Timelines", true);
			CreateModelFromSQLTable("Users", true);

			return "CodeGenFromSql.Init() complete....";
		}

		private static ILookup<string, ColumnsSchema> GetTablesFromSQL()
		{
			using (var conn = new SqlConnection(sqlConnString))
			{
				conn.Open();

				DataTable tables = conn.GetSchema("Columns");
				var tablesList = tables.DataTableToList<ColumnsSchema>().OrderBy(o => o.Ordinal_Position)
					            .ToLookup(g => $"{g.Table_Name}");

				return tablesList;
			};
		}

		private static bool CreateModelFromSQLTable(string tableName, bool overwrite = true)
		{
			string output =
			$@"
				using System;
				using System.ComponentModel.DataAnnotations;

                // For table: {tableName}

				namespace {namespaceRoot}.Models
				{{
					public class {tableName}
					{{
						{ CreateModelPropertiesWithKeys(tableName) }
					}}
				}}";

			bool isSuccess = output.RemoveLineIndents(4, "\t")
					.WriteToFile(($"{outputDir}/{tableName}.cs"), overwrite);

			return isSuccess;
		}

        // ===============================================================================
        // ORIGINAL TECHNIQUE WITH PK Data NOT NEEDED
        // ===============================================================================

        private static string CreateModelProperties(string tableName)
		{
			var tableSchema = sqlTables.First(t => t.Key == tableName).ToList();
			var sb = new StringBuilder();

			foreach (var column in tableSchema)
			{
				bool isNullable = column.Is_Nullable.ToBool("YES");
				string dataType = column.Data_Type.FromTSqlTypeToCSharpType(isNullable);

				sb.AppendLine($"{start}public {dataType} {column.Column_Name} {{ get; set; }}");
			}
			return sb.ToString().RemoveStartEnd(start, end);
		}


        // ===============================================================================
        // ALTERNATE TECHNIQUE WITH PK Data Required - EXTRA SQL QUERIES
        // ===============================================================================

        private static string CreateModelPropertiesWithKeys(string tableName)
        {
            // REFERENCE: https ://docs.microsoft.com/en-us/dotnet/api/system.data.datatablereader.getschematable?redirectedfrom=MSDN&view=netframework-4.8

            var sb = new StringBuilder();
            var dataTable = new DataTable();

            using (var conn = new SqlConnection(sqlConnString))
            {
                conn.Open();

                var adapter = new SqlDataAdapter("SELECT * FROM " + tableName + " WHERE 0=1", conn)
                {
                    MissingSchemaAction = MissingSchemaAction.AddWithKey
                };
                adapter.FillSchema(dataTable, SchemaType.Mapped);
            };

            // loops through all the rows of the data table
            foreach (DataColumn col in dataTable.Columns)
            {
                string dataTypeName = col.DataType.Name.FromDotNetTypeToCSharpType();
                string columnName = col.ColumnName;

                string isKey = dataTable.PrimaryKey.Contains(col) ? $"{start}[Key]{end}" : "";
                bool hasMaxLength = (dataTypeName == "string" && col.MaxLength > 0 && col.MaxLength < 10000);
                string stringLength = hasMaxLength ? $"{start}[StringLength({col.MaxLength})]{end}" : "";

                sb.Append($"{isKey}");
                sb.Append($"{stringLength}");
                sb.AppendLine($"{start}public {dataTypeName} {columnName} {{ get; set; }}{end}");
            }

            sb.AppendLine($"{start}public override string ToString()");
            sb.AppendLine($"{start}{{");
            sb.AppendLine($"{start}\treturn $\"{ dataTable.Columns[0].ColumnName}: {{{dataTable.Columns[0].ColumnName}}}\";");
            sb.AppendLine($"{start}}}");

            return sb.ToString().RemoveStartEnd(start, end);
		}
	}
}
