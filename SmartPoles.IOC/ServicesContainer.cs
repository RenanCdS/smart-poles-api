using Microsoft.Extensions.DependencyInjection;
using SmartPoles.Domain.Interfaces;
using SmartPoles.BLL.Services;

namespace SmartPoles.IOC
{
    public static class ServicesContainer
    {
        public static IServiceCollection AddServices(this IServiceCollection services) 
        {
            services.AddSingleton<ICondominiumsService, CondominiumsService>();
            return services;
        }
    }
}