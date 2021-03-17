
using System;

namespace WildHare.Tests.Models
{
    public  class IsPlaceValue : Prop
    {
        public bool Val => Convert.ToBoolean(Value);
    }
}
