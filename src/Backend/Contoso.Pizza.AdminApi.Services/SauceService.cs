using AutoMapper;
using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminApi.Services.Contracts;
using Contoso.Pizza.Data.Contracts;
using Contoso.Pizza.Data.Models;
using Contoso.Pizza.Data.Repositories;

namespace Contoso.Pizza.AdminApi.Services;

public class SauceService : ISauceService
{
    private readonly ISauceRepository _repository;
    private readonly IMapper _mapper;

    public SauceService(ISauceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SauceEntity>> GetAllAsync()
    {
        var sauces = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<SauceEntity>>(sauces);
    }

    public async Task<SauceEntity?> GetByIdAsync(Guid id)
    {
        var sauce = await _repository.GetByIdAsync(id);
        return _mapper.Map<SauceEntity>(sauce);
    }

    public async Task<SauceEntity> AddAsync(SauceEntity entity)
    {
        var newSauce = _mapper.Map<Sauce>(entity);
        await _repository.AddAsync(newSauce);
        return _mapper.Map<SauceEntity>(newSauce);
    }

    public async Task<int> UpdateAsync(SauceEntity entity)
    {
        var sauceToUpdate = _mapper.Map<Sauce>(entity);
        return await _repository.UpdateAsync(sauceToUpdate);
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
