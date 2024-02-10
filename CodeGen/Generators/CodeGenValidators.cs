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
using AngleSharp.Dom;
using System.Threading;

namespace CodeGen.Generators
{
    public static class CodeGenValidators
    {
        /* ==========================================================================
         * DIRECTIONS:
           WildHare.Web.CodeGenAdapters.Init(c:\github\WildHare);
        ========================================================================== */

        private static readonly string assemblyName = "WildHare.Web";
        private static readonly string namespaceName = "WildHare.Web.Models";
        private static readonly string excludeList = "AppSettings";
        private static readonly string indent = "\t";
        private static readonly string end = $",{NewLine}";
        private static readonly object namespaceRoot;
        private const int pad = -20;


        public static string Init(string projectRoot, string outputDir, bool overWrite = false)
        {
            if (projectRoot.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenValidators)}.{nameof(Init)} projectRoot is null or empty.");

            if (outputDir.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenValidators)}.{nameof(Init)} outputDir is null or empty.");

            string outputPath = $"{projectRoot}{outputDir}";

            bool isSuccess = GenerateValidators(projectRoot, outputDir, overWrite);

            string result = $"{nameof(CodeGenValidators)}.{nameof(Init)} code written to '{outputPath}'. Overwrite: {overWrite}";
            Debug.WriteLine(result);

            return result;
        }

        public static bool GenerateValidators(string projectRoot, string outputDir, bool overwrite = false)
        {
            string validatorsFileName = $"Validators.js";
            Type[] typeList = GetTypeList(assemblyName, namespaceName);


            string output =
            $$"""
            The typelist
            {{BuildTypeList(typeList)}}
            """;

            bool isSuccess = output.WriteToFile($"{projectRoot}{outputDir}{validatorsFileName}", overwrite);

            if (isSuccess)
                Debug.WriteLine($"Generated file {validatorsFileName} in {outputDir}.");

            return isSuccess;
        }

        private static Type[] GetTypeList(string assemblyName, string namespaceName) //, string exludeList = null)
        {
            var assembly = Assembly.Load(assemblyName); // "WildHare.Web"
            var typeList = assembly.GetTypesInNamespace(namespaceName);  // OLD "WildHare.Web.Entities"

            return typeList;
        }


        private static string BuildTypeList(Type[] types)
        {
            var sb = new StringBuilder();
            foreach (var type in types)
            {
                sb.AppendLine($"{indent}{type.Name}");

                foreach (var prop in type.GetProperties())
                {
                    sb.AppendLine($"{indent}{indent}{prop.Name + ":",pad} {{}}");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

    }
}
