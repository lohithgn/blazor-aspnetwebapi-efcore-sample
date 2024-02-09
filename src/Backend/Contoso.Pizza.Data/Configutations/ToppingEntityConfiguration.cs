using D=Contoso.Pizza.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Contoso.Pizza.Data.Extensions;
using Contoso.Pizza.Data.Models;

namespace Contoso.Pizza.Data.Configutations;

public class ToppingEntityConfiguration : IEntityTypeConfiguration<D.Topping>
{
    public void Configure(EntityTypeBuilder<D.Topping> builder)
    {
        builder.ToTable("Toppings");

        builder.ConfigureBaseEntity();
        
        builder.Property(s => s.Name)
               .IsRequired()
               .HasColumnType("nvarchar(100)");
               
        builder.Property(s => s.Description)
               .HasColumnType("nvarchar(300)");

        builder.HasMany(p => p.Pizzas)
               .WithMany(p => p.Toppings)
               .UsingEntity<PizzaTopping>(
                    l => l.HasOne(pt => pt.Pizza).WithMany(p => p.PizzaToppings),
                    r => r.HasOne(pt => pt.Topping).WithMany(p => p.PizzaToppings)
               );
    }
}
