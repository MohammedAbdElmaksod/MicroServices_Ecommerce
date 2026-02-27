
using AutoMapper;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specs;
using MongoDB.Driver.Search;

namespace Catalog.Application.Mappers;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<ProductBrand, BrandResponseDto>().ReverseMap();
        CreateMap<ProductResponseDto, Product>().ReverseMap();
        CreateMap<TypeResponseDto, ProductType>().ReverseMap();
        CreateMap<Pagination<ProductResponseDto>, Pagination<Product>>().ReverseMap();
    }
}
