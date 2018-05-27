using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WildHare.Models
{
    public class MetaProperty
    {
        // Private Properties
        private PropertyInfo propertyInfo;
        private object modelInstance;

        // Constructor
        public MetaProperty(PropertyInfo propertyinfo)
        {
            propertyInfo = propertyinfo;
        }

        public MetaProperty(PropertyInfo propertyinfo, object modelinstance)
        {
            propertyInfo = propertyinfo;
            modelInstance = modelinstance;
        }

        // Public Properties
        public string Name { get; set; }

        public Type PropertyType
        {

            get { return propertyInfo.PropertyType; }
        }

        public bool IsKey
        {
            get { return propertyInfo.IsDefined(typeof(KeyAttribute), false); }
        }

        public bool CanWrite
        {
            get { return propertyInfo.CanWrite; }
        }

        public bool Implements(string interfaceName)
        {
            return PropertyType.GetInterfaces().Any(a => a.Name == interfaceName);
        }

        // Public Methods
        public dynamic GetInstanceValue(object instance = null)
        {
            // ModelInstance is passed in through constructor.
            // If instance passed in as an argument to this method, use that one
            if (instance != null)
            {
                modelInstance = instance;
            }

            // If constructor modelInstance and method argument instance are both null throw error
            if (modelInstance == null)
            {
                throw new Exception("This method requires a non-null instance in the MetaType or MetaProperty constructor or as an argument.");
            }

            return propertyInfo.GetValue(modelInstance, null);
        }

        public void SetInstanceValue(object value, object instance = null)
        {
            if (instance != null)
            {
                modelInstance = instance;
            }
            if (modelInstance == null)
            {
                throw new Exception("This method requires a non-null instance in the MetaType or MetaProperty constructor or as an argument.");
            }

            propertyInfo.SetValue(modelInstance, value, null);
        }
    }
}
