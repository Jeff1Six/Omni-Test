using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public class UpdateCartProfile : Profile
{
    public UpdateCartProfile()
    {
        CreateMap<CartProductRequest, UpdateCartCommand.CartProductCommand>();

        CreateMap<UpdateCartRequest, UpdateCartCommand>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<UpdateCartResult, UpdateCartResponse>();
        CreateMap<UpdateCartResult.CartProductResult, UpdateCartResponse.CartProductResponse>();
    }
}
