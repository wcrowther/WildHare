using System;
using System.Collections.Generic;
using System.Text;
using System.Dynamic;
using System.Linq;

namespace WildHare
{
    /// <summary>Test Class Summary</summary>
    public class DynamicExpando : DynamicObject
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        /// <summary>Test Property...</summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!_properties.ContainsKey(binder.Name))
            {
                result = GetDefault(binder.ReturnType);
                return true;
            }

            return _properties.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this._properties[binder.Name] = value;
            return true;
        }

        public object this[string key]
        {
            get
            {
                try { return _properties[key]; }
                catch { return null; }
            }
            set { _properties[key] = value; }
        }

        public void Remove(string key)
        {
            _properties.Remove(key);
        }

        public void Clear()
        {
            _properties.Clear();
        }

        public int Count => _properties.Count;

        public object First()
        {
            return _properties.First();
        }

        public object FirstOrDefault()
        {
            return _properties.FirstOrDefault();
        }

        // Exension methods used on directly on Dynamic do not work.
        // but can be called using non-extension invocation
        // Instead add LINQ extension methods in this class as so:

        public object ToList()
        {
            return _properties.ToList();
        }

        public Dictionary<string, object> ToDictionary()
        {
            return _properties;
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return default;
        }
    }
}
