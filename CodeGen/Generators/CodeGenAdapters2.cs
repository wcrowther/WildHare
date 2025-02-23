using CodeGen.Entities;
using CodeGen.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using WildHare.Extensions;
using static System.Environment;

namespace CodeGen.Generators
{
	public class CodeGenAdapters2
	{
		private App _app;

		private string projectRoot;
		private string outputFolder;

		private readonly string indent = "\t".Repeat(4);
		private readonly string _end = $",{NewLine}";
		private const int pad = -20;

		public CodeGenAdapters2(App app)
		{
			_app            = app;
			projectRoot     = string.Empty;
			outputFolder    = $"{projectRoot}{_app.Adapter.OutputFolder}";
		}

		public string Init(Type typeInFromNamespace, Type typeInToNamespace)
		{
			Debug.WriteLine("=".Repeat(50));
			Debug.WriteLine("GenerateMenu Adapters");
			Debug.WriteLine("=".Repeat(50));

			var adapterList				= GetFromTypes(typeInFromNamespace);
			string adapterListString	= WriteGeneratorAdapterList(adapterList, "Model");

			// Write out the adapterlist to the Debug window generated from toTypeProps particular namespace
			Debug.Write(adapterListString.AddEnd("=".Repeat(80) + NewLine));

			// adapterList.ForEach(type => GenerateAdapter(type, Type.GetType($"{type.Name}Model, CodeGen"), _app.Overwrite));

			// foreach (var type in adapterList)
			// {
			//		GenerateAdapter(type, Type.GetType($"CodeGen.Models.{type.Name}Model, CodeGen"), _app.Overwrite);
			// }

			// Copy and paste adapterlist from Debug Output window here if needed.

			// GenerateAdapter(typeof(Account),		typeof(AccountModel),		true);
			// GenerateAdapter(typeof(Invoice),		typeof(InvoiceModel),		true);
			// GenerateAdapter(typeof(InvoiceItem),	typeof(InvoiceItemModel),	true);

			// To Delete:  Array.ForEach(Directory.GetFiles(outputDir), file => File.Delete(file));

			Debug.WriteLine("=".Repeat(50));

			string result = $"{nameof(CodeGenAdapters)}.{nameof(Init)} code written to '{_app.Adapter.OutputFolder}'. Overwrite: {_app.Overwrite}";

			Debug.WriteLine(result);

			return result;
		}

		public bool GenerateAdapter(Type type1, Type type2, bool overwrite = false, bool generateListCode = true)
		{
			string class1 = type1.Name;
			string class2 = type2.Name;
			string map1 = _app.Adapter.MapName1;
			string map2 = _app.Adapter.MapName2;

			string adapterFileName = $"{class1}Adapter.cs";

			string output =
			$$"""
			using {{_app.Adapter.Namespace1}};
			using {{_app.Adapter.Namespace2}};
			using System.Linq;
			using System.Collections.Generic;
			
			namespace {{_app.Adapter.Namespace}}
			{ 
				public static partial class Adapter
				{
					public static {{class2}} To{{class2}} (this {{class1}} {{map1}})
					{
						return {{map1}} == null ? null : new {{class2}}
						{
							{{PropertiesList(type1, type2, map1)}}
						};
					}
			
					public static {{class1}} To{{class1}} (this {{class2}} {{map2}})
					{
						return {{map2}} == null ? null : new {{class1}}
						{
							{{PropertiesList(type2, type1, map2)}}
						};
					}
					{{GetListCode(class1, class2, map1, map2, generateListCode)}}
				}
			}
			""";

			string outputPath = $"{_app.Adapter.OutputFolder}{adapterFileName}";
			bool isSuccess = output
						     .WriteToFile(outputPath, overwrite);

			if (isSuccess)
				Debug.WriteLine($"Generated file {adapterFileName} in {outputPath}.");

			return isSuccess;
		}

		private string PropertiesList(Type type, Type toType, string mapName)
		{
			var sb = new StringBuilder();

			foreach (var prop in type.GetProperties())
			{
				var toTypeProps = toType.GetProperties().Select(n => n.Name.ToLower()).ToArray();

				if (prop.Name.EqualsAnyIgnoreCase(toTypeProps))
				{
					sb.Append($"{indent}{prop.Name,pad} = {mapName}.{prop.Name}{UseListAdapter(prop)}{_end}");
				}
				else
				{
					sb.Append($"{indent}// No Match // {prop.Name,pad} = {mapName}.{prop.Name}{UseListAdapter(prop)}{_end}");
				}

			    // if (toType.GetProperties().Any(toTypeProps => toTypeProps.Name.ToLower() == prop.Name.ToLower()))
			 }
			 return sb.ToString().RemoveStartEnd(indent, _end);
		}

		private string GetListCode(string class1, string class2, string mapName1, string mapName2, bool genListCode = true)
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

			   return template.ForEachLine(a => $"\t\t{a}")
						      .RemoveStart("\t\t");   // Add initial line formatting if needed
		}

		private static string UseListAdapter(PropertyInfo prop)
		{
			var sb = new StringBuilder();

			if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
			{
				var itemName = prop.PropertyType?.GenericTypeArguments.ElementAtOrDefault(0)?.Name ?? prop.Name;
				sb.Append(itemName.Contains("Model") ? $".To{itemName.RemoveEnd("Model")}List()" : $".To{itemName}ModelList()");
			}
			return sb.ToString();
		}

		public static List<Type> GetFromTypes(Type typeInNamespace)
		{
			var assembly = typeInNamespace.GetAssemblyFromType();
			return assembly.GetTypesInNamespace(typeInNamespace.Namespace).ToList();
		}

		public static string WriteGeneratorAdapterList(List<Type> typeList, string suffix) // Use this string to set up models in CodeGen constructor
		{
			var sb = new StringBuilder();

			foreach (var type in typeList)
			{
				sb.AppendLine($"GenerateAdapter(typeof({type.Name}), typeof({type.Name}{suffix}), true);");
			}
			return sb.ToString();
		}
	}
}
