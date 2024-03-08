using AngleSharp.Html.Parser;
using CodeGen.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using WildHare.Extensions;
using static System.Environment;

namespace CodeGen.Generators
{
    public class CodeGenPartialsSummary
    {
        private App _app;
        private const int columnWidth           = -10;
        private string currentDirectoryName     = "";
        private string start                    = "\t".Repeat(4);

        public CodeGenPartialsSummary(App app)
        {
            _app        = app;
        }

        public string Init()
        {
            string writeToFilePath = $@"{_app.WriteToRoot}{_app.PartialsSummaryFileName}";
            string start = _app.LineStart.Repeat(4);

            if (_app.SourceRoot.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenPartialsSummary)}.{nameof(Init)} sourceFolderPath is null or empty.");

            if (writeToFilePath.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenPartialsSummary)}.{nameof(Init)} writeToFilePath is null or empty.");

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"// Summary of Partials for .cshtml files");
            sb.AppendLine($"// For Files Under {_app.SourceRoot}");
            sb.AppendLine($"// Generated Using CodeTemplate '{nameof(CodeGenPartialsSummary)}' On {DateTime.Now}{NewLine}");

            var allFiles = _app.SourceRoot
                               .GetAllFiles("*.cshtml");

            foreach (var file in allFiles)
            {
                GetPartialsForFile(sb, file);
            }

            if (allFiles.Count() > 0)
                sb.AppendLine("=".Repeat(100));
            else
                sb.AppendLine("No Files To Process");

            bool success = sb.ToString()
                             .WriteToFile(writeToFilePath, _app.Overwrite);

            string result = $"{nameof(CodeGenPartialsSummary)}.{nameof(Init)} code written to {NewLine}" +
                            $"'{writeToFilePath}'.{NewLine}" +
                            $"Success: {success}{NewLine}" +
                            $"Overwrite: {_app.Overwrite}{NewLine}";

            Debug.WriteLine(result);

            return result;
        }


        private void GetPartialsForFile(StringBuilder sb, FileInfo file)
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

            if (partials.Count() > 0)
            {
                sb.AppendLine($"\t{file.Name, columnWidth}");

                foreach (var p in partials)
                {
                    sb.AppendLine($"{start}Partial: {p.GetAttribute("name")}");
                }

                sb.AppendLine();
            }

            var tags = doc.QuerySelectorAll("*");
            var customTags = tags.Where(w => w.LocalName.StartsWith("rp-"));

            if (customTags.Count() > 0)
            {
                sb.AppendLine($"\t{file.Name, columnWidth}");

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
