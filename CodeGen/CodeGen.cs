using CodeGen.Generators;
using CodeGen.Models;
using System;
using System.Linq;
using WildHare.Extensions;
using static CodeGen.Helpers.CodeHelpers;
using static System.Console;
using static System.Environment;

namespace CodeGen;

public class CodeGen(App app)
{
	static string menuMessage;

	// =====================================================================================

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

		if(app.ClearConsole) 
			Clear();

		var inputs = inputStr.Split(" ", true, true);

		menuMessage = RunCodeGen(inputs);

		return app.ConsoleRemainOpen;
	}

	// =====================================================================================

	public static void DisplayMenu()
	{
		string menu =
		$"""

			 {divider}        
			 Generate Code - Enter a number (or x to Exit)
			 {divider}
			 
			 1) Generate AdaptersSetting List
			 2) Generate AdaptersSetting
			 3) Partials Summary Report
			 4) List Of Stylesheets
			 5) TransformFilesSettings Entities to Models Folder
			 6) Generate JS validators
			 x) Exit
			 
			 {menuMessage}
			 
			 Select an option: 
			""";

		Write(menu);
	}

	// =====================================================================================

	public string RunCodeGen(string[] inputs)
	{
		var @params = inputs.Skip(1).ToArray();

		int inputNumber = inputs[0].ToInt();

		return inputNumber switch
		{
			1 => CodeGenAdaptersList.Generate(app.AdaptersSettings),
			2 => new CodeGenAdapters(app).Init(),
			3 => new CodeGenPartialsSummary(app).Init(),
			4 => new CodeGenCssStylesheets(app).Init(),
			5 => new TransformFilesToFolder(app).Init(),
			6 => CodeGenValidators.Generate(app),
			7 => new CodeGenClassesFromSqlTables().Init("", ""),       // Needs work
			9 => $"Choice 9 - params: {@params.AsString("\", \"").AddStartEnd("\"")}",
			_ => $"Your input '{inputs.AsString(" ")}' is not valid.",
		};
	}

}








// 5 => CodeGenCssClassesUsedInProject.Init(sourceRoot, writeToRoot + _appSettings.CssClassesFilename, overwrite),
// 6 => CodeGenFromAppsettings.Init(_config, "app", codeGenTempPath, overwrite),
// 7 => CodeGenSummary.Init(_appSettings.SourceRoot, @"C:\Git\WildHare\Temp\MECodeSummary.txt", overwrite),
// 8 => "This choice has not been configured", // CodeGenFromSql.Init(@"c:\Temp\Models", "TestNamespace", _config.GetConnectionString("MachineEnglishDB"), true),

