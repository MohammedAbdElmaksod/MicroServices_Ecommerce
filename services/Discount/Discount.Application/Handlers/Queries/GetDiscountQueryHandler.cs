

using AutoMapper;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Proto;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Handlers.Queries;

public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDiscountQueryHandler> _logger;

    public GetDiscountQueryHandler(IDiscountRepository discountRepository, ILogger<GetDiscountQueryHandler> logger, IMapper mapper)
    {
        _discountRepository = discountRepository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _discountRepository.GetDiscount(request.ProductName);
        if(coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"this productName: {request.ProductName} has not discount"));
        }
        var couponModel = _mapper.Map<CouponModel>(coupon);
        _logger.LogInformation($"Coupon for {request.ProductName} is fetched");
        return couponModel;
    }
}
