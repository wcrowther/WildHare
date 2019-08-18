using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Web.Entities;
using WildHare.Web.Models;

namespace WildHare.Web
{
    public static class CodeGenAdapters
    {
        /* ==========================================================================
         * DIRECTIONS
         * 
         * PLACE FOLLOWING LINE OF CODE SOMEWHERE IT WILL BE RUN ON COMPLIE
         * OR ALTERNATIVELY RUN IN THE IMMEDIATE WINDOW:
         * 
           WildHare.Web.CodeGenAdapters.Init();
        ========================================================================== */

        private static readonly string namespaceRoot = "WildHare.Web";
        private static readonly string outputDir = @"C:\Code\Trunk\WildHare\WildHare.Web\Adapters\";
        private static readonly string mapName1 = "entity";
        private static readonly string mapName2 = "model";

        public static string Init()
        {
            Debug.WriteLine("=".Repeat(50));
            Debug.WriteLine("Running CodeGenAdapters");
            Debug.WriteLine("=".Repeat(50));

            GenerateAdapter(typeof(InvoiceItem),typeof(InvoiceItemModel), false);
            GenerateAdapter(typeof(Invoice),    typeof(InvoiceModel)    , false);
            GenerateAdapter(typeof(Account),    typeof(AccountModel)    , false);

            Debug.WriteLine("=".Repeat(50));

            return "CodeGen.Init() complete....";
        }

        public static bool GenerateAdapter(Type type1, Type type2, bool overwrite = false)
        {
            string class1 = type1.Name;
            string class2 = type2.Name;
            string adapterName = $"{class1}Adapter.cs";

            string output =
            $@"using {namespaceRoot}.Models;
            using {namespaceRoot}.Entities;
            using System.Linq;
            using System.Collections.Generic;

            namespace {namespaceRoot}.Adapters
            {{ 
                public static partial class Adapter
                {{
                    public static {class2} To{class2} (this {class1} {mapName1})
                    {{
                        return {mapName1} == null ? null : new {class2}
                        {{
                            {PropertiesList(type1, type2, mapName1)}
                        }};
                    }}

                    public static List<{class2}> To{class2}List (this IEnumerable<{class1}> {mapName1}List)
                    {{
                        return {mapName1}List?.Select(a => a.To{class2}()).ToList() ?? new List<{class2}>();
                    }}

                    public static {class1} To{class1} (this {class2} {mapName2})
                    {{
                        return {mapName2} == null ? null : new {class1}
                        {{
                            {PropertiesList(type2, type1, mapName2)}
                        }};
                    }}

                    public static List<{class1}> To{class1}List (this IEnumerable<{class2}> {mapName2}List)
                    {{
                        return {mapName2}List?.Select(a => a.To{class1}()).ToList() ?? new List<{class1}>();
                    }}
                }}
            }}";

            bool isSuccess = output.RemoveLineIndents(12).WriteToFile($"{outputDir}{adapterName}", overwrite);

            if (isSuccess)
                Debug.WriteLine($"Generated file {adapterName} in {outputDir}.");

            return isSuccess;
        }

        private static string PropertiesList(Type type, Type toType, string mapName)
        {
            string output = "";
            string start = "\t\t\t\t";
            string end = ",\r\n";

            foreach (var prop in type.GetProperties())
            {
                if (toType.GetProperties().Any(a => a.Name.ToLower() == prop.Name.ToLower()))
                {
                    output += $"{start}{prop.Name} = {mapName}.{prop.Name}{UseListAdapter(prop)}{end}";
                }
                else
                {
                    output += $"// No Match // {start}{prop.Name} = {mapName}.{prop.Name}{UseListAdapter(prop)}{end}";
                }
            }
            return output.RemoveStartEnd(start, end);
        }

        private static string UseListAdapter(PropertyInfo prop)
        {
            string adapterString = "";
            if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
            {
                var itemName = prop.PropertyType?.GenericTypeArguments.ElementAtOrDefault(0)?.Name ?? prop.Name;
                adapterString = itemName.Contains("Model") ? $".To{itemName.RemoveEnd("Model")}List()" : $".To{itemName}ModelList()";
            }
            return adapterString;
        }

        public static string GetGeneratorAdapterList() // Use this string to set up models in CodeGen constructor
        {
            string output = "";           
            var assembly = Assembly.Load("WildHare.Web");
            var typeList = assembly.GetTypesInNamespace("WildHare.Web.Entities");

            foreach (var type in typeList)
            {
                output += $"\n GenerateAdapter(typeof({type.Name}), typeof({type.Name}Model));";
            }

            return output.TrimStart('\n');
        }
    }
}
