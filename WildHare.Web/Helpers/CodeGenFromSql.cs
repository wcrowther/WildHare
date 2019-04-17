using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WildHare.Extensions;
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

		private static ILookup<string, ColumnsSchema> sqlTables;

		public static string Init()
        {
			sqlTables = GetTablesFromSQL();

			CreateModelFromSQLTable("__MigrationHistory", true);
			CreateModelFromSQLTable("Acts", true);
			CreateModelFromSQLTable("AspNetRoles", true);
			CreateModelFromSQLTable("AspNetUserClaims", true);
			CreateModelFromSQLTable("AspNetUserLogins", true);
			CreateModelFromSQLTable("AspNetUserRoles", true);
			CreateModelFromSQLTable("AspNetUsers", true);
			CreateModelFromSQLTable("Controls", true);
			CreateModelFromSQLTable("ControlValues", true);
			CreateModelFromSQLTable("Description", true);
			CreateModelFromSQLTable("Layouts", true);
			CreateModelFromSQLTable("Locations", true);
			CreateModelFromSQLTable("sysdiagrams", true);
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
				var tablesList = tables.DataTableToList<ColumnsSchema>().OrderBy(o => o.ORDINAL_POSITION)
					            .ToLookup(g => $"{g.TABLE_NAME}");

				// Create list on tables to use in Init() if needed.
				var tableNames = string.Join("\r\n", tablesList.Select(s => $"CreateModelFromTable(\"{s.Key}\", true);"));

				return tablesList;
			};
		}

		private static bool CreateModelFromSQLTable(string tableName, bool overwrite = true)
		{
			string output =
			$@"
				using System;

				namespace {namespaceRoot}.Models
				{{
					public class {tableName}
					{{
						{ CreateModelProperty(tableName) }
					}}
				}}";

			bool isSuccess = output.RemoveLineIndents(4, "\t")
					.WriteToFile(($"{outputDir}/{tableName}.cs"), true);

			return isSuccess;
		}

		private static string CreateModelProperty(string tableName)
		{
			var tableColumns = sqlTables.First(t => t.Key == tableName).ToList();

			string output = "";
			string start = "\t\t";
			string end = "\r\n\r\n";

			foreach (var col in tableColumns)
			{
				output += $"{start}public {col.DATA_TYPE.RemoveStart("System.")} {col.COLUMN_NAME} {{ get; set; }}{end}";
			}

			return output.RemoveStartEnd(start, end);
		}

		private static string ToCSharpType(string sqlTypeName, bool isNull = false)
		{
			string cstype = "";
			string nullable = isNull ? "?" : "";

			switch (sqlTypeName.ToLower())
			{ 
				case "binary":				cstype = "byte[]";					break;
				case "image":				cstype = "byte[]";					break;
				case "varbinary":			cstype = "byte[]";					break;
				case "text":				cstype = "string";					break;
				case "char":				cstype = "string";					break; 
				case "nchar":				cstype = "string";					break;
				case "ntext":				cstype = "string";					break;
				case "varchar":				cstype = "string";					break;
				case "nvarchar":			cstype = "string";					break;
				case "real":				cstype = "float";					break;
				case "time":				cstype = "TimeSpan";				break;
				case "tinyint":				cstype = "byte";					break;
				case "uniqueidentifier":	cstype = "Guid";					break;
				case "datetimeoffset":		cstype = "DateTimeOffset";			break;
				case "bit":					cstype = $"bool{nullable}";			break;
				case "smallint":			cstype = $"short{nullable}";		break;
				case "int":					cstype = $"int{nullable}";			break;
				case "bigint":				cstype = $"long{nullable}";			break;
				case "timestamp":			cstype = $"long{nullable}";			break;
				case "smalldatetime":		cstype = $"DateTime{nullable}";		break;
				case "date":				cstype = $"DateTime{nullable}";		break;
				case "datetime":			cstype = $"DateTime{nullable}";		break;
				case "datetime2":			cstype = $"DateTime{nullable}";		break;
				case "decimal":				cstype = $"decimal{nullable}";		break;
				case "float":				cstype = $"double{nullable}";		break;
				case "smallmoney":			cstype = $"decimal{nullable}";		break;
				case "numeric":				cstype = $"decimal{nullable}";		break;
				default:					cstype = "UNKNOWN";					break;
			}
			return cstype;
		}

	}
}