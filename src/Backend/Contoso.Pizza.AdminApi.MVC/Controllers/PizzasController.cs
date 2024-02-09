using Contoso.Pizza.AdminApi.Models;
using Contoso.Pizza.AdminApi.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.Pizza.AdminApi.MVC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PizzasController : ControllerBase
{
    private readonly IPizzaService _service;

    public PizzasController(IPizzaService service)
    {
        _service = service;
    }

    [HttpGet( Name = nameof(GetPizzas))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<PizzaEntity>> GetPizzas()
    {
        var pizzas = await _service.GetAllAsync();
        return pizzas;
    }

    [HttpGet("{id:guid}", Name = "PizzaById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<NotFound, Ok<PizzaEntity>>> GetPizzaById(Guid id)
    {
        var pizza = await _service.GetByIdAsync(id);

        return pizza == null ? TypedResults.NotFound() :
                               TypedResults.Ok(pizza);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] PizzaEntity newPizza)
    {
        var createdPizza = await _service.AddAsync(newPizza);
        return CreatedAtAction(nameof(GetPizzaById), new { id = createdPizza.Id }, createdPizza);
    }


    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Put(Guid id, [FromBody] PizzaEntity pizza)
    {
        pizza.Id = id;
        var result = await _service.UpdateAsync(pizza);
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
