using System;
using System.Collections.Generic;
using System.Text;

namespace WildHare.Tests.Interfaces
{
    interface I_Food : I_Object
    {
        static new decimal Specificity() => 2.0m;

        public static I_Food[] operator + (I_Food lvalue, I_Food[] foodAray)
        {
            // Default implementation that is different for this interface
            throw new NotImplementedException();
        }
    }
}
