
using System;

namespace WildHare.Tests.Models
{
    public class DoubleValue : Prop
    {
        public double Val => Convert.ToDouble(Value);
    }
}
