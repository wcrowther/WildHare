using System;
using System.Collections.Generic;
using System.Text;

namespace WildHare.Tests.SourceFiles
{
    public class TrimmedTestClass
    {
        public TrimmedTestClass() { }

        public string Name { get; set; }

        public string Description { get; set; } 

        public string Type { get; set; }  
        
        public string TypeDescription { get; set; } = string.Empty;

        public int Props1 { get; set; } 

        public int Props2 { get; set; }

        public int Tags1 { get; set; }

        public int Tags2 { get; set; }

        public int Hints1 { get; set; }

        public int Hints2 { get; set; }

    }
}
