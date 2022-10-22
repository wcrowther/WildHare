using Microsoft.Extensions.Configuration;
using System;
using WildHare.Extensions;
using WildHare.Web;
using WildHare.Web.Models;
using static System.Environment;
using static System.Console;

namespace WildHare.CodeGen
{
    public class CodeGen
    {
        private readonly AppSettings _app;
        private readonly IConfiguration _config;

        public CodeGen(AppSettings appSettings, IConfiguration configuration)
        {
            _app    = appSettings;
            _config = configuration;
        }

        public static void GenerateMenu()
        {
            string divider = "=".Repeat(60);
            string menu =    $@"
                   {divider}        
                   Choose an option:
                   {divider}
                   1) CSS Summary Report
                   2) List Of Stylesheets
                   3) List Of CSS Classes
                   4) Generate Models for each table in SQL DB
                   5) Generate Summary
                   x) Exit

                   Select an option: ";

            Write(menu.RemoveIndents());
        }

        public bool Generate(string input)
        {
            bool remainOpen     = _app.RemainOpenAfterCodeGen;
            bool overwrite      = _app.CodeGenOverwrite;
            string sourceRoot   = _app.SourceFolderRootPath;
            string wwwRoot      = _app.WwwFolderRootPath;
            string writeToRoot  = _app.CssWriteToFolderPath;

            if (input.EqualsAny(true, "exit", "x"))
            {
                Console.WriteLine($"{NewLine}--> Exiting console...");

                return false; // remain open
            }

            string result = input.ToLower() switch
            {
                "1" => CodeGenSummary.Init(sourceRoot, writeToRoot + _app.CssSummaryByFileName_Filename, overwrite),
                "2" => CodeGenCssStylesheets.Init(wwwRoot, writeToRoot + _app.CssListOfStylesheets_Filename, overwrite),
                "3" => CodeGenCssClassesUsedInProject.Init(sourceRoot, writeToRoot + _app.CssListOfClasses_Filename, overwrite),
                "4" => CodeGenFromSql.Init(@"c:\Temp\Models", "TestNamespace", _config.GetConnectionString("MachineEnglishDB"), true),
                "5" => CodeGenSummary.Init(_app.MESourceFolderRootPath, @"C:\Git\WildHare\Temp\MECodeSummary.txt", overwrite),
                 _  => $"The input {input} is not valid.",
            };

            Console.WriteLine(NewLine + result);

            return remainOpen;
        }
    }
}


// Console.Clear();
// WriteLine(divider);
// WriteLine("Choose an option:");
// WriteLine(divider);
// WriteLine("1) CSS Summary Report");
// WriteLine("2) List Of Stylesheets");
// WriteLine("3) List Of CSS Classes");
// WriteLine("4) Generate Models for each table in SQL DB");
// WriteLine("5) Generate Summary");
// WriteLine("x) Exit" + NewLine);
// Write("Select an option: ");