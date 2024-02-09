namespace Contoso.Pizza.AdminApi.Models;

public class SauceEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsVegan { get; set; }
}
