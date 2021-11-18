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

    public static class CodeGenCssMap
    {
        /* ==========================================================================
         * DIRECTIONS:
         * 
         * PLACE FOLLOWING LINE OF CODE SOMEWHERE IT WILL BE RUN ON COMPILE, RUN IN THE IMMEDIATE WINDOW, 
         * or in the .NET Core StartUp Configure() -> passing in env.ContentRootPath

           WildHare.Web.CodeGenCssMap.Init(c:\github\WildHare);
        ========================================================================== */

        public static string Init(string projectRoot)
        {
            if (projectRoot.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenCssMap)}.{nameof(Init)} projectRoot is null or empty.");
            
            bool overWrite = false;
            string pathToWriteTo = $@"{projectRoot}\Analytics\ClassTagList.txt";
            string pathRoot = @"C:\Git\SeedPacket\SeedPacket.Examples\Pages";
            var allFiles = $@"{pathRoot}"
                            .GetAllFiles("*.cshtml");

            var classTags = new List<ClassTag>();

            string start = "\t";
            var sb = new StringBuilder();

            sb.AppendLine("=".Repeat(100));
            sb.AppendLine($"ClassTag List  (generated from CodeGenCssMap.cs on {DateTime.Now}) {NewLine}");

            foreach (var file in allFiles)
            {
                GetClassTagList(sb, file, classTags);
            }

            // ---------------------------------------------------
            // Render ClassTag list

            var groupedClassTags = classTags.OrderBy(o => o.Name)
                                            .GroupBy(g => g.Name, g => g.Reference,
                                                    (name, refs) => new ClassTag { Name = name, References = refs.ToList() }
                                             );

            if (groupedClassTags.Count() > 0)
            {
                sb.AppendLine(start + "-".Repeat(90));
                sb.AppendLine($"{start}{groupedClassTags.Count()} class references");
                sb.AppendLine(start + "-".Repeat(90));
            }

            foreach (var classTag in groupedClassTags)
            {
                sb.AppendLine($"{start}{classTag.Name,-40} count: {classTag.References.Count} ");
            }

            if (groupedClassTags.Count() > 0)
                sb.AppendLine("=".Repeat(100));

            bool success = sb.ToString().WriteToFile(pathToWriteTo, overWrite);

            string result = $"{nameof(CodeGenCssMap)}.{nameof(Init)} code written to '{pathToWriteTo}'. Success: {success} Overwrite: {overWrite}";
            Debug.WriteLine(result);

            return result;
        }


        // =======================================================================
        // PRIVATE FUNCTIONS
        // =======================================================================

        private static void GetClassTagList(StringBuilder sb, FileInfo file, List<ClassTag> classTags)
        {
            string source = File.ReadAllText(file.FullName);

            var parser = new HtmlParser(new HtmlParserOptions { IsKeepingSourceReferences = true });
            var doc = parser.ParseDocument(source);
            var classTagList = doc.QuerySelectorAll("*[class]");

            // ---------------------------------------------------
            // Get class info

            foreach (var classTag in classTagList)
            {
                var classTagArray = classTag.GetAttribute("class").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string lineNumber = $"line {classTag.SourceReference.Position.Line}";

                foreach (var @class in classTagArray)
                {
                    classTags.Add(new ClassTag { Name = @class, Reference = $"{file.Name} {lineNumber}" });
                }
            }
        }
    }


    // =======================================================================
    // CLASSES 
    // =======================================================================

    public class ClassTag
    {

        public string Name { get; set; }

        public string Reference { get; set; }

        public List<string> References { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"{Name} Count: {References.Count}";
        }
    }
}
