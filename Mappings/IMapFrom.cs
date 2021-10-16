using System;
using AutoMapper;

namespace Patterns.Mediator.Mappings
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
        
        public static void DefaultMapping(Profile profile, Type destinationType)
        {
            profile.CreateMap(typeof(T), destinationType);
        }
    }
}
