using SeedPacket.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;
using WildHare.Web.Models;
using WildHare.Web.SchemaModels;

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
            GenereateSQLInserts<ControlValues>(20, "ControlValues", "dbo", "ControlValueId", true);
            GenereateSQLInserts<Account>(20, "Account", null, null, true); // Example using temporary private class below

            return "CodeGenSqlRowInsert.Init() complete....";
		}

        public static bool GenereateSQLInserts<T>(  int count, string tableName = null, string schema = null,
                                                    string excludeColumns = null, bool overwrite = true)
        {
            var metaModel = typeof(T).GetMetaModel();
            var metaProperties = typeof(T).GetMetaProperties(excludeColumns);
            var rowList = new List<T>().Seed(count, new Random(randomSeed)).ToList();

            tableName = tableName ?? metaModel.TypeName;
            schema = schema ?? "dbo";

            string output =
            $@"
              --===========================================================
              -- Generating Insert Data for {tableName}
              --===========================================================

            { CreateData(rowList, schema, tableName, metaProperties) }
            ";

            // OUTPUT ===================================================================

            string fileName = $"{tableName}_Insert.sql";
            bool isSuccess = output.RemoveLineIndents(12).WriteToFile($"{outputDir}{fileName}", overwrite);

            LogResult (fileName, isSuccess);

            return isSuccess;
        }

        private static string CreateData<T>(List<T> rowList, string schema, string tableName, List<MetaProperty> metaProperties)
		{
            var tableColumns = string.Join("", metaProperties.Select(a => $"[{a.Name}],")).RemoveEnd(",");

            string output =
            $@"
            INSERT [{schema}].[{tableName}] (
                {tableColumns}
            )
            VALUES
                { CreateRows(rowList, metaProperties) }

            GO";

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
                        valuesSb.Append($"CAST('{val}') AS DateTime2, ");
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
        public class Account
        {
            public int AccountId { get; set; }

            public string AccountName { get; set; }

            public DateTime Created { get; set; }
        }
        // =====================================================================
    }
}
