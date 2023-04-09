using System;
using System.Collections.Generic;
using System.Text;

namespace WildHare.Tests.SourceFiles
{
    public class TestClass
    {
        public TestClass() { }

        public string Name { get; set; }

        public string Description { get; set; } 

        public string Type { get; set; }  
        
        public string TypeDescription { get; set; } = string.Empty;

        #region Props

        public int Props1 { get; set; } 

        public int Props2 { get; set; }

        #endregion

        #region Tags

        public int Tags1 { get; set; }

        public int Tags2 { get; set; }

        #endregion

        #region Hints

        public int Hints1 { get; set; }

        public int Hints2 { get; set; }

        #endregion
    }
}
