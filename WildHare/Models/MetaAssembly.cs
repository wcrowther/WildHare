using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WildHare.Extensions;
using WildHare.Extensions.Xtra;

namespace WildHare
{
    public class MetaAssembly
    {
        private readonly Assembly _assembly;
        private List<MetaModel> metaTypes = null;


        public MetaAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new Exception("MetaAssembly assembly cannot be null.");
            _assembly = assembly;
        }

        public string AssemblyName { get => _assembly.GetName().Name; }

        public List<MetaModel> GetMetaModels()
        {
            return MetaModels;
        }

        private List<MetaModel> MetaModels
        {
            // Includes inherited Methods but they are excluded by default in public GetMetaMethods()
            get
            {
                if (metaTypes == null)
                    metaTypes = new List<MetaModel>();

                if (metaTypes.Count == 0)
                {
                    foreach (var type in _assembly.GetTypes())
                    {
                        metaTypes.Add(new MetaModel(type));
                    }
                }
                return metaTypes;
            }
        }

        public override string ToString() => $"MetaAssembly: {AssemblyName} Type count: {MetaModels.Count}";

    }
}
