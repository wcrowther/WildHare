using System;
using System.Collections.Generic;
using System.Text;

namespace WildHare.Tests.Models
{
    public class ClassRequiringCtorParam
    {
        public ClassRequiringCtorParam(string className)
        {
            ClassName = className;
        }

        public string ClassName { get; set; }
    }
}
