using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Extensions.Xtra;

namespace WildHare
{
    public class MetaGetterSetter
    {
        private MethodInfo getterMethodInfo;
        private MethodInfo setterMethodInfo;


        private readonly string namespaceName;

        public MetaGetterSetter(MethodInfo getterMethodInfo, MethodInfo setterMethodInfo)
        {
            this.getterMethodInfo = getterMethodInfo;
            this.setterMethodInfo = setterMethodInfo;
        }

        public string Name { get => getterMethodInfo.Name; }

        public Type GetterType { get => getterMethodInfo.ReturnType; }

        public Type SetterType { get => setterMethodInfo.ReturnType; }

        public string Summary { get; set; }

        public override string ToString()
        {
            return $"{Name} ";
        }
    }
}
