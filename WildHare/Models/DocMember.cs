using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;

namespace WildHare
{
    public class DocMember
    {
        private readonly Assembly _assemblyToDocument;
        private readonly string _rawName;
        private string[] codeArray;

        public DocMember(string rawName, Assembly assemblyToDocument)
        {
            _assemblyToDocument = assemblyToDocument;
            _rawName = rawName;

            Process();
        }

        public string RawName { get; set; }

        public string CodeElement { get; private set; }

        public string MethodParams { get; private set; }

        public string MethodParamsGenericType { get; private set; }

        public string Documentation { get; set; }

        public string Summary { get; set; }

        public Type ClassType { get; private set; }

        public bool IsExtensionMethod { get; set; }

        public List<string> Examples { get; set; } = new List<string>();

        public string Reference { get; set; }

        public bool Display { get; set; } = true;

        public string Member
        {
            get
            {
                return codeArray.LastOrDefault();
            }
        }

        public string ClassName
        {
            get
            {

                if (CodeElement == "Type" && codeArray.Length == 2)
                    return default;

                if (codeArray.Length >= 2)
                {
                    return codeArray.ElementAtOrDefault(codeArray.Length - 2);
                }
                return default;
            }
        }

        public string Namespace
        {
            get
            {
                if (codeArray.Length > 2)
                {
                    return string.Join(".", codeArray.Reverse().Skip(2).Reverse());
                }
                return default;
            }
        }

        public override string ToString()
        {
            string addThis = IsExtensionMethod ? "this " : "";
            string parameters = $"{addThis}{MethodParams}"; 

            return $"{CodeElement} {Member} {parameters.AddStartEnd("(", ")")}";
        }

        // =================================================
        // Private
        // ==================================================

        private void Process()
        {
            if (_rawName.IsNullOrEmpty())
                return;

            var code = _rawName;
            code = SetCodeElement(code);
            code = SetMethodParams(code);
            codeArray = code.Split('.');

            GetClassType();
        }

        private string SetCodeElement(string temp)
        {
            if (temp.Substring(1, 1) == ":")
            {
                switch (temp.Substring(0, 1))
                {
                    case "M": CodeElement = "Method";    break;
                    case "T": CodeElement = "Type";      break;
                    case "F": CodeElement = "Field";     break;
                    case "P": CodeElement = "Property";  break;
                    case "C": CodeElement = "Ctor";      break;
                    case "E": CodeElement = "Event";     break;
                    default:  CodeElement = null;        break;
                }
                temp = temp.Substring(2);
            }
            return temp;
        }

        private string SetMethodParams(string temp)
        {
            int startIndex = temp.IndexOf('(');
            int endIndex = temp.IndexOf(')');

            if (startIndex != -1 && endIndex != -1)
            {
                int length = endIndex - startIndex;
                MethodParams = temp.Substring(startIndex + 1, length - 1).Replace(",", ", ");

                return temp.Substring(0, startIndex);
            }
            return temp;
        }

        //    if (tempArray.Length > 1 && !tempArray[1].IsNullOrEmpty())
        //    {
        //        var prams = tempArray[1]
        //            .RemoveStartEnd("(", ")")
        //            .Split(',')
        //            .Select(a => a.FromDotNetTypeToCSharpType());

        //        string pramsJoined  = string.Join(", ", prams);
        //        MethodParams = IsExtensionMethod ? "this " : "";
        //        MethodParams += pramsJoined.AddStartEnd("(", ")");
        //    }
        //    return temp;
        //}


        private void GetClassType()
        {
            string typeName = $"{Namespace}{ClassName.AddStart(".")}, {_assemblyToDocument}";
            ClassType = Type.GetType(typeName);
        }
    }
}
