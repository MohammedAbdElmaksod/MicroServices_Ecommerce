
using AutoMapper;
using Discount.Core.Entities;
using Discount.Grpc.Proto;

namespace Discount.Application.Mappers;

public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<Coupon, CouponModel>().ReverseMap();
    }
}
