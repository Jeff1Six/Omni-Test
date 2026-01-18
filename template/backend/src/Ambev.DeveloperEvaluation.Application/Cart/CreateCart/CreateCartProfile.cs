using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        // CartItem -> ResultProduct
        CreateMap<CartItem, CreateCartResult.CartProductResult>();

        // Cart -> Result
        CreateMap<Cart, CreateCartResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
    }
}
