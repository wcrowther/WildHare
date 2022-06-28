using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using static System.Environment;

namespace WildHare.Web
{
    public static class CodeGenCssSummaryReport
    {
        const string start = "\t";
        const int columnWidth = -10;

        public static string Init(string sourceFolderRootPath, string writeToFilePath, bool overwrite = true)
        {
            if (sourceFolderRootPath.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenCssSummaryReport)}.{nameof(Init)} sourceFolderPath is null or empty.");

            if (writeToFilePath.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenCssSummaryReport)}.{nameof(Init)} writeToFilePath is null or empty.");

            var classTags = new List<ClassTag>();

            var allFiles = $@"{sourceFolderRootPath}"
                .GetAllFiles("*.cshtml");

            var sb = new StringBuilder();
            sb.AppendLine("=".Repeat(100));
            sb.AppendLine($"CSS Summary Report for .cshtml files{NewLine}Inline Styles, Classes, & Stylesheets by Folder/FileName");
            sb.AppendLine($"For Files Under {sourceFolderRootPath}");
            sb.AppendLine($"Generated Using CodeTemplate {nameof(CodeGenCssSummaryReport)} On {DateTime.Now}");

            sb.AppendLine("=".Repeat(100) + NewLine + $"{"FOLDER",columnWidth} FILENAME");

            foreach (var file in allFiles)
            {
                GetStyleInfoForFile(sb, file);
            }

            if (allFiles.Count() > 0)
                sb.AppendLine("=".Repeat(100));

            bool success = sb.ToString()
                             .WriteToFile(writeToFilePath, overwrite);

            string result = $"{nameof(CodeGenCssSummaryReport)}.{nameof(Init)} code written to {NewLine}" +
                            $"'{writeToFilePath}'.{NewLine}" +
                            $"Success: {success}{NewLine}" +
                            $"Overwrite: {overwrite}{NewLine}";

            Debug.WriteLine(result);
            Console.WriteLine(result);

            return result;
        }


        private static void GetStyleInfoForFile(StringBuilder sb, FileInfo file)
        {
            string start = "\t\t   ";
            string source = File.ReadAllText(file.FullName);

            var parser = new HtmlParser(new HtmlParserOptions { IsKeepingSourceReferences = true });
            var doc = parser.ParseDocument(source);

            sb.AppendLine("=".Repeat(100));
            sb.AppendLine($"{file.Directory.Name, columnWidth} {file.Name}");

            // ---------------------------------------------------
            // Get inline style info

            var styles = doc.QuerySelectorAll("*[style]");

            if (styles.Count() > 0)
            {
                sb.AppendLine(start + "-".Repeat(90));
                sb.AppendLine($"{start}{styles.Count()} inline style references");
                sb.AppendLine(start + "-".Repeat(90));
            }

            foreach (var style in styles)
            {
                string lineNumber = $"line {style.SourceReference.Position.Line}";
                sb.AppendLine($"{start}{lineNumber} : {style.GetAttribute("style")} ");
            }

            // ---------------------------------------------------
            // Get stylesheets

            var styleImports = doc.QuerySelectorAll("link[rel=stylesheet]");

            if (styleImports.Count() > 0)
            {
                sb.AppendLine(start + "-".Repeat(90));
                sb.AppendLine($"{start}{styleImports.Count()} stylesheet references");
                sb.AppendLine(start + "-".Repeat(90));
            }

            foreach (var import in styleImports)
            {
                string lineNumber = $"line {import.SourceReference.Position.Line}";

                sb.AppendLine($"{start}{lineNumber} : {import.GetAttribute("href")}");
            }

            // ---------------------------------------------------
            // Get class info

            var classTagList = doc.QuerySelectorAll("*[class]");

            if (classTagList.Count() > 0)
            {
                sb.AppendLine(start + "-".Repeat(90));
                sb.AppendLine($"{start}{classTagList.Count()} class references");
                sb.AppendLine(start + "-".Repeat(90));
            }

            foreach (var classTag in classTagList)
            {
                string lineNumber = $"line {classTag.SourceReference.Position.Line}";

                var classTagArray = classTag.GetAttribute("class").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (var @class in classTagArray)
                {
                    sb.AppendLine($"{start}{lineNumber} : {@class} ");
                }
            }

            // ---------------------------------------------------
            // Get .net Style.Render references

            var textLines = source.Split('\n')
                            .Select((x, lineNum) => $"line {lineNum}: {x.TrimStart()}")
                            .Where(w => w.Contains("@Styles.Render"))
                            .Select(s => $"{start}{s.Truncate(150).EnsureEnd("\n")}");

            if (textLines.Count() > 0)
                sb.AppendLine(start + "-".Repeat(90));

            sb.Append(string.Join("", textLines));

            // ---------------------------------------------------
            // Get .net Scripts.Render references (for js)


            var scriptRenderLines = source.Split('\n')
                            .Select((x, lineNum) => $"line {lineNum}: {x.TrimStart()}")
                            .Where(w => w.Contains("@Scripts.Render"))
                            .Select(s => $"{start}{s.Truncate(150).EnsureEnd("\n")}");

            if (scriptRenderLines.Count() > 0)
                sb.AppendLine(start + "-".Repeat(90));

            sb.Append(string.Join("", scriptRenderLines));
        }

    }
}
