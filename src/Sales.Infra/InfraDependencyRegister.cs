using Sales.Infra.Context;
using Sales.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Sales.Infra.Repositories;

namespace Sales.Infra
{
    [ExcludeFromCodeCoverage]
    public static class InfraDependencyRegister
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDependencyContext(configuration);
            services.AddDependencyServices();
        }

        private static IServiceCollection AddDependencyContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringPostgre = configuration.GetConnectionString("SalesConnectionPostgre");
            services.AddDbContext<AppDbContextPostgre>(options => options.UseNpgsql(connectionStringPostgre));

            return services;
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(ISaleRepository), typeof(SaleRepository));

            return services;
        }
    }
}
