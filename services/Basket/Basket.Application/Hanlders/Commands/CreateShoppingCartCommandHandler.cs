
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Hanlders.Commands;

public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
{
    private readonly IBasketRepository _basketRepo;
    private readonly IMapper _mapper;
    private readonly DiscountGrpcService _discountGrpcService;

    public CreateShoppingCartCommandHandler(IBasketRepository basketRepo, IMapper mapper, DiscountGrpcService discountGrpcService)
    {
        _basketRepo = basketRepo;
        _mapper = mapper;
        _discountGrpcService = discountGrpcService;
    }

    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        // apply discount using grpc service
        foreach(var item in request.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProdcuteName);
            if (coupon != null)
            {
                item.Price -= coupon.Amount;
            }
        }

        var result = await _basketRepo.UpdateBasketAsync(new ShppingCart
        {
            UserName = request.UserName,
            Items = request.Items,
        });
        var shoppingCartResponse = _mapper.Map<ShoppingCartResponse>(result);
        return shoppingCartResponse;
    }
}
