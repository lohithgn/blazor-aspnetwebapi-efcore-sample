using Contoso.Pizza.Data;
using Contoso.Pizza.Data.Initializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Contoso.Pizza.AdminApi.Services.Extensions;

public static class HostExtensions
{
    public static void CreateDbIfNotExists(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ContosoPizzaDataContext>();
            context.Database.EnsureCreated();
            DbInitializer.Initialize(context);
        }
    }
}
