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
                object o => o.ToJson()
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
                object o => o.ToJson()
            };
            Debug.WriteLine($"{"-".Repeat(30)}{NewLine}{name}{NewLine}{"-".Repeat(30)}");
            Debug.WriteLine(str);
        }

        public static string ToJson(this object obj, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

    }
}
