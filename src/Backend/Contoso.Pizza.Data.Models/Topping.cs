namespace Contoso.Pizza.Data.Models;

public class Topping : BaseModel
{
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Calories { get; set; }
    public List<Pizza> Pizzas { get; set; } = new();
    public List<PizzaTopping> PizzaToppings { get; set; } = new();
}