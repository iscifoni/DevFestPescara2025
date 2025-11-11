using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIB.Core.Infrastructure.Events.Dapr
{
    internal static class Utility
    {
        public static string GetFriendlyTypeName(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            var genericTypeName = type.Name.Substring(0, type.Name.IndexOf('`'));
            var genericArgs = type.GetGenericArguments()
                                  .Select(t => GetFriendlyTypeName(t));
            return $"{genericTypeName}_{string.Join("_", genericArgs)}";
        }

    }
}
