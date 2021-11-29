using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WildHare
{
    /// <summary>Simplified FieldInfo meta data </summary>
    public class MetaField
    {
        private FieldInfo fieldInfo;
        private object modelInstance;

        public MetaField(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;
        }

        public string Name  { get => fieldInfo.Name; }

        public Type FieldType { get => fieldInfo.FieldType;  }

        public bool IsKey { get => fieldInfo.IsDefined(typeof(KeyAttribute), false); }

 
        public override string ToString() => $"Field: {Name} ({FieldType})";
    }
}
