using System;

namespace WildHare.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>A simple shortcut method to test if an object {obj} is NOT null.</summary>
        public static bool Is(this Object obj)
        {
            return (obj != null);
        }

        /// <summary>A simple shortcut method to test if an object {obj} is null.</summary>
        public static bool IsNull(this Object obj)
        {
            return (obj == null);
        }
    }
}
