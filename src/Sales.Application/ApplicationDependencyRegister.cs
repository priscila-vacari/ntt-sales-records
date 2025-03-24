using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Factories;
using Sales.Application.Interfaces;
using Sales.Application.Strategies;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Sales.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationDependencyRegister
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDependencyServices();
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            services.AddTransient<IDiscountFactory, DiscountFactory>();

            services.AddSingleton<IDiscountStrategy>(provider => new DiscountStrategy(0));
            services.AddSingleton<IDiscountStrategy>(provider => new DiscountStrategy(10));
            services.AddSingleton<IDiscountStrategy>(provider => new DiscountStrategy(20));

            #region MediaR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            #endregion

            return services;
        }
    }
}
