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

        public string Name { get => methodInfo.Name; }

        public Type DeclaringType { get => methodInfo.DeclaringType; }

        public bool IsExtensionMethod { get => methodInfo.IsDefined(typeof(ExtensionAttribute)); } 

        public bool IsStaticMethod { get => methodInfo.IsStatic; }

        public bool IsGetter { get => Name.StartsWith("get_") && methodInfo.IsSpecialName; }

        public bool IsSetter { get => Name.StartsWith("set_") && methodInfo.IsSpecialName; }

        public bool IsInherited(string typeName) => DeclaringType.Name == typeName;

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
    }
}
