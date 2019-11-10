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
         * PLACE FOLLOWING LINE OF CODE SOMEWHERE IT WILL BE RUN ON COMPLIE
         * OR ALTERNATIVELY RUN IN THE IMMEDIATE WINDOW:
         * 
           WildHare.Web.CodeGenSqlRowInsert.Init();
        ========================================================================== */

		private static readonly string outputDir = @"C:\Code\Trunk\WildHare\WildHare.Web\SqlInserts\";

		private static readonly string start = "\t\t"; // Indentation
		private static readonly string end = Environment.NewLine;

		public static string Init()
        {
            GenerateRowData<ControlValues>(100, null, "ControlValueId", true);

            return "CodeGenSqlRowInsert.Init() complete....";
		}

        public static bool GenerateRowData<T>(int count, string tableName = null, string excludeColumns = null, bool overwrite = true)
        {
            var metaModel = typeof(T).GetMetaModel();
            var metaProperties = metaModel.GetMetaProperties(excludeColumns);
            var columns = string.Join("", metaProperties.Select(a => $"[{a.Name}],") ).RemoveEnd(",");
            //var list = new List<T>().Seed(count);

            string schema = "dbo";
            string name = tableName ?? metaModel.TypeName;
            string fileName = $"{name}_Insert.sql";

            string output =
            $@"
            INSERT [{schema}].[{name}]({columns})
            VALUES (
                { CreateData(count, metaProperties) }
            )";

            // =======================================================================
            
            bool isSuccess = output.RemoveLineIndents(12).WriteToFile($"{outputDir}{fileName}", overwrite);

            if (isSuccess)
            {
                Debug.WriteLine("=".Repeat(50));
                Debug.WriteLine($"Generated file {fileName} in {outputDir}.");
                Debug.WriteLine("=".Repeat(50));
            }
            return isSuccess;
        }


        private static string CreateData(int rowCount, List<MetaProperty> properties)
		{
			var sb = new StringBuilder();
            for (int i = 0; i < rowCount; i++)
            {
                sb.AppendLine($"{start}(values),");
            }
            return sb.ToString().RemoveStartEnd("\t", ",");
		}
	}
}
