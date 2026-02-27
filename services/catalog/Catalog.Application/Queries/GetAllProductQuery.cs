
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Queries;

public class GetAllProductQuery : IRequest<Pagination<ProductResponseDto>>
{
    public CatalogSpecsParams Specs { get; set; }
    public GetAllProductQuery(CatalogSpecsParams specs)
    {
        Specs = specs;
    }
}
