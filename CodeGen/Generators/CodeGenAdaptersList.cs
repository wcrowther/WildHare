using CodeGen.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using WildHare.Extensions;
using static CodeGen.Helpers.CodeHelpers;
using static System.Environment;
using TypeExts = WildHare.Extensions.TypeExtensions;

namespace CodeGen.Generators;

/*  ==========================================================================
	string result = CodeGenAdaptersList.Init(appSettings); OR
	string result = new CodeGenAdaptersList(appSettings).Generate();
	========================================================================== */

public class CodeGenAdaptersList(AppSettings app)
{
	readonly Adapters settings = app.Adapters;

	public string Init ()
	{
		string mapNamespace1 = settings.MapNamespace1;
		var adapterList		 = TypeExts.GetTypesInNamespace(mapNamespace1);

		var adapterListTemplate = AdaptersListTemplate(adapterList, settings.AdapterSuffix);
		adapterListTemplate.WriteToFile(AdapterListOutputFile, true);

		return $"Success. List written to file: {AdapterListOutputFile}";
	}

	public static string GenAdaptersList(Type[] typeList, string suffix)
	{
		Debug.WriteLine($"{divider}Creating List of adapters for CodeGenAdapters_RunAdaptersList.cs");

		var sb = new StringBuilder();

		foreach (var type in typeList)
		{
			sb.AppendLine($"\t\tAdaptersTemplate(typeof({type.Name}), typeof({type.Name}{suffix}), true, true);");
		}
		
		Debug.WriteLine(sb.ToString().AddEnd(divider));

		return sb.ToString().RemoveStartEnd("\t",NewLine);
	}

	public static string Generate(AppSettings app)
	{
		return new CodeGenAdaptersList(app).Init();
	}

	// ==================================================================================

	private string AdaptersListTemplate(Type[] typeList, string suffix)
	{
		string output =
		$$"""
		using {{settings.MapNamespace1}};
		using {{settings.MapNamespace2}};

		namespace CodeGen.Generators;
		
		public partial class CodeGenAdapters
		{
			public void RunAdaptersList()
			{
			{{GenAdaptersList(typeList, suffix)}}
			}
		}
		""";

		return output;
	}

	private string AdapterListOutputFile => Path.Combine(app.ProjectRoot, settings.AdapterListOutputFile);

}
