using Contoso.Pizza.Data.Contracts;
using Contoso.Pizza.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Pizza.Data.Repositories;

public class SauceRepository : ISauceRepository
{
    private readonly ContosoPizzaDataContext _dbContext;

    public SauceRepository(ContosoPizzaDataContext context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<Sauce>> GetAllAsync()
    {
        return await _dbContext.Sauces.OrderByDescending(s => s.Modified)
                                      .ThenBy(s => s.Created)                
                                      .AsNoTracking()
                                      .ToListAsync();
    }

    public async Task<Sauce?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Sauces.FindAsync(id);
    }

    public async Task<Sauce> AddAsync(Sauce sauce)
    {
        sauce.Id = Guid.NewGuid();
        await _dbContext.Sauces.AddAsync(sauce);
        await _dbContext.SaveChangesAsync();
        return sauce;
    }

    public async Task<int> UpdateAsync(Sauce sauce)
    {
        var result = await _dbContext.Sauces.Where(s => s.Id == sauce.Id)
                                            .ExecuteUpdateAsync(setter => setter
                                                .SetProperty(s => s.Name, sauce.Name)
                                                .SetProperty(s => s.Description, sauce.Description)
                                                .SetProperty(s => s.IsVegan, sauce.IsVegan)
                                                .SetProperty(s => s.Modified, DateTime.UtcNow)
                                            );
        return result;
    }


    public async Task<int> DeleteAsync(Guid id)
    {
        var childCount = await _dbContext.Pizza
                                         .Where(p => p.SauceId == id)
                                         .CountAsync();
        if (childCount > 0)
        {
            throw new InvalidOperationException("Sauce with ID " + id + " cannot be deleted because it is currently associated with a pizza.");
        }
        return await _dbContext.Sauces.Where(s => s.Id == id).ExecuteDeleteAsync();
    }
}