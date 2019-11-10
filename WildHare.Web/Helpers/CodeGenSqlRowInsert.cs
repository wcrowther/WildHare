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
		private static readonly string start = "\t\t"; // Indentation
		private static readonly string end = Environment.NewLine;

        public static string Init()
        {
            GenerateRowData<ControlValues>(20, null, "ControlValueId", true);
            //GenerateRowData<Account>(20, null, null, true); // Example using temporary private class below

            return "CodeGenSqlRowInsert.Init() complete....";
		}

        public static bool GenerateRowData<T>(int count, string tableName = null, string excludeColumns = null, bool overwrite = true)
        {
            var metaModel = typeof(T).GetMetaModel();
            var metaProperties = metaModel.GetMetaProperties(excludeColumns);
            var columns = string.Join("", metaProperties.Select(a => $"[{a.Name}],") ).RemoveEnd(",");
            var list = new List<T>().Seed(count).ToList();

            string schema = "dbo";
            string name = tableName ?? metaModel.TypeName;
            string fileName = $"{name}_Insert.sql";

            string output =
            $@"
            INSERT [{schema}].[{name}]({columns})
            VALUES (
                { CreateData<T>(list, metaProperties) }
            )";

            // OUTPUT ===================================================================
            
            bool isSuccess = output.RemoveLineIndents(12).WriteToFile($"{outputDir}{fileName}", overwrite);

            if (isSuccess)
            {
                Debug.WriteLine("=".Repeat(50));
                Debug.WriteLine($"Generated file {fileName} in {outputDir}.");
                Debug.WriteLine("=".Repeat(50));
            }
            return isSuccess;
        }


        private static string CreateData<T>(List<T> dataList, List<MetaProperty> properties)
		{
			var sb = new StringBuilder();
            var columns = string.Join("", properties.Select(a => $"[{a.Name}],")).RemoveEnd(",");


            foreach (var item in dataList)
            {
                sb.Append($"{start}({columns}),{end}");
            }
            return sb.ToString().RemoveStartEnd("\t", $",{end}");
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
