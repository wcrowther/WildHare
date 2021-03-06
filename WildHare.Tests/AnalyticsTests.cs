using AngleSharp.Css.Dom;
using AngleSharp.Css.Parser;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using WildHare.Tests.Models;
using static System.Environment;

namespace WildHare.Tests
{
    [TestFixture]
    public class AnalyticsTests
    {
        const int columnWidth = -10;
        string approot = "";

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appSettings.json")
                            .Build();

            approot = config["App:Root"];
        }

        [Test]
        public void Test_GetAll_CsHtml_Files_And_Parse_With_AngleSharp()
        {
            string pathToWriteTo = $@"{approot}\WildHare\WildHare.Web\Analytics\WildHare.Web\Css-Summary-By-File.txt";
            string pathRoot      = $@"{approot}\WildHare\WildHare.Web\Pages";
            var allFiles         = $@"{pathRoot}".GetAllFiles("*.cshtml");

            var sb = new StringBuilder();
            sb.AppendLine("=".Repeat(100));
            sb.AppendLine($"CSS Summary Report for .cshtml files{NewLine}Inline Styles, Classes, & Stylesheets by Folder / FileName");
            sb.AppendLine($"For Files Under {pathRoot}");

            sb.AppendLine("=".Repeat(100) + NewLine + $"{"FOLDER", columnWidth} FILENAME");

            foreach (var file in allFiles)
            {
                GetStyleInfoForFile(sb, file);
            }

            if (allFiles.Count() > 0)
                sb.AppendLine("=".Repeat(100));

            sb.ToString().WriteToFile(pathToWriteTo, true);

            Assert.AreEqual(11, allFiles.Count());  // 10 for WildHare //30 for SeedPacket
        }


        [Test]
        public void Test_List_of_CSS_Stylesheets_In_The_Project()
        {
            string pathToWriteTo = $@"{approot}\WildHare\WildHare.Web\Analytics\WildHare.Web\List-Of-Stylesheets-With-Selectors.txt";
            string pathRoot      = $@"{approot}\WildHare\WildHare.Web\wwwroot";
            var allFiles         = $@"{pathRoot}"
                                    .GetAllFiles("*.css")
                                    .Where(w => !w.Name.Contains("bootstrap", StringComparison.OrdinalIgnoreCase) &&
                                                !w.Name.Contains("min", StringComparison.OrdinalIgnoreCase));

            var sb = new StringBuilder();
            sb.AppendLine("=".Repeat(100) + NewLine);
            sb.AppendLine( $"CSS Stylesheets In The Project");
            sb.AppendLine($"Under {pathRoot} (omits bootstrap and min files){NewLine}");
            sb.AppendLine($"Generated Using a CodeTemplate On {DateTime.Now}");

            foreach (var file in allFiles)
            {
                GetStylesheetInfo(sb, file);
            }

            if (allFiles.Count() > 0)
                sb.AppendLine("=".Repeat(100));

            sb.ToString().WriteToFile(pathToWriteTo, true);

            Assert.AreEqual(1, allFiles.Count());  // 7 files currently
        }

        [Test]
        public void Test_List_of_CSS_Classes_Used_In_The_Project()
        {
            string pathToWriteTo    = $@"{approot}\WildHare\WildHare.Web\Analytics\WildHare.Web\List-Of-CSS-Classes.txt";
            string pathRoot         = $@"{approot}\WildHare\WildHare.Web\Pages";
            var allFiles            = $@"{pathRoot}"
                                        .GetAllFiles("*.cshtml");

            var classTags = new List<ClassTag>();

            string start = "\t\t   ";
            var sb = new StringBuilder();

            sb.AppendLine("=".Repeat(100) + NewLine + "Css List of Classes Used in the Project");
            sb.AppendLine($"For All Files Under {pathRoot}" );
            sb.AppendLine($"Generated On {DateTime.Now}");

            foreach (var file in allFiles)
            {
                GetClassTagList(sb, file, classTags);
            }

            var groupedClassTags = classTags.OrderBy(o => o.Name)
                                            .GroupBy(g => g.Name, g => g.Reference,
                                                    (name, refs) => new ClassTag { Name = name, References = refs.ToList() });
            
            if (groupedClassTags.Count() > 0)
            {
                sb.AppendLine(start + "-".Repeat(90));
                sb.AppendLine($"{start}{groupedClassTags.Count()} class references");
                sb.AppendLine(start + "-".Repeat(90));
            }

            foreach (var classTag in groupedClassTags)
            {
                sb.AppendLine($"{start}{classTag.Name, -40} count: {classTag.References.Count} ");
            }


            if (groupedClassTags.Count() > 0)
                sb.AppendLine("=".Repeat(100));

            sb.ToString().WriteToFile(pathToWriteTo, true);

            Assert.AreEqual(11, allFiles.Count());  // 10 for WildHare //30 for SeedPacket
        }


        // =======================================================================
        // PRIVATE FUNCTIONS
        // =======================================================================

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

        private static void GetStylesheetInfo(StringBuilder sb, FileInfo file)
        {
            // https: //www.meziantou.net/a-tool-to-help-you-identifying-unused-css-rules.htm
            // https: //stackoverflow.com/questions/59219106/parsing-css-with-anglesharp
            // https: //csharp.hotexamples.com/examples/AngleSharp.Parser.Css/CssParser/-/php-cssparser-class-examples.html

            string start = "\t";
            string source = File.ReadAllText(file.FullName);

            var parser = new CssParser(new CssParserOptions { });
            var styleSheet = parser.ParseStyleSheet(source);

            sb.AppendLine("=".Repeat(100));
            sb.AppendLine($"{file.Directory.Name,-10} {file.Name}");
            sb.AppendLine("=".Repeat(100));

            //=========================================================================
            // Get Css for file

            var selectorList = new List<Selector>();

            foreach (ICssStyleRule rule in styleSheet.Rules.Where(w => w.Type == CssRuleType.Style))
            {
                var selectors = GetAllSelectors(rule.Selector);
                foreach (var selector in selectors)
                {
                    selectorList.Add(new Selector(selector.Text));
                }
            }

            //=========================================================================
            //  Group selectors and write file

            var selectorGroups = selectorList.OrderBy(o => o.Main)
                                             .GroupBy(g => g.Type)
                                             .ToDictionary(g => g.Key.ToString(), g => g.ToList());

            foreach (var group in selectorGroups.OrderBy(o => o.Key))
            {
                sb.AppendLine($"{NewLine}{group.Key} selector{NewLine}");
                foreach (var selector in group.Value)
                {
                    sb.AppendLine($"{start}{selector.Main,-50} {selector.Secondary}"); // {rule.Selector.Specificity}
                }
            }

            sb.AppendLine("");
        }

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


        // Get all selectors of a CSS rule
        // ".a, .b, .c" contains 3 selectors ".a", ".b" and ".c"
        private static IEnumerable<ISelector> GetAllSelectors(ISelector selector)
        {
            if (selector is IEnumerable<ISelector> selectors)
            {
                foreach (var innerSelector in selectors)
                    foreach (var s in GetAllSelectors(innerSelector))
                        yield return s;
            }
            else
            {
                yield return selector;
            }
        }
    }
}

