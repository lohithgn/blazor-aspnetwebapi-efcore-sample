using Contoso.Pizza.Data.Contracts;
using DM = Contoso.Pizza.Data.Models;
using Microsoft.EntityFrameworkCore;
using Contoso.Pizza.Data.Models;

namespace Contoso.Pizza.Data.Repositories;

public class PizzaRepository : IPizzaRepository
{
    private readonly ContosoPizzaDataContext _dbContext;

    public PizzaRepository(ContosoPizzaDataContext context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<DM.Pizza>> GetAllAsync()
    {
        return await _dbContext.Pizza
                               .Include(p => p.Sauce)
                               .Include(p => p.Toppings)
                               .OrderByDescending(t => t.Modified)
                               .ThenBy(t => t.Created)
                               .AsNoTracking()
                               .ToListAsync();
    }

    public async Task<DM.Pizza?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Pizza
                               .Include(p => p.Sauce)  
                               .Include(p => p.Toppings)
                               .AsNoTracking()
                               .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<DM.Pizza> AddAsync(DM.Pizza pizza)
    {
        pizza.Id = Guid.NewGuid();

        _dbContext.Entry(pizza.Sauce).State = EntityState.Unchanged;

        // Load the toppings from the database
        foreach (var topping in pizza.Toppings)
        {
            _dbContext.Entry(topping).State = EntityState.Unchanged;
        }

        await _dbContext.Pizza.AddAsync(pizza);
        await _dbContext.SaveChangesAsync();

        return pizza;
    }

    public async Task<int> UpdateAsync(DM.Pizza pizza)
    {
        var storedPizza = await _dbContext.Pizza
                                 .Include(p => p.Toppings)
                                 .Where(t => t.Id == pizza.Id)
                                 .FirstOrDefaultAsync();

        if (storedPizza == null)
        {
            return -1;
        }
        storedPizza.Name = pizza.Name;
        storedPizza.Description = pizza.Description;
        storedPizza.SauceId = pizza.SauceId;
        storedPizza.Modified = DateTime.UtcNow;

        storedPizza.Toppings.RemoveAll(topping => !pizza.Toppings.Any(p => p.Id == topping.Id));
        
        foreach (var topping in pizza.Toppings)
        {
            if (!storedPizza.Toppings.Any(p => p.Id == topping.Id))
            {
                storedPizza.Toppings.Add(topping);
            }
        }
        
        var result = await _dbContext.SaveChangesAsync();

        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var result = await _dbContext.Pizza
                                     .Where(t => t.Id == id)
                                     .ExecuteDeleteAsync();
        return result;
    }
}