using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WildHare.Extensions;

namespace WildHare.Tests.Models
{
    public class ClassTag
    {

        public string Name { get; set; }

        public string Reference { get; set; }

        public List<string> References { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"{Name} Count: {References.Count}";
        }
    }
}
