using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using WildHare.Extensions;
using WildHare.Web.Entities;
using WildHare.Web.Models;
using static System.Environment;
using static System.Console;
using Microsoft.Extensions.Primitives;

namespace CodeGen.Generators
{
    public static class CodeGenAdapters
    {
        /* ==========================================================================
         * DIRECTIONS:
         * 
         * PLACE FOLLOWING LINE OF CODE SOMEWHERE IT WILL BE RUN ON COMPILE, RUN IN THE IMMEDIATE WINDOW, 
         * or in the .NET Core StartUp Configure() -> passing in env.ContentRootPath
         
           WildHare.Web.CodeGenAdapters.Init(c:\github\WildHare);
        ========================================================================== */


        private static bool overWrite = true;
        private static readonly string namespaceRoot = "WildHare.Web";
        private static string outputDir = @"\Adapters\";

        private static readonly string mapName1 = "entity";
        private static readonly string mapName2 = "model";

        private static readonly string indent = "\t".Repeat(4);
        private static readonly string end = $",{NewLine}";
        private const int pad = -20;

        public static string Init(string projectRoot)
        {
            if (projectRoot.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenAdapters)}.{nameof(Init)} projectRoot is null or empty.");

            outputDir = $"{projectRoot}{outputDir}";

            Debug.WriteLine("=".Repeat(50));
            Debug.WriteLine("Running CodeGenAdapters");
            Debug.WriteLine("=".Repeat(50));

            string adapterList = GetGeneratorAdapterList();

            // Write out the adapterlist to the Debug window generated from toTypeProps particular namespace
            Debug.Write(adapterList.AddEnd("=".Repeat(80) + NewLine));

            // Copy and paste adapterlist from Debug Output window here if needed.

            GenerateAdapter(typeof(InvoiceItem), typeof(InvoiceItemModel), overWrite);
            GenerateAdapter(typeof(Invoice), typeof(InvoiceModel), overWrite);
            GenerateAdapter(typeof(Account), typeof(AccountModel), overWrite);

            // To Delete:  Array.ForEach(Directory.GetFiles(outputDir), file => File.Delete(file));


            Debug.WriteLine("=".Repeat(80));

            string result = $"{nameof(CodeGenAdapters)}.{nameof(Init)} code written to '{outputDir}'. Overwrite: {overWrite}";

            Debug.WriteLine(result);

            return result;
        }

        public static bool GenerateAdapter(Type type1, Type type2, bool overwrite = false, bool generateListCode = true)
        {
            string class1 = type1.Name;
            string class2 = type2.Name;

            string adapterName = $"{class1}Adapter.cs";

            string output =
            $$"""
            using {{namespaceRoot}}.Models;
            using {{namespaceRoot}}.Entities;
            using System.Linq;
            using System.Collections.Generic;

            namespace {{namespaceRoot}}.Adapters
            { 
                public static partial class Adapter
                {
                    public static {{class2}} To{{class2}} (this {{class1}} {{mapName1}})
                    {
                        return {{mapName1}} == null ? null : new {{class2}}
                        {
                            {{PropertiesList(type1, type2, mapName1)}}
                        };
                    }

                    public static {{class1}} To{{class1}} (this {{class2}} {{mapName2}})
                    {
                        return {{mapName2}} == null ? null : new {{class1}}
                        {
                            {{PropertiesList(type2, type1, mapName2)}}
                        };
                    }
                    {{GetListCode(class1, class2, mapName1, mapName2, generateListCode)}}
                }
            }
            """;

            bool isSuccess = output.WriteToFile($"{outputDir}{adapterName}", overwrite);

            if (isSuccess)
                Debug.WriteLine($"Generated file {adapterName} in {outputDir}.");

            return isSuccess;
        }

        private static string PropertiesList(Type type, Type toType, string mapName)
        {
            var sb = new StringBuilder();

            foreach (var prop in type.GetProperties())
            {
                // var toTypeProps = toType.GetProperties().Select(n => n.Name.ToLower()).ToArray();
                // if (prop.Name.EqualsAny(true, toTypeProps))

                if (toType.GetProperties().Any(toTypeProps => toTypeProps.Name.ToLower() == prop.Name.ToLower()))
                {
                    sb.Append($"{indent}{prop.Name,pad} = {mapName}.{prop.Name}{UseListAdapter(prop)}{end}");
                }
                else
                {
                    sb.Append($"// No Match // {indent}{prop.Name,pad} = {mapName}.{prop.Name}{UseListAdapter(prop)}{end}");
                }
            }
            return sb.ToString().RemoveStartEnd(indent, end);
        }

        private static string GetListCode(string class1, string class2, string mapName1, string mapName2, bool genListCode = true)
        {
            if (!genListCode)
                return "";

            string template =
             $$"""

             public static List<{{class2}}> To{{class2}}List (this IEnumerable<{{class1}}> {{mapName1}}List)
             {
                 return {{mapName1}}List?.Select(a => a.To{{class2}}()).ToList() ?? new List<{{class2}}>();
             }
             
             public static List<{{class1}}> To{{class1}}List (this IEnumerable<{{class2}}> {{mapName2}}List)
             {
                return {{mapName2}}List?.Select(a => a.To{{class1}}()).ToList() ?? new List<{{class1}}>();
             }
             """;

            return template.ForEachLine(a => "\t\t" + a);   // Add initial line formatting if needed
        }

        private static string UseListAdapter(PropertyInfo prop)
        {
            var sb = new StringBuilder();

            if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
            {
                var itemName = prop.PropertyType?.GenericTypeArguments.ElementAtOrDefault(0)?.Name ?? prop.Name;
                sb.Append(itemName.Contains("Model") ? $".To{itemName.RemoveEnd("Model")}List()" : $".To{itemName}ModelList()");
            }
            return sb.ToString();
        }

        public static string GetGeneratorAdapterList() // Use this string to set up models in CodeGen constructor
        {
            var sb = new StringBuilder();

            var assembly = Assembly.Load("WildHare.Web");
            var typeList = assembly.GetTypesInNamespace("WildHare.Web.Entities");

            foreach (var type in typeList)
            {
                sb.AppendLine($"GenerateAdapter(typeof({type.Name}), typeof({type.Name}Model), true);");
            }
            return sb.ToString();
        }
    }
}
