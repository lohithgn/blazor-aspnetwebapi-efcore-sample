using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminApi.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.Pizza.AdminApi.Minimal.Handlers;

public class SauceEndpointHandler
{
    public async Task<IEnumerable<SauceEntity>> GetAllSauces([FromServices] ISauceService service)
    {
        return await service.GetAllAsync();
    }

    public async Task<Results<NotFound, Ok<SauceEntity>>> GetSauceById([FromServices] ISauceService service, Guid id)
    {
        var sauce = await service.GetByIdAsync(id);

        return sauce == null ? TypedResults.NotFound() :
                               TypedResults.Ok(sauce);
    }

    public async Task<CreatedAtRoute> AddSauce([FromServices] ISauceService service, [FromBody] SauceEntity newSauce)
    {
        var createdSauce = await service.AddAsync(newSauce);
        return TypedResults.CreatedAtRoute(nameof(GetSauceById), new { id = createdSauce.Id });
    }

    public async Task<Results<Ok,NotFound>> UpdateSauce([FromServices] ISauceService service, Guid id, [FromBody] SauceEntity sauce)
    {
        sauce.Id = id;
        var result = await service.UpdateAsync(sauce);
        return result == 1 ? TypedResults.Ok() : TypedResults.NotFound();
    }

    public async Task<Results<Ok,NotFound,Conflict<string>>> DeleteSauce([FromServices] ISauceService service, Guid id)
    {
        try
        {
            var result = await service.DeleteAsync(id);
            return result == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }

}
