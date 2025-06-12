using AngleSharp.Html.Parser;
using CodeGen.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using static System.Environment;

namespace CodeGen.Generators
{
    public class CodeGenPartialsSummary(AppSettings appSettings)
	{
		private const int columnWidth           = -10;
        private string currentDirectoryName     = "";
        //private readonly string start           = "\t".Repeat(2);

		public string Init()
		{
			string writeToFilePath = $@"{appSettings.WriteToRoot}{appSettings.PartialsSummaryFileName}";

			if (appSettings.SourceRoot.IsNullOrEmpty())
				throw new ArgumentNullException($"{nameof(CodeGenPartialsSummary)}.{nameof(Init)} sourceFolderPath is null or empty.");

			if (writeToFilePath.IsNullOrEmpty())
				throw new ArgumentNullException($"{nameof(CodeGenPartialsSummary)}.{nameof(Init)} writeToFilePath is null or empty.");

			var sb			= BuildHeading();
			var allFiles	= appSettings.SourceRoot
								 .GetAllFiles("*.cshtml");

			foreach (var file in allFiles)
			{
				GetPartialsForFile(sb, file);
			}

			if (allFiles.Count > 0)
				sb.AppendLine("=".Repeat(100));
			else
				sb.AppendLine("No Files To Process");

			bool success = sb.ToString()
							 .WriteToFile(writeToFilePath, appSettings.Overwrite);

			string result = $"{nameof(CodeGenPartialsSummary)}.{nameof(Init)} code written to " +
							$"'{writeToFilePath}'.{NewLine}" +
							$" ToSuccess: {success} Overwrite: {appSettings.Overwrite}{NewLine}";

			Debug.WriteLine(result);

			return result;

			StringBuilder BuildHeading()
			{
				var sb = new StringBuilder();
				sb.AppendLine();
				sb.AppendLine($"// Summary of Partials for .cshtml files");
				sb.AppendLine($"// For Files Under {appSettings.SourceRoot}");
				sb.AppendLine($"// Generated Using CodeTemplate '{nameof(CodeGenPartialsSummary)}' On {DateTime.Now}{NewLine}");
				return sb;
			}
		}

		private void GetPartialsForFile(StringBuilder sb, FileInfo file)
        {
            string source	= File.ReadAllText(file.FullName);
            var parser		= new HtmlParser(new HtmlParserOptions { IsKeepingSourceReferences = true });
            var doc			= parser.ParseDocument(source);
            var partials	= doc.QuerySelectorAll("partial");
            var tags		= doc.QuerySelectorAll("*");
            var customTags	= tags.Where(w => w.LocalName.StartsWith("rp-"));

            if (currentDirectoryName != file.Directory.Name)
            {
                sb.AppendLine("=".Repeat(100));
                sb.AppendLine($"{file.Directory.Name} Folder");
                sb.AppendLine("=".Repeat(100) + NewLine);
            }

            if (partials.Length > 0)
            {
                sb.AppendLine($"{file.Name, columnWidth}");

                foreach (var p in partials)
                {
                    sb.AppendLine($"\tPartial: {p.GetAttribute("name")}");
                }

                sb.AppendLine();
            }

            if (customTags.Any())
            {
                sb.AppendLine($"{file.Name, columnWidth}");

                foreach (var c in customTags)
                {
                    sb.AppendLine($"\tCustom: {c.LocalName}");
                }

                sb.AppendLine();
            }

            currentDirectoryName = file.Directory.Name;
        }

    }
}
