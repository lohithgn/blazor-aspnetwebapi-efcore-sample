namespace Contoso.Pizza.Data.Models;

public class PizzaTopping : BaseModel
{
    public Guid PizzaId { get; set; }
    public Guid ToppingId { get; set; }
    public Pizza Pizza { get; set; } = null!;
    public Topping Topping { get; set; } = null!;
}
