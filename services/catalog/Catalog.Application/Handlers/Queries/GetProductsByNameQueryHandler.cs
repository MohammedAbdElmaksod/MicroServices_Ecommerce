

using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public class GetProductsByNameQueryHandler : IRequestHandler<GetProductByNameQuery, IList<ProductResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    public GetProductsByNameQueryHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<IList<ProductResponseDto>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllProductsByName(request.Name);
        var productsDto = _mapper.Map<IList<ProductResponseDto>>(products);
        return productsDto;
    }
}
