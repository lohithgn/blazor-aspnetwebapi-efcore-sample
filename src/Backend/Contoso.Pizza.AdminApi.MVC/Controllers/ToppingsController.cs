using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminApi.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.Pizza.AdminApi.MVC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToppingsController : ControllerBase
{
    private readonly IToppingService _service;

    public ToppingsController(IToppingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<ToppingEntity>> GetToppings()
    {
        var toppings = await _service.GetAllAsync();
        return toppings;
    }

    [HttpGet("{id:guid}", Name = "ToppingById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<NotFound, Ok<ToppingEntity>>> GetToppingById(Guid id)
    {
        var topping = await _service.GetByIdAsync(id);

        return topping == null ? TypedResults.NotFound() :
                               TypedResults.Ok(topping);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] ToppingEntity newTopping)
    {
        var createdTopping = await _service.AddAsync(newTopping);
        return CreatedAtAction(nameof(GetToppingById), new { id = createdTopping.Id }, createdTopping);
    }


    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Put(Guid id, [FromBody] ToppingEntity topping)
    {
        topping.Id = id;
        var result = await _service.UpdateAsync(topping);
        return result == 1 ? Ok() : NotFound();
    }

    // DELETE api/sauces/5
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
        catch 
        { 
            return BadRequest(); 
        }
    }
}
