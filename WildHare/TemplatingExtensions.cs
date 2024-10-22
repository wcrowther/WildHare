
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WildHare.Extensions.ForTemplating
{
	public static class TemplatingExtensions
	{
		const string startTag = "[";
		const string endTag = "]";

		/// <summary>Converts a TSQL type name to a C# type name. It will remove the "System." namespace, if present</summary>
		public static string TSqlTypeToCSharpType(this string sqlTypeName, bool isNullable = false)
		{
			string nullable = isNullable ? "?" : "";
			string cstype = sqlTypeName.RemoveStart("System.").ToLower() switch
			{
				"binary"			=> "byte[]",
				"image"			=> "byte[]",
				"varbinary"		=> "byte[]",
				"text"			=> "string",
				"char"			=> "string",
				"nchar"			=> "string",
				"ntext"			=> "string",
				"varchar"			=> "string",
				"nvarchar"		=> "string",
				"real"			=> "float",
				"time"			=> "TimeSpan",
				"tinyint"			=> "byte",
				"uniqueidentifier"	=> "Guid",
				"datetimeoffset"	=> "DateTimeOffset",
				"bit"			=> $"bool{nullable}",
				"smallint"		=> $"short{nullable}",
				"int"			=> $"int{nullable}",
				"bigint"			=> $"long{nullable}",
				"timestamp"		=> $"long{nullable}",
				"smalldatetime"	=> $"DateTime{nullable}",
				"date"			=> $"DateTime{nullable}",
				"datetime"		=> $"DateTime{nullable}",
				"datetime2"		=> $"DateTime{nullable}",
				"decimal"			=> $"decimal{nullable}",
				"float"			=> $"double{nullable}",
				"smallmoney"		=> $"decimal{nullable}",
				"numeric"			=> $"decimal{nullable}",
				_				=> "UNKNOWN",
			};
			return cstype;
		}

		/// <summary>Converts a .Net type name to a C# type name. It will remove the "System." namespace, if present</summary>
		public static string DotNetTypeToCSharpType(this string dotNetTypeName, bool isNullable = false)
		{
			string nullable = isNullable ? "?" : "";

			if (dotNetTypeName.StartsWith("System.Nullable{"))
			{
				dotNetTypeName = dotNetTypeName.RemoveStartEnd("System.Nullable{", "}");
				nullable = "?";
			};

			if (dotNetTypeName.EndsWith("?"))
			{
				dotNetTypeName = dotNetTypeName.RemoveEnd("?");
				nullable = "?";
			};
			string cstype = dotNetTypeName.RemoveStart("System.") switch
			{
				"Boolean"		=> "bool",
				"Byte"		=> "byte",
				"SByte"		=> "sbyte",
				"Char"		=> "char",
				"Decimal"		=> "decimal",
				"Double"		=> "double",
				"Single"		=> "float",
				"Int32"		=> "int",
				"UInt32"		=> "uint",
				"Int64"		=> "long",
				"UInt64"		=> "ulong",
				"Object"		=> "object",
				"Int16"		=> "short",
				"UInt16"		=> "ushort",
				"String"		=> "string",
				"DateTime"	=> "DateTime",
				_			=> dotNetTypeName,
			};
			string[] nullableIgnoreList = { "string", "object" };

			return nullableIgnoreList.Any(cstype.Contains) ? cstype : $"{cstype}{nullable}"; // string? not currently supported
		}

		/// <summary>Given something like a string appsetting {value}, returns the string name from the first parsable type 
		/// (using invariant culture) in the following: bool, int, long, double, or string. If the param {strict} is true,
		/// the function will throw an exception. The {errorMessage} thrown can be customized.</summary>
		public static string BasicTypeNameFromValue(this string value, bool strict = false, string errorMessage = null)
		{
			if (strict && value.IsNullOrEmpty())
				throw new Exception(errorMessage ?? "The BasicTypeNameFromValue value cannot be empty or null when in strict mode.");

			var style = NumberStyles.Any;
			var culture = CultureInfo.InvariantCulture;
			value       = value.IfNull().Replace(",", "");

			return value switch
			{
				{ } when value.Equals("true", true) || value.Equals("false", true) => "bool",
				{ } when !value.Contains(".") && int.TryParse(value, style, culture, out int intValue) => "int",
				{ } when !value.Contains(".") && long.TryParse(value, style, culture, out long longValue) => "long",
				{ } when decimal.TryParse(value, style, culture, out decimal decimalValue) => "decimal",
				_ => "string"
			};
		}

		/// <summary>Returns a string that replaces the placeholder elements [placeholder] in the {string} template with the matching the dictionary 
		/// lookup value with the. It will call .ToString() on non-string objects values in the dictionary if necessary.</summary>
		public static string Template(this Dictionary<string, object> lookups, string template, string startTag = startTag, string endTag = endTag)
		{
			var strBuilder = new StringBuilder(template ?? "");

			foreach (var p in lookups)
			{
				string replacement = (p.Value is string) ? p.Value as string : p.Value.ToString();
				strBuilder.Replace(startTag + p.Key + endTag, (replacement ?? ""));
			}
			return strBuilder.ToString();
		}

		/// <summary>Returns a string that replaces the placeholder elements [placeholder] from the {templateFile} template with the matching the dictionary 
		/// lookup value with the. It will call .ToString() on non-string objects values in the dictionary if necessary.</summary>
		public static string Template(this Dictionary<string, object> lookups, FileInfo templateFile, string startTag = startTag, string endTag = endTag)
		{
			return lookups.Template(templateFile.ReadFile(), startTag, endTag);
		}

		/// <summary>Returns a string that replaces the placeholder elements [placeholder] in the {string} template with the matching the properties  
		/// of the current object. It will call .ToString() on non-string objects values in the dictionary if necessary.</summary>
		public static string Template(this object obj, string template, string startTag = startTag, string endTag = endTag)
		{
			var strBuilder = new StringBuilder(template ?? "");
			Type type = obj.GetType();

			foreach (var t in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				object o = type.GetProperty(t.Name).GetValue(obj, null);
				string replacement = (o == null || o is string) ? o as string : o.ToString();
				strBuilder.Replace(startTag + t.Name + endTag, (replacement ?? ""));
			}
			return strBuilder.ToString();
		}

		/// <summary>Returns a string that replaces the placeholder elements [placeholder] in the {templateFile} template with the matching the properties  
		/// of the current object. It will call .ToString() on non-string objects values in the dictionary if necessary.</summary>
		public static string Template(this object obj, FileInfo templateFile, string startTag = startTag, string endTag = endTag)
		{
			return obj.Template(templateFile.ReadFile(), startTag, endTag);
		}

		/// <summary>Returns a string that replaces the placeholder elements [placeholder] in the {templateFile} template with the matching the properties  
		/// of the current object of type &lt;T&gt;. It will call .ToString() on non-string objects values in the dictionary if necessary.</summary>
		public static string Template<T>(this T obj, FileInfo templateFile, string startTag = startTag, string endTag = endTag)
		{
			return obj.Template(templateFile.ReadFile(), startTag, endTag);
		}

		/// <summary>Returns a string that replaces the placeholder elements '[placeholder]' in the string {template} matching the properties  
		/// for each item of type &lt;T&gt; in the {list}. It will call .ToString() on non-string objects values in the dictionary if necessary.
		/// If not null, {lineEnd} is added after each line of text, except for the last line. {startTag} and {endTag} default to '[' and ']' 
		/// respectively, wrapping the placeholder text but can be customized to the users preferences.</summary>
		public static string TemplateList<T>(this IEnumerable<T> list, string template, string lineEnd = null, string startTag = startTag, string endTag = endTag)
		{
			var templateBuilder = new StringBuilder("");

			foreach (var obj in list)
			{
				var strBuilder = new StringBuilder(template ?? "");
				Type type = obj.GetType();
				foreach (var t in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
				{
					object o = type.GetProperty(t.Name).GetValue(obj, null);
					string replacement = (o is string) ? o as string : (o != null) ? o.ToString() : "";
					strBuilder.Replace(startTag + t.Name + endTag, replacement);
				}
				templateBuilder.Append(strBuilder.ToString().EnsureEnd(lineEnd));
			}
			return templateBuilder.ToString().RemoveEnd(lineEnd);
		}

		/// <summary>Returns a string that replaces the placeholder elements '[placeholder]' in the string returned from the fileInfo {template}
		/// matching the properties for each item of type &lt;T&gt; in the {list}. It will call .ToString() on non-string objects values
		/// in the dictionary if necessary. If not null, {lineEnd} is added after each line of text, except for the last line. {startTag} and {endTag}
		/// default to '[' and ']' respectively, wrapping the placeholder text but can be customized to the users preferences.</summary>
		public static string TemplateList<T>(this IEnumerable<T> list, FileInfo templateFile, string lineEnd = null, string startTag = startTag, string endTag = endTag)
		{
			return list.TemplateList(templateFile.ReadFile(), lineEnd, startTag, endTag);
		}
	}
}

/*
    // Other possible Template Extensions


    public static bool TemplateToFiles(this Dictionary<string, object> filesList, string template, string outputDirectory, string extension = ".txt", string startTag = startTag, string endTag = endTag)
    {
        foreach (var file in filesList)
        {
            var fileinfo = new FileInfo(outputDirectory + file.Key + extension);
            if (fileinfo.Exists)
            {
                fileinfo.Delete();
            }
            file.Value.Template(template, startTag, endTag).WriteToFile(outputDirectory + file.Key + extension);
        }
        return true;
    }

    public static bool TemplateToFiles(this Dictionary<string, object> filesList, FileInfo templateFile, string outputDirectory, string extension = ".txt", string startTag = startTag, string endTag = endTag)
    {
        string template = templateFile.ReadFile();

        foreach (var file in filesList)
        {
            var fileinfo = new FileInfo(outputDirectory + file.Key + extension);
            if (fileinfo.Exists)
            {
                fileinfo.Delete();
            }
            file.Value.Template(template, startTag, endTag).WriteToFile(outputDirectory + file.Key + extension);
        }
        return true;
    }

*/
