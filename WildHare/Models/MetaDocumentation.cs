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
    public class MetaDocumentation
    {
        private string _rawName { get; set; }

        public MetaDocumentation(string rawName)
        {
            _rawName = rawName;
        }

        public string MemberName
        { 
            get => _rawName.RemoveStart(MemberId() + ":");
        }

        public string MemberId()
        {
            if (_rawName.IsNullOrSpace() || _rawName.Length <= 3)
                return null;

            if (_rawName.Substring(1, 1) != ":")
                return null;

            return _rawName.Substring(0, 1);
        }

        public string MemberType()
        {
            string codeElement;

            switch (MemberId())
            {
                case "M": codeElement = "Method"; break;
                case "T": codeElement = "Type"; break;
                case "F": codeElement = "Field"; break;
                case "P": codeElement = "Property"; break;
                case "C": codeElement = "Ctor"; break;
                case "E": codeElement = "Event"; break;
                case "!": codeElement = "Error"; break;
                default: codeElement = "Unknown"; break;
            }

            return codeElement;
        }

        public string Documentation { get; set; }

        public string Summary { get; set; }

        public override string ToString()
        {
            return $"{MemberName} : {MemberType()}";
        }
    }
}
