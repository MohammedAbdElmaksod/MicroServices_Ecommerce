

using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IList<TypeResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly ITypeRepository _typeRepository;

    public GetAllTypesQueryHandler(ITypeRepository typeRepository, IMapper mapper)
    {
        _typeRepository = typeRepository;
        _mapper = mapper;
    }

    public async Task<IList<TypeResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await _typeRepository.GetAllTypes();
        var typesDto = _mapper.Map<IList<ProductType>, IList<TypeResponseDto>>(types.ToList());
        return typesDto;
    }
}
