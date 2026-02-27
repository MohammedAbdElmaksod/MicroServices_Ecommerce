

using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    public GetProductByIdHandler(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductById(request.Id);
        var productResponseDto = _mapper.Map<Product, ProductResponseDto>(product);
        return productResponseDto;
    }
}
