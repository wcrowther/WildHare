using System.Collections.Generic;
using System.Dynamic;

namespace WildHare
{

    // REMOVE - USE DynamicExpando ???

    public class DynamicDictionary : DynamicObject
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            values.TryGetValue(binder.Name, out result);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            values[binder.Name] = value;
            return true;
        }

        public object this[string key]
        {
            get
            {
                try { return values[key]; }
                catch { return null; }
            }
            set { values[key] = value; }
        }

        public void Clear()
        {
            values.Clear();
        }
    }
}
