using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.WebApi.Seed;

public static class ProductSeeder
{
    public static async Task SeedAsync(DefaultContext context)
    {
        // Evita duplicar sempre que subir
        if (await context.Products.AnyAsync())
            return;

        var categories = new[]
        {
            "men's clothing",
            "women's clothing",
            "electronics",
            "jewelery"
        };

        var faker = new Faker<Product>("pt_BR")
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
            .RuleFor(x => x.Category, f => f.PickRandom(categories))
            .RuleFor(x => x.Image, f => f.Image.PicsumUrl())
            .RuleFor(x => x.Price, f => decimal.Parse(f.Commerce.Price(10, 900)))
            .RuleFor(x => x.RatingRate, f => Math.Round(f.Random.Double(1, 5), 1))
            .RuleFor(x => x.RatingCount, f => f.Random.Int(1, 500));

        var products = faker.Generate(30);

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}
