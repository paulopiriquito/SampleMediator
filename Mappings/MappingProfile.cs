using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Patterns.Mediator.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => 
                    i.IsGenericType && 
                    (i.GetGenericTypeDefinition() == typeof(IMapFrom<>) || i.GetGenericTypeDefinition() == typeof(IMapTo<>) 
                    )))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methods = type.GetRuntimeMethods();
                var mappingMethod = type.GetMethod("Mapping");

                mappingMethod?.Invoke(instance, new object[] { this });
            }
        }
    }
}