using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Common;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResult>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRatingResult
            {
                Rate = src.RatingRate,
                Count = src.RatingCount
            }));
    }
}
