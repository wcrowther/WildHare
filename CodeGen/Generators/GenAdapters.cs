using CodeGen.Models;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using WildHare.Extensions;
using static System.Environment;

namespace CodeGen.Generators
{
	public class GenAdapters
	{
		private App _app;

		private string projectRoot;
		private string outputFolder;

		private readonly string indent = "\t".Repeat(4);
		private readonly string end = $",{NewLine}";
		private const int pad = -20;

		public GenAdapters(App app)
		{
			_app            = app;
			projectRoot     = string.Empty;
			outputFolder    = $"{projectRoot}{_app.AdapterOutputFolder}";
		}

		public string Init(Type typeInNamespace)
		{
			Debug.WriteLine("=".Repeat(50));
			Debug.WriteLine("Generate Adapters");
			Debug.WriteLine("=".Repeat(50));

			Type[] adapterList = GetGeneratorAdapterList(typeInNamespace);
			string adapterListString = WriteGeneratorAdapterList(adapterList, "Model");

			// Write out the adapterlist to the Debug window generated from toTypeProps particular namespace
			Debug.Write(adapterListString.AddEnd("=".Repeat(80) + NewLine));

			foreach (var type in adapterList)
			{
				GenerateAdapter(type, typeof(InvoiceItemModel), _app.Overwrite);
			}

			// Copy and paste adapterlist from Debug Output window here if needed.

			GenerateAdapter(typeof(InvoiceItem), typeof(InvoiceItemModel), _app.Overwrite);
			GenerateAdapter(typeof(Invoice), typeof(InvoiceModel), _app.Overwrite);
			GenerateAdapter(typeof(Account), typeof(AccountModel), _app.Overwrite);

			// To Delete:  Array.ForEach(Directory.GetFiles(outputDir), file => File.Delete(file));

			Debug.WriteLine("=".Repeat(80));

			string result = $"{nameof(GenAdapters)}.{nameof(Init)} code written to '{_app.AdapterOutputFolder}'. Overwrite: {_app.Overwrite}";

			Debug.WriteLine(result);

			return result;
		}

		public bool GenerateAdapter(Type type1, Type type2, bool overwrite = false, bool generateListCode = true)
		{
			string class1 = type1.Name;
			string class2 = type2.Name;
			string map1 = _app.AdapterMapName1;
			string map2 = _app.AdapterMapName2;

			string adapterFileName = $"{class1}Adapter.cs";

			string output =
			   $$"""
			using {{_app.AdapterNamespace1}};
			using {{_app.AdapterNamespace2}};
			using System.Linq;
			using System.Collections.Generic;
			
			namespace {{_app.AdapterNamespace}}
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

			string outputPath = $"{_app.AdapterOutputFolder}{adapterFileName}";
			bool isSuccess = output.WriteToFile(outputPath, overwrite);

			if (isSuccess)
				Debug.WriteLine($"Generated file {adapterFileName} in {outputPath}.");

			return isSuccess;
		}

		private string PropertiesList(Type type, Type toType, string mapName)
		{
			var sb = new StringBuilder();

			foreach (var prop in type.GetProperties())
			{
				// var toTypeProps = toType.GetProperties().Select(n => n.Name.ToLower()).ToArray();
				// if (prop.Name.EqualsAny(true, toTypeProps))

				if (toType.GetProperties().Any(toTypeProps => toTypeProps.Name.ToLower() == prop.Name.ToLower()))
				{
					sb.Append($"{prop.Name,pad} = {mapName}.{prop.Name}{UseListAdapter(prop)}{end}");
				}
				else
				{
					sb.Append($"// No Match // {prop.Name,pad} = {mapName}.{prop.Name}{UseListAdapter(prop)}{end}");
				}
			}
			return sb.ToString().RemoveEnd(end);
		}

		private string GetListCode(string class1, string class2, string mapName1, string mapName2, bool genListCode = true)
		{
			if (!genListCode)
				return "";

			string template =
			   $$"""
			
			public static List<{{class2}}> To{{class2}}List (this IEnumerable<{{class1}}> {{mapName1}}List)
			{
				return {{mapName1}}List?.Select(a => a.To{{class2}}()).ToList() ?? new List<{{class2}}>();
			}
			
			public static List<{{class1}}> To{{class1}}List (this IEnumerable<{{class2}}> {{mapName2}}List)
			{
				return {{mapName2}}List?.Select(a => a.To{{class1}}()).ToList() ?? new List<{{class1}}>();
			}
			""";

			return template; //.ForEachLine(a => "\t\t" + a);   // Add initial line formatting if needed
		}

		private string UseListAdapter(PropertyInfo prop)
		{
			var sb = new StringBuilder();

			if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
			{
				var itemName = prop.PropertyType?.GenericTypeArguments.ElementAtOrDefault(0)?.Name ?? prop.Name;
				sb.Append(itemName.Contains("Model") ? $".To{itemName.RemoveEnd("Model")}List()" : $".To{itemName}ModelList()");
			}
			return sb.ToString();
		}

		public Type[] GetGeneratorAdapterList(Type typeInNamespace)
		{
			var assembly = typeInNamespace.GetAssemblyFromType();
			return assembly.GetTypesInNamespace(typeInNamespace.Namespace);
		}

		public string WriteGeneratorAdapterList(Type[] typeList, string suffix) // Use this string to set up models in CodeGen constructor
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
