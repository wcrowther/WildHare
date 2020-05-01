using AngleSharp;
using AngleSharp.Css.Dom;
using AngleSharp.Css.Parser;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Io;
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
        [Test]
        public void Test_GetAll_CsHtml_Files_And_Parse_With_AngleSharp()
        {
            // string pathRoot = @"C:\Code\Trunk\WildHare\WildHare.Web\Pages";
            // string pathToWriteTo = $@"{pathRoot}\AllCsHtmlFiles.txt";

            string pathToWriteTo = @"C:\Code\Trunk\WildHare\WildHare.Web\Analytics\SeedPacketCss.txt";
            string pathRoot = @"C:\Code\Trunk\SeedPacket\Examples\Views";

            var allFiles = $@"{pathRoot}".GetAllFiles("*.cshtml");

            var sb = new StringBuilder();
            sb.AppendLine("=".Repeat(100) + "\nCSS Report - Style and Stylesheets (generated from AnalyticsTests.cs)\n");

            foreach (var file in allFiles)
            {
                GetStyleInfoForFile(sb, file);
            }

            if (allFiles.Count() > 0)
                sb.AppendLine("=".Repeat(100));

            sb.ToString().WriteToFile(pathToWriteTo, true);

            Assert.AreEqual(31, allFiles.Count());  // 10 for WildHare //30 for SeedPacket
        }


        [Test]
        public void Test_GetAll_Css_Stylesheets()
        {
            string pathToWriteTo = @"C:\Code\Trunk\WildHare\WildHare.Web\Analytics\SeedPacket_Stylesheets.txt";
            string pathRoot = @"C:\Code\Trunk\SeedPacket\Examples\Content";
            var allFiles = $@"{pathRoot}".GetAllFiles("*.css").Where(w => !w.Name.Contains("bootstrap", StringComparison.OrdinalIgnoreCase));

            var sb = new StringBuilder();
            sb.AppendLine("=".Repeat(100) + "\nCSS Stylesheets (generated from AnalyticsTests.cs)\n");

            foreach (var file in allFiles)
            {
                GetStylesheetInfo(sb, file);
            }

            if (allFiles.Count() > 0)
                sb.AppendLine("=".Repeat(100));

            sb.ToString().WriteToFile(pathToWriteTo, true);

            Assert.AreEqual(2, allFiles.Count());  // 7 files currently
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
            sb.AppendLine($"{file.Directory.Name,-10} {file.Name}");

            // ---------------------------------------------------

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

            var classes = doc.QuerySelectorAll("*[class]");

            if (classes.Count() > 0)
            {
                sb.AppendLine(start + "-".Repeat(90));
                sb.AppendLine($"{start}{classes.Count()} class references");
                sb.AppendLine(start + "-".Repeat(90));
            }

            foreach (var @class in classes)
            {
                string lineNumber = $"line {@class.SourceReference.Position.Line}";
                sb.AppendLine($"{start}{lineNumber} : {@class.GetAttribute("class")} ");
            }

            // ---------------------------------------------------

            var textLines = source.Split('\n')
                            .Select((x, lineNum) => $"line {lineNum}: {x.TrimStart()}")
                            .Where(w => w.Contains("@Styles.Render"))
                            .Select(s => $"{start}{s.Truncate(150).EnsureEnd("\n")}");

            if (textLines.Count() > 0)
                sb.AppendLine(start + "-".Repeat(90));

            sb.Append(string.Join("", textLines));

            // ---------------------------------------------------

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

