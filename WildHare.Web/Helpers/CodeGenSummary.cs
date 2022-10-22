using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WildHare.Extensions;
using static System.Environment;

namespace WildHare.Web
{
    public static class CodeGenSummary
    {
        const int columnWidth = -20;
        static string start = "\t".Repeat(4);
        static string currentDirectoryName = "";

        public static string Init(string sourceFolderRootPath, string writeToFilePath, bool overwrite = true)
        {
            if (sourceFolderRootPath.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenSummary)}.{nameof(Init)} sourceFolderPath is null or empty.");

            if (writeToFilePath.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenSummary)}.{nameof(Init)} writeToFilePath is null or empty.");

            var allFiles = $@"{sourceFolderRootPath}"
                            .GetAllFiles("*.cshtml");

            var sb = new StringBuilder();
            sb.AppendLine("=".Repeat(100));
            sb.AppendLine($"Summary Report for .cshtml files{NewLine}");
            sb.AppendLine($"For Files Under {sourceFolderRootPath}");
            sb.AppendLine($"Generated Using CodeTemplate '{nameof(CodeGenSummary)}' On {DateTime.Now}");

            foreach (var file in allFiles)
            {
                GetPartialsForFile(sb, file);
            }

            if (allFiles.Count() > 0)
                sb.AppendLine("=".Repeat(100));

            bool success = sb.ToString()
                             .WriteToFile(writeToFilePath, overwrite);

            string result = $"{nameof(CodeGenSummary)}.{nameof(Init)} code written to {NewLine}" +
                            $"'{writeToFilePath}'.{NewLine}" +
                            $"Success: {success}{NewLine}" +
                            $"Overwrite: {overwrite}{NewLine}";

            Debug.WriteLine(result);

            return result;
        }


        private static void GetPartialsForFile(StringBuilder sb, FileInfo file)
        {
            string source = File.ReadAllText(file.FullName);

            var parser = new HtmlParser(new HtmlParserOptions { IsKeepingSourceReferences = true });
            var doc = parser.ParseDocument(source);

            if (currentDirectoryName != file.Directory.Name)
            { 
                sb.AppendLine("=".Repeat(100));
                sb.AppendLine($"{file.Directory.Name} Folder");
                sb.AppendLine("=".Repeat(100) + NewLine);
            }

            var partials = doc.QuerySelectorAll("partial");

            if(partials.Count() > 0)
            {
                sb.AppendLine($"\t\t{file.Name, columnWidth}");

                foreach (var p in partials)
                {
                    sb.AppendLine($"{start}Partial: {p.GetAttribute("name")}");
                }

                sb.AppendLine();
            }

            var tags        = doc.QuerySelectorAll("*");
            var customTags  = tags.Where(w => w.LocalName.StartsWith("rp-"));

            if (customTags.Count() > 0)
            {
                sb.AppendLine($"\t\t{file.Name, columnWidth}");

                foreach (var c in customTags)
                {
                    sb.AppendLine($"{start} Custom: {c.LocalName}");
                }

                sb.AppendLine();
            }

            currentDirectoryName = file.Directory.Name;
        }

    }
}
