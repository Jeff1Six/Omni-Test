using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartProfile : Profile
{
    public UpdateCartProfile()
    {
        CreateMap<UpdateCartCommand.CartProductCommand, CartItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CartId, opt => opt.Ignore())
            .ForMember(dest => dest.Cart, opt => opt.Ignore());

        CreateMap<CartItem, UpdateCartResult.CartProductResult>();

        CreateMap<Cart, UpdateCartResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
    }
}
