using CodeGen.Models;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using WildHare.Extensions;
using static System.Environment;

namespace CodeGen.Generators;

public partial class CodeGenAdapters(AppSettings appSettings)
{
	private readonly string indent		= "\t".Repeat(3);
	private readonly string end			= $",{NewLine}";
	private readonly string separator	= "=".Repeat(50);
	private const int pad = -20;
	private string projectRoot;
	private string outputFolder;

	public string Init (Type typeInNamespace)
	{
		// To Delete:  Array.ForEach(Directory.GetFiles(outputDir), file => File.Delete(file));

		outputFolder = $"{projectRoot}{appSettings.Adapter.OutputFolder}";

		Debug.WriteLine($"{"=".Repeat(80)}{NewLine}GenerateMenu Adapters{"=".Repeat(80)}");

		var adapterList = typeInNamespace.GetTypesInNamespace();
		WriteGeneratorAdapterList(adapterList, "Model");

		// ==================================================================================
		// Copy and paste adapterlist from Debug Output window to 
		// partial RunAdapterList() in separate file CodeGenAdapters_Run.cs.
		// ==================================================================================

		RunAdapterList();

		Debug.WriteLine("=".Repeat(80));
		string result = $"{nameof(CodeGenAdapters)}.{nameof(RunAdapterList)} code written to '{appSettings.Adapter.OutputFolder}'. Overwrite: {appSettings.Overwrite}";
		Debug.WriteLine(result);

		return result;
	}

	// ==================================================================================

	private bool GenerateAdapter(Type type1, Type type2, bool overwrite = false, bool generateListCode = true)
	{
		string class1 = type1.Name;
		string class2 = type2.Name;
		string map1 = appSettings.Adapter.MapName1;
		string map2 = appSettings.Adapter.MapName2;

		string adapterFileName = $"{class1}Adapter.cs";

		string output =
		$$"""
		using {{appSettings.Adapter.Namespace1}};
		using {{appSettings.Adapter.Namespace2}};
		using System.Linq;
		using System.Collections.Generic;
		
		namespace {{appSettings.Adapter.Namespace}};
		
		public static partial class Adapter
		{
			public static {{class2}} To{{class2}} (this {{class1}} {{map1}})
			{
				return {{map1}} == null ? null : new {{class2}}
				{
				{{GenAdapterPropertiesList(type1, type2, map1)}}
				};
			}
		
			public static {{class1}} To{{class1}} (this {{class2}} {{map2}})
			{
				return {{map2}} == null ? null : new {{class1}}
				{
				{{GenAdapterPropertiesList(type2, type1, map2)}}
				};
			}
			{{GenAdapterListCode(class1, class2, map1, map2, generateListCode)}}
		}
		""";

		string outputPath = $"{appSettings.Adapter.OutputFolder}{adapterFileName}";
		bool isSuccess = output
					     .WriteToFile(outputPath, overwrite);

		if (isSuccess)
			Debug.WriteLine($"Generated file {adapterFileName} in {outputPath}.");

		return isSuccess;
	}

	private string GenAdapterPropertiesList(Type type, Type toType, string mapName)
	{
		var sb = new StringBuilder();

		foreach (var prop in type.GetProperties())
		{
			var toTypeProps = toType.GetProperties().Select(n => n.Name.ToLower()).ToArray();

			if (prop.Name.EqualsAnyIgnoreCase(toTypeProps))
			{
				sb.Append($"{indent}{prop.Name,pad} = {mapName}.{prop.Name}{GenToListAdapter(prop)}{end}");
			}
			else
			{
				sb.Append($"{indent}// No Match // {prop.Name,pad} = {mapName}.{prop.Name}{GenToListAdapter(prop)}{end}");
			}
		 }
		 return sb.ToString().RemoveStartEnd("\t\t", end);
	}

	private string GenAdapterListCode(string class1, string class2, string mapName1, string mapName2, bool genListCode = true)
	{
		if (!genListCode)
			return "";

		string template =
		$$"""	
		
		public static List<{{class2}}> To{{class2}}List (this IEnumerable<{{class1}}> {{mapName1}}List)
		{
			return {{mapName1}}List?.Select(a => a.To{{class2}}()).ToList() ?? [];
		}
		
		public static List<{{class1}}> To{{class1}}List (this IEnumerable<{{class2}}> {{mapName2}}List)
		{
			return {{mapName2}}List?.Select(a => a.To{{class1}}()).ToList() ?? [];
		}
		""";

		return template.ForEachLine(a => $"\t{a}")
					    .RemoveStart("\t");   // Add initial line formatting if needed
	}

	private string GenToListAdapter(PropertyInfo prop)
	{
		if (!typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) || prop.PropertyType == typeof(string))
			return null;

		var itemName = prop.PropertyType?.GenericTypeArguments.ElementAtOrDefault(0)?.Name ?? prop.Name;

		return itemName.Contains("Model") ? $".To{itemName.RemoveEnd("Model")}List()" : $".To{itemName}ModelList()";
	}

	// ==================================================================================

	public static string WriteGeneratorAdapterList(Type[] typeList, string suffix)
	{
		Debug.WriteLine("=".Repeat(80));
		Debug.WriteLine("Creating List of GeneratorAdapters to paste in to CodeGenAdapters_Run.cs");
		Debug.WriteLine("=".Repeat(80));

		var sb = new StringBuilder();

		foreach (var type in typeList)
		{
			sb.AppendLine($"GenerateAdapter(typeof({type.Name}), typeof({type.Name}{suffix}), true);");
		}
		
		string adapterListString = sb.ToString();

		Debug.WriteLine(adapterListString.AddEnd("=".Repeat(80)));

		return adapterListString;
	}
}
