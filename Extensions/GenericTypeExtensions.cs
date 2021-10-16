using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace Patterns.Mediator.Extensions
{
    public static class GenericTypeExtensions
    {
        private static readonly IDictionary<Type, string[]> GenericTypeArgumentNames = new Dictionary<Type, string[]>();
        
        private static string GetTypeFullName(this Type type)
        {
            var typeName = type.IsGenericType ? GetGenericTypeFullName(type) : type.Name;
            return typeName;
        }

        private static string GetGenericTypeFullName(Type type)
        {
            if (!GenericTypeArgumentNames.TryGetValue(type, out var genericTypeArgumentsNames))
                genericTypeArgumentsNames = GetGenericTypeArgumentsNames(type);
            
            var typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypeArgumentsNames}>";
            
            return typeName;
        }

        private static string[] GetGenericTypeArgumentsNames(Type type)
        {
            var genericTypeArgumentsNames = type.GetGenericArguments().Select(t => t.Name).ToArray();
            GenericTypeArgumentNames.Add(type, genericTypeArgumentsNames);
            return genericTypeArgumentsNames;
        }

        public static string GetTypeFullName<T>(this IRequest<T> @object)
        {
            return @object.GetType().GetTypeFullName();
        }
    }
}
