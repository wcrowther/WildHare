using CodeGen.Entities;
using CodeGen.Generators;
using CodeGen.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WildHare.Extensions;
using static System.Console;
using static System.Environment;

namespace CodeGen
{
     public class CodeGen
    {
        private readonly App _app;
        private readonly IConfiguration _config;

        public CodeGen(App appSettings, IConfiguration configuration)
        {
            _app    = appSettings;
            _config = configuration;
        }

        public static void GenerateMenu()
        {
            string divider = "=".Repeat(60);
            string menu =    
            $@"
             {divider}        
             Choose an option:
             {divider}
             
             1) Generate Adapters
             2) Partials Summary Report
             3) List Of Stylesheets
             4) Copy Entities to Models Folder
             x) Exit

             Select an option: "
            .RemoveIndents();

            Write(menu);
        }

        public bool Generate(string input)
        {
            if (input.IsNullOrSpace())
                return true; // keep open
            
            if (input.EqualsAnyIgnoreCase("exit", "x"))
            {
                WriteLine($"{NewLine}--> Exiting console...");
                return false; // close window
            }

            var inputArray  = input.Split(' ');
            var parameters  = inputArray.Skip(1).ToArray();

            Clear();

            string result = inputArray[0].ToInt() switch
            {
                1 => new GenAdapters(_app).Init(typeof(Account)),
                2 => new CodeGenPartialsSummary(_app).Init(),
                3 => new CodeGenCssStylesheets(_app).Init(),
                4 => new CopyEntitiesToModelsFolder(_app).Init(),
                // 3 => CodeGenCssClassesUsedInProject.Init(sourceRoot, writeToRoot + _app.CssClassesFilename, overwrite),
                // 4 => CodeGenFromAppsettings.Init(_config, "app", codeGenTempPath, overwrite),
                // 5 => CodeGenSummary.Init(_app.SourceRoot, @"C:\Git\WildHare\Temp\MECodeSummary.txt", overwrite),
                // 6 => "This choice has not been configured", // CodeGenFromSql.Init(@"c:\Temp\Models", "TestNamespace", _config.GetConnectionString("MachineEnglishDB"), true),
                _ => $"The input {input} is not valid.",
            };

            WriteLine(NewLine + result);

            return _app.ConsoleRemainOpen;
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