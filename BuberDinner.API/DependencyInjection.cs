using BuberDinner.API.Common.Errors;
using BuberDinner.API.Common.Mapping;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BuberDinner.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<ProblemDetailsFactory, BuberProblemDetailsFactory>();
            services.AddMapping();

            return services;
        }
    }
}
