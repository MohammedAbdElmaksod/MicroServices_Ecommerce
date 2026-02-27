
using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.Contexts;

public static class BrandContextSeed
{
    public static async Task SeedAsync(IMongoCollection<ProductBrand> brandContext)
    {
        var count = await brandContext.CountDocumentsAsync(FilterDefinition<ProductBrand>.Empty);
        if (count>0)
            return;

        var filePate = Path.Combine("Data", "SeedData", "brands.json");
        if(!File.Exists(filePate))
            return;

        var brandData = await File.ReadAllTextAsync(filePate);
        var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

        if (brands?.Any() == true)
        {
            await brandContext.InsertManyAsync(brands);
        }
    }
}
