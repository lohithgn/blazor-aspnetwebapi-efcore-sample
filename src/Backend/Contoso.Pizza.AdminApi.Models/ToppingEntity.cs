namespace Contoso.Pizza.AdminApi.Models;

public class ToppingEntity : BaseEntity
{
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Calories { get; set; }
}
