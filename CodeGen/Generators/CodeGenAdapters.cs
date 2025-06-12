using CodeGen.Models;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using WildHare.Extensions;
using static CodeGen.Helpers.CodeHelpers;
using static System.Environment;

namespace CodeGen.Generators;

public partial class CodeGenAdapters(AppSettings appSettings)
{
	private readonly string indent		= "\t".Repeat(3);
	private readonly string end			= $",{NewLine}";
	private const int pad				= -20;
	private int adaptersRun				= 0;

	public string Init ()
	{
		// ===============================================================================================
		// Generate the Adapters List Template code to be pasted in to CodeGenAdapters_RunAdaptersList.cs
		// Run CodeGentAdaptersList.cs to generate from a specific namespace or type.
		// ===============================================================================================

		RunAdaptersList();

		if (adaptersRun == 0)
		{ 
			return	$"No adapters were generated. Run 'Generate Adapters List' to populate{NewLine} " +
					$"the RunAdaptersList() method in 'CodeGenAdapters_Run.cs'";		
		}

		string result = $"{nameof(CodeGenAdapters)}.{nameof(RunAdaptersList)} code written to " +
						$"'{OutputFolder}'. Overwrite: {appSettings.Overwrite}";

		Debug.WriteLine($"{divider}{result}");

		return result;
	}

	// ==================================================================================
	// To Delete:  Array.ForEach(Directory.GetFiles(outputDir), file => File.Delete(file));

	private string OutputFolder => Path.Combine(appSettings.ProjectRoot, appSettings.Adapters.OutputFolder).EnsureEnd("\\");


	private bool AdaptersTemplate(Type type1, Type type2, bool overwrite = false, bool generateListCode = true)
	{
		string class1	= type1.Name;
		string class2	= type2.Name;
		string map1		= appSettings.Adapters.MapName1;
		string map2		= appSettings.Adapters.MapName2;

		string adapterFileName = $"{class1}Adapters.cs";

		string output =
		$$"""
		using {{appSettings.Adapters.MapNamespace1}};
		using {{appSettings.Adapters.MapNamespace2}};
		using System.Linq;
		using System.Collections.Generic;
		
		namespace {{appSettings.Adapters.AdapterNamespace}};
		
		public static partial class Adapters
		{
			public static {{class2}} To{{class2}} (this {{class1}} {{map1}})
			{
				return {{map1}} == null ? null : new {{class2}}
				{
				{{TemplateAdapterProperties(type1, type2, map1)}}
				};
			}
		
			public static {{class1}} To{{class1}} (this {{class2}} {{map2}})
			{
				return {{map2}} == null ? null : new {{class1}}
				{
				{{TemplateAdapterProperties(type2, type1, map2)}}
				};
			}
			{{TemplateListAdapters(class1, class2, map1, map2, generateListCode)}}
		}
		""";

		string outputPath = $"{OutputFolder}{adapterFileName}";

		bool isSuccess = output.WriteToFile(outputPath, overwrite);

		if (isSuccess)
		{
			Debug.WriteLine($"Generated file {adapterFileName} in {outputPath}.");
			adaptersRun++;
		}

		return isSuccess;
	}

	private string TemplateAdapterProperties(Type type, Type toType, string mapName)
	{
		var sb = new StringBuilder();

		foreach (var prop in type.GetProperties())
		{
			var toTypeProps = toType.GetProperties().Select(n => n.Name.ToLower()).ToArray();

			if (prop.Name.EqualsAnyIgnoreCase(toTypeProps))
			{
				sb.Append($"{indent}{prop.Name,pad} = {mapName}.{prop.Name}{TemplateAdapterToList(prop)}{end}");
			}
			else
			{
				sb.Append($"{indent}// No Match // {prop.Name,pad} = {mapName}.{prop.Name}{TemplateAdapterToList(prop)}{end}");
			}
		 }
		 return sb.ToString().RemoveStartEnd("\t\t", end);
	}

	private static string TemplateListAdapters(string class1, string class2, string mapName1, string mapName2, bool genListCode = true)
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

	private static string TemplateAdapterToList(PropertyInfo prop)
	{
		if (!typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) || prop.PropertyType == typeof(string))
			return null;

		var itemName = prop.PropertyType?.GenericTypeArguments.ElementAtOrDefault(0)?.Name ?? prop.Name;

		return itemName.Contains("Model") ? $".To{itemName.RemoveEnd("Model")}List()" : $".To{itemName}ModelList()";
	}
}
