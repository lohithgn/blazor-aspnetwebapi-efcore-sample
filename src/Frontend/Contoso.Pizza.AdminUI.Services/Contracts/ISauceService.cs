using Contoso.Pizza.AdminApi.Models;

namespace Contoso.Pizza.AdminUI.Services.Contracts;

public interface ISauceService
{
    Task<IEnumerable<SauceEntity>> GetAllSaucesAsync();
    Task<SauceEntity> AddSauceAsync(SauceEntity entity);
    Task UpdateSauceAsync(SauceEntity entity);
    Task DeleteSauceAsync(SauceEntity entity);
}
