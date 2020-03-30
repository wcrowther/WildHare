
using System.Linq;

namespace WildHare.Extensions.ForTemplating
{
    public static class TemplatingExtensions
    {
		/// <summary>Converts a TSQL type name to a C# type name. It will remove the "System." namespace, if present,</summary>
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

		/// <summary>Converts a .Net type name to a C# type name. It will remove the "System." namespace, if present,</summary>
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
	}
}
