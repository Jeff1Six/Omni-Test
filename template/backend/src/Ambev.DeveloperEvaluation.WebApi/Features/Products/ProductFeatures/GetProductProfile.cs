using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

public class GetProductProfile : Profile
{
    public GetProductProfile()
    {
        CreateMap<GetProductResult, GetProductResponse>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new GetProductResponse.RatingResponse
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            }));
    }
}
