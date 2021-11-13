using System;
using System.Collections.Generic;
using System.Text;
using WildHare.Models;

namespace WildHare.Tests.Models
{
    public class DerivedFromTestModel : TestModel
    {
        public DerivedFromTestModel()
        {
            base.Name = nameof(DerivedFromTestModel);
        }
    }

    public class DerivedFromTestModel2 : TestModel
    {
        public DerivedFromTestModel2()
        {
            base.Name = nameof(DerivedFromTestModel2);
        }
    }
}
