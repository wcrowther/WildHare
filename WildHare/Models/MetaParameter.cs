using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WildHare.Models
{
    public class MetaParameter
    {
        private ParameterInfo parameterInfo;

        public MetaParameter(ParameterInfo parameterInfo)
        {
            this.parameterInfo = parameterInfo;
        }

        public string Name { get => parameterInfo.Name; }

        public Type ParameterType { get => parameterInfo.ParameterType; }

        public override string ToString()
        {
            return $"Parameter: '{Name}' of type {ParameterType}";
        }
    }
}
