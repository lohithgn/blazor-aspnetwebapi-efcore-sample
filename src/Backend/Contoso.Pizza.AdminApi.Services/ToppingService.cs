using AutoMapper;
using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminApi.Services.Contracts;
using Contoso.Pizza.Data.Contracts;
using Contoso.Pizza.Data.Models;

namespace Contoso.Pizza.AdminApi.Services;

public class ToppingService : IToppingService
{
    private readonly IToppingRepository _repository;
    private readonly IMapper _mapper;

    public ToppingService(IToppingRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ToppingEntity>> GetAllAsync()
    {
        var toppings = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<ToppingEntity>>(toppings);
    }

    public async Task<ToppingEntity?> GetByIdAsync(Guid id)
    {
        var sauce = await _repository.GetByIdAsync(id);
        return _mapper.Map<ToppingEntity>(sauce);
    }

    public async Task<ToppingEntity> AddAsync(ToppingEntity entity)
    {
        var newTopping = _mapper.Map<Topping>(entity);
        await _repository.AddAsync(newTopping);
        return _mapper.Map<ToppingEntity>(newTopping);
    }

    public async Task<int> UpdateAsync(ToppingEntity entity)
    {
        var toppingToUpdate = _mapper.Map<Topping>(entity);
        return await _repository.UpdateAsync(toppingToUpdate);
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
