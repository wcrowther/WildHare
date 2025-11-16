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
	string result = CodeGenAdaptersList.Init(app.adaptersSettings); OR
	string result = new CodeGenAdaptersList(app.adaptersSettings).Generate();
	========================================================================== */

public class CodeGenAdaptersList(AdaptersSetting adaptersSettings)
{
	public string Init ()
	{
		string mapNamespace1 = adaptersSettings.MapNamespace1;
		var adapterList		 = TypeExts.GetTypesInNamespace(mapNamespace1);

		var adapterListTemplate = AdaptersListTemplate(adapterList, adaptersSettings.AdapterSuffix);
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

	public static string Generate(AdaptersSetting adaptersSettings)
	{
		return new CodeGenAdaptersList(adaptersSettings).Init();
	}

	// ==================================================================================

	private string AdaptersListTemplate(Type[] typeList, string suffix)
	{
		string output =
		$$"""
		using {{adaptersSettings.MapNamespace1}};
		using {{adaptersSettings.MapNamespace2}};

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

	private string AdapterListOutputFile => Path.Combine(adaptersSettings.ProjectRoot, adaptersSettings.AdapterListOutputFile);

}
