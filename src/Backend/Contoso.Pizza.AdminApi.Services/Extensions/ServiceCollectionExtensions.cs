using Contoso.Pizza.AdminApi.Services.Contracts;
using Contoso.Pizza.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Contoso.Pizza.AdminApi.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContosoPizzaServices(this IServiceCollection services,
            IConfiguration configuration,
            bool isProduction = true)
        {
            services.AddContosoPizzaData(configuration, isProduction);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ISauceService, SauceService>();
            services.AddScoped<IToppingService, ToppingService>();
            services.AddScoped<IPizzaService, PizzaService>();
            return services;
        }
    }
}
