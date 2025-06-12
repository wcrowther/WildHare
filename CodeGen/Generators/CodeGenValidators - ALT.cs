using CodeGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WildHare.Extensions;
using WildHare.Tests.Models.Generics;
using static System.Environment;

namespace CodeGen.Generators;

/*  ==========================================================================
	new CodeGenValidators().Init();
	========================================================================== */

public partial class CodeGenValidatorsALT(AppSettings app)
{
	// validators like: required, minLength, maxLength, etc.
	static readonly List<string> validatorsList = []; 

	private static readonly string indent = "\t";
	private const int pad = -20;


	public string Init()
    {
		string outputFilePath			= $"{app.ProjectRoot}{app.Validators.OutputFile}";
		bool overwrite					= app.Overwrite;

		var result = GenerateValidators(outputFilePath, overwrite);

        return result.Message;
    }

    public Result GenerateValidators(string outputFilePath, bool overwrite)
	{
		var sb = new StringBuilder();
		Type typeInNamespace	= GetTypeInNamespace();
		Type[] typeList			= typeInNamespace.GetTypesInNamespace(app.Validators.SourceNamespace);

		if (typeList.Length == 0)
		{
			string errorMsg = $"No types found in namespace: '{app.Validators.SourceNamespace}' assembly: " +
							  $"'{app.Validators.SourceAssemblyName}'. Please check the namespace and try again.";
			return new Result(false, errorMsg);
		}

		string template =
		$$"""
        The typelist:
        {{BuildTypeList(typeList, sb)}}
        """;

		sb.Append(template);

		string listStr = validatorsList.Distinct().AsString();
		string output = sb.ToString()
						.AddStart($"import {{ {listStr} }} from '@vuelidate/validators'{NewLine}{NewLine}");


		bool isSuccess = output.WriteToFile(outputFilePath, overwrite);
		string message = isSuccess ? $"Generated file to {outputFilePath} for {typeList.Length} types."
								   : $"Failed to generate file to {outputFilePath}. File could not be written.";

		return new Result(isSuccess, message);
	}

	// ===================================================================================

	private Type GetTypeInNamespace()
	{
		string typeName					= $"{app.Validators.SourceNamespace}.{app.Validators.SourceTypeInNamespace}";
		string typeNameWithAssemblyName	= typeName.AddEnd(app.Validators.SourceAssemblyName.AddStart(", ")); 
		Type typeInNamespace			= Type.GetType(typeNameWithAssemblyName);

		return typeInNamespace;
	}

	private static string BuildTypeList(Type[] types, StringBuilder sb)
    {
        foreach (var type in types)
		{
			var props = type.GetProperties();

			if (props.Length == 0) // No properties to display, skip this type
				continue;

			AppendType(sb, type, props);
		}
		return sb.ToString();
    }

	private static void AppendType(StringBuilder sb, Type type, PropertyInfo[] props)
	{
		sb.AppendLine($"{indent}{type.Name}");

		foreach (var prop in props)
		{
			sb.AppendLine($"{indent}{indent}{prop.Name + ":",pad} {{}}");
		}
		sb.AppendLine();
	}
}
