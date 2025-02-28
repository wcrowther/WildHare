using CodeGen.Models;
using System;
using System.Diagnostics;
using System.Text;
using WildHare.Extensions;
using static CodeGen.Helpers.CodeHelpers;
using static System.Environment;


namespace CodeGen.Generators;

public partial class CodeGenAdaptersList(AppSettings appSettings)
{
	const string indent = "\t\t";

	public string Init (Type typeInNamespace)
	{
		var adapterList = typeInNamespace.GetTypesInNamespace("CodeGen.Entities");

		var adapterListTemplate = GenAdapterList(adapterList, "Model");
		adapterListTemplate.WriteToFile(appSettings.Adapter.AdapterListOutputFile, true);

		return "Success";
	}

	public string GenAdapterList(Type[] typeList, string suffix)
	{
		string output =
		$$"""
		using {{appSettings.Adapter.Namespace1}};
		using {{appSettings.Adapter.Namespace2}};

		namespace CodeGen.Generators;
		
		public partial class CodeGenAdapters
		{
			public void RunAdapterList()
			{
			{{GenList(typeList, suffix) }}
			}
		}
		""";

		return output;
	}

	public static string GenList(Type[] typeList, string suffix)
	{
		Debug.WriteLine($"{divider}Creating List of GeneratorAdapters to paste in to CodeGenAdapters_Run.cs");

		var sb = new StringBuilder();

		foreach (var type in typeList)
		{
			sb.Append($"{indent}GenerateAdapter(typeof({type.Name}), typeof({type.Name}{suffix}), true, true);{NewLine}");
		}
		
		string adapterListString = sb.ToString()
									 .RemoveStartEnd("\t", NewLine);

		Debug.WriteLine(adapterListString.AddEnd(divider));

		return adapterListString;
	}
}
