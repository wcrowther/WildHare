using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using WildHare.Extensions;

namespace WildHare
{
    public class MetaParameter
    {
        private ParameterInfo parameterInfo;
        private MetaModel parameterType;


        public MetaParameter(ParameterInfo parameterInfo)
        {
            this.parameterInfo = parameterInfo;
        }

        public string Name 
        { 
            get => parameterInfo.Name; 
        }

        public string TypeName 
        { 
            get => parameterType.FullName ?? parameterType.Name; 
        }

        public string Signature 
        { 
            get => $"{TypeName} {Name}";
        }

        public string DocParameterName 
        { 
            get 
            {
                // TODO WJC - More logic here
                
                string paramTypeStr = parameterType.ToString();

                if (paramTypeStr.StartsWith("System.Func"))
                {
                    string genIndicator = GetGenericsIndicator(paramTypeStr);
                    
                    return paramTypeStr.ReplaceFirst(genIndicator, "")
                                       .Replace("[", "{")
                                       .Replace("]", "}");
                }

                return paramTypeStr; 
            } 
        }

        public MetaModel ParameterMetaType { get =>  new MetaModel(parameterInfo.ParameterType); }

        public override string ToString()
        {
            return $"Parameter: '{Name}' of type {parameterType.}";
        }

        private string GetGenericsIndicator(string str)
        {
            if(str.IsNullOrSpace() || !str.Contains("`"))
                return str;
            
            string endStr = str.GetEndAfter("`").TakeWhile(t => t.IsNumber()).AsString();

            return "`" + endStr;
        }
    }
}
