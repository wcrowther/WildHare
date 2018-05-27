using System;

namespace WildHare.Extensions.ForObject
{
    public static class ObjectExtensions
    {
        public static bool Is(this Object obj)
        {
            return (obj != null);
        }

        public static bool IsNull(this Object obj)
        {
            return (obj == null);
        }
    }
}
