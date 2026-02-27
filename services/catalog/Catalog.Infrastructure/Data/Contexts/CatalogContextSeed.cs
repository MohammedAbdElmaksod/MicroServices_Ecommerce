
using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.Contexts;

public static class CatalogContextSeed
{
    public static async Task SeedAsync(IMongoCollection<Product> productContext)
    {
        var count = await productContext.CountDocumentsAsync(FilterDefinition<Product>.Empty);

        if (count > 0)
            return;

        var filePath = Path.Combine(
            
            "Data",
            "SeedData",
            "products.json"
        );


        if (!File.Exists(filePath))
            return;
        

        var productsData = await File.ReadAllTextAsync(filePath);
        var products = JsonSerializer.Deserialize<List<Product>>(productsData);


        if (products?.Any() == true)
        {
            await productContext.InsertManyAsync(products);
        }
    }
}

