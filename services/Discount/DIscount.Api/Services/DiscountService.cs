using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Proto;
using Grpc.Core;
using MediatR;

namespace DIscount.Api.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IMediator _mediator;

    public DiscountService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var query = new GetDiscountQuery(request.ProductName);
        return await _mediator.Send(query);
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var cmd = new CreateDiscountCommand
        {
            ProductName = request.Coupon.ProductName,
            Description = request.Coupon.Description,
            Amount = request.Coupon.Amount
        };
        var coupon = await _mediator.Send(cmd);
        return coupon;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var cmd = new UpdateDiscountCommand
        {
            Id = request.Coupon.Id,
            ProductName = request.Coupon.ProductName,
            Description = request.Coupon.Description,
            Amount = request.Coupon.Amount
        };
        var coupon = await _mediator.Send(cmd);
        return coupon;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var cmd = new DeleteDiscountCommand(request.ProductName);
        var result = await _mediator.Send(cmd);
        return new DeleteDiscountResponse { Success = result };
    }
}
