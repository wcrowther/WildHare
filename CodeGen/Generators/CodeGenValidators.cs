using CodeGen.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using WildHare;
using WildHare.Extensions;
using WildHare.Tests.Models.Generics;
using static System.Environment;

namespace CodeGen.Generators;

/*  ==========================================================================
	new CodeGenValidators().Init();
	========================================================================== */

public partial class CodeGenValidators(AppSettings app)
{
	// validators like: required, minLength, maxLength, etc.
	static readonly List<string> validatorsList = []; 

	private static readonly string indent = "\t";
	private const int pad = -20;


	public string Init()
    {
		string outputFilePath		= $"{app.ProjectRoot}{app.Validators.OutputFile}";
		bool overwrite				= app.Overwrite;
		string sourceNamespace		= app.Validators.SourceNamespace;
		string assemblyName			= app.Validators.SourceAssemblyName;
		string[] excludeClasses		= app.Validators.ExcludeClasses.Split(",", true, true);

		var assembly = Assembly.Load(assemblyName);
		var result   = GenerateValidators(assembly, sourceNamespace, outputFilePath, overwrite, excludeClasses);

        return result.Message;
    }

	// ===========================================================================================

	private static Result GenerateValidators(Assembly assembly, string sourceNamespace, string outputFilePath, bool overwrite, string[] excludeClasses = null)
	{
		var sb = new StringBuilder();
		var typeList = assembly.GetTypesInNamespace(sourceNamespace, excludeClasses); // exclude: 
		int validatorCount = 0;

		foreach (var type in typeList)
		{
			var props			= type.GetMetaProperties();
			var attributeCount	= props.SelectMany(p => p.Attributes()).Count();

			if (attributeCount == 0)
				continue;

			string classStr = 
			$$"""

			export const {{type.Name}}Validator =
			{
			{{WriteProps(props, validatorsList)}}
			}

			""";

			sb.Append(classStr);
			validatorCount++;
		}

		string listStr = validatorsList.Distinct().AsString();

		string output = sb.ToString()
						.AddStart($"import {{ {listStr} }} from '@vuelidate/validators'{NewLine}");

		bool isSuccess = output.WriteToFile(outputFilePath, overwrite);

		string exclusions	= excludeClasses.Length > 0 ? $"{NewLine} Excluded classes: {excludeClasses.AsString(",",true,true)}" : "";
		string message		= isSuccess ? $"Generated {validatorCount} valididators to {outputFilePath} for {typeList.Length} types.{exclusions}"
										: $"Failed to generate file to {outputFilePath}. File could not be written.";
		
		return new Result(isSuccess, message);
	}

	private static string WriteProps(List<MetaProperty> props, List<string> validatorsList)
	{
		var wp = new StringBuilder();

		foreach (var prop in props)
		{
			wp.Append(WriteAttributesLine(prop, validatorsList));
		}

		return wp.ToString().RemoveEnd(NewLine);
	}

	private static string WriteAttributesLine(MetaProperty prop, List<string> validatorsList)
	{
		const int pad  = -20;
		var attributes = prop.Attributes().OfType<Attribute>();

		if (attributes.Any())
		{
			var wa = new StringBuilder();

			wa.Append($"\t{prop.Name + ":",pad}{{ ");

			foreach (var attr in attributes)
			{
				string attrStr = AttributeString(attr);

				validatorsList.Add(attrStr.GetStartBefore(":"));

				wa.Append(attrStr.AddEnd(", "));
			}

			return wa.ToString()
					 .RemoveEnd(", ")
					 .AddEnd($" }},{NewLine}");
		}

		return ""; 
	}

	private static string AttributeString(Attribute attribute) => attribute switch
	{
		RequiredAttribute  _ => $"required",
		MinLengthAttribute _ => $"minLength: minLength({(attribute as MinLengthAttribute).Length})",
		MaxLengthAttribute _ => $"maxLength: maxLength({(attribute as MaxLengthAttribute).Length})",
		null or _			 => null
	};

}
