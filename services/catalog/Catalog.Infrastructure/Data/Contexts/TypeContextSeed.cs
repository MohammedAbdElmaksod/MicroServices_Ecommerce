using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.Contexts;

public static class TypeContextSeed
{
    public static async Task SeedAsync(IMongoCollection<ProductType> typeContext)
    {
        var count = await typeContext.CountDocumentsAsync(FilterDefinition<ProductType>.Empty);
        if (count > 0)
            return;

        var filePate = Path.Combine("Data", "SeedData", "types.json");
        if (!File.Exists(filePate))
            return;

        var typesData = await File.ReadAllTextAsync(filePate);
        var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

        if (types?.Any() == true)
        {
            await typeContext.InsertManyAsync(types);
        }
    }

}
