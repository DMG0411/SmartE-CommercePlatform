using FluentValidation;
using Application.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(
                cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            return services;
        }
    }
}
