using Contoso.Pizza.AdminUI.Components;
using Contoso.Pizza.AdminUI.Services.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Contoso.Pizza.AdminUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddAdminUIServices(builder);
            
            AddBlazorComponents(builder);

            var app = builder.Build();

            ConfigureBlazorComonents(app);

            app.Run();
        }

        private static void ConfigureBlazorComonents(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
        }

        private static void AddAdminUIServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAdminUIServices(builder.Configuration);
        }

        private static void AddBlazorComponents(WebApplicationBuilder builder)
        {
            builder.Services.AddRazorComponents()
                            .AddInteractiveServerComponents();

            builder.Services.AddFluentUIComponents();
        }
    }
}
