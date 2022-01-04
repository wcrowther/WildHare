using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using WildHare.Extensions;

namespace WildHare
{
    /// <summary>Simplified MethodInfo with custom methods</summary>
    public class MetaMethod
    {
        private MethodInfo methodInfo;
        private List<MetaParameter> metaParameters = null;

        public MetaMethod(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new NullReferenceException("The MetaMethod constructor requires a non-null MethodInfo.");

            this.methodInfo = methodInfo;
        }

        public string Name { get => methodInfo.Name; }

        public string Signature
        {
            get
            {
                return $"{Name}({ParametersString()})";
            }
        }

        public string Namespace
        {
            get
            {
                return $"{DeclaringType?.Namespace}";
            }
        }

        public string DocMemberName 
        { 
            get
            {
                return $"M:{Namespace.AddEnd(".")}{Name}({ParametersString()})";
            }
        }

        // USING GetXmlName from S W Harden
        // public string DocMemberName { get => GetXmlName(this.methodInfo); }

        public Type DeclaringType { get => methodInfo.DeclaringType; }

        public bool IsExtensionMethod { get => methodInfo.IsDefined(typeof(ExtensionAttribute)); } 

        public bool IsStaticMethod { get => methodInfo.IsStatic; }

        public bool IsGetter { get => Name.StartsWith("get_") && methodInfo.IsSpecialName; }

        public bool IsSetter { get => Name.StartsWith("set_") && methodInfo.IsSpecialName; }

        public bool IsInherited(string typeName) => DeclaringType.Name == typeName;

        // public int GenericMethods { get => }

        public List<MetaParameter> Parameters
        {
            get 
            {
                if (metaParameters == null)
                    metaParameters = new List<MetaParameter>();

                // Populate for first 
                if (metaParameters.Count == 0)
                { 
                    foreach (var parameterInfo in methodInfo.GetParameters())
                    {
                        metaParameters.Add(new MetaParameter(parameterInfo));
                    }
                }
                return metaParameters;
            }
        }
        public string ParametersString()
        {
            

            string parmStr = string.Join(", ", Parameters.Select(s => s.Signature));

            return parmStr.Trim();
        }

        public string DocParametersString()
        {
            string parmStr = string.Join(",", Parameters.Select(s => s.DocParameterName));

            return parmStr;
        }

        public string Summary { get; set; }

        public override string ToString()
        {
            return $"Method: '{Name}' Parameters: {Parameters.Count}";
        }


        // ==============================================================================================
        // BELOW FROM: https://swharden.com/blog/2021-01-31-xml-doc-name-reflection/
        // ==============================================================================================

        public static string GetXmlName(MethodInfo info)
        {
            string declaringTypeName = info.DeclaringType.FullName;

            if (declaringTypeName is null)
                throw new NotImplementedException("inherited classes are not supported");

            string xmlName = "M:" + declaringTypeName + "." + info.Name;
            xmlName = string.Join("", xmlName.Split(']').Select(x => x.Split('[')[0]));
            xmlName = xmlName.Replace(",", "");

            if (info.IsGenericMethod)
                xmlName += "``#";

            int genericParameterCount = 0;
            List<string> paramNames = new List<string>();
            foreach (var parameter in info.GetParameters())
            {
                Type paramType = parameter.ParameterType;
                string paramName = GetXmlNameForMethodParameter(paramType);
                if (paramName.Contains("#"))
                    paramName = paramName.Replace("#", (genericParameterCount++).ToString());
                paramNames.Add(paramName);
            }
            xmlName = xmlName.Replace("#", genericParameterCount.ToString());

            if (paramNames.Any())
                xmlName += "(" + string.Join(",", paramNames) + ")";

            return xmlName;
        }

        private static string GetXmlNameForMethodParameter(Type type)
        {
            string xmlName = type.FullName ?? type.BaseType.FullName;
            bool isNullable = xmlName.StartsWith("System.Nullable");
            Type nullableType = isNullable ? type.GetGenericArguments()[0] : null;

            // special formatting for generics (also Func, Nullable, and ValueTulpe)
            if (type.IsGenericType)
            {
                var genericNames = type.GetGenericArguments().Select(x => GetXmlNameForMethodParameter(x));
                var typeName = type.FullName.Split('`')[0];
                xmlName = typeName + "{" + string.Join(",", genericNames) + "}";
            }

            // special case for generic nullables
            if (type.IsGenericType && isNullable && type.IsArray == false)
                xmlName = "System.Nullable{" + nullableType.FullName + "}";

            // special case for multidimensional arrays
            if (type.IsArray && (type.GetArrayRank() > 1))
            {
                string arrayName = type.FullName.Split('[')[0].Split('`')[0];
                if (isNullable)
                    arrayName += "{" + nullableType.FullName + "}";
                string arrayContents = string.Join(",", Enumerable.Repeat("0:", type.GetArrayRank()));
                xmlName = arrayName + "[" + arrayContents + "]";
            }

            // special case for generic arrays
            if (type.IsArray && type.FullName is null)
                xmlName = "``#[]";

            // special case for value types
            if (xmlName.Contains("System.ValueType"))
                xmlName = "`#";

            return xmlName;
        }




    }
}
