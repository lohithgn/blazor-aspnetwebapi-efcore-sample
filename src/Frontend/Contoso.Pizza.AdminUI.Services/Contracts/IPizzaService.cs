using Contoso.Pizza.AdminApi.Models;

namespace Contoso.Pizza.AdminUI.Services.Contracts;

public interface IPizzaService
{
    Task<IEnumerable<PizzaEntity>> GetAllPizzasAsync();
    Task<PizzaEntity> AddPizzaAsync(PizzaEntity entity);
    Task UpdatePizzaAsync(PizzaEntity entity);
    Task DeletePizzaAsync(PizzaEntity entity);
}
