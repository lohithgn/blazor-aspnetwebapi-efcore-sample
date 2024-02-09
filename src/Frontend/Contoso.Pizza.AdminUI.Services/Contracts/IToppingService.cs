using Contoso.Pizza.AdminApi.Models;

namespace Contoso.Pizza.AdminUI.Services.Contracts;

public interface IToppingService
{
    Task<IEnumerable<ToppingEntity>> GetAllToppingsAsync();
    Task<ToppingEntity> AddToppingAsync(ToppingEntity entity);
    Task UpdateToppingAsync(ToppingEntity entity);
    Task DeleteToppingAsync(ToppingEntity entity);
}
