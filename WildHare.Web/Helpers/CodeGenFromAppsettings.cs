using AngleSharp.Common;
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
using WildHare.Web;
using static System.Environment;

namespace CodeGen
{
    public class CodeGenFromAppsettings
    {
        private static readonly string indent = "\t\t"; 

        public static string Init(IEnumerable<string> appSettings, bool overwrite)
        {
            string writeToFilePath = "C:\\Git\\WildHare\\WildHare.Web\\Entities\\Appsettings.cs";

            var sb = new StringBuilder();
            sb.AppendLine
            (
                $@"
                namespace Me2.Models
                {{
                    public class AppSettings
                    {{
                        { WriteSettings(appSettings) }
                    }}
                }}"
                .RemoveIndents(false) 
            )
            .ToString()
            
            .WriteToFile(writeToFilePath, overwrite);

            bool success  = sb.ToString()
                              .WriteToFile(writeToFilePath, overwrite);

            string result = $"{nameof(CodeGenFromAppsettings)}.{nameof(Init)} code written to '{writeToFilePath}'.{NewLine}" +
                            $"Success: {success}{NewLine}" +
                            $"Overwrite: {overwrite}{NewLine}";
            return result;
        }

        private static string WriteSettings(IEnumerable<string> appProps)
        {
            var sb = new StringBuilder();
            foreach (var prop in appProps)
            {
                sb.AppendLine($"{indent}public string {prop} {{ get; set; }}{NewLine}");
            }
            return sb.ToString()
                     .RemoveStartEnd(indent, NewLine + NewLine);
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