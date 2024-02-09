using Contoso.Pizza.AdminApi.Minimal.Endpoints;
using Contoso.Pizza.AdminApi.Minimal.Handlers;
using Contoso.Pizza.AdminApi.Services.Extensions;

namespace Contoso.Pizza.AdminApi.Minimal;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        AddContosoServices(builder);

        // Add minimal API services to the container.
        AddMinimalApiServices(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        ConfigureMinimalApiServices(app);
        
        // Configure endpoints.
        ConfigureEndpoints(app);

        app.Run();
    }

    private static void ConfigureEndpoints(WebApplication app)
    {
        SauceEndpoints.Map(app);
    }

    private static void ConfigureMinimalApiServices(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
    }

    private static void AddMinimalApiServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    private static void AddContosoServices(WebApplicationBuilder builder)
    {
        builder.Services.AddContosoPizzaServices(builder.Configuration, builder.Environment.IsProduction());
        builder.Services.AddScoped<SauceEndpointHandler>();
    }
}
