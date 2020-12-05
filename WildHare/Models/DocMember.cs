using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;

namespace WildHare
{
    public class DocMember
    {
        private readonly Assembly _assemblyToDocument;
        private readonly string _rawName;
        private string[] codeArray;
        private string paramStr;
        private string[] parameterArray;

        public DocMember(string rawName, Assembly assemblyToDocument)
        {
            _assemblyToDocument = assemblyToDocument;
            _rawName = rawName;

            Process();
        }

        public string RawName { get; set; }

        public string CodeElement { get; private set; }

        public string Generics { get; private set; }

        public string Documentation { get; set; }

        public string Summary { get; set; }

        public Type ClassType { get; private set; }

        public bool IsExtensionMethod => MethodInfo?.IsDefined(typeof(ExtensionAttribute), true) ?? false;

        public bool IsStaticMethod => MethodInfo?.IsStatic ?? false;

        public List<string> Examples { get; set; } = new List<string>();

        public string Reference { get; set; }

        public bool Display { get; set; } = true;

        public string Namespace
        {
            get
            {
                if (CodeElement == "Type" && codeArray.Length > 1)
                {
                    return string.Join(".", codeArray, 0, codeArray.Length - 1);
                }

                if (codeArray.Length > 2)
                {
                    return string.Join(".", codeArray, 0, codeArray.Length - 2);
                }
                return default;
            }
        }

        public string ClassName
        {
            get
            {
                if (CodeElement == "Type")
                    return codeArray.LastOrDefault();

                if (codeArray.Length >= 2)
                {
                    return codeArray.ElementAtOrDefault(codeArray.Length - 2);
                }
                return default;
            }
        }

        public string Member
        {
            get
            {
                if (CodeElement == "Type")
                    return default;

                return codeArray.LastOrDefault();
            }
        }

        public string MethodParams
        {
            get
            {
                if (parameterArray == null)
                    return null;

                string cSharpStr = string.Join(", ", parameterArray.Select(s => s.DotNetTypeToCSharpType()));

                return cSharpStr.Replace(new[] { "{", "}", "``0", "``1", "``2" }, new[] { "<", ">", "T1", "T2", "T3" });
            }
        }

        public override string ToString()
        {
            if (CodeElement == "Type")
                return ClassName;

            string isExt = IsExtensionMethod ? "this " : "";
            string parameters = $"{isExt}{MethodParams}";

            return $"{Member} {Generics}{parameters.AddStartEnd("(", ")")}";
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
            code = SetGenerics(code);
            codeArray = code.Split('.');

            GetClassType();
        }

        private string SetCodeElement(string temp)
        {
            if (temp.Substring(1, 1) == ":")
            {
                switch (temp.Substring(0, 1))
                {
                    case "M": CodeElement = "Method"; break;
                    case "T": CodeElement = "Type"; break;
                    case "F": CodeElement = "Field"; break;
                    case "P": CodeElement = "Property"; break;
                    case "C": CodeElement = "Ctor"; break;
                    case "E": CodeElement = "Event"; break;
                    default: CodeElement = null; break;
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
                paramStr = temp.Substring(startIndex + 1, length - 1);
                parameterArray = paramStr.Split(',');

                return temp.Substring(0, startIndex);
            }
            return temp;
        }

        private string SetGenerics(string temp)
        {
            string generics = temp.GetEnd("``", true);
            if (generics == null)
                return temp;

            string[] replacements = { "<T>", "<T,T2>", "<T,T2,T3>" };
            Generics = generics.Replace(new[] { "``1", "``2", "``3" }, replacements);

            return temp.RemoveEnd(generics);
        }


        private void GetClassType()
        {
            string typeName = $"{Namespace}{ClassName.AddStart(".")}, {_assemblyToDocument}";
            try
            {
                ClassType = Type.GetType(typeName);
            }
            catch
            {
                Debug.WriteLine($"Error getting ClassType for: {typeName}");
            }
        }

        public List<MetaProperty> ClassProperties
        {
            get
            {
                if (CodeElement != "Type" || ClassType == null)
                    return null;

                return ClassType?.GetMetaProperties();
            }
        }

        public MethodInfo MethodInfo
        {
            // TODO - ISSUE HERE IS THAT WE NEED TO PASS IN THE TYPES IN
            // THE METHOD SIGNATURE, UNFORTUNATELY THE TYPES DESCRIBED IN
            // THE XML DOCUMENTATION DON'T ALWAYS WORK WHEN USING Type.GetType(name)

            get
            {
                if (CodeElement == "Method" || ClassType != null)
                {
                    try
                    {
                        var types = ToTypeArray(parameterArray);

                        if(types.Length == 0)
                            ClassType.GetMethod(Member);

                        return ClassType.GetMethod(Member, types);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Not able to get MethodInfo for Member: {Member} Parameters: {paramStr}. {ex.Message}");

                        return null;
                    }
                }
                return null;
            }
        }

        // Type type = Type.GetType(System.Nullable{System.Boolean})

        private Type[] ToTypeArray(string[] typeNames)
        {
            var types = new List<Type>();

            if (types != null && types.Count > 0)
            {
                foreach (string name in typeNames)
                {
                    if(!name.IsNullOrEmpty())
                        types.Add(Type.GetType(name));
                }
            }


            return types.ToArray();
        }
    }
}
