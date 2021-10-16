using System.Reflection;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patterns.Mediator.Behaviours;

namespace Patterns.Mediator
{
    public static class Startup
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, IConfiguration configuration)
        {
            services = services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services = services.AddMediatR(Assembly.GetExecutingAssembly());
            services = services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            AddApplicationBehaviours(services, configuration);

            return services;
        }

        private static void AddApplicationBehaviours(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(ExceptionHandler<,,>));
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLogger<,>));
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestSlaPerformance<,>));
        }
    }
}