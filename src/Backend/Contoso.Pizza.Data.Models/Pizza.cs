namespace Contoso.Pizza.Data.Models;

public class Pizza : BaseModel
{
    public required string Name { get; set; }
    public string? Description { get; set; } = string.Empty;
    public Guid SauceId { get; set; }
    public required Sauce Sauce { get; set; }
    public List<Topping> Toppings { get; set; } = new();
    public List<PizzaTopping> PizzaToppings { get; set; } = new();
}