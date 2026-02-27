
using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public class GetProductsByBrandQueryHandler : IRequestHandler<GetProductByBrandQuery, IList<ProductResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    public GetProductsByBrandQueryHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<IList<ProductResponseDto>> Handle(GetProductByBrandQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllProductsByBrand(request.BrandName);
        var productsDto = _mapper.Map<IList<ProductResponseDto>>(products);
        return productsDto;
    }
}
