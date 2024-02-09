using Contoso.Pizza.Data.Contracts;
using Contoso.Pizza.Data.Extensions;
using Contoso.Pizza.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contoso.Pizza.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //load configuration
            var configuration = new ConfigurationBuilder()
                                        .AddJsonFile("appsettings.json")
                                        .Build();

            //setup DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddContosoPizzaData(configuration, false)
                .BuildServiceProvider();

            //Get Sauce Repository
            var sauceRepository = serviceProvider.GetService<ISauceRepository>()!;

            //Get all sauces
            var sauces = await sauceRepository.GetAllAsync();
            if (sauces.Count() == 0)
            {
                _ = await sauceRepository.AddAsync(new Sauce
                {
                    Name = $"Tomato Sauce {DateTime.Now:yyMMddhhmmss}",
                    Description = "This is a great sauce",
                });
            }

            //Display all sauces
            foreach (var sauce in sauces)
            {
                System.Console.WriteLine($"Sauce Id: {sauce.Id}, Name: {sauce.Name}");
            }
            System.Console.WriteLine("Press any key to close...");
            System.Console.ReadKey();
        }
    }
}
