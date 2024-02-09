using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminApi.Services.Contracts;
using Contoso.Pizza.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.Pizza.AdminApi.MVC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SaucesController : ControllerBase
{
    private readonly ISauceService _service;

    public SaucesController(ISauceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<SauceEntity>> GetSauces()
    {
        var sauces = await _service.GetAllAsync();
        return sauces;
    }

    [HttpGet("{id:guid}", Name = "SauceById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<NotFound, Ok<SauceEntity>>> GetSauceById(Guid id)
    {
        var sauce = await _service.GetByIdAsync(id);

        return sauce == null ? TypedResults.NotFound() :
                               TypedResults.Ok(sauce);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] SauceEntity newSauce)
    {
        var createdSauce = await _service.AddAsync(newSauce);
        return CreatedAtAction(nameof(GetSauceById), new { id = createdSauce.Id }, createdSauce);
    }


    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Put(Guid id, [FromBody] SauceEntity sauce)
    {
        sauce.Id = id;
        var result = await _service.UpdateAsync(sauce);
        return result == 1 ? Ok() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            return result == 1 ? Ok() : NotFound();
        }
        catch (InvalidOperationException ex) 
        { 
            return Conflict(ex.Message);
        }
    }
}
