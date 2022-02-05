using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

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

        public string Name => methodInfo.Name;

        public string XmlDocMemberName => GetXmlDocMemberName(); 

        // public string XmlDocMemberName => AltGetXmlDocMemberName(methodInfo); 

        public Type DeclaringType => methodInfo.DeclaringType;

        public bool IsExtensionMethod => methodInfo.IsDefined(typeof(ExtensionAttribute)); 

        public bool IsStaticMethod => methodInfo.IsStatic;

        public bool IsGetter => Name.StartsWith("get_") && methodInfo.IsSpecialName;

        public bool IsSetter => Name.StartsWith("set_") && methodInfo.IsSpecialName;

        public bool IsInherited(string typeName) => DeclaringType.Name == typeName;

        public Type[] GetGenericArguments() => methodInfo.GetGenericArguments();

        public List<MetaParameter> Parameters
        {
            get 
            {
                if (metaParameters == null)
                    metaParameters = new List<MetaParameter>();

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

        public string Summary { get; set; }

        public override string ToString()
        {
            return $"Method: '{Name}' Parameters: {Parameters.Count}";
        }

        // PRIVATE =======================================================================

        private string ParametersString()
        {
            return string.Join(",", Parameters.Select(s => s.ParameterType));
        }

        private string GetXmlDocMemberName()
        {
            return $"{DeclaringType.FullName}.{Name}({ParametersString()})";
        }

        // EXAMPLE CODE FROM SWHARDEN
        // FROM https:// swharden.com/blog/2021-01-31-xml-doc-name-reflection/
        private static string AltGetXmlDocMemberName(MethodInfo info)
        {
            string declaringTypeName = info.DeclaringType?.FullName;

            if (declaringTypeName is null)
                throw new NotImplementedException("inherited classes are not supported");

            string xmlName =  declaringTypeName + "." + info.Name;
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
            string xmlName = type.FullName ?? type.BaseType?.FullName;

            if (xmlName is null)
                return "";

            bool isNullable = xmlName.StartsWith("System.Nullable");
            Type nullableType = isNullable ? type.GetGenericArguments()[0] : null;

            // special formatting for generics (also Func, Nullable, and ValueTulpe)
            if (type.IsGenericType)
            {
                var genericNames = type.GetGenericArguments().Select(x => GetXmlNameForMethodParameter(x));
                var typeName = xmlName.Split('`')[0];
                // var typeName = type.FullName.Split('`')[0];
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
