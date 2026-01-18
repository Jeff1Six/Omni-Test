using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>()
            .ForMember(dest => dest.RatingRate, opt => opt.MapFrom(src => src.Rating.Rate))
            .ForMember(dest => dest.RatingCount, opt => opt.MapFrom(src => src.Rating.Count));

        CreateMap<CreateProductResult, CreateProductResponse>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new CreateProductResponse.RatingResponse
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            }));
    }
}
