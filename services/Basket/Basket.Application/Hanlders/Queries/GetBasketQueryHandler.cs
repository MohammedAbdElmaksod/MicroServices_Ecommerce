
using AutoMapper;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Hanlders.Queries;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepo;
    private readonly IMapper _mapper;
    public GetBasketQueryHandler(IBasketRepository basketRepo, IMapper mapper)
    {
        _basketRepo = basketRepo;
        _mapper = mapper;
    }

    public async Task<ShoppingCartResponse> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var result = await _basketRepo.GetBasketAsync(request.UserName);
        var shoppingCartResponse = _mapper.Map<ShoppingCartResponse>(result);
        return shoppingCartResponse;
    }
}
