using System;
using System.Collections.Generic;
using System.Text;

namespace WildHare.Tests.Interfaces
{
    interface I_Fruit : I_Object, I_Food
    {
        static new decimal Specificity() => 3.0m;
    }
}
