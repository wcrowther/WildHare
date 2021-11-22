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
            this.methodInfo = methodInfo;
        }

        public string Name { get => methodInfo.Name; }

        public Type DeclaringType { get => methodInfo.DeclaringType; }

        public bool IsExtensionMethod { get => methodInfo.IsDefined(typeof(ExtensionAttribute)); } 

        public bool IsStaticMethod { get => methodInfo.IsStatic; }

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

        public override string ToString()
        {
            return $"Method: '{Name}' Parameter Count: {Parameters.Count}";
        }
    }
}
