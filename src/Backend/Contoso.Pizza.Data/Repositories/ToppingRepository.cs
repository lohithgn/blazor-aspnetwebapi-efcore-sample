using Contoso.Pizza.Data.Contracts;
using Contoso.Pizza.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Pizza.Data.Repositories;

public class ToppingRepository : IToppingRepository
{
    private readonly ContosoPizzaDataContext _dbContext;

    public ToppingRepository(ContosoPizzaDataContext context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<Topping>> GetAllAsync()
    {
        return await _dbContext.Toppings
                               .OrderByDescending(t => t.Modified)
                               .ThenBy(t => t.Created)
                               .AsNoTracking()
                               .ToListAsync();
    }

    public async Task<Topping?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Toppings.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Topping> AddAsync(Topping topping)
    {
        topping.Id = Guid.NewGuid();
        await _dbContext.Toppings.AddAsync(topping);
        await _dbContext.SaveChangesAsync();
        return topping;
    }

    public async Task<int> UpdateAsync(Topping topping)
    {
        var result = await _dbContext.Toppings.Where(t => t.Id == topping.Id)
                                              .ExecuteUpdateAsync(setter => setter
                                                    .SetProperty(t => t.Name, topping.Name)
                                                    .SetProperty(t => t.Description, topping.Description)
                                                    .SetProperty(t => t.Calories, topping.Calories)
                                                    .SetProperty(t => t.Modified, DateTime.UtcNow)
                                              );
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var childCount = await _dbContext.Toppings
                                         .Include(t => t.PizzaToppings)
                                         .Where(t => t.Id == id)
                                         .CountAsync(t => t.PizzaToppings.Any());
        if (childCount > 0)
        {
            throw new InvalidOperationException("Topping with ID " + id + " cannot be deleted because it is currently associated with a pizza.");
        }
        var result = await _dbContext.Toppings.Where(t => t.Id == id).ExecuteDeleteAsync();
        return result;
    }
}