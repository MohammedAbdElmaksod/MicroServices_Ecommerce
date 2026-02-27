
namespace Catalog.Core.Specs;

public class CatalogSpecsParams
{
    private const int _maxPageSize = 50;
    private int _pageSize = 10;
    public int PageIndex { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
    }
    public string? BrandId { get; set; }
    public string? TypeId { get; set; }
    public string? Sort { get; set; }
    public string? Search { get; set; }
}
