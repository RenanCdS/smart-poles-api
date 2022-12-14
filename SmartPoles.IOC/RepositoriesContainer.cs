using Microsoft.Extensions.DependencyInjection;
using SmartPoles.Domain.Interfaces;
using SmartPoles.Data.Repositories;

namespace SmartPoles.IOC
{
    public static class RepositoriesContainer
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) 
        {
            services.AddSingleton<IMetricRepository, PrometheusRepository>();
            services.AddSingleton<IStorageRepository, S3Repository>();
            services.AddSingleton<ICondominiumsRepository, CondominiumsRepository>();
            return services;
        }
    }
}