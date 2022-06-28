using Microsoft.Extensions.Configuration;
using System;
using WildHare.Extensions;
using WildHare.Web;
using WildHare.Web.Models;
using static System.Environment;

namespace WildHare.CodeGen
{
    public class CodeGen
    {
        private readonly AppSettings _app;
        private readonly IConfiguration _config;

        public CodeGen(AppSettings appSettings, IConfiguration configuration)
        {
            _app = appSettings;
            _config = configuration;
        }

        public static void GenerateMenu()
        {
            string line = "=".Repeat(60);

            // Console.Clear();
            Console.WriteLine(line);
            Console.WriteLine("Choose an option:");
            Console.WriteLine(line);
            Console.WriteLine("1) CSS Summary Report");
            Console.WriteLine("2) List Of Stylesheets");
            Console.WriteLine("3) List Of CSS Classes");
            Console.WriteLine("4) Generate Models for each table in SQL DB");
            Console.WriteLine("x) Exit");
            Console.Write("\r\nSelect an option: ");
        }

        public bool Generate(string input)
        {
            bool remainOpen     = true;
            bool overwrite      = _app.CodeGenOverwrite;
            string sourceRoot   = _app.SourceFolderRootPath;
            string wwwRoot      = _app.WwwFolderRootPath;
            string writeToRoot  = _app.CssWriteToFolderPath;

            switch (input.ToLower())
            {
                case "1":
                    CodeGenCssSummaryReport.Init(sourceRoot, writeToRoot + _app.CssSummaryByFileName_Filename, overwrite);
                    break;
                case "2":
                    CodeGenCssStylesheets.Init(wwwRoot, writeToRoot + _app.CssListOfStylesheets_Filename, overwrite);
                    break;
                case "3":
                    CodeGenCssClassesUsedInProject.Init(sourceRoot, writeToRoot + _app.CssListOfClasses_Filename, overwrite);
                    break;
                case "4":
                    CodeGenFromSql.Init(@"c:\Temp\Models", "TestNamespace", _config.GetConnectionString("MachineEnglishDB"), true);
                    break;
                case "exit": case "x":
                    Console.WriteLine($"{NewLine}--> Exiting console...");
                    remainOpen = false;
                    break;
                default:
                    Console.WriteLine("That input is not valid.");
                    break;
            }
            return remainOpen;
        }


    }
}
