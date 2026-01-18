using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Product, CreateProductResult>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new Common.ProductRatingResult
            {
                Rate = src.RatingRate,
                Count = src.RatingCount
            }));
    }
}
