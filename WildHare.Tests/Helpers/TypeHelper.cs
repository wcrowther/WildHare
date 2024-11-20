using System;
using System.Collections;
using System.Collections.Generic;

namespace WildHare.Tests.Helpers
{
	 public static class TypeHelper
	 {
		public static D GetDefault<D>() => typeof(D) switch
		{
			// Add other types here

			Type t when typeof(D).IsArray => (D)(object)Array.CreateInstance(t.GetElementType()!, 0),
			Type t when typeof(IEnumerable).IsAssignableFrom(t) &&  t.IsGenericType => 
										   (D)Activator.CreateInstance(typeof(List<>)
										   .MakeGenericType(t.GetGenericArguments())),
			_ => default
		};
	 }
}
