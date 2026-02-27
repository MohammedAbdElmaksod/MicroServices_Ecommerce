
using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.Core.Repositories;

public interface IProductRepository
{
    Task<Pagination<Product>> GetAllProducts(CatalogSpecsParams catalogSpecsParams);
    Task<Product?> GetProductById(string id);
    Task<IEnumerable<Product>> GetAllProductsByName(string name);
    Task<IEnumerable<Product>> GetAllProductsByBrand(string name);
    Task<Product> CreateProduct(Product model);
    Task<bool> UpdateProduct(Product model);
    Task<bool> DeleteProduct(string id);
}
