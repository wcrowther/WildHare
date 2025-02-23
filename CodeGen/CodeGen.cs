using CodeGen.Entities;
using CodeGen.Generators;
using CodeGen.Models;
using System;
using System.Linq;
using WildHare.Extensions;
using static System.Console;
using static System.Environment;

namespace CodeGen
{
	public class CodeGen(App _app)
	{
		readonly static string _divider = "=".Repeat(60);
		static string _message;

		public bool GenerateMenu()
		{
			DisplayMenu();

			string inputStr = ReadLine();

			if (inputStr.IsNullOrSpace())
				return true; // keep open

			if (inputStr.EqualsAnyIgnoreCase("exit", "x"))
			{
				WriteLine($"{NewLine}--> Exiting console...");
				return false; // close window
			}

			var inputs = inputStr.Split(" ", true, true);
			var @params = inputs.Skip(1).ToArray();

			if(_app.ClearConsole) Clear();

			_message = inputs[0].ToInt() switch
			{
				1 => new CodeGenAdapters(_app).Init(typeof(Account)),
				// 2 => new CodeGenAdapters2(_app).Init(typeof(Account)),
				2 => new CodeGenPartialsSummary(_app).Init(),
				3 => new CodeGenCssStylesheets(_app).Init(),
				4 => new CopyEntitiesToModelsFolder(_app).Init(),
				5 => new CodeGenValidators().Init("","",true), // Needs work
				9 => $"Choice 9 - params: {@params.AsString("\", \"").AddStartEnd("\"")}",
				_ => $"Your input {inputStr} is not valid.",
			};

			return _app.ConsoleRemainOpen;
		}

		public static void DisplayMenu()
		{
			string menu =
			$"""

				 {_divider}        
				 Generate Code - Enter a number (or x to Exit)
				 {_divider}
				 
				 1) Generate Adapters
				 2) Partials Summary Report
				 3) List Of Stylesheets
				 4) Copy Entities to Models Folder
				 5) Generate JS validators
				 x) Exit
				 
				 {_message}
				 
				 Select an option: 
				""";

			Write(menu);
		}
	}
}








// 5 => CodeGenCssClassesUsedInProject.Init(sourceRoot, writeToRoot + _app.CssClassesFilename, overwrite),
// 6 => CodeGenFromAppsettings.Init(_config, "app", codeGenTempPath, overwrite),
// 7 => CodeGenSummary.Init(_app.SourceRoot, @"C:\Git\WildHare\Temp\MECodeSummary.txt", overwrite),
// 8 => "This choice has not been configured", // CodeGenFromSql.Init(@"c:\Temp\Models", "TestNamespace", _config.GetConnectionString("MachineEnglishDB"), true),

