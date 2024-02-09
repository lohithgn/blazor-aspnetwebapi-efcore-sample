using Contoso.Pizza.Data.Contracts;
using Contoso.Pizza.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contoso.Pizza.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddContosoPizzaData(this IServiceCollection services, 
        IConfiguration configuration,
        bool isProduction = true)
    {
        var contosoPizzaDbConnectionString = configuration["ConnectionStrings:ContosoPizza"];

        services.AddDbContext<ContosoPizzaDataContext>(options =>
        {
            options.UseSqlServer(contosoPizzaDbConnectionString);
            //If we are not in production, log to console
            if(!isProduction)
            {
                options.LogTo(Console.WriteLine);
            }
        });
        services.AddScoped<ISauceRepository, SauceRepository>();
        services.AddScoped<IToppingRepository, ToppingRepository>();
        services.AddScoped<IPizzaRepository, PizzaRepository>();

        return services;
    }
}
