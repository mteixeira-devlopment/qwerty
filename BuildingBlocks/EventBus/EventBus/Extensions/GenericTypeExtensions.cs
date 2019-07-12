using System;
using System.Linq;

namespace EventBus.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class GenericTypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            if (!type.IsGenericType) return type.Name;

            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            var name = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";

            return name;

        }

        public static string GetGenericTypeName(this object @object) => @object.GetType().GetGenericTypeName();
    }
}
