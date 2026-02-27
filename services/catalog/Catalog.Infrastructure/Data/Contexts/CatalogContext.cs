

using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data.Contexts;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products { get; }

    public IMongoCollection<ProductBrand> Brands { get; }

    public IMongoCollection<ProductType> Types { get; }

    public CatalogContext(IConfiguration config)
    {
        var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
        var database = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
        Types = database.GetCollection<ProductType>(config["DatabaseSettings:TypesCollection"]);
        Brands = database.GetCollection<ProductBrand>(config["DatabaseSettings:BrandsCollection"]);
        Products = database.GetCollection<Product>(config["DatabaseSettings:ProductsCollection"]);

        _ = TypeContextSeed.SeedAsync(Types);
        _ = BrandContextSeed.SeedAsync(Brands);
        _ = CatalogContextSeed.SeedAsync(Products);
    }
}
