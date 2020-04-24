using Newtonsoft.Json;
using System;
using System.Diagnostics;
using WildHare.Extensions;
using static System.Environment;

namespace WildHare.Tests.Helpers
{
    public static class HelperExtensions
    {
        public static void Dump(this object obj)
        {
            string str = obj switch
            {
                string s => s,
                null => null,
                int i => i.ToString(),
                object o => JsonConvert.SerializeObject(o, Formatting.Indented)
            };
            Console.WriteLine(str);
        }

        public static void Debugger(this object obj)
        {
            string name = nameof(obj);
            string str = obj switch
            {
                string s => s,
                null => null,
                int i => i.ToString(),
                object o => JsonConvert.SerializeObject(o, Formatting.Indented)
            };
            Debug.WriteLine($"{"-".Repeat(30)}{NewLine}{name}{NewLine}{"-".Repeat(30)}");
            Debug.WriteLine(str);
        }
    }
}
