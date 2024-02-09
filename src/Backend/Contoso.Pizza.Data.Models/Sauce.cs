namespace Contoso.Pizza.Data.Models;

public class Sauce : BaseModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsVegan { get; set; }
}
