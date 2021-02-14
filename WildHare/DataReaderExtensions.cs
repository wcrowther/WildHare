using System;
using System.Data;

namespace WildHare.Extensions
{
    public static class DataReaderExtensions
    {
        public static T Get<T>(this IDataReader row, string fieldName, T defaultValue = default)
        {
            int ordinal = row.GetOrdinal(fieldName);

            return row.Get<T>(ordinal, defaultValue);
        }

        public static T Get<T>(this IDataReader row, int ordinal, T defaultValue = default)
        {
            var value = row.IsDBNull(ordinal) ? defaultValue : row.GetValue(ordinal);

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
