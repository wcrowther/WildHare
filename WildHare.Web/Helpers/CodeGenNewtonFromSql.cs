using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;
using WildHare.Web.SchemaModels;
using static System.Environment;

namespace WildHare.Web
{
	public static class CodeGenNewtonFromSql
    {

		// FOR SCHEMA DOCS SEE: https: //docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-schema-collections

		private static readonly string namespaceRoot = "Me.Logic";
		private static readonly string outputDir     = @"C:\Code\Trunk\WildHare\WildHare.Web\NewtonModels\";
		private static readonly string sqlConnString = "Data Source=Behemoth;Initial Catalog=Newton;Connect Timeout=30;Persist Security Info=True;MultipleActiveResultSets=True;User ID=Newton_User;Password=!london!";

		private static readonly string start = "\t\t"; // Indentation
        private static readonly string end = NewLine;

        private static ILookup<string, ColumnsSchema> sqlTables;

		public static string Init()
        {

			// Get list of table from SQL database
			sqlTables = GetTablesFromSQL(exclude: "__MigrationHistory");

            // 1) Loop through the tables
            // ============================================================
            // a) Generates models for all tables in the database. 

            //foreach (var table in sqlTables)
            //{
            //    CreateModelFromSQLTable(table.Key);
            //}

            // 2) Pre-Generate a list of tables - Alternate approach
            // ============================================================
            // a) Create a string list of tables to use in this Init(). 
            // b) ie: generate a string using the modelsToCreate function below and paste it into this Init. 
            // c) This gives you the ability to remove tables that are not needed. 
            // d) Mark the 'overwrite' property as false if it has customizations that should not be overridden later.

            var modelsToCreate = string.Join("\r\n", sqlTables.Select(s => $"CreateModelFromSQLTable(\"{s.Key}\", overwrite: true);"));
            Debug.Write(NewLine + modelsToCreate + NewLine.Repeat(2));

            CreateModelFromSQLTable("Abstract", overwrite:  false);
            CreateModelFromSQLTable("Categories", "Category", overwrite:  false);
            CreateModelFromSQLTable("ComplexWords", overwrite:  false);
            CreateModelFromSQLTable("ComplexWordsDetail", overwrite:  false);
            CreateModelFromSQLTable("ComplexWordToken", overwrite:  false);
            CreateModelFromSQLTable("Encrypted", overwrite:  false);
            CreateModelFromSQLTable("Features", overwrite:  false);
            CreateModelFromSQLTable("LinkProperties", overwrite:  false);
            CreateModelFromSQLTable("LinkToAbstract", overwrite:  false);
            CreateModelFromSQLTable("Pattern", overwrite:  false);
            CreateModelFromSQLTable("PatternDetail", overwrite:  false);
            CreateModelFromSQLTable("SymbolRule", overwrite:  false);
            CreateModelFromSQLTable("TestCase", overwrite:  false);
            CreateModelFromSQLTable("TestCaseType", overwrite:  false);
            CreateModelFromSQLTable("TestResult", overwrite:  false);
            CreateModelFromSQLTable("TestRun", overwrite:  false);
            CreateModelFromSQLTable("Token", overwrite:  false);
            CreateModelFromSQLTable("TokenProperty", overwrite:  false);
            CreateModelFromSQLTable("Word", overwrite:  false);
            CreateModelFromSQLTable("WordToken", overwrite:  false);

            return "CodeGenFromSql.Init() complete....";
		}

		private static ILookup<string, ColumnsSchema> GetTablesFromSQL(string exclude = "")
		{
            string[] excludeList = exclude.Split(',').Select(a => a.Trim()).ToArray();

            using (var conn = new SqlConnection(sqlConnString))
			{
				conn.Open();

				DataTable tables = conn.GetSchema("Columns");
				var tablesList = tables.ToList<ColumnsSchema>()
                                .Where(w => !excludeList.Any(e => w.Table_Name == e))
                                .OrderBy(o => o.Ordinal_Position)
					            .ToLookup(g => $"{g.Table_Name}");
                return tablesList;
			};
		}

		private static bool CreateModelFromSQLTable(string tableName, string modelName = null, bool overwrite = true)
		{
            modelName = modelName ?? tableName.RemoveEnd("s");;
            string indent = " ".Repeat(12);

            string output =  
            $@"using System;
            using System.ComponentModel.DataAnnotations;
    
            // Generated from table: {tableName}

            namespace {namespaceRoot}.Models
            {{
                public class {modelName}
                {{
                    { CreateModelPropertiesWithKeys(tableName) }
                }}
            }}";

            bool isSuccess = output
                             .RemoveStartFromAllLines(indent)
                             .WriteToFile($"{outputDir}/{modelName}.cs", overwrite);

            return isSuccess;
		}


        // ===============================================================================
        // TECHNIQUE WITH PK Data Required - EXTRA SQL QUERIES
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
                bool isNullable = col.AllowDBNull;
                string dataTypeName = col.DataType.Name.DotNetTypeToCSharpType(isNullable);
                string columnName = col.ColumnName;

                string isKey = dataTable.PrimaryKey.Contains(col) ? $"{start}[Key]{end}" : "";
                bool hasMaxLength = (dataTypeName == "string" && col.MaxLength > 0 && col.MaxLength < 10000);
                string stringLength = hasMaxLength ? $"{start}[StringLength({col.MaxLength})]{end}" : "";

                sb.Append($"{isKey}");
                sb.Append($"{stringLength}");
                sb.AppendLine($"{start}public {dataTypeName} {columnName} {{ get; set; }}{end}");
            }

            var pkColumn = dataTable.Columns.Cast<DataColumn>().FirstOrDefault(f => dataTable.PrimaryKey.Contains(f)) ?? dataTable.Columns[0];

            sb.AppendLine($"{start}public override string ToString()");
            sb.AppendLine($"{start}{{");
            sb.AppendLine($"{start}\treturn $\"{ pkColumn.ColumnName}: {{{pkColumn.ColumnName}}}\";");
            sb.AppendLine($"{start}}}");

            return sb.ToString().RemoveStartEnd(start, end);
		}


        // ===============================================================================
        // ALTERNATE TECHNIQUE IF PK DATA NOT NEEDED
        // ===============================================================================

        private static string CreateModelProperties(string tableName)
        {
            var tableSchema = sqlTables.First(t => t.Key == tableName).ToList();
            var sb = new StringBuilder();

            foreach (var column in tableSchema)
            {
                bool isNullable = column.Is_Nullable.ToBool("YES");
                string dataType = column.Data_Type.TSqlTypeToCSharpType(isNullable);

                sb.AppendLine($"{start}public {dataType} {column.Column_Name} {{ get; set; }}");
            }
            return sb.ToString().RemoveStartEnd(start, end);
        }

    }
}
