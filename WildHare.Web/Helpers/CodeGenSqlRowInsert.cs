using SeedPacket.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WildHare.Extensions;

namespace WildHare.Web
{
    public static class CodeGenSqlRowInsert
    {
        /* ==========================================================================
         * DIRECTIONS
         * 
         * PLACE FOLLOWING LINE OF CODE SOMEWHERE IT WILL BE RUN ON COMPILE
         * OR ALTERNATIVELY RUN IN THE IMMEDIATE WINDOW:
         * 
           WildHare.Web.CodeGenSqlRowInsert.Init();
        ========================================================================== */

		private static readonly string outputDir = @"C:\Code\Trunk\WildHare\WildHare.Web\SqlInserts\";
		private static readonly string start = "\t"; // Indentation
		private static readonly string end = Environment.NewLine;
        private static readonly int randomSeed = 34335;

        public static string Init()
        {
            // GenereateSQLInserts<ControlValues>(5002, "ControlValues", "dbo", "ControlValueId", true, true);
            GenereateSQLInserts<User>(2000, excludeColumns: "Created", identityInsertOn: true);   // Example using temporary private class below

            return "CodeGenSqlRowInsert.Init() complete....";
		}

        public static bool GenereateSQLInserts<T>( int count, string tableName = null, string schema = null, string excludeColumns = null, 
                                                   bool identityInsertOn = false, bool overwrite = true)
        {
            int batchSize = 1000;
            var metaModel = typeof(T).GetMetaModel();
            var metaProperties = typeof(T).GetMetaProperties(excludeColumns);
            var rowList = new List<T>().Seed(count, new Random(randomSeed)).ToList();

            tableName = tableName ?? metaModel.TypeName;
            schema = schema ?? "dbo";

            string identity_insert     = identityInsertOn ? $"SET IDENTITY_INSERT [{schema}].[{tableName}]" : "";
            string identity_insert_ON  = identity_insert.AddEnd(" ON");
            string identity_insert_OFF = identity_insert.AddEnd(" OFF");

            string output =
            $@"
            --===========================================================
            -- Generating Insert Data for {tableName}
            --===========================================================

            {identity_insert_ON}

            { CreateData(rowList, schema, tableName, metaProperties, batchSize) }

            {identity_insert_OFF}
            ";

            // OUTPUT ===================================================================

            string fileName = $"{tableName}_Insert.sql";
            bool isSuccess = output.RemoveLineIndents(12).WriteToFile($"{outputDir}{fileName}", overwrite);

            LogResult (fileName, isSuccess);

            return isSuccess;
        }

        private static object CreateData<T>(List<T> rowList, string schema, string tableName, List<MetaProperty> metaProperties, int batchSize)
        {
            var sb = new StringBuilder();

            for (int i = 0; i <= (rowList.Count/batchSize); i++)
            {
                var rows = rowList.Skip(i * batchSize).Take(batchSize).ToList();
                if (rows.Count > 0)
                {
                    string batch = CreateBatchData(rows, schema, tableName, metaProperties);
                    sb.Append(batch);
                }
            }
            return sb.ToString();
        }

        private static string CreateBatchData<T>(List<T> rowList, string schema, string tableName, List<MetaProperty> metaProperties)
		{
            var tableColumns = string.Join("", metaProperties.Select(a => $"[{a.Name}],")).RemoveEnd(",");

            string output =
            $@"
            INSERT [{schema}].[{tableName}] (
                {tableColumns}
            )
            VALUES
                { CreateRows(rowList, metaProperties) }
            GO
            ";

            return output;
		}

        private static object CreateRows<T>(List<T> rowList, List<MetaProperty> metaProperties)
        {
            var rowSb = new StringBuilder();
            foreach (T row in rowList)
            {
                var valuesSb = new StringBuilder();
                foreach (var prop in metaProperties)
                {
                    object val = prop.GetInstanceValue(row);

                    if (prop.PropertyType == typeof(string))
                    {
                        valuesSb.Append($"N'{val}', ");
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        valuesSb.Append($"CAST('{val}' AS DateTime2), ");
                    }
                    else // Numbers etc.
                    {
                        valuesSb.Append($"{val}, ");
                    }
                }
                rowSb.Append($"{start}({valuesSb.ToString().RemoveEnd(", ")}),{end}");
            }
            return rowSb.ToString().RemoveStartEnd("\t", $",{end}");
        }

        private static void LogResult(string fileName, bool isSuccess)
        {
            if (isSuccess)
            {
                Debug.WriteLine("=".Repeat(50));
                Debug.WriteLine($"Generated file {fileName} in {outputDir}.");
                Debug.WriteLine("=".Repeat(50));
            }
        }

        // =====================================================================
        // EXAMPLE OF PRIVATE NESTED CLASS
        // =====================================================================

        public class User
        {
            public int UserId { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string UserName { get; set; }

            public string Email { get; set; }

            public DateTime Created { get; set; } = DateTime.Now;
        }

        // =====================================================================
    }
}
