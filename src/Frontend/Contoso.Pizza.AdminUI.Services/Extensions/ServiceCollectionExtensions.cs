using Contoso.Pizza.AdminUI.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contoso.Pizza.AdminUI.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdminUIServices(this IServiceCollection services, IConfiguration configuration)
    {
        var apiUrl = configuration["Api:Url"]!;
        services.AddHttpClient("AdminApi",client =>
        {
            client.BaseAddress = new Uri(apiUrl, UriKind.Absolute);
        });
        services.AddScoped<ISauceService, SauceService>();
        services.AddScoped<IToppingService, ToppingService>();
        services.AddScoped<IPizzaService, PizzaService>();
        return services;
    }
}
