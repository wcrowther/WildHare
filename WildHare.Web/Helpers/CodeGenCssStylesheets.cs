using AngleSharp.Css.Dom;
using AngleSharp.Css.Parser;
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
    public static class CodeGenCssStylesheets
    {
        const string start = "\t";
        const int columnWidth = -10;

        public static string Init(string sourceFolderRootPath, string writeToFilePath, bool overwrite = true)
        {
            if (sourceFolderRootPath.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenCssSummaryReport)}.{nameof(Init)} sourceFolderPath is null or empty.");

            if (writeToFilePath.IsNullOrEmpty())
                throw new ArgumentNullException($"{nameof(CodeGenCssSummaryReport)}.{nameof(Init)} writeToFilePath is null or empty.");

            var allFiles = $@"{sourceFolderRootPath}"
                                    .GetAllFiles("*.css")
                                    .Where(w => !w.Name.Contains("bootstrap", StringComparison.OrdinalIgnoreCase) &&
                                                !w.Name.Contains("min", StringComparison.OrdinalIgnoreCase));

            var sb = new StringBuilder();
            sb.AppendLine("=".Repeat(100));
            sb.AppendLine($"CSS Stylesheets In The Project");
            sb.AppendLine($"Under {sourceFolderRootPath} (omits bootstrap and min files){NewLine}");
            sb.AppendLine($"Generated Using CodeTemplate {nameof(CodeGenCssSummaryReport)} On {DateTime.Now}");

            foreach (var file in allFiles)
            {
                GetStylesheetInfo(sb, file);
            }

            bool success =  sb.ToString()
                              .WriteToFile(writeToFilePath, overwrite);

            string result = $"{nameof(CodeGenCssSummaryReport)}.{nameof(Init)} code written to {NewLine}" +
                            $"'{writeToFilePath}'.{NewLine}" +
                            $"Success: {success}{NewLine}" +
                            $"Overwrite: {overwrite}{NewLine}";

            Debug.WriteLine(result);
            Console.WriteLine(result);

            return result;
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

    public class Selector
    {
        public Selector(string rawSelector)
        {
            RawText = rawSelector;
        }

        public string RawText { get; set; }

        public string Main
        {
            get
            {
                return RawText.GetStartBefore(" ", true).RemoveStart(new[] { ".", "#" });
            }
        }

        public string Secondary
        {
            get
            {
                string start = RawText.GetStartBefore(" ", true);
                return RawText.RemoveStart(start);
            }
        }

        public SelectorType Type
        {
            get
            {
                char firstChar = RawText.Trim().CharAt(0);

                if (firstChar == '.')
                    return SelectorType.@class;
                else if (firstChar == '#')
                    return SelectorType.id;
                else if (char.IsLetter(firstChar))
                    return SelectorType.element;
                else
                    return SelectorType.other;
            }
        }

        public string Specificity { get; set; }

        public string Location { get; set; }

        public override string ToString()
        {
            return $"{RawText} {Type} in: {Location}";
        }
    }

    public enum SelectorType
    {
        id,
        @class,
        element,
        other
    }
}
