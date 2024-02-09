using Contoso.Pizza.Data.Extensions;
using Contoso.Pizza.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contoso.Pizza.Data.Configutations;

public class SauceEntityConfiguration : IEntityTypeConfiguration<Sauce>
{
    public void Configure(EntityTypeBuilder<Sauce> builder)
    {
        builder.ToTable("Sauces");

        builder.ConfigureBaseEntity();

        builder.Property(s => s.Name)
               .IsRequired()
               .HasColumnType("nvarchar(100)");
               
        builder.Property(s => s.Description)
               .HasColumnType("nvarchar(300)");
    }
}
