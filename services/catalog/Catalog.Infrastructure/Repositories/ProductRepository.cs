using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data.Contexts;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository, IBrandRepository, ITypeRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<Pagination<Product>> GetAllProducts(CatalogSpecsParams catalogSpecsParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;
        if (!string.IsNullOrEmpty(catalogSpecsParams.Search))
            filter = filter & builder.Where(p => p.Name.ToLower().Contains(catalogSpecsParams.Search.ToLower()));

        if (!string.IsNullOrEmpty(catalogSpecsParams.BrandId))
            filter = filter & builder.Eq(p => p.Brand.Id, catalogSpecsParams.BrandId.ToString());

        if (!string.IsNullOrEmpty(catalogSpecsParams.TypeId))
            filter = filter & builder.Eq(p => p.Type.Id, catalogSpecsParams.TypeId.ToString());


        var totalItems = await _context.Products.CountDocumentsAsync(filter);
        var data = await DataFilter(catalogSpecsParams, filter);

        return new Pagination<Product>(catalogSpecsParams.PageIndex, catalogSpecsParams.PageSize, (int)totalItems, data);
        //return await _context.Products.Find(filter).ToListAsync();
    }
    public async Task<Product?> GetProductById(string id)
    {
        return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<Product>> GetAllProductsByBrand(string name)
    {
        return await _context.Products.Find(p => p.Brand.Name == name).ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetAllProductsByName(string name)
    {
        return await _context.Products.Find(p => p.Name == name).ToListAsync();
    }
    public async Task<Product> CreateProduct(Product model)
    {
        await _context.Products.InsertOneAsync(model);
        return model;
    }
    public async Task<bool> UpdateProduct(Product model)
    {
        var result = await _context.Products.ReplaceOneAsync(p => p.Id == model.Id, model);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
    public async Task<bool> DeleteProduct(string id)
    {
        var result = await _context.Products.DeleteOneAsync(p => p.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await _context.Brands.Find(_ => true).ToListAsync();
    }
    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        return await _context.Types.Find(_ => true).ToListAsync();
    }

    private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecsParams catalogSpecsParams, FilterDefinition<Product> filter)
    {
        var filterDefinition = Builders<Product>.Sort.Ascending(p => p.Name);
        if (!string.IsNullOrEmpty(catalogSpecsParams.Sort))
        {
            switch (catalogSpecsParams.Sort)
            {
                case "priceAsc":
                    filterDefinition = Builders<Product>.Sort.Ascending(p => p.Price);
                    break;
                case "priceDesc":
                    filterDefinition = Builders<Product>.Sort.Descending(p => p.Price);
                    break;
                default:
                    filterDefinition = Builders<Product>.Sort.Ascending("Name");
                    break;
            }
        }
        return await _context.Products.Find(filter)
            .Sort(filterDefinition)
            .Skip((catalogSpecsParams.PageIndex - 1) * catalogSpecsParams.PageSize)
            .Limit(catalogSpecsParams.PageSize)
            .ToListAsync();
    }
}
