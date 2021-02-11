
using System;
using System.Collections.Generic;
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
		public static string TSqlTypeToCSharpType(this string sqlTypeName, bool isNull = false)
		{
			string cstype;
			string nullable = isNull ? "?" : "";

			switch (sqlTypeName.RemoveStart("System.").ToLower())
			{
				case "binary":				cstype = "byte[]";				break;
				case "image":				cstype = "byte[]";				break;
				case "varbinary":			cstype = "byte[]";				break;
				case "text":				cstype = "string";				break;
				case "char":				cstype = "string";				break;
				case "nchar":				cstype = "string";				break;
				case "ntext":				cstype = "string";				break;
				case "varchar":				cstype = "string";				break;
				case "nvarchar":			cstype = "string";				break;
				case "real":				cstype = "float";				break;
				case "time":				cstype = "TimeSpan";			break;
				case "tinyint":				cstype = "byte";				break;
				case "uniqueidentifier":	cstype = "Guid";				break;
				case "datetimeoffset":		cstype = "DateTimeOffset";		break;
				case "bit":					cstype = $"bool{nullable}";		break;
				case "smallint":			cstype = $"short{nullable}";	break;
				case "int":					cstype = $"int{nullable}";		break;
				case "bigint":				cstype = $"long{nullable}";		break;
				case "timestamp":			cstype = $"long{nullable}";		break;
				case "smalldatetime":		cstype = $"DateTime{nullable}"; break;
				case "date":				cstype = $"DateTime{nullable}"; break;
				case "datetime":			cstype = $"DateTime{nullable}"; break;
				case "datetime2":			cstype = $"DateTime{nullable}"; break;
				case "decimal":				cstype = $"decimal{nullable}";	break;
				case "float":				cstype = $"double{nullable}";	break;
				case "smallmoney":			cstype = $"decimal{nullable}";	break;
				case "numeric":				cstype = $"decimal{nullable}";	break;
				default:					cstype = "UNKNOWN";				break;
			}
			return cstype;
		}

		/// <summary>Converts a .Net type name to a C# type name. It will remove the "System." namespace, if present</summary>
		public static string DotNetTypeToCSharpType(this string dotNetTypeName, bool isNull = false)
		{
			string cstype;
			string nullable = isNull ? "?" : "";

            if(dotNetTypeName.StartsWith("System.Nullable{"))
            {
                dotNetTypeName = dotNetTypeName.RemoveStartEnd("System.Nullable{", "}");
                nullable = "?";
            };

            if (dotNetTypeName.EndsWith("?"))
            {
                dotNetTypeName = dotNetTypeName.RemoveEnd("?");
                nullable = "?";
            };

            switch (dotNetTypeName.RemoveStart("System."))
			{
				case "Boolean":     cstype = "bool";    break;
				case "Byte":	    cstype = "byte";    break;
				case "SByte":	    cstype = "sbyte";   break;
				case "Char":	    cstype = "char";    break;
				case "Decimal":     cstype = "decimal"; break;
				case "Double":	    cstype = "double";  break;
				case "Single":	    cstype = "float";   break;
				case "Int32":	    cstype = "int";     break;
				case "UInt32":	    cstype = "uint";    break;
				case "Int64":	    cstype = "long";    break;
				case "UInt64":	    cstype = "ulong";   break;
				case "Object":	    cstype = "object";  break;
				case "Int16":	    cstype = "short";   break;
				case "UInt16":	    cstype = "ushort";  break;
                case "String":      cstype = "string";  break;
                case "DateTime":    cstype = "DateTime";break;

                default: cstype = dotNetTypeName;       break; // do nothing
			}
            string[] nullableIgnoreList = { "string", "object" };

            return nullableIgnoreList.Any(cstype.Contains) ? cstype : $"{cstype}{nullable}"; // string? not currently supported
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


    public static bool TemplateToFiles(this Dictionary<string, object> filesList, string template, string templateDir, string extension = ".txt", string startTag = startTag, string endTag = endTag)
    {
        foreach (var file in filesList)
        {
            FileInfo fileinfo = new FileInfo(templateDir + file.Key + extension);
            if (fileinfo.Exists)
            {
                fileinfo.Delete();
            }
            file.Value.Template(template, startTag, endTag).WriteToFile(templateDir + file.Key + extension);
        }
        return true;
    }

    public static bool TemplateToFiles(this Dictionary<string, object> filesList, FileInfo templateFile, string templateDir, string extension = ".txt", string startTag = startTag, string endTag = endTag)
    {
        string template;
        using (StreamReader reader = templateFile.OpenText())
        {
            template = reader.ReadToEnd();
        }

        foreach (var file in filesList)
        {
            FileInfo fileinfo = new FileInfo(templateDir + file.Key + extension);
            if (fileinfo.Exists)
            {
                fileinfo.Delete();
            }
            file.Value.Template(template, startTag, endTag).WriteToFile(templateDir + file.Key + extension);
        }
        return true;
    }

*/
