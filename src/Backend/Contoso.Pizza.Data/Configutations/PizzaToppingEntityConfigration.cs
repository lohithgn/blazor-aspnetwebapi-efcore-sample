using Contoso.Pizza.Data.Extensions;
using Contoso.Pizza.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contoso.Pizza.Data.Configutations;

public class PizzaToppingEntityConfigration : IEntityTypeConfiguration<PizzaTopping>
{
    public void Configure(EntityTypeBuilder<PizzaTopping> builder)
    {
        builder.ToTable("PizzaToppings");
        builder.ConfigureBaseEntity();
    }
}
