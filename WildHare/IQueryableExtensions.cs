using System;
using System.Linq;
using System.Linq.Expressions;

namespace WildHare.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> items, string propertyName,
                                               bool desc = false, string defaultProperty = "")
        {
            var typeOfT = typeof(T);
            var parameter = Expression.Parameter(typeOfT, "parameter");
            var property = (!propertyName.IsNullOrSpace()) ? 
                                typeOfT.GetProperty(propertyName) : 
                                typeOfT.GetProperty(defaultProperty);
            if (property == null) // Invalid propertyName - use first property
            {
                property = typeOfT.GetProperties()[0];
            }
            var propertyAccess = Expression.PropertyOrField(parameter, property.Name);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);
            string orderString = (desc == false) ? "OrderBy" : "OrderByDescending";

            var expression = Expression.Call(typeof(Queryable), orderString,
                                    new Type[] { typeOfT, property.PropertyType }, 
                                    items.Expression, Expression.Quote(orderExpression));

            return items.Provider.CreateQuery<T>(expression);
        }
    }
}
