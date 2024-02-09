namespace Contoso.Pizza.AdminApi.Models;

public class PizzaEntity : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; } = string.Empty;
    public Guid SauceId { get; set; }
    public SauceEntity? Sauce { get; set; }
    public List<ToppingEntity> Toppings { get; set; } = new();
}
