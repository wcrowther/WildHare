using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WildHare
{
	/// <summary>Simplified PropertyInfo meta data with custom Get and Set of the instance.</summary>
	public class MetaProperty
	{
		private string name;
		private PropertyInfo propertyInfo;
		private object modelInstance;

		public MetaProperty(PropertyInfo propertyinfo)
		{
			propertyInfo = propertyinfo;
			name = propertyInfo.Name;
		}

		public MetaProperty(PropertyInfo propertyinfo, object modelinstance)
		{
			propertyInfo = propertyinfo;
			modelInstance = modelinstance;
			name = propertyInfo.Name;
		}

		public string Name { get => name; set => name = value; }

		public Type PropertyType { get => propertyInfo.PropertyType; }

		public bool IsKey { get => propertyInfo.IsDefined(typeof(KeyAttribute), false); }

		public bool CanWrite { get => propertyInfo.CanWrite; }

		public object[] Attributes() => propertyInfo.GetCustomAttributes(true);

		public T AttributeOfType<T>() => propertyInfo.GetCustomAttributes(true).OfType<T>().FirstOrDefault();

		public bool Implements(string interfaceName) => PropertyType.GetInterfaces().Any(a => a.Name == interfaceName);

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
				throw new Exception("This method requires a non-null instance in the MetaModel or MetaProperty constructor or as an argument.");
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
				throw new Exception("This method requires a non-null instance in the MetaModel or MetaProperty constructor or as an argument.");
			}

			propertyInfo.SetValue(modelInstance, value, null);
		}

		public override string ToString() => $"Property: {Name} ({PropertyType})";
	}
}
