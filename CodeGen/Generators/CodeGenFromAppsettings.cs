using AngleSharp.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;
using static System.Environment;

namespace CodeGen.Generators
{
	public class CodeGenFromAppsettings
    {
        private static readonly string indent = "\t\t";
        private static readonly string namespaceStr = "Me2.Models";

        public static string Init(IConfiguration configuration, string appSectionName, string writeToFilePath, bool overwrite)
        {
            var appSettings = GetAppSectionDictionary(configuration, appSectionName);

            if (appSettings == null || appSettings.Count == 0)
            {
				throw new ArgumentNullException(nameof(appSettings));
            }

            bool success =
            $$"""
            namespace {{namespaceStr}}
            {
                public class AppSettings
                {
                    {{GenerateSettings(appSettings)}}
                }
            }
            """
            .WriteToFile(writeToFilePath, overwrite);

            return Result($"{nameof(CodeGenFromAppsettings)}.{nameof(Init)}", writeToFilePath, success, overwrite);
        }

        private static Dictionary<string, string> GetAppSectionDictionary(IConfiguration configuration, string appSectionName)
        {
            var appSettings = configuration.GetSection(appSectionName)
                                           .AsEnumerable(true)
                                           .OrderBy(o => o.Key)
                                           .ToDictionary(a => a.Key, a => a.Value);
            return appSettings;
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
                   $"ToSuccess: {success}{NewLine}" +
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
// var appProps             = doc.RootElement.GetProperty("AppSettings");
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