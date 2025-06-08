using CodeGen.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using WildHare.Extensions;
using static CodeGen.Helpers.CodeHelpers;
using static System.Environment;
using Types = WildHare.Extensions.TypeExtensions;

namespace CodeGen.Generators;

public partial class CodeGenAdaptersList(AppSettings appSettings)
{
	private const string indent = "\t\t";

	public string Init ()
	{
		string mapNamespace1 = appSettings.Adapter.MapNamespace1;
		var adapterList = Types.GetTypesInNamespace(mapNamespace1);

		var adapterListTemplate = AdaptersListTemplate(adapterList, "Model");
		adapterListTemplate.WriteToFile(AdapterListOutputFile, true);

		return $"Success. List written to file: {AdapterListOutputFile}";
	}

	public string AdaptersListTemplate(Type[] typeList, string suffix)
	{
		string output =
		$$"""
		using {{appSettings.Adapter.MapNamespace1}};
		using {{appSettings.Adapter.MapNamespace2}};

		namespace CodeGen.Generators;
		
		public partial class CodeGenAdapters
		{
			public void RunAdaptersList()
			{
			{{GenAdaptersList(typeList, suffix) }}
			}
		}
		""";

		return output;
	}

	public static string GenAdaptersList(Type[] typeList, string suffix)
	{
		Debug.WriteLine($"{divider}Creating List of GeneratorAdapters to paste in to CodeGenAdapters_Run.cs");

		var sb = new StringBuilder();

		foreach (var type in typeList)
		{
			sb.Append($"{indent}AdaptersTemplate(typeof({type.Name}), typeof({type.Name}{suffix}), true, true);{NewLine}");
		}
		
		string adapterListString = sb.ToString().RemoveStartEnd("\t", NewLine);

		Debug.WriteLine(adapterListString.AddEnd(divider));

		return adapterListString;
	}

	// ==================================================================================

	private string AdapterListOutputFile => Path.Combine(appSettings.ProjectRoot, appSettings.Adapter.AdapterListOutputFile);

}
