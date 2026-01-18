using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        CreateMap<CartProductRequest, CreateCartCommand.CartProductCommand>();

        CreateMap<CreateCartRequest, CreateCartCommand>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<CreateCartResult, CreateCartResponse>();
        CreateMap<CreateCartResult.CartProductResult, CreateCartResponse.CartProductResponse>();
    }
}
