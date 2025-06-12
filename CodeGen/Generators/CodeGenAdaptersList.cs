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

public partial class CodeGenAdaptersList(AppSettings appSettings)
{
	public string Init ()
	{
		string mapNamespace1 = appSettings.Adapters.MapNamespace1;
		var adapterList		 = TypeExts.GetTypesInNamespace(mapNamespace1);

		var adapterListTemplate = AdaptersListTemplate(adapterList, "Model");
		adapterListTemplate.WriteToFile(AdapterListOutputFile, true);

		return $"ToSuccess. List written to file: {AdapterListOutputFile}";
	}

	public string AdaptersListTemplate(Type[] typeList, string suffix)
	{
		string output =
		$$"""
		using {{appSettings.Adapters.MapNamespace1}};
		using {{appSettings.Adapters.MapNamespace2}};

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
		Debug.WriteLine($"{divider}Creating List of adapters for CodeGenAdapters_RunAdaptersList.cs");

		var sb = new StringBuilder();

		foreach (var type in typeList)
		{
			sb.AppendLine($"\t\tAdaptersTemplate(typeof({type.Name}), typeof({type.Name}{suffix}), true, true);");
		}
		
		Debug.WriteLine(sb.ToString().AddEnd(divider));

		return sb.ToString().RemoveStartEnd("\t",NewLine);
	}

	// ==================================================================================

	private string AdapterListOutputFile => Path.Combine(appSettings.ProjectRoot, appSettings.Adapters.AdapterListOutputFile);

}
