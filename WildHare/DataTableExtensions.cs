using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WildHare.Extensions
{
    public static class DataTableExtensions
    {
        /// <summary>Converts a DataTable to a list of T</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns>List{T}</returns>
        public static List<T> ToList<T>(this DataTable table) where T : new()
		{
			var list = new List<T>();
			var typeProperties = typeof(T).GetProperties().Select(propertyInfo => new
			{
				PropertyInfo = propertyInfo,
				Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType

			}).ToList();

			foreach (var row in table.Rows.Cast<DataRow>())
			{
				var obj = new T();
				foreach (var typeProperty in typeProperties)
				{
					object value = row[typeProperty.PropertyInfo.Name];
					object safeValue =  (value == null || DBNull.Value.Equals(value)) ? null : Convert.ChangeType(value, typeProperty.Type);
					typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
				}
				list.Add(obj);
			}
			return list;
		}
	}
}
