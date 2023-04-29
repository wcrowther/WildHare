using AngleSharp.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;
using WildHare.Web;
using static System.Environment;

namespace CodeGen
{
    public class CodeGenFromAppsettings
    {
        private static readonly string indent = "\t\t";
        private static readonly string namespaceStr = "Me2.Models";

        public static string Init(Dictionary<string,string> appSettings, string writeToFilePath, bool overwrite)
        {
            bool success =
            $@"
            namespace { namespaceStr }
            {{
                public class AppSettings
                {{
                    { GenerateSettings(appSettings) }
                }}
            }}"
            .RemoveIndents(false)
            .WriteToFile(writeToFilePath, overwrite);

            return Result($"{nameof(CodeGenFromAppsettings)}.{nameof(Init)}", writeToFilePath, success, overwrite);
        }

        private static string GenerateSettings(Dictionary<string, string> appProps)
        {
            var sb = new StringBuilder();
            foreach (var prop in appProps)
            {
                sb.AppendLine($"{indent}public {prop.Value.BasicTypeNameFromValue()} {prop.Key} {{ get; set; }}{NewLine}");
            }
            return sb.ToString()
                     .RemoveStartEnd(indent, NewLine + NewLine);
        }

        private static string Result(string methodName, string writeToFilePath, bool success, bool overwrite)
        {
            return $"{methodName} code written to '{writeToFilePath}'.{NewLine}" +
                   $"Success: {success}{NewLine}" +
                   $"Overwrite: {overwrite}{NewLine}";
        }
    }
}




// ==============================================================================
// ALT: CODE FOR BUILDING FROM JSON APPSETTINGS (remove Init param)
// ==============================================================================
// string appSettingsPath   = "C:\\Git\\MachineEnglish\\ME2\\appsettings.json";
// var json                 = File.ReadAllText(appSettingsPath);
// var doc                  = JsonDocument.Parse(json);
// var appProps             = doc.RootElement.GetProperty("App");
// var appSettings          = new List<string>();
//
// foreach (var prop in appProps.EnumerateObject())
// {
//     appSettings.Add(prop.Name);
// }
//
// ==============================================================================
// Possible alternate version instead of method
// ==============================================================================
// { appSettings.Aggregate(new StringBuilder(), (sb, prop) =>
//   sb.AppendLine($"{indent}public string {prop} {{ get; set; }}{NewLine}"))
//     .ToString().RemoveStartEnd(indent, NewLine + NewLine) }