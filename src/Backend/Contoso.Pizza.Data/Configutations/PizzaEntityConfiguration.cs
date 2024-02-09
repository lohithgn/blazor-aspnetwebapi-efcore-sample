using D=Contoso.Pizza.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Contoso.Pizza.Data.Models;
using Contoso.Pizza.Data.Extensions;

namespace Contoso.Pizza.Data.Configutations;

public class PizzaEntityConfiguration : IEntityTypeConfiguration<D.Pizza>
{
    public void Configure(EntityTypeBuilder<D.Pizza> builder)
    {
        builder.ToTable("Pizzas");

        builder.ConfigureBaseEntity();

        builder.Property(s => s.Name)
               .IsRequired()
               .HasColumnType("nvarchar(100)");

        builder.Property(s => s.Description)
               .HasColumnType("nvarchar(300)");

        builder.HasOne(s => s.Sauce)
               .WithMany()
               .HasForeignKey(s => s.SauceId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Toppings)
               .WithMany(t => t.Pizzas)
               .UsingEntity<PizzaTopping>(
                    l => l.HasOne(pt => pt.Topping).WithMany(t => t.PizzaToppings),
                    r => r.HasOne(pt => pt.Pizza).WithMany(p => p.PizzaToppings)
               );
    }
}
