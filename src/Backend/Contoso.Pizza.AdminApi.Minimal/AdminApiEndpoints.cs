using Contoso.Pizza.Data.Contracts;

namespace Contoso.Pizza.AdminApi.Minimal
{
    public static class AdminApiEndpoints
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/lohith", async (ISauceRepository sauceRepository) =>
            {
                return await sauceRepository.GetAllAsync();
            });
        }
    }
}
