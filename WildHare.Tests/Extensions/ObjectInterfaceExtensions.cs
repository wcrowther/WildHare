using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WildHare.Extensions;
using WildHare.Tests.Interfaces;

namespace WildHare.Tests.Extensions
{
    public static class ObjectInterfaceExtensions
    {
        public static I_Object GetCommonObjectInterface(this object[] objects)
        {
            throw new NotImplementedException();
            
            //var interfaces = objects.GetCommonInterfaces()
            //                        .Where(w => typeof(I_Object).IsAssignableFrom(w))
            //                        .OrderBy(o => o.GetMethod("Specificity").Invoke(null, null))
            //                        .ToArray();

            //I_Object inter = interfaces.FirstOrDefault();

            //return inter;
        }
    }
}
