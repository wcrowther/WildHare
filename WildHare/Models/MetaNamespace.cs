using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Extensions.Xtra;

namespace WildHare
{
    public class MetaNamespace
    {
        private readonly string namespaceName;

        public MetaNamespace(string NamespaceName)
        {
            namespaceName = NamespaceName;
        }
        public string NamespaceName { get => namespaceName; }

        public List<MetaModel>  MetaModels = null;

        public string Summary { get; set; }

        public override string ToString()
        {
            return $"{NamespaceName} {MetaModels.Count} MetaModels.";
        }
    }
}
