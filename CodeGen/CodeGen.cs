using CodeGen.Generators;
using CodeGen.Models;
using System;
using System.Linq;
using WildHare.Extensions;
using static CodeGen.Helpers.CodeHelpers;
using static System.Console;
using static System.Environment;

namespace CodeGen;

public class CodeGen(AppSettings appSettings)
{
	static string menuMessage;

	public bool GenerateMenu()
	{
		DisplayMenu();

		string inputStr = ReadLine();

		if (inputStr.IsNullOrSpace())
		{
			Clear();
			menuMessage = "Your input was invalid.";
			return true; 
		}

		if (inputStr.EqualsAnyIgnoreCase("exit", "x"))
		{
			WriteLine($"{NewLine}--> Exiting console...");
			return false; // close window
		}

		var inputs = inputStr.Split(" ", true, true);
		var @params = inputs.Skip(1).ToArray();

		if(appSettings.ClearConsole) 
			Clear();

		menuMessage = inputs[0].ToInt() switch
		{
			1 => new CodeGenAdaptersList(appSettings).Init(),
			2 => new CodeGenAdapters(appSettings).Init(),
			3 => new CodeGenPartialsSummary(appSettings).Init(),
			4 => new CodeGenCssStylesheets(appSettings).Init(),
			5 => new TransformFilesToFolder(appSettings).Init(),
			6 => CodeGenValidators.Init(appSettings), 
			7 => new CodeGenClassesFromSqlTables().Init("",""),       // Needs work
			9 => $"Choice 9 - params: {@params.AsString("\", \"").AddStartEnd("\"")}",
			_ => $"Your input {inputStr} is not valid.",
		};

		return appSettings.ConsoleRemainOpen;
	}

	public static void DisplayMenu()
	{
		string menu =
		$"""

			 {divider}        
			 Generate Code - Enter a number (or x to Exit)
			 {divider}
			 
			 1) Generate Adapters List
			 2) Generate Adapters
			 3) Partials Summary Report
			 4) List Of Stylesheets
			 5) TransformFiles Entities to Models Folder
			 6) Generate JS validators
			 x) Exit
			 
			 {menuMessage}
			 
			 Select an option: 
			""";

		Write(menu);
	}
}








// 5 => CodeGenCssClassesUsedInProject.Init(sourceRoot, writeToRoot + _appSettings.CssClassesFilename, overwrite),
// 6 => CodeGenFromAppsettings.Init(_config, "app", codeGenTempPath, overwrite),
// 7 => CodeGenSummary.Init(_appSettings.SourceRoot, @"C:\Git\WildHare\Temp\MECodeSummary.txt", overwrite),
// 8 => "This choice has not been configured", // CodeGenFromSql.Init(@"c:\Temp\Models", "TestNamespace", _config.GetConnectionString("MachineEnglishDB"), true),

