using Contoso.Pizza.Data.Configutations;
using Contoso.Pizza.Data.Models;
using Microsoft.EntityFrameworkCore;
using DM = Contoso.Pizza.Data.Models;
namespace Contoso.Pizza.Data;

public class ContosoPizzaDataContext : DbContext 
{
    public DbSet<Sauce> Sauces { get; set; }
    public DbSet<DM.Pizza> Pizza { get; set; }
    public DbSet<Topping> Toppings { get; set; }

    public ContosoPizzaDataContext(DbContextOptions<ContosoPizzaDataContext> options) 
        : base(options) { }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new SauceEntityConfiguration().Configure(modelBuilder.Entity<Sauce>());
        new PizzaEntityConfiguration().Configure(modelBuilder.Entity<DM.Pizza>());
        new ToppingEntityConfiguration().Configure(modelBuilder.Entity<Topping>());
        new PizzaToppingEntityConfigration().Configure(modelBuilder.Entity<PizzaTopping>());
    }
}
