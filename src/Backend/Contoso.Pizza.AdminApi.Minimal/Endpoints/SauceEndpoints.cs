using Contoso.Pizza.AdminApi.Minimal.Handlers;

namespace Contoso.Pizza.AdminApi.Minimal.Endpoints;

public static class SauceEndpoints
{
    public static void Map(WebApplication app)
    {
        var handler = new SauceEndpointHandler();
        app.MapGroup("/api/sauces")
           .MapGet("/", handler.GetAllSauces);
    }
}
